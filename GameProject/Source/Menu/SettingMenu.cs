using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Source.Menu
{
    public record SettingMenu
    {
        SpriteFont Font;
        Basic2d SoundButton, MusicButton, ActiveMusicButton, ActiveSoundButton;
        Vector2 MousePosition;
        public SettingMenu()
        {
            Font = Globals.Content.Load<SpriteFont>("Fonts\\SettingMenuFont");
            ActiveSoundButton = new Basic2d("2d\\Hp", new Vector2(450, 305), 
                new Vector2(250 * SoundEffect.MasterVolume, 30), 0.9f);
            ActiveSoundButton.Color = Color.Blue;
            SoundButton = new Basic2d("2d\\Hp", new Vector2(575, 325), new Vector2(250, 30), 0.9f);
            SoundButton.Color = Color.LightBlue;
            ActiveMusicButton = new Basic2d("2d\\Hp", new Vector2(450, 405), 
                new Vector2(250 * MediaPlayer.Volume, 30), 0.9f);
            ActiveMusicButton.Color = Color.Blue;
            MusicButton = new Basic2d("2d\\Hp", new Vector2(575, 425), new Vector2(250, 30), 0.9f);
            MusicButton.Color = Color.LightBlue;
            MousePosition = new Vector2(0, 0);
        }

        public void Update()
        {
            ActiveSoundButton.StartDrawingPosition = new Vector2(0, 0);
            ActiveMusicButton.StartDrawingPosition = new Vector2(0, 0);
            var mousePosition = Globals.Control.MouseState.Position;
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (MousePosition.Length() == 0)
                    MousePosition = mousePosition.ToVector2();
                if (MousePosition.X > 450 && MousePosition.X < 700 &&
                    MousePosition.Y > 405 && MousePosition.Y < 440)
                {
                    MediaPlayer.Volume = Math.Max(0.0f, Math.Min(1.0f, 
                        (float)Math.Round((double)(mousePosition.X - 450) / 250, 3)));
                }
                if (MousePosition.X > 450 &&
                    MousePosition.X < 700 && MousePosition.Y > 305 && MousePosition.Y < 340)
                {
                    SoundEffect.MasterVolume = Math.Max(0.0f, Math.Min(1.0f, 
                        (float)Math.Round((double)(mousePosition.X - 450) / 250, 3)));
                }
            }
            else
                MousePosition = new Vector2(0, 0);
            ActiveMusicButton.Dims = new Vector2(250 * MediaPlayer.Volume, 30);
            ActiveSoundButton.Dims = new Vector2(250 * SoundEffect.MasterVolume, 30);
        }

        public void Draw()
        {
            SoundButton.Draw();
            MusicButton.Draw();
            ActiveSoundButton.Draw();
            ActiveMusicButton.Draw();
            Globals.SpriteBatch.DrawString(Font, "Звуки", new Vector2(450, 265), Color.White);
            Globals.SpriteBatch.DrawString(Font, "Музыка", new Vector2(450, 365), Color.White);
        }
    }
}
