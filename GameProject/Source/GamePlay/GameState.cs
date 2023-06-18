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

namespace GameProject.Source.GamePlay
{
    public record GameState
    {
        public static Camera Camera = new Camera();
        public static ChoiceObject ChoiceEntity = new ChoiceObject();
        public static List<FriendlyEntity> AllFriendlyEntity = new List<FriendlyEntity>();
        public static List<FriendlyBuilding> AllFriendlyBuilder = new List<FriendlyBuilding>();
        public static List<EnemyEntity> AllEnemyEntity = new List<EnemyEntity>();
        public static List<NeutralEntity> AllNeutralEntity = new List<NeutralEntity>();
        public static List<Bullet<Entity>> Bullets = new List<Bullet<Entity>>();
        public static List<Entity> AllEntity = new List<Entity>();
        public static int GameTime;
    }
}
