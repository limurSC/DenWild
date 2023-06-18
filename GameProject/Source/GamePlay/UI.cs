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
using GameProject.Source.Menu;

namespace GameProject.Source.GamePlay
{
    public record UI
    {
        private Basic2d UnitInterface; //лучше сделать словар(название интерфейса : basic2d)
        private SpriteFont Font, TimeFont, BindFont, PriceFont;
        private readonly Vector2 BuilderInterfacePosition = new Vector2(1716, 957);
        private readonly Vector2 BuilderInterfaceSize = new Vector2(349, 209);
        private readonly Vector2 WarriorInterfacePosition = new Vector2(1716, 957);
        private readonly Vector2 WarriorInterfaceSize = new Vector2(349, 209);
        private readonly Vector2 MoveButtonPosition = new Vector2(1577, 888);
        private readonly Vector2 MoveButtonSize = new Vector2(71, 70);
        private readonly Vector2 AttackButtonPosition = new Vector2(1854, 888);
        private readonly Vector2 AttackButtonSize = new Vector2(71, 70);
        private readonly Vector2 SetAsidePosition = new Vector2(1646, 888);
        private readonly Vector2 SetAsideSize = new Vector2(69, 69);
        private readonly Vector2 DroneButtonPosition = new Vector2(1647, 888);
        private readonly Vector2 DroneButtonSize = new Vector2(69, 68);
        private readonly Vector2 BuildingButtonPosition = new Vector2(1577, 888);
        private readonly Vector2 BuildingButtonSize = new Vector2(69, 68);
        private readonly Vector2 DronePricePosition = new Vector2(1623, 903);
        private readonly Vector2 BuilderPricePosition = new Vector2(1550, 903);
        private readonly Vector2 BasePricePosition = new Vector2(1550, 903);
        private readonly Vector2 MinerPricePosition = new Vector2(1623, 903);
        private Map Map;
        private Vector2 InterfacePosition;
        private Education Education;
        private Mission Mission;

        public UI()
        {
            InterfacePosition = new Vector2(960, 824);
            UnitInterface = new Basic2d("2d\\Interface", InterfacePosition,
                    new Vector2(1920, 512), 0.4f);
            Font = Globals.Content.Load<SpriteFont>("Fonts\\GoldFont");
            TimeFont = Globals.Content.Load<SpriteFont>("Fonts\\TimeFont");
            BindFont = Globals.Content.Load<SpriteFont>("Fonts\\BindFont");
            PriceFont = Globals.Content.Load<SpriteFont>("Fonts\\PriceFont");
            Map = new Map();
            Education = new Education();
            Mission = new Mission();
        }

        public virtual void Update()
        {
            UnitInterface.Position = InterfacePosition;
            Map.Update();
            Education.Update();
            Mission.Update();
        }

        public virtual void Draw()
        {
            if (MenuState.CheckEducation)
                Education.Draw();
            DrawSelectedEntity();
            Globals.SpriteBatch.DrawString(Font, Globals.Pragmanit.ToString(),
                new Vector2(960, 20), new Color(0, 255, 255));
            UnitInterface.Draw();
            Map.Draw();
            DrawInterface();
            DrawGameTime();
            DrawBaseBuildEntityUI();
            DrawBaseNumberEntityUI();
            DrawActiveBind();
            DrawBinds();
            DrawActiveGameMenu();
            DrawActiveF2Button();
            DrawActiveF1Button();
            DrawUnitStats();
            Mission.Draw();
        }

        public void DrawGameTime()
        {
            var gameTime = (Globals.GameTime / 3600).ToString() + ':' + (Globals.GameTime / 60 % 60).ToString();
            Globals.SpriteBatch.DrawString(TimeFont, gameTime,
                new Vector2(287, 785), Color.Wheat);
        }

        private void DrawSelectedEntity()
        {
            if (Globals.ChoiceEntity.selectedEntity.Count == 1)
            {
                var entity = Globals.ChoiceEntity.selectedEntity[0];
                var myModel = entity.MyModel;
                Globals.SpriteBatch.DrawString(Font, entity.Name,
                    new Vector2(860, 900), Color.Wheat);
                Globals.SpriteBatch.Draw(myModel,
                    new Rectangle(500, 900,
                    120, 120), null, Color.White, 0.0f,
                    new Vector2(0, 0), new SpriteEffects(), 0);
            }
            else
            {
                int i = 0;
                foreach (var entity in Globals.ChoiceEntity.selectedEntity)
                {
                    var myModel = entity.MyModel;
                    Globals.SpriteBatch.Draw(myModel,
                        new Rectangle(500 + 60 * (i % 12), 900 + 60 * (i / 12),
                        60, 60), null, Color.White, 0.0f,
                        new Vector2(0, 0), new SpriteEffects(), 0);
                    i++;
                }
            }
        }

