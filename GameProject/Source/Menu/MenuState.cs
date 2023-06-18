using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DenWild.Source.Engine;
using DenWild.World;
using GameProject.Source.GamePlay.world.Entities;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world;

namespace GameProject.Source.Menu
{
    public record MenuState
    {
        public static bool CheckWinGame;
        public static bool CheckGameMenu;
        public static bool CheckMainMenu;
        public static bool CheckStartGame;
        public static bool CheckExitGame;
        public static bool CheckLevelsMenu;
        public static bool CheckSettingMenu;
        public static bool CheckEducation;
        public static bool CheckBackToGame;

        public MenuState()
        {
            CheckStartGame = false;
            CheckExitGame = false;
            CheckWinGame = false;
            CheckGameMenu = false;
            CheckMainMenu = true;
            CheckLevelsMenu = false;
            CheckSettingMenu = false;
            CheckBackToGame = false;
        }

    }
}
