using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using DenWild.World;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using System.Collections;
using System.Diagnostics;

namespace GameProject.Source.GamePlay
{
    public record Mission
    {
        private SpriteFont Font;
        private Basic2d MissionBax, CompletEnemyBox, CompletDroneBox;
        public Mission()
        {
            CompletEnemyBox = new Basic2d("2d\\Hp", new Vector2(15, 30), new Vector2(15, 15), 0.2f);
            CompletDroneBox = new Basic2d("2d\\Hp", new Vector2(15, 50), new Vector2(15, 15), 0.2f);
            MissionBax = new Basic2d("2d\\Hp", new Vector2(205, 55), new Vector2(400, 100), 0.21f);
            MissionBax.Color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
            Font = Globals.Content.Load<SpriteFont>("Fonts\\MissionFont");
        }

        public void Update()
        {
            CheckCompletBox();
        }

        public void CheckCompletBox()
        {
            if(Globals.AllEnemyEntity.Count == 0)
                CompletEnemyBox.Color = new Color(0.2f, 1, 0.2f, 0.2f);
            else
                CompletEnemyBox.Color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
            if (Globals.AllFriendlyEntity.Where(x => x.Name == "Drone").ToList().Count >= 5)
                CompletDroneBox.Color = new Color(0.2f, 1, 0.2f, 0.2f);
            else
                CompletDroneBox.Color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        }

        public void Draw()
        {
            MissionBax.Draw();
            CompletEnemyBox.Draw();
            Globals.SpriteBatch.DrawString(Font, $"Уничтожьте всех врагов - осталось({Globals.AllEnemyEntity.Count})",
                new Vector2(25, 20), Color.White);
            if(Globals.LevelName == "Education")
            {
                CompletDroneBox.Draw();
                Globals.SpriteBatch.DrawString(Font, $"Постройте 5 дронов(Drone)",
                    new Vector2(25, 40), Color.White);
            }
        }

    }
}