        private void DrawInterface()
        {
            if (Globals.ChoiceEntity.selectedEntity.Count != 0)
            {
                if (Globals.ChoiceEntity.selectedEntity[0].Name == "Builder" && Globals.Control.CheckBuild)
                {
                    new Basic2d("2d\\BuildInterface", WarriorInterfacePosition
                        , WarriorInterfaceSize, 0.28f).Draw();
                    DrawPrice(5000, BasePricePosition);
                    DrawPrice(2000, MinerPricePosition);
                }
                else if (Globals.ChoiceEntity.selectedEntity[0].Name == "Builder")
                {
                    new Basic2d("2d\\BuilderInterface", BuilderInterfacePosition
                        , BuilderInterfaceSize, 0.28f).Draw();
                    DrawActiveEntityUI();
                }
                else if (Globals.ChoiceEntity.selectedEntity[0].Name == "Drone")
                {
                    new Basic2d("2d\\WarriorInterface", WarriorInterfacePosition
                        , WarriorInterfaceSize, 0.28f).Draw();
                    DrawActiveEntityUI();
                }
                else if (Globals.ChoiceEntity.selectedEntity[0].Name == "Base")
                {
                    new Basic2d("2d\\BuildingInterface", WarriorInterfacePosition
                        , WarriorInterfaceSize, 0.28f).Draw();
                    DrowBuildingUI();
                }
            }
        }

        public void DrowBuildingUI()
        {
            var keyboardState = Keyboard.GetState().GetPressedKeys();
            var mouseState = Mouse.GetState();
            if (keyboardState.Contains(Keys.B) || mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1540 && mouseState.Position.X < 1611 &&
                mouseState.Position.Y > 853 && mouseState.Position.Y < 923)
            {
                DrawButton("2d\\ActiveBuilderButton", BuildingButtonPosition, BuildingButtonSize);
            }
            if (keyboardState.Contains(Keys.D) || mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1603 && mouseState.Position.X < 1682 &&
                mouseState.Position.Y > 853 && mouseState.Position.Y < 923)
            {
                DrawButton("2d\\ActiveDroneButton", DroneButtonPosition, DroneButtonSize);
            }
            DrawPrice(1000, BuilderPricePosition);
            DrawPrice(1000, DronePricePosition);
        }

        public void DrawPrice(int price, Vector2 pricePosition)
        {
            if(price < Globals.Pragmanit)
                Globals.SpriteBatch.DrawString(PriceFont, price.ToString(), pricePosition, Color.Green);
            else
                Globals.SpriteBatch.DrawString(PriceFont, price.ToString(), pricePosition, Color.Red);
        }

        public static void DrawButton(string pathToButton, Vector2 buttonPosition, Vector2 buttonSize)
        {
            new Basic2d(pathToButton, buttonPosition
                    , buttonSize, 0.27f).Draw();
        }

        public void DrawActiveEntityUI()
        {
            var keyboardState = Keyboard.GetState().GetPressedKeys();
            var mouseState = Mouse.GetState();
            var entity = Globals.ChoiceEntity.selectedEntity[0];
            if (entity.MotionVectors.Count != 0 || mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1540 && mouseState.Position.X < 1611 &&
                mouseState.Position.Y > 853 && mouseState.Position.Y < 923)
            {
                DrawButton("2d\\ActiveMoveButton", MoveButtonPosition, MoveButtonSize);
            }
            if (keyboardState.Contains(Keys.A) || mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1818 && mouseState.Position.X < 1889 &&
                mouseState.Position.Y > 853 && mouseState.Position.Y < 923)
            {
                DrawButton("2d\\ActiveAttackButton", AttackButtonPosition, AttackButtonSize);
            }
            if (entity.MotionVectors.Count == 0 || mouseState.LeftButton == ButtonState.Pressed &&
                mouseState.Position.X > 1603 && mouseState.Position.X < 1682 &&
                mouseState.Position.Y > 853 && mouseState.Position.Y < 923)
            {
                DrawButton("2d\\ActiveSetAside", SetAsidePosition, SetAsideSize);
            }
        }

        public void DrawBaseBuildEntityUI()
        {
            if (Globals.ChoiceEntity.selectedEntity.Count == 1 &&
                Globals.ChoiceEntity.selectedEntity[0].Name == "Base")
            {
                var queue = ((Base)Globals.ChoiceEntity.selectedEntity[0]);
                var myModel = "2d\\Hp";
                if (queue.QueueCreateEntity.Count > 0)
                {
                    double createProgress = (double)queue.CreateProgress / queue.QueueCreateEntity[0].CreateTime;
                    Globals.SpriteBatch.Draw(Globals.Content.Load<Texture2D>(myModel),
                        new Rectangle(720, 1020, (int)(60 * createProgress), 2), null, Color.White, 0,
                        new Vector2(0, 0), new SpriteEffects(), 0.27f);
                }
                for (var i = 0; i < queue.QueueCreateEntity.Count; i++)
                {
                    myModel = "2d\\Build" + queue.QueueCreateEntity[i].Name + "Button";
                    DrawButton(myModel, new Vector2(750 + 70 * i, 980), new Vector2(68, 69));
                }
                for (var i = 0; i < 5 - queue.QueueCreateEntity.Count; i++)
                {
                    myModel = "2d\\Button";
                    DrawButton(myModel, new Vector2(1030 - 70 * i, 980), new Vector2(68, 69));
                }
            }
        }

