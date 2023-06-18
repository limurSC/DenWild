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


namespace GameProject.Source.GamePlay.world.Entities
{
    public record FriendlyBuilding : Entity
    {
        public bool CheckCoompletBuild;
        public int Price;
        public int Vision;
        public FriendlyBuilding(string path, Vector2 position,
            Vector2 dims, float height) : base(path, position, dims, height)
        {
            Team = "Team 1";
            permissionToAttack = true;
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
        }
        public override void Update()
        {
            if (Hp <= 0)
                CheckToKill = true;
            if (!CheckCoompletBuild)
                BuildBuilding();
            base.Update();
        }

        public void BuildBuilding()
        {
            if (Transparency >= 1.0f)
                CheckCoompletBuild = true;
            else
            {
                Hp += MaxHp / 500;
                Transparency += 0.002f;
                Color = new Color(Transparency, Transparency, Transparency, Transparency);
            }
        }


        public override void Draw()
        {
            DrawHp();
            base.Draw();
        }
    }
}
