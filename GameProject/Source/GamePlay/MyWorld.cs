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
using GameProject.Source.Menu;

namespace DenWild.Source.GamePlay
{
    public record MyWorld
    {
        public Basic2d MapBackground;
        List<Basic2d> FogOfWar;
        public string LevelName;

        public MyWorld()
        {
            MapBackground = new Basic2d("2d\\MapBackground", new Vector2(1250, 1250), new Vector2(2500, 2500), 1);
            LevelName = "";
        }

        public virtual void Update()
        {
            if(LevelName != "")
                AddStartEntity(LevelName);
            Globals.GameTime++;
            UpdateEntity();
            if ((Globals.AllEnemyEntity.Count == 0 || (Globals.AllFriendlyEntity.Count == 0 && 
                Globals.AllFriendlyBuilder.Where(x => x.Name == "Base").ToList().Count == 0)
                 ) && !MenuState.CheckBackToGame && MenuState.CheckStartGame)
                MenuState.CheckWinGame = true;
        }

        public void AddStartEntity(string levelName)
        {
            if (levelName == "1 level")
            {
                MenuState.CheckEducation = false;
                AddStartFirstLevel();
            }
            if (levelName == "Education")
            {
                MenuState.CheckEducation = true;
                AddStartEducationLevel();
            }
            Globals.LevelName = levelName;
            LevelName = "";
        }

        public void AddStartFirstLevel()
        {
            MapBackground = new Basic2d("2d\\MapBackground", new Vector2(2500, 2500), new Vector2(5000, 5000), 1);
            for (int i = 0; i < 10; i++)
            {
                var vurunduk = new Vurunduk("2d\\Vurunduk", new Vector2(2900 + 200 * (i + 1), 2900),
                        new Vector2(59, 59), 0.5f, new int[4] {2000, 2000, 4500, 4500});
                Globals.AllEnemyEntity.Add(vurunduk);
                Globals.AllEntity.Add(vurunduk);
            }
            var base1 = new Base("2d\\Base 1", new Vector2(1000, 500),
                    new Vector2(500, 500), 0.5f);
            Globals.AllFriendlyBuilder.Add(base1);
            Globals.AllEntity.Add(base1);
            var miner = new Miner("2d\\Miner", new Vector2(300, 300),
                    new Vector2(200, 200), 0.5f);
            Globals.AllFriendlyBuilder.Add(miner);
            Globals.AllEntity.Add(miner);
            var crystal1 = new Crystal(50000, "2d\\Crystal", new Vector2(300, 300),
                    new Vector2(100, 100), 0.51f);
            Globals.AllNeutralEntity.Add(crystal1);
            Globals.AllEntity.Add(crystal1);
            var crystal2 = new Crystal(50000, "2d\\Crystal", new Vector2(4500, 500),
                    new Vector2(100, 100), 0.51f);
            Globals.AllNeutralEntity.Add(crystal2);
            Globals.AllEntity.Add(crystal2);
            var crystal3 = new Crystal(50000, "2d\\Crystal", new Vector2(500, 4000),
                    new Vector2(100, 100), 0.51f);
            Globals.AllNeutralEntity.Add(crystal3);
            Globals.AllEntity.Add(crystal3);
            AddFogOfWar(5000);
        }

        public void AddStartEducationLevel()
        {
            MapBackground = new Basic2d("2d\\MapBackground2", new Vector2(1250, 1250), new Vector2(2500, 2500), 1);
            Globals.Pragmanit = 10000;
            var vurunduk = new Vurunduk("2d\\Vurunduk", new Vector2(1500, 1500),
                    new Vector2(59, 59), 0.5f, new int[4] { 1800, 1800, 2000, 2000 });
            Globals.AllEnemyEntity.Add(vurunduk);
            Globals.AllEntity.Add(vurunduk);
            var builder = new Builder("2d\\Builder", new Vector2(1000, 500),
                    new Vector2(40, 40), 0.5f);
            Globals.AllFriendlyEntity.Add(builder);
            Globals.AllEntity.Add(builder);
            var miner = new Miner("2d\\Miner", new Vector2(300, 300),
                    new Vector2(200, 200), 0.5f);
            Globals.AllFriendlyBuilder.Add(miner);
            Globals.AllEntity.Add(miner);
            var crystal1 = new Crystal(50000, "2d\\Crystal", new Vector2(300, 300),
                    new Vector2(100, 100), 0.51f);
            Globals.AllNeutralEntity.Add(crystal1);
            Globals.AllEntity.Add(crystal1);
            var crystal2 = new Crystal(50000, "2d\\Crystal", new Vector2(300, 600),
                    new Vector2(100, 100), 0.51f);
            Globals.AllNeutralEntity.Add(crystal2);
            Globals.AllEntity.Add(crystal2);
            AddFogOfWar(10000);
        }

