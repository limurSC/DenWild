using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Net.Mime.MediaTypeNames;

namespace GameProject.Source.GamePlay.world.Entities.FriendlyBuilders
{
    internal record Miner : FriendlyBuilding
    {
        public Miner(string path, Vector2 position, Vector2 dims, float height) :
            base( path, position, dims, height)
        {
            Hp = 2000;
            MaxHp = 2000;
            Price = 2000;
            Name = "Miner";
            Vision = 400;
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
