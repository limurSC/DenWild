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
    public record FriendlyEntity : Entity
    {
        public int PragmanitCount;
        public int CreateTime;
        public int Vision;
        public FriendlyEntity(string path, Vector2 position,
            Vector2 dims, float height) : base(path, position, dims, height)
        {
            Team = "Team 1";
            permissionToAttack = true;
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
        }

        public override void Update()
        {
            if ((!Attack(Damage, AttackRange) && !permissionToAttack) ||
                Attack(Damage, AttackRange))
                Moving(EntitySpeed);
            if (Hp <= 0)
                CheckToKill = true;
            base.Update();
        }


        public override void Draw()
        {
            DrawHp();
            base.Draw();
        }
    }
}
