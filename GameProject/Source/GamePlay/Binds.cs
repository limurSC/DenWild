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

namespace GameProject.Source.GamePlay
{
    public record Binds
    {
        public List<List<Entity>> ListBinds;
        public Binds()
        {
            ListBinds = new List<List<Entity>>();
            for (int i = 0; i < 10; i++)
                ListBinds.Add(new List<Entity>());
        }

        public void Update(int countBind, List<Entity> entities, KeyboardState keyboardState)
        {
            if (keyboardState.GetPressedKeys().Contains(Keys.LeftControl))
                ListBinds[countBind] = new List<Entity> (entities);
            else if (keyboardState.GetPressedKeys().Contains(Keys.LeftShift))
                AddBind(countBind, entities);
            else if (keyboardState.GetPressedKeys().Contains(Keys.LeftAlt))
                RemoveBind(countBind, entities);
            else if(ListBinds[countBind].Count != 0)
                Globals.ChoiceEntity.selectedEntity = new List<Entity> (ListBinds[countBind]);
            ListBinds = ListBinds.Distinct().ToList();
        }

        public void AddBind(int countBind, List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                if(!Globals.AllEnemyEntity.Contains(entity) && !Globals.AllNeutralEntity.Contains(entity) && 
                    !ListBinds[countBind].Contains(entity))
                    ListBinds[countBind].Add(entity);
            }
        }
        public void RemoveBind(int countBind, List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (!Globals.AllEnemyEntity.Contains(entity) && !Globals.AllNeutralEntity.Contains(entity))
                    ListBinds[countBind].Remove(entity);
            }
        }
    }
}
