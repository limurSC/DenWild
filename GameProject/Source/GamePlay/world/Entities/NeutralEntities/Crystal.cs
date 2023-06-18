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

namespace GameProject.Source.GamePlay.world.Entities.NeutralEntities
{
    internal record Crystal : NeutralEntity
    {
        public int PragmanitCount;
        public Crystal(int pragmanitCount, string path, Vector2 position, Vector2 dims, float height) :
            base(path, position, dims, height)
        {
            PragmanitCount = pragmanitCount;
            Name = "Crystal";
        }
        public override void Update()
        {
            foreach(var entity in Globals.AllFriendlyBuilder)
            {
                if (entity.Name == "Miner" && entity.Position == Position &&
                    entity.CheckCoompletBuild)
                {
                    entity.Rotation = (entity.Rotation + 0.025f) % 360;
                    PragmanitCount--;
                    Globals.Pragmanit++;
                    if (PragmanitCount <= 0)
                        CheckToKill = true;
                }
            }
            base.Update();
        }


        public override void Draw()
        {
            base.Draw();
        }
    }
}
