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
using DenWild;

namespace GameProject.Source.Menu
{
    public record PlanetMenu
    {
        Basic2d MenuBackground, PlayButton, ExitButton, SettingsButton;
        public PlanetMenu()
        {
            MenuBackground = new Basic2d("2d\\Hp", new Vector2(960, 540), new Vector2(1720, 880), 0.9f);
            PlayButton = new Basic2d("2d\\Play", new Vector2(175, 225), new Vector2(250, 50), 0.9f);
            ExitButton = new Basic2d("2d\\Exit", new Vector2(175, 425), new Vector2(250, 50), 0.9f);
        }
    }
}
