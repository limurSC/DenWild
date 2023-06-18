using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DenWild.Source.Engine;
using GameProject.Source.Engine;
using DenWild.World;
using GameProject.Source.GamePlay.world.Entities;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using System.Diagnostics;

namespace GameProject.Source.GamePlay.world.Entities.FriendlyBuilders
{
    public record Base : FriendlyBuilding
    {
        public Vector2 UnitSwitchPosition;
        public List<FriendlyEntity> QueueCreateEntity;
        public int CreateProgress;
        public Base(string path, Vector2 position, Vector2 dims, float height) :
            base(path, position, dims, height)
        {
            CreateProgress = 0;
            Hp = 5000;
            MaxHp = 5000;
            Price = 5000;
            Name = "Base";
            Vision = 600;
            permissionToAttack = true;
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
            QueueCreateEntity = new List<FriendlyEntity>();
            UnitSwitchPosition = Position;
        }
        public override void Update()
        {
            GetUnitSwitchPosition();
            if (CheckCoompletBuild)
            {
                if (QueueCreateEntity.Count < 5)
                    Globals.Control.AddCreateEntity(QueueCreateEntity, Position, this);
                CreateEntity();
            }
            base.Update();
        }

        public void GetUnitSwitchPosition()
        {
            var mouseState = Mouse.GetState();
            if(mouseState.RightButton == ButtonState.Pressed && Globals.ChoiceEntity.selectedEntity.Contains(this))
            {
                UnitSwitchPosition = mouseState.Position.ToVector2() + Globals.Camera.Position;
            }
        }

        public void CreateEntity()
        {
            if(QueueCreateEntity.Count > 0)
            {
                CreateProgress++;
                if (QueueCreateEntity[0].CreateTime <= CreateProgress)
                {
                    CreateProgress = 0;
                    var entity = QueueCreateEntity[0];
                    QueueCreateEntity.RemoveAt(0);
                    var rotation = Extensions.ChangeRotationed(UnitSwitchPosition - Position);
                    entity.Position += new Vector2(((Dims + entity.Dims).X / 2), 0).Rotate((float)(rotation));
                    entity.Position = GetCreateEntityPosition(entity.Position, entity);
                    Control.AddMoveEntity(new Keys[] { }, entity.MotionVectors,
                        entity.SwitchingPositions, entity.Position, UnitSwitchPosition);
                    Globals.AllFriendlyEntity.Add(entity);
                    Globals.AllEntity.Add(entity);
                }
            }
        }

        public Vector2 GetCreateEntityPosition(Vector2 position, FriendlyEntity entity)
        {
            var queue = new Queue<Vector2>();
            queue.Enqueue(position);
            var visited = new HashSet<Vector2>();
            while (!CheckCreate(position, entity) && queue.Count != 0)
            {
                var node = queue.Dequeue();

                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        if (Math.Abs(dx) == Math.Abs(dy)) continue;
                        var nextNode = new Vector2(node.X + dx, node.Y + dy);
                        if(visited.Contains(nextNode)) continue;
                        if (CheckCreate(nextNode, entity)) 
                            return nextNode;
                        visited.Add(nextNode);
                        queue.Enqueue(nextNode);
                    }
                }
            }
            return position;
        }


        public bool CheckCreate(Vector2 position, Entity checkEntity)
        {
            foreach (var entity in Globals.AllEntity)
            {
                if (entity != checkEntity &&
                    (entity.Position - position).Length() <
                    (entity.Dims + checkEntity.Dims).X * 0.4)
                    return false;
            }
            return true;
        }

        public override void Draw()
        {
            if (Globals.ChoiceEntity.selectedEntity.Contains(this))
            {
                var rotation = Extensions.ChangeRotationed(UnitSwitchPosition - Position);
                var myModel = Globals.Content.Load<Texture2D>("2d\\Hp");
                Globals.SpriteBatch.Draw(myModel, new Rectangle((int)Position.X, (int)Position.Y,
                    (int)(UnitSwitchPosition - Position).Length() , 2), 
                    null, Color.White, rotation,
                    new Vector2(0, 0), 
                    new SpriteEffects(), 0.49f);
            }
            base.Draw();
        }

    }
}
