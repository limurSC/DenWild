using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Source.Engine
{
    public record Camera
    {
        // Construct a new Camera class with standard zoom (no scaling)
        public Camera()
        {
            Position = new Vector2(0, 0);
        }

        // Centered Position of the Camera in pixels.
        public Vector2 Position { get; private set; }
        // Current Rotation amount with 0.0f being standard orientation
        public float Rotation { get; private set; }

        // Height and width of the viewport window which we need to adjust
        // any time the player resizes the game window.
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        // Center of the Viewport which does not account for scale
        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(0, 0);
            }
        }

        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X,
                -(int)Position.Y, 0) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public void Translate(Vector2 newPos)
        {
            Position += newPos;
        }

        public void SetPosition(Vector2 newPos)
        {
            Position = newPos;
        }
    }
}
