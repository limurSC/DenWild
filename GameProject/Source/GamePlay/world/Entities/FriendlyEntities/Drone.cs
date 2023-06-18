using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DenWild.Source.Engine;
using DenWild.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Source.GamePlay.world.Entities.FriendlyEntities
{
    public record Drone : FriendlyEntity
    {
        public Drone(string path, Vector2 position,
            Vector2 dims, float height) : base(path, position, dims, height)
        {
            AttackRange = 300;
            Damage = 200;
            EntitySpeed = 3;
            Hp = 1000;
            MaxHp = 1000;
            CreateTime = 60;
            Name = "Drone";
            Vision = 500;
            permissionToAttack = true;
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
        }
        public override void Update()
        {
            base.Update();
        }


        public override void Draw()
        {
            base.Draw();
        }
    }
}
