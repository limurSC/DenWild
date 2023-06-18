using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild.Source.GamePlay;
using DenWild.World;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay;
using GameProject.Source.GamePlay.world;
using GameProject.Source.GamePlay.world.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DenWild.Source.Engine
{
    public static class Globals
    {
        public static int Pragmanit = 5000;
        public static ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static Camera Camera = new Camera();
        public static ChoiceObject ChoiceEntity = new ChoiceObject();
        public static List<FriendlyEntity> AllFriendlyEntity = new List<FriendlyEntity>();
        public static List<FriendlyBuilding> AllFriendlyBuilder = new List<FriendlyBuilding>();
        public static List<EnemyEntity> AllEnemyEntity = new List<EnemyEntity>();
        public static List<NeutralEntity> AllNeutralEntity = new List<NeutralEntity>();
        public static List<Bullet<Entity>> Bullets = new List<Bullet<Entity>>();
        public static List<Entity> AllEntity = new List<Entity>();
        public static int GameTime;
        public static String LevelName;
        public static MyWorld World = new MyWorld();
        public static Audio Audio;
        public static Control Control;

    }
}
