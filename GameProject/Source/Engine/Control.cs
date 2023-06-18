using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild.Source.Engine;
using DenWild.World;
using GameProject.Source.GamePlay.world.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using GameProject.Source.GamePlay;
using DenWild;

namespace GameProject.Source.Engine
{
    public record Control
    {
        public Vector2 StartChoiceWindow;
        public bool CheckStartChoiceWindow;
        public bool CheckMoveCommandEntity;
        public bool CheckChoiceEntity;
        public List<Basic2d> Clicks;
        public MouseState MouseState;
        public KeyboardState KeyboardState;
        public MouseState LastMouseState;
        public KeyboardState LastKeyboardState;
        public Binds Binds;
        public bool CheckBuild;
        public int CameraSpeed;

        public Control() 
        {
            CheckChoiceEntity = true;
            Clicks = new List<Basic2d>();
            CheckStartChoiceWindow = false;
            CheckMoveCommandEntity = false;
            Binds = new Binds();
            CameraSpeed = 30;
        }

        public virtual void Update()
        {
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            CheckGetCommand();
            if(CheckSelectedEntity())
                foreach (var entity in Globals.ChoiceEntity.selectedEntity)
                    MoveEntity(entity);
            CameraMovement();
            UnitCameraMovement();
            if (CheckChoiceEntity && !CheckBuild)
                ChoiceEntity();
            AddClicks();
            if (CheckBuild)
                StartConstruction((Builder)Globals.ChoiceEntity.selectedEntity[0]);
            if (Globals.ChoiceEntity.selectedEntity.Count > 0)
                CheckBuildUI();
            BindsUpdate();
            SelectAllArmy();
            SelectAllBuilder();
            GetEsc();
            LastMouseState = Mouse.GetState();
            LastKeyboardState = Keyboard.GetState();
        }

        public void GetEsc()
        {
            if (CheckOneKeyClick(Keys.Escape))
            {
                CheckChoiceEntity = true;
                Globals.ChoiceEntity.selectedEntity = new List<Entity>();
            }
        }

        public bool CheckSelectedEntity()
        {
            return Globals.ChoiceEntity.selectedEntity.Count > 1 ||
                (Globals.ChoiceEntity.selectedEntity.Count == 1 &&
                !Globals.AllEnemyEntity.Contains(Globals.ChoiceEntity.selectedEntity[0]));
        }

        public void MoveEntity(Entity entity)
        {
            var position = entity.Position;
            var motionVectors = entity.MotionVectors;
            var mouseClicks = entity.SwitchingPositions;
            var pressedKeys = KeyboardState.GetPressedKeys();
            if (MouseState.RightButton == ButtonState.Pressed && (MouseState.Position.Y < 780 || MouseState.Position.X > 360)
                && (MouseState.Position.Y < 800 || MouseState.Position.X < 1480) && MouseState.Position.Y < 835)
            {
                entity.permissionToAttack = false;
                var mousePosition = MouseState.Position.ToVector2() + Globals.Camera.Position;
                AddMoveEntity(pressedKeys, motionVectors, mouseClicks, position, mousePosition);
            }
            else if(MouseState.RightButton == ButtonState.Pressed && MouseState.Position.Y < 1073 && MouseState.Position.X > 29
                && MouseState.Position.Y > 815 && MouseState.Position.X < 287)
            {
                entity.permissionToAttack = false;
                var mousePosition = ((MouseState.Position.ToVector2() - new Vector2(29, 815)) * 5000 / 258);
                AddMoveEntity(pressedKeys, motionVectors, mouseClicks, position, mousePosition);
            }
            else if (MouseState.LeftButton == ButtonState.Pressed && CheckMoveCommandEntity
                && (MouseState.Position.Y < 780 || MouseState.Position.X > 360)
                && (MouseState.Position.Y < 800 || MouseState.Position.X < 1480) && MouseState.Position.Y < 860)
            {
                entity.permissionToAttack = true;
                var mousePosition = MouseState.Position.ToVector2() + Globals.Camera.Position;
                AddMoveEntity(pressedKeys, motionVectors, mouseClicks, position, mousePosition);
            }
            else if (MouseState.LeftButton == ButtonState.Pressed && CheckMoveCommandEntity
                && MouseState.Position.Y < 1073 && MouseState.Position.X > 29
                && MouseState.Position.Y > 815 && MouseState.Position.X < 287)
            {
                entity.permissionToAttack = true;
                var mousePosition = ((MouseState.Position.ToVector2() - new Vector2(29, 815)) * 5000 / 258);
                AddMoveEntity(pressedKeys, motionVectors, mouseClicks, position, mousePosition);
            }
        }

