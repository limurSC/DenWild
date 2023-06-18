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

namespace DenWild
{
    public record Basic2d
    {
        public Vector2 Position, Dims;
        public Texture2D MyModel;
        public string Path;
        public float Height, Rotation;
        public Color Color;
        public float Transparency;
        private bool CheckStartDrawingPosition;
        public Vector2 StartDrawingPosition;

        public Basic2d(string path, Vector2 position, Vector2 dims, float height)
        {
            Position = position;
            Dims = dims;
            Path = path;
            Height = height;
            Rotation = 0.0f;
            Color = Color.White;
            Transparency = 1.0f;
            CheckStartDrawingPosition = true;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            MyModel = Globals.Content.Load<Texture2D>(Path);
            if (CheckStartDrawingPosition)
            {
                CheckStartDrawingPosition = false;
                StartDrawingPosition = new Vector2(MyModel.Bounds.Width / 2, MyModel.Bounds.Height / 2);
            }
            if (MyModel != null)
                Globals.SpriteBatch.Draw(MyModel, new Rectangle((int)Position.X, (int)Position.Y, 
                    (int)Dims.X, (int)Dims.Y), null, Color, Rotation,
                    StartDrawingPosition, new SpriteEffects(), Height);
        }
    }
}
