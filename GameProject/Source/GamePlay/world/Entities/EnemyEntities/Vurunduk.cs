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

namespace GameProject.Source.GamePlay.world.Entities.EnemyEntities
{
    public record Vurunduk : EnemyEntity
    {
        public Vurunduk(string path, Vector2 position,
            Vector2 dims, float height, int[] movingPlace) : base(path, position, dims, height, movingPlace)
        {
            AttackRange = 300;
            Damage = 300;
            EntitySpeed = 3;
            Hp = 2000;
            MaxHp = 2000;
            Name = "Vurunduk";
        }
        public override void Update()
        {
            MovingEnemy();
            base.Update();
        }


        public override void Draw()
        {
            base.Draw();
        }
    }
}
