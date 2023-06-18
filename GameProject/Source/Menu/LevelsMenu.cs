using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using DenWild.Source.GamePlay;
using GameProject.Source.GamePlay;
using GameProject.Source.GamePlay.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Source.Menu
{
    public record LevelsMenu
    {
        Basic2d MapBackground, EducationButton, PlanetaAButton, ExitButton;
        public LevelsMenu()
        {
            MapBackground = new Basic2d("2d\\LevelsMenu", new Vector2(960, 540), new Vector2(1920, 1080), 0.9f);
            EducationButton = new Basic2d("2d\\Education", new Vector2(960, 225), new Vector2(250, 50), 0.9f);
            PlanetaAButton = new Basic2d("2d\\PlanetaA", new Vector2(1280, 700), new Vector2(250, 50), 0.9f);
            ExitButton = new Basic2d("2d\\ExitToMenu", new Vector2(1750, 1035), new Vector2(300, 50), 0.1f);
        }
        public virtual void Update()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 835 && mouseState.Position.X < 1085 &&
                mouseState.Position.Y > 200 && mouseState.Position.Y < 250)
            {
                StartGame();
                Globals.World.LevelName = "Education";
            }
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1155 && mouseState.Position.X < 1405 &&
                mouseState.Position.Y > 675 && mouseState.Position.Y < 725)
            {
                StartGame();
                Globals.World.LevelName = "1 level";
            }
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1600 && mouseState.Position.X < 1900 &&
                mouseState.Position.Y > 1010 && mouseState.Position.Y < 1060)
            {
                Globals.Audio.PlayAudio();
                MenuState.CheckMainMenu = true;
                MenuState.CheckLevelsMenu = false;
            }
            CheckButton();
        }

        public void StartGame()
        {
            Globals.Audio.PlayAudio();
            MenuState.CheckStartGame = true;
            MenuState.CheckLevelsMenu = false;
            Globals.Control.Binds = new Binds();
            Globals.ChoiceEntity = new ChoiceObject();
            Globals.Camera.SetPosition(new Vector2(0, 0));
        }

        public void CheckButton()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.Position.X > 835 && mouseState.Position.X < 1085 &&
                mouseState.Position.Y > 200 && mouseState.Position.Y < 250)
                EducationButton.Path = "2d\\ActiveEducation";
            else
                EducationButton.Path = "2d\\Education";
            if (mouseState.Position.X > 1155 && mouseState.Position.X < 1405 &&
                mouseState.Position.Y > 675 && mouseState.Position.Y < 725)
                PlanetaAButton.Path = "2d\\ActivePlanetaA";
            else
                PlanetaAButton.Path = "2d\\PlanetaA";
            if (mouseState.Position.X > 1600 && mouseState.Position.X < 1900 &&
                mouseState.Position.Y > 1010 && mouseState.Position.Y < 1060)
                ExitButton.Path = "2d\\ActiveExitToMenu";
            else
                ExitButton.Path = "2d\\ExitToMenu";
        }

        public virtual void Draw()
        {
            MapBackground.Draw();
            EducationButton.Draw();
            PlanetaAButton.Draw();
            ExitButton.Draw();
        }
    }
}
