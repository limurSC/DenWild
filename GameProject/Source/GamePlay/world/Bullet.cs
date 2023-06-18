using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using DenWild.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static System.Net.Mime.MediaTypeNames;

namespace GameProject.Source.GamePlay.world
{
    public record Bullet<T> : Basic2d where T : Entity
    {
        public bool CheckToKill;
        private T EndEntity;
        private Vector2 MotionVectors;
        private int Damage;
        public Bullet(int damage, T entity, string path, Vector2 position, Vector2 dims, float height)
            : base(path, position, dims, height) 
        {
            CheckToKill = false;
            EndEntity = entity;
            Damage = damage;
        }

        public override void Update()
        {
            var motion = new Vector2(EndEntity.Position.X - Position.X,
                EndEntity.Position.Y - Position.Y);
            var distance = motion.Length();
            MotionVectors = new Vector2(motion.X / distance, motion.Y / distance);
            Moving(40);
            ChangeRotation(MotionVectors);
            if(EndEntity.Position == Position)
            {
                CheckToKill = true;
                EndEntity.Hp -= Damage;
            }
            base.Update();
        }

        public void Moving(int entitySpeed)
        {
            if ((EndEntity.Position - Position).Length() >= entitySpeed)
            {
                Position += MotionVectors * entitySpeed;
            }
            else if ((EndEntity.Position - Position).Length() < entitySpeed)
            {
                Position = EndEntity.Position;
            }
        }

        public void ChangeRotation(Vector2 rotatVector)
        {
            if (rotatVector.Y > 0 && rotatVector.X > 0)
                this.Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X));
            else if (rotatVector.Y < 0 && rotatVector.X < 0)
                this.Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X));
            else if (rotatVector.Y > 0 && rotatVector.X < 0)
                this.Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X));
            else if (rotatVector.Y < 0 && rotatVector.X > 0)
                this.Rotation = (float)(Math.Atan(rotatVector.Y / rotatVector.X));
        }

        public override void Draw()
        {
            base.Draw();
        }

    }
}