        public void AddFogOfWar(int mapLength)
        {
            FogOfWar = new List<Basic2d>();
            for (var i = 0; i <= mapLength; i += 100)
                for (var j = 0; j <= mapLength; j += 100)
                {
                    var a = new Basic2d("2d\\Hp", new Vector2(i, j), new Vector2(100, 100), 0.4f);
                    a.Color = new Color(0, 0, 0, 0.4f);
                    FogOfWar.Add(a);
                }
        }

        public void UpdateEntity()
        {
            for (int i = 0; i < Globals.AllFriendlyEntity.Count; i++)
            {
                Globals.AllFriendlyEntity[i].Update();
            }
            for (int i = 0; i < Globals.AllEnemyEntity.Count; i++)
            {
                Globals.AllEnemyEntity[i].Update();
            }
            for (int i = 0; i < Globals.AllNeutralEntity.Count; i++)
            {
                Globals.AllNeutralEntity[i].Update();
            }
            for (int i = 0; i < Globals.AllFriendlyBuilder.Count; i++)
            {
                Globals.AllFriendlyBuilder[i].Update();
            }
            for (int i = 0; i < Globals.Bullets.Count; i++)
            {
                Globals.Bullets[i].Update();
            }
            KillEtity();
        }
        
        public void KillEtity()
        {
            Globals.AllFriendlyEntity.RemoveAll(x => x.CheckToKill == true);
            Globals.AllEnemyEntity.RemoveAll(x => x.CheckToKill == true);
            Globals.ChoiceEntity.selectedEntity.RemoveAll(x => x.CheckToKill == true);
            Globals.AllNeutralEntity.RemoveAll(x => x.CheckToKill == true);
            Globals.AllEntity.RemoveAll(x => x.CheckToKill == true);
            Globals.Bullets.RemoveAll(x => x.CheckToKill == true);
            Globals.Control.Binds.ListBinds = Globals.Control.Binds.ListBinds
                .Select(x =>
                {
                    x.RemoveAll(y => y.CheckToKill == true);
                    return x;
                }).ToList();
        }


        public virtual void Draw()
        {
            MapBackground.Draw();
            DrawEntity();
            DrawClicks();
            DrawFogOfWar();
        }

        public void DrawFogOfWar()
        {
            foreach (var fog in FogOfWar)
            {
                var check = true;
                foreach (var entity in Globals.AllFriendlyBuilder)
                    if ((fog.Position - entity.Position).Length() < entity.Vision)
                        check = false;
                foreach (var entity in Globals.AllFriendlyEntity)
                    if ((fog.Position - entity.Position).Length() < entity.Vision)
                        check = false;

                if (check)
                    fog.Draw();
            }
        }

        public void DrawClicks()
        {
            var removeClicks = new List<Basic2d>{ };
            for(var i = 0; i < Globals.Control.Clicks.Count; i++)
            {
                Globals.Control.Clicks[i].Dims *= 0.9f;
                if (Globals.Control.Clicks[i].Dims.Length() < 2)
                    removeClicks.Add(Globals.Control.Clicks[i]);
                Globals.Control.Clicks[i].Draw();
            }
            foreach (var click in removeClicks)
                Globals.Control.Clicks.Remove(click);
        }

        public void DrawEntity()
        {
            for (int i = 0; i < Globals.AllFriendlyEntity.Count; i++)
            {
                Globals.AllFriendlyEntity[i].Draw();
            }
            for (int i = 0; i < Globals.AllEnemyEntity.Count; i++)
            {
                Globals.AllEnemyEntity[i].Draw();
            }
            for (int i = 0; i < Globals.AllNeutralEntity.Count; i++)
            {
                Globals.AllNeutralEntity[i].Draw();
            }
            for (int i = 0; i < Globals.AllFriendlyBuilder.Count; i++)
            {
                Globals.AllFriendlyBuilder[i].Draw();
            }
            for (int i = 0; i < Globals.Bullets.Count; i++)
            {
                Globals.Bullets[i].Draw();
            }
            Globals.ChoiceEntity.Draw();
        }
    }
}
