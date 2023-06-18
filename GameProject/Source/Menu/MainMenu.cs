using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using GameProject.Source.GamePlay.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Source.Menu
{
    public record MainMenu
    {
        Basic2d MenuBackground, PlayButton, ExitButton, SettingsButton;
        public MainMenu()
        {
            MenuBackground = new Basic2d("2d\\MainMenuBackground", new Vector2(960, 540), new Vector2(1920, 1080), 0.9f);
            PlayButton = new Basic2d("2d\\Play", new Vector2(175, 225), new Vector2(250, 50), 0.9f);
            SettingsButton = new Basic2d("2d\\Settings", new Vector2(175, 325), new Vector2(250, 50), 0.9f);
            ExitButton = new Basic2d("2d\\Exit", new Vector2(175, 425), new Vector2(250, 50), 0.9f);
        }

        public virtual void Update()
        {
            var mouseState = Mouse.GetState();
            if(Globals.Control.CheckOneLeftClick() && 
                mouseState.Position.X > 50 && mouseState.Position.X < 300 &&
                mouseState.Position.Y > 200 && mouseState.Position.Y < 250)
            {
                Globals.Audio.PlayAudio();
                MenuState.CheckLevelsMenu = true;
                MenuState.CheckMainMenu = false;
                MenuState.CheckSettingMenu = false;
            }
            if (!MenuState.CheckStartGame && Globals.Control.CheckOneLeftClick() &&
                mouseState.Position.X > 50 && mouseState.Position.X < 300 &&
                mouseState.Position.Y > 300 && mouseState.Position.Y < 350)
            {
                Globals.Audio.PlayAudio();
                if (MenuState.CheckSettingMenu)
                    MenuState.CheckSettingMenu = false;
                else
                    MenuState.CheckSettingMenu = true;
            }
            if (!MenuState.CheckStartGame && Globals.Control.CheckOneLeftClick() &&
                mouseState.Position.X > 50 && mouseState.Position.X < 300 &&
                mouseState.Position.Y > 400 && mouseState.Position.Y < 450)
            {
                Globals.Audio.PlayAudio();
                MenuState.CheckExitGame = true;
            }
            CheckButton();
        }

        public void CheckButton()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.Position.X > 50 && mouseState.Position.X < 300 &&
                mouseState.Position.Y > 200 && mouseState.Position.Y < 250)
                PlayButton.Path = "2d\\ActivePlay";
            else
                PlayButton.Path = "2d\\Play";
            if (mouseState.Position.X > 50 && mouseState.Position.X < 300 &&
                mouseState.Position.Y > 300 && mouseState.Position.Y < 350)
                SettingsButton.Path = "2d\\ActiveSettings";
            else
                SettingsButton.Path = "2d\\Settings";
            if (mouseState.Position.X > 50 && mouseState.Position.X < 300 &&
                mouseState.Position.Y > 400 && mouseState.Position.Y < 450)
                ExitButton.Path = "2d\\ActiveExit";
            else
                ExitButton.Path = "2d\\Exit";
        }

        public virtual void Draw()
        {
            MenuBackground.Draw();
            PlayButton.Draw();
            SettingsButton.Draw();
            ExitButton.Draw();
        }
    }
}
