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
    public record Menu
    {
        public MainMenu MainMenu;
        public WinMenu WinMenu;
        public GameMenu GameMenu;
        public LevelsMenu LevelsMenu;
        public SettingMenu SettingMenu;
        public static MenuState MenuState;

        public Menu()
        {
            MenuState = new MenuState();
            GameMenu = new GameMenu(); 
            MainMenu = new MainMenu();
            WinMenu = new WinMenu();
            LevelsMenu = new LevelsMenu();
            SettingMenu = new SettingMenu();
        }

        public void Update()
        {
            if (!MenuState.CheckStartGame)
            {
                if (MenuState.CheckMainMenu)
                    MainMenu.Update();
                else if (MenuState.CheckLevelsMenu)
                    LevelsMenu.Update();
                if (MenuState.CheckSettingMenu)
                    SettingMenu.Update();
            }
            else
            {
                if (Globals.Control.CheckOneLeftClick() && !MenuState.CheckGameMenu && !MenuState.CheckWinGame &&
                    Mouse.GetState().Position.X > 1840 && Mouse.GetState().Position.Y > 800 && Mouse.GetState().Position.Y < 840)
                {
                    MenuState.CheckGameMenu = true;
                    Globals.Audio.PlayAudio();
                }
                if (MenuState.CheckGameMenu)
                    GameMenu.Update();
                else if (MenuState.CheckWinGame)
                    WinMenu.Update();
            }
        }

        public void Draw()
        {
            if (MenuState.CheckGameMenu)
                GameMenu.Draw();
            if (MenuState.CheckWinGame)
                WinMenu.Draw();
            if (MenuState.CheckMainMenu)
                MainMenu.Draw();
            if (MenuState.CheckLevelsMenu)
                LevelsMenu.Draw();
            if (MenuState.CheckSettingMenu)
                SettingMenu.Draw();
        }
    }
}
