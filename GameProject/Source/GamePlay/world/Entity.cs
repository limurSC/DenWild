using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild.Source.Engine;
using GameProject.Source.GamePlay.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DenWild.World;
using GameProject.Source.GamePlay.world.Entities;
using System.Xml;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using GameProject.Source.GamePlay;
using DenWild;

namespace DenWild.World
{
    public record Entity : Basic2d
    {
        public bool CheckToKill;
        public string Name;
        public int LastTimeAttack;
        public double Hp;
        public double MaxHp;
        public List<Vector2> MotionVectors;
        public List<Vector2> SwitchingPositions;
        public bool permissionToAttack;
        public string Team;
        private Texture2D HpModel;
        public int EntitySpeed;
        public int Damage;
        public int AttackRange;

        public Entity(string path, Vector2 position, Vector2 dims, float height) : 
            base(path, position, dims, height)
        {
            CheckToKill = false;
            LastTimeAttack = 0;
            permissionToAttack = true; 
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
            HpModel = Globals.Content.Load<Texture2D>("2d\\Hp");
        }

        public override void Update()
        {
            base.Update();
        }

        public bool Attack(int damage, int attackRange)
        {
            var attackEntity = CheckAttack(attackRange);
            if (permissionToAttack && Globals.GameTime - LastTimeAttack >= 60)
            {
                if (attackEntity != null)
                {
                    Globals.Bullets.Add(new Bullet<Entity>(damage, attackEntity, "2d\\" + Name + "Bullet",
                        Position, new Vector2(10, 3), 0.49f));
                    LastTimeAttack = Globals.GameTime;
                }
            }
            if(Globals.GameTime - LastTimeAttack > 60)
                return true;
            if (attackEntity != null)
                ChangeRotation(attackEntity.Position - Position);
            return false;
        }

        public Entity CheckAttack(int attackRange)
        {
            Entity nearestEntity = null;
            if (Globals.AllFriendlyEntity.Contains(this))
                foreach (Entity entity in Globals.AllEnemyEntity)
                {
                    if ((entity.Position - Position).Length() < attackRange && (nearestEntity == null ||
                    (nearestEntity.Position - Position).Length() > (entity.Position - Position).Length()))
                    {
                        nearestEntity = entity;
                    }
                }
            if (Globals.AllEnemyEntity.Contains(this))
            {
                foreach (Entity entity in Globals.AllFriendlyEntity)
                {
                    if ((entity.Position - Position).Length() < attackRange && (nearestEntity == null ||
                    (nearestEntity.Position - Position).Length() > (entity.Position - Position).Length()))
                    {
                        nearestEntity = entity;
                    }
                }
                foreach (Entity entity in Globals.AllFriendlyBuilder)
                {
                    if ((entity.Position - Position).Length() < attackRange && (nearestEntity == null ||
                    (nearestEntity.Position - Position).Length() > (entity.Position - Position).Length()))
                    {
                        nearestEntity = entity;
                    }
                }
            }
            return nearestEntity;
        }

        public void Moving(int entitySpeed)
        {
            var mouseState = Mouse.GetState();
            if (!Globals.Control.CheckBuild && (Globals.Control.CheckOneKeyClick(Keys.S) || 
                mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1603 && mouseState.Position.X < 1682 &&
                mouseState.Position.Y > 853 && mouseState.Position.Y < 923))
            {
                MotionVectors.Clear();
                SwitchingPositions.Clear();
            }
            if (SwitchingPositions.Count != 0 && (SwitchingPositions[0] - Position).Length() >= entitySpeed)
            {
                if (CheckMoving(entitySpeed))
                {
                    ChangeRotation(MotionVectors[0]);
                    Position += MotionVectors[0] * entitySpeed;
                }
            }
            else if (SwitchingPositions.Count != 0 && (SwitchingPositions[0] - Position).Length() < entitySpeed)
            {
                if (CheckMoving(entitySpeed))
                    Position = SwitchingPositions[0];
                MotionVectors.RemoveAt(0);
                SwitchingPositions.RemoveAt(0);
            }
            else
                permissionToAttack = true;
        }

        public void ChangeRotation(Vector2 rotatVector)
        {
            if (rotatVector.Y > 0 && rotatVector.X > 0)
                Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X) - Math.PI / 2);
            else if (rotatVector.Y < 0 && rotatVector.X < 0)
                Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X) + Math.PI / 2);
            else if (rotatVector.Y > 0 && rotatVector.X < 0)
                Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X) + Math.PI / 2);
            else if (rotatVector.Y < 0 && rotatVector.X > 0)
                Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X) - Math.PI / 2);
        }

        public bool CheckMoving(int entitySpeed)
        {
            foreach(var entity in Globals.AllEntity)
            {
                if (entity != this && 
                    (entity.Position - Position - MotionVectors[0] * entitySpeed).Length() < 
                    (entity.Dims + Dims).X * 0.4)
                    {
                        if (entity != this && entity.Team == "Team 1" && 
                        (entity.Name == "Drone" || entity.Name == "Builder") &&
                            entity.MotionVectors.Count == 0)
                        {
                            var switchingPosition = entity.Position + (entity.Position - Position) / 2;
                            entity.SwitchingPositions = new List<Vector2> { switchingPosition };
                            var motion = new Vector2(switchingPosition.X - entity.Position.X,
                                switchingPosition.Y - entity.Position.Y);
                            var distance = motion.Length();
                            entity.MotionVectors = new List<Vector2> { 
                                new Vector2(motion.X / distance, motion.Y / distance) };
                        }
                        return false;
                    }
            }
            return true;
        }

        public override void Draw()
        {
            base.Draw();
        }
        
        public void DrawHp()
        {
            Globals.SpriteBatch.Draw(HpModel, new Rectangle((int)(Position.X - Dims.X / 2),
                (int)(Position.Y - Dims.Y / 2),
                    (int)(Dims.X * Hp / MaxHp), 6), null,
                    new Color((1 - (float)(Hp / MaxHp)), (float)(Hp / MaxHp), 0, 0.6f), 0,
                    new Vector2(0, 0),
                    new SpriteEffects(), Height - 0.02f);
        }
    }
}