        public static void AddMoveEntity(Keys[] pressedKeys, List<Vector2> motionVectors, 
            List<Vector2> mouseClicks, Vector2 position, Vector2 mousePosition)
        {
            Vector2 motionVector;
            Vector2 mouseClick;;
            mouseClick = new Vector2(mousePosition.X, mousePosition.Y);
            if (motionVectors.Count > 0 && pressedKeys.Length > 0 && pressedKeys.Contains(Keys.LeftShift))
            {
                motionVector = new Vector2(mouseClick.X - mouseClicks[mouseClicks.Count - 1].X,
                    mouseClick.Y - mouseClicks[mouseClicks.Count - 1].Y);
                var distance = motionVector.Length();
                if (distance > 0)
                    motionVector = new Vector2(motionVector.X / distance,
                        motionVector.Y / distance);
            }
            else
            {
                motionVector = new Vector2(mouseClick.X - position.X,
                mouseClick.Y - position.Y);
                var distance = motionVector.Length();
                motionVector = new Vector2(motionVector.X / distance,
                motionVector.Y / distance);
            }
            if (pressedKeys.Length == 0 || !pressedKeys.Contains(Keys.LeftShift))
            {
                if (motionVectors.Count > 0)
                {
                    motionVectors.Clear();
                    mouseClicks.Clear();
                }
                motionVectors.Add(motionVector);
                mouseClicks.Add(mouseClick);
            }
            if (pressedKeys.Length > 0 && pressedKeys.Contains(Keys.LeftShift))
            {
                motionVectors.Add(motionVector);
                mouseClicks.Add(mouseClick);
            }
        }


        public void AddClicks()
        {
            if (CheckOneRightClick())
            {
                Clicks.Add(new Basic2d("2d\\ClickСircle",
                    Mouse.GetState().Position.ToVector2() + Globals.Camera.Position
                    , new Vector2(45, 45), 0.47f));
            }
            if (LastMouseState.RightButton == ButtonState.Pressed)
            {
                Clicks.Add(new Basic2d("2d\\ClickСircle",
                    Mouse.GetState().Position.ToVector2() + Globals.Camera.Position
                    , new Vector2(10, 10), 0.47f));
            }
            if (CheckOneLeftClick() && CheckMoveCommandEntity)
            {
                var click = new Basic2d("2d\\ClickСircle",
                    Mouse.GetState().Position.ToVector2() + Globals.Camera.Position
                    , new Vector2(45, 45), 0.47f);
                click.Color = Color.Red;
                Clicks.Add(click);
            }
            if (LastMouseState.LeftButton == ButtonState.Pressed && CheckMoveCommandEntity)
            {
                var click = new Basic2d("2d\\ClickСircle",
                    Mouse.GetState().Position.ToVector2() + Globals.Camera.Position
                    , new Vector2(10, 10), 0.47f);
                click.Color = Color.Red;
                Clicks.Add(click);
            }
        }

        public void CheckGetCommand()
        {
            var keyboardState = Keyboard.GetState().GetPressedKeys();
            if (keyboardState.Contains(Keys.A) && Globals.ChoiceEntity.selectedEntity.Count > 0)
                CheckMoveCommandEntity = true;
            if (keyboardState.Contains(Keys.Escape) || CheckOneRightClick() ||
                Globals.ChoiceEntity.selectedEntity.Count == 0)
                CheckMoveCommandEntity = false;
        }

        public void ChoiceEntity()
        {
            var mouseState = Mouse.GetState();
            if (LastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed
                 && !CheckMoveCommandEntity && (mouseState.Position.Y < 780 || mouseState.Position.X > 360)
                && (mouseState.Position.Y < 800 || mouseState.Position.X < 1480) && mouseState.Position.Y < 840)
            {
                if (!CheckStartChoiceWindow)
                {
                    CheckStartChoiceWindow = true;
                    StartChoiceWindow = LastMouseState.Position.ToVector2() + Globals.Camera.Position;
                }
                Globals.ChoiceEntity.EntityChoiceWindow = new Vector2[2] { StartChoiceWindow, 
                        mouseState.Position.ToVector2() + Globals.Camera.Position };
            }
            else if(!CheckMoveCommandEntity)
            {
                Globals.ChoiceEntity.ChoiceWindow();
                CheckStartChoiceWindow = false;
                Globals.ChoiceEntity.EntityChoiceWindow = new Vector2[] { };
            }
        }

