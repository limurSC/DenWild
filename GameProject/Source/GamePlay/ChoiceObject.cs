using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using DenWild.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Source.GamePlay
{
    public record ChoiceObject
    {
        public Vector2[] EntityChoiceWindow;
        public List<Entity> selectedEntity;
        private Basic2d choiceWindow;

        public ChoiceObject()
        {
            EntityChoiceWindow = new Vector2[] { };
            selectedEntity = new List<Entity>();
            choiceWindow = new Basic2d("2d\\ChoiceWindow", new Vector2(0, 0), new Vector2(0, 0), 1);
            choiceWindow.StartDrawingPosition = new Vector2(0, 0);
            choiceWindow.Color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        }

        public void ChoiceWindow()
        {
            var mouseState = Mouse.GetState();
            if (EntityChoiceWindow.Length != 0)
            {
                var pressedKeys = Keyboard.GetState().GetPressedKeys();
                if (mouseState.LeftButton == ButtonState.Released && (mouseState.Position.Y < 780
                    || mouseState.Position.X > 360) && (mouseState.Position.Y < 800 || mouseState.Position.X < 1480)
                    && mouseState.Position.Y < 860)
                {
                    if (!pressedKeys.Contains(Keys.LeftShift) && !pressedKeys.Contains(Keys.LeftControl))
                    {
                        selectedEntity.Clear();
                    }
                    AddClickSelectedEntity(Globals.AllFriendlyEntity);
                    AddClickSelectedEntity(Globals.AllFriendlyBuilder);
                    if (selectedEntity.Count == 0)
                    {
                        AddClickSelectedEntity(Globals.AllNeutralEntity);
                        AddClickSelectedEntity(Globals.AllEnemyEntity);
                    }
                }
                else if (!pressedKeys.Contains(Keys.LeftShift) && !pressedKeys.Contains(Keys.LeftControl))
                {
                    selectedEntity.Clear();
                }
                AddSelectedEntity(Globals.AllFriendlyEntity);
                AddSelectedEntity(Globals.AllFriendlyBuilder);
            }

        }

        public void AddClickSelectedEntity<T>(List<T> Entities) where T : Entity
        {
            var mouseState = Mouse.GetState();
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (var entity in Entities)
            {
                if ((entity.Position - mouseState.Position.ToVector2() - Globals.Camera.Position).Length() <
                    entity.Dims.X / 2 * 0.9)
                {
                    if (!pressedKeys.Contains(Keys.LeftControl) && !selectedEntity.Contains(entity))
                        selectedEntity.Add(entity);
                    else if (pressedKeys.Contains(Keys.LeftControl))
                        selectedEntity.Remove(entity);
                }
            }
        }

        public void AddSelectedEntity<T>(List<T> Entities) where T : Entity
        {
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (var entity in Entities)
                if (entity.Position.X > Math.Min(EntityChoiceWindow[0].X, EntityChoiceWindow[1].X) &&
                    entity.Position.X < Math.Max(EntityChoiceWindow[0].X, EntityChoiceWindow[1].X) &&
                    entity.Position.Y > Math.Min(EntityChoiceWindow[0].Y, EntityChoiceWindow[1].Y) &&
                    entity.Position.Y < Math.Max(EntityChoiceWindow[0].Y, EntityChoiceWindow[1].Y))
                {
                    if (!pressedKeys.Contains(Keys.LeftControl) && !selectedEntity.Contains(entity))
                        selectedEntity.Add(entity);
                    else if (pressedKeys.Contains(Keys.LeftControl))
                        selectedEntity.Remove(entity);
                }
        }


        public void Draw()
        {
            EntityChoiceWindowDraw();
            ChoiceObjectDraw();
        }

        public void EntityChoiceWindowDraw()
        {
            if (EntityChoiceWindow.Length != 0)
            {
                if(choiceWindow.StartDrawingPosition != new Vector2(0, 0))
                    choiceWindow.StartDrawingPosition = new Vector2(0, 0);
                var position = EntityChoiceWindow[0];
                var dims = EntityChoiceWindow[1] - EntityChoiceWindow[0];
                if (dims.X > 0 && dims.Y < 0)
                {
                    choiceWindow.Dims = new Vector2(-dims.X, dims.Y);
                    choiceWindow.Position = new Vector2(position.X + dims.X, position.Y);
                }
                if (dims.X < 0 && dims.Y > 0)
                {
                    choiceWindow.Dims = new Vector2(dims.X, -dims.Y);
                    choiceWindow.Position = new Vector2(position.X , position.Y + dims.Y);
                }
                else
                {
                    choiceWindow.Dims = new Vector2(dims.X, dims.Y);
                    choiceWindow.Position = new Vector2(position.X, position.Y);
                }
                choiceWindow.Draw();
            }
        }

        public void ChoiceObjectDraw()
        {
            for (int i = 0; i < Globals.ChoiceEntity.selectedEntity.Count; i++)
            {
                var entity = Globals.ChoiceEntity.selectedEntity[i];
                var position = entity.Position;
                var dims = entity.Dims;
                var myModel = Globals.Content.Load<Texture2D>("2d\\selected");
                Globals.SpriteBatch.Draw(myModel,
                    new Rectangle((int)position.X, (int)position.Y, (int)dims.X, (int)dims.Y), null,
                    new Color(1, 1, 1, 0.01f), 0.0f, new Vector2(myModel.Bounds.Width / 2,
                    myModel.Bounds.Height / 2), new SpriteEffects(), 1);
            }
        }
    }
}
