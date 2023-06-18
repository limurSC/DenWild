using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild.Source.Engine;
using DenWild.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Source.GamePlay.world.Entities
{
    public record EnemyEntity : Entity
    {
        public int[] MovingPlace = new int[4];
        public EnemyEntity(string path, Vector2 position,
            Vector2 dims, float height, int[] movingPlace) : base(path, position, dims, height)
        {
            MovingPlace = movingPlace;
            Team = "Team 2";
            permissionToAttack = true;
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
        }

        public override void Update()
        {
            if (Attack(Damage, AttackRange) && EntitySpeed > 0)
                Moving(EntitySpeed);
            if (Hp <= 0)
                CheckToKill = true;
            base.Update();
        }

        public void MovingEnemy()
        {
            var rand = new Random();
            if (Globals.GameTime % rand.Next(50, 100) == 0)
            {
                var switchingPosition = new Vector2(rand.Next(MovingPlace[0], MovingPlace[2]), 
                    rand.Next(MovingPlace[1], MovingPlace[3]));
                SwitchingPositions = new List<Vector2> { switchingPosition };
                var motion = new Vector2(switchingPosition.X - Position.X,
                    switchingPosition.Y - Position.Y);
                var distance = motion.Length();
                MotionVectors = new List<Vector2> { new Vector2(motion.X / distance, motion.Y / distance) };
            }
        }


        public override void Draw()
        {
            if (CheckVision())
            {
                DrawHp();
                base.Draw();
            }
        }

        public bool CheckVision()
        {
            foreach (var entity in Globals.AllFriendlyBuilder)
            {
                if ((entity.Position - Position).Length() < entity.Vision)
                    return true;
            }
            foreach (var entity in Globals.AllFriendlyEntity)
            {
                if ((entity.Position - Position).Length() < entity.Vision)
                    return true;
            }
            return false;
        }
    }
}