        public void CameraMovement() 
        {
            CameraMovementInMiniMap();
            var mousePosition = MouseState.Position;
            if (mousePosition.X < 10 && Globals.Camera.Position.X > CameraSpeed)
                Globals.Camera.Translate(new Vector2(-CameraSpeed, 0));
            if (mousePosition.Y < 10 && Globals.Camera.Position.Y > CameraSpeed)
                Globals.Camera.Translate(new Vector2(0, -CameraSpeed));
            if (mousePosition.X > 1910 && Globals.Camera.Position.X < 
                Globals.World.MapBackground.Dims.X - 1920 - CameraSpeed)
                Globals.Camera.Translate(new Vector2(CameraSpeed, 0));
            if (mousePosition.Y > 1070 && Globals.Camera.Position.Y <
                Globals.World.MapBackground.Dims.Y - 1080 - CameraSpeed)
                Globals.Camera.Translate(new Vector2(0, CameraSpeed));
        }

        public void CameraMovementInMiniMap()
        {
            if(MouseState.LeftButton == ButtonState.Pressed && MouseState.Position.Y < 1073 && 
                MouseState.Position.X > 29 && MouseState.Position.Y > 815 && MouseState.Position.X < 287)
            {
                var mousePosition = (MouseState.Position.ToVector2() - new Vector2(1920, 1080) *
                    (258.0f / (Globals.World.MapBackground.Dims.X * 2)) - new Vector2(29 ,815));

                mousePosition.X = (int)(mousePosition.X * Globals.World.MapBackground.Dims.X / 258);
                mousePosition.Y = (int)(mousePosition.Y * Globals.World.MapBackground.Dims.Y / 258);
                Globals.Camera.SetPosition(CheckCameraPosition(mousePosition));
            }
        }

        public void UnitCameraMovement()
        {
            if (Globals.ChoiceEntity.selectedEntity.Count != 0 && MouseState.LeftButton == ButtonState.Pressed &&
                MouseState.Position.Y < 1075 && MouseState.Position.X > 1386
                && MouseState.Position.Y > 880 && MouseState.Position.X < 1510)
            {
                var cameraPosition = Globals.ChoiceEntity.selectedEntity[0].Position - new Vector2(960, 540);

                Globals.Camera.SetPosition(CheckCameraPosition(cameraPosition));
            }
        }

        public Vector2 CheckCameraPosition(Vector2 cameraPosition)
        {
            if (cameraPosition.Y < CameraSpeed)
                cameraPosition.Y = CameraSpeed;
            if (cameraPosition.X < CameraSpeed)
                cameraPosition.X = CameraSpeed;
            if (cameraPosition.Y > Globals.World.MapBackground.Dims.Y - 1080 - CameraSpeed)
                cameraPosition.Y = Globals.World.MapBackground.Dims.Y - 1080;
            if (cameraPosition.X > Globals.World.MapBackground.Dims.Y - 1920 - CameraSpeed)
                cameraPosition.X = Globals.World.MapBackground.Dims.Y - 1920;
            return cameraPosition;
        }

        public void AddCreateEntity(List<FriendlyEntity> QueueCreateEntity, 
            Vector2 Position, Base selectBase)
        {
            if (Globals.ChoiceEntity.selectedEntity.Contains(selectBase))
            {
                if ((CheckOneKeyClick(Keys.B) || (CheckOneLeftClick() &&
                    MouseState.Position.X > 1540 && MouseState.Position.X < 1611 &&
                    MouseState.Position.Y > 853 && MouseState.Position.Y < 923)) && Globals.Pragmanit > 500)
                {
                    QueueCreateEntity.Add(new Builder("2d\\Builder", Position,
                        new Vector2(40, 40), 0.5f));
                    Globals.Pragmanit -= 500;
                }
                if ((CheckOneKeyClick(Keys.D) || (CheckOneLeftClick() &&
                    MouseState.Position.X > 1603 && MouseState.Position.X < 1682 &&
                    MouseState.Position.Y > 853 && MouseState.Position.Y < 923)) && Globals.Pragmanit > 1000)
                {
                    QueueCreateEntity.Add(new Drone("2d\\Drone", Position,
                        new Vector2(60, 60), 0.5f));
                    Globals.Pragmanit -= 1000;
                }
            }
        }

