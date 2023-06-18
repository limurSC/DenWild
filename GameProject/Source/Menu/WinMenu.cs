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
    public record WinMenu
    {
        Basic2d PlayButton, ExitButton, WinButton, LoseButton, MenuBackground;
        public WinMenu()
        {
            MenuBackground = new Basic2d("2d\\Hp", new Vector2(980, 525), new Vector2(275, 275), 0.21f);
            MenuBackground.Color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
            PlayButton = new Basic2d("2d\\BackToGame", new Vector2(980, 525), new Vector2(250, 50), 0.1f);
            ExitButton = new Basic2d("2d\\ExitToMenu", new Vector2(980, 625), new Vector2(250, 50), 0.1f);
            WinButton = new Basic2d("2d\\Win", new Vector2(980, 425), new Vector2(250, 50), 0.1f);
            LoseButton = new Basic2d("2d\\Lose", new Vector2(980, 425), new Vector2(250, 50), 0.1f);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (MenuState.CheckStartGame && Globals.Control.CheckOneLeftClick() &&
                mouseState.Position.X > 855 && mouseState.Position.X < 1105 &&
                mouseState.Position.Y > 600 && mouseState.Position.Y < 650)
            {
                Globals.Audio.PlayAudio();
                MenuState.CheckStartGame = false;
                MenuState.CheckWinGame = false;
                MenuState.CheckMainMenu = true;
                KillAllEntity();
            }
            if (Globals.Control.CheckOneLeftClick() &&
                mouseState.Position.X > 855 && mouseState.Position.X < 1105 &&
                mouseState.Position.Y > 500 && mouseState.Position.Y < 550)
            {
                Globals.Audio.PlayAudio();
                MenuState.CheckWinGame = false;
                MenuState.CheckBackToGame = true;
            }
            CheckButton();
        }

        public void KillAllEntity()
        {
            Globals.AllEntity.Clear();
            Globals.AllEnemyEntity.Clear();
            Globals.AllFriendlyBuilder.Clear();
            Globals.AllFriendlyEntity.Clear();
            Globals.AllNeutralEntity.Clear();
            Globals.Bullets.Clear();
        }

        public void CheckButton()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.Position.X > 855 && mouseState.Position.X < 1105 &&
                mouseState.Position.Y > 500 && mouseState.Position.Y < 550)
                PlayButton.Path = "2d\\ActiveBackToGame";
            else
                PlayButton.Path = "2d\\BackToGame";
            if (mouseState.Position.X > 855 && mouseState.Position.X < 1105 &&
                mouseState.Position.Y > 600 && mouseState.Position.Y < 650)
                ExitButton.Path = "2d\\ActiveExitToMenu";
            else
                ExitButton.Path = "2d\\ExitToMenu";
        }

        public virtual void Draw()
        {
            PlayButton.Draw();
            ExitButton.Draw();
            MenuBackground.Draw();
            if (Globals.AllEnemyEntity.Count == 0)
                WinButton.Draw();
            else
                LoseButton.Draw();
        }
    }
}
