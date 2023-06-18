﻿using System;
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
    public record NeutralEntity : Entity
    {
        public NeutralEntity(string path, Vector2 position, Vector2 dims, float height) :
            base(path, position, dims, height)
        {
            Team = "Neutral Team";
            Name = "Crystal";
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