        public void CheckBuildUI()
        {
            var entity = Globals.ChoiceEntity.selectedEntity[0];
            if (entity.Name == "Builder" && ((CheckOneKeyClick(Keys.B)) ||
                (CheckOneLeftClick() &&
                MouseState.Position.X > 1543 && MouseState.Position.X < 1612 &&
                MouseState.Position.Y > 993 && MouseState.Position.Y < 1063)))
            {
                CheckBuild = true;
            }
            else if (KeyboardState.GetPressedKeys().Contains(Keys.Escape) || entity.Name != "Builder")
                CheckBuild = false;
        }

        public void StartConstruction(Builder entity)
        {
            if (CheckOneKeyClick(Keys.B) || (CheckOneLeftClick() &&
                MouseState.Position.X > 1540 && MouseState.Position.X < 1611 &&
                MouseState.Position.Y > 853 && MouseState.Position.Y < 923) && Globals.Pragmanit >= 5000)
            {
                entity.BuildingForConstruction = new Base("2d\\Base 1",
                    Mouse.GetState().Position.ToVector2() + Globals.Camera.Position, new Vector2(500, 500), 0.49f);
                entity.BuildingForConstruction.Color = new Color(1, 1, 1, 0.5f);
                CheckChoiceEntity = false;
            }
            if (CheckOneKeyClick(Keys.S) || (CheckOneLeftClick() &&
                MouseState.Position.X > 1603 && MouseState.Position.X < 1682 &&
                MouseState.Position.Y > 853 && MouseState.Position.Y < 923)
                 && Globals.Pragmanit >= 2000)
            {
                entity.BuildingForConstruction = new Miner("2d\\Miner",
                    Mouse.GetState().Position.ToVector2() + Globals.Camera.Position, new Vector2(200, 200), 0.49f);
                entity.BuildingForConstruction.Color = new Color(1, 1, 1, 0.5f);
                CheckChoiceEntity = false;
            }
        }

        public void SelectAllArmy()
        {
            if (CheckOneKeyClick(Keys.F2))
            {
                var selectedEntity = new List<Entity>();
                foreach (var entity in Globals.AllFriendlyEntity)
                {
                    if (entity.Name != "Builder")
                        selectedEntity.Add(entity);
                }
                Globals.ChoiceEntity.selectedEntity = selectedEntity;
            }
        }

        public void SelectAllBuilder()
        {
            if (CheckOneKeyClick(Keys.F1))
            {
                var selectedEntity = new List<Entity>();
                foreach (var entity in Globals.AllFriendlyEntity)
                {
                    if (entity.Name == "Builder")
                        selectedEntity.Add(entity);
                }
                Globals.ChoiceEntity.selectedEntity = selectedEntity;
            }
        }

        public void BindsUpdate()
        {
            if (CheckOneKeyClick(Keys.D1) || CheckClickBind(0))
                Binds.Update(0, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D2) || CheckClickBind(1))
                Binds.Update(1, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D3) || CheckClickBind(2))
                Binds.Update(2, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D4) || CheckClickBind(3))
                Binds.Update(3, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D5) || CheckClickBind(4))
                Binds.Update(4, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D6) || CheckClickBind(5))
                Binds.Update(5, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D7) || CheckClickBind(6))
                Binds.Update(6, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D8) || CheckClickBind(7))
                Binds.Update(7, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D9) || CheckClickBind(8))
                Binds.Update(8, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
            if (CheckOneKeyClick(Keys.D0) || CheckClickBind(9))
                Binds.Update(9, Globals.ChoiceEntity.selectedEntity, LastKeyboardState);
        }

        public bool CheckClickBind(int countBind)
        {
            var mousePosition = MouseState.Position;
            return CheckOneLeftClick() && mousePosition.X > 527 + 69 * countBind &&
                mousePosition.X < 587 + 69 * countBind && mousePosition.Y > 834 && mousePosition.Y < 860;
        }

        public bool CheckOneRightClick()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed &&
                LastMouseState.RightButton != ButtonState.Pressed;
        }

        public bool CheckOneLeftClick()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed &&
                LastMouseState.LeftButton != ButtonState.Pressed;
        }

        public bool CheckOneKeyClick(Keys key)
        {
            return Keyboard.GetState().GetPressedKeys().Contains(key) &&
                !LastKeyboardState.GetPressedKeys().Contains(key);
        }

    }
}