        public void DrawBaseNumberEntityUI()
        {
            if (Globals.ChoiceEntity.selectedEntity.Count == 1 &&
                    Globals.ChoiceEntity.selectedEntity[0].Name == "Base")
            {
                for (var i = 0; i < 5; i++)
                {
                    Globals.SpriteBatch.DrawString(Font, (i + 1).ToString(), new Vector2(725 + 70 * i, 955), 
                        Color.White);
                }
            }
        }

        public void DrawBinds()
        {
            for (var i = 0; i < Globals.Control.Binds.ListBinds.Count; i++)
            {
                DrawBind(Globals.Control.Binds.ListBinds[i], i);
            }
        }

        public void DrawBind(List<Entity> entities, int bindNumber)
        {
            if(entities.Count != 0)
            {
                Globals.SpriteBatch.DrawString(BindFont, (entities.Count).ToString(),
                    new Vector2(540 + 68 * bindNumber, 840), Color.White);
                Globals.SpriteBatch.Draw(entities[0].MyModel,
                        new Rectangle(560 + 68 * bindNumber, 835, 24, 24), 
                        null, Color.White, 0,
                        new Vector2(0, 0), new SpriteEffects(), 0.26f);
            }
        }
        
        public void DrawActiveBind()
        {
            var mousePosition = Globals.Control.MouseState.Position;
            for (var i = 0; i < 10; i++)
                if (mousePosition.X > 525 + 68 * i &&
                    mousePosition.X < 590 + 68 * i && mousePosition.Y > 833 && mousePosition.Y < 863)
                    DrawButton("2d\\ActiveBindButton", new Vector2(557 + 68 * i, 848), new Vector2(65, 30));
        }

        public void DrawActiveGameMenu()
        {
            var mousePosition = Globals.Control.MouseState.Position;
            if (mousePosition.X > 1848 && mousePosition.X < 1911 &&
                mousePosition.Y > 803 && mousePosition.Y < 836)
                DrawButton("2d\\ActiveGameMenu", new Vector2(1879, 819), new Vector2(63, 33));
        }

        public void DrawActiveF2Button()
        {
            var ArmyCount = Globals.AllFriendlyEntity.Where(x => x.Name != "Builder").Count();
            if (Globals.AllFriendlyEntity.Where(x => x.Name != "Builder").Count() > 0)
            {
                Globals.SpriteBatch.DrawString(BindFont, ArmyCount.ToString(),
                    new Vector2(135, 755), Color.White);
                DrawButton("2d\\F2ActiveButton", new Vector2(121, 779), new Vector2(75, 56));
            }

        }

        public void DrawActiveF1Button()
        {
            var BuilderCount = Globals.AllFriendlyEntity.Where(x => x.Name == "Builder").Count();
            if (BuilderCount > 0)
            {
                Globals.SpriteBatch.DrawString(BindFont, BuilderCount.ToString(),
                    new Vector2(55, 755), Color.White);
                DrawButton("2d\\F1ActiveButton", new Vector2(44, 779), new Vector2(75, 56));
            }

        }

        public void DrawUnitStats()
        {
            if (Globals.ChoiceEntity.selectedEntity.Count == 1)
            {
                var entity = Globals.ChoiceEntity.selectedEntity[0];
                DrawHp(entity.Hp, entity.MaxHp);
                if(Globals.AllEnemyEntity.Contains(entity) || Globals.AllFriendlyEntity.Contains(entity))
                {
                    Globals.SpriteBatch.DrawString(TimeFont, "Урон - " + entity.Damage.ToString(),
                    new Vector2(730, 946), Color.White);
                    Globals.SpriteBatch.DrawString(TimeFont, "Дальность атаки - " + entity.AttackRange.ToString(),
                        new Vector2(730, 976), Color.White);
                    Globals.SpriteBatch.DrawString(TimeFont, "Скорость атаки - 60 (атак/минуту)",
                        new Vector2(730, 1006), Color.White);
                }
            }
        }

        public void DrawHp(double hp, double maxHp)
        {
            var HpModel = Globals.Content.Load<Texture2D>("2d\\Hp");
            Globals.SpriteBatch.Draw(HpModel, new Rectangle(500, 1048, (int)(120 * hp / maxHp), 18), null,
                    new Color((1 - (float)(hp / maxHp)), (float)(hp / maxHp), 0, 0.6f), 0,
                    new Vector2(0, 0), new SpriteEffects(), 0.26f);
        }
    }
}
