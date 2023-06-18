using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DenWild;
using DenWild.Source.Engine;
using DenWild.World;
using GameProject.Source.Engine;
using Microsoft.Xna.Framework;

namespace GameProject.Source.GamePlay
{
    internal record Map
    {
        Basic2d MapBackground, CameraFrame;
        private Vector2 MapPosition, CameraFramePosition;
        public Vector2 MapSize;
        private float mapLength = 258.0f;

        public Map()
        {
            MapSize = Globals.World.MapBackground.Dims;
            MapPosition = new Vector2(158, 944);
            MapBackground = new Basic2d("2d\\MapBackground", MapPosition,
                    new Vector2(258, 258), 0.3f);
            CameraFramePosition = new Vector2(29, 815) + 
                new Vector2(1920 * mapLength / (MapSize.X * 2), 1080 * mapLength / (MapSize.Y * 2));
            CameraFrame = new Basic2d("2d\\CameraFrame", CameraFramePosition,
                new Vector2(1920 * mapLength / MapSize.X , 1080 * mapLength / MapSize.Y) , 0.28f);
        }
        // 129
        //29 815
        public virtual void Update()
        {
            if (MapSize != Globals.World.MapBackground.Dims)
            {
                MapSize = Globals.World.MapBackground.Dims;
                MapBackground.Dims = new Vector2(258 * Math.Min(1, MapSize.X / MapSize.Y),
                    258 * Math.Min(1, MapSize.Y / MapSize.X));
                CameraFramePosition = new Vector2(29, 815) +
                    new Vector2(1920 * mapLength / (MapSize.X * 2), 1080 * mapLength / (MapSize.Y * 2));
                CameraFrame.Dims = new Vector2(1920 * mapLength / MapSize.X, 1080 * mapLength / MapSize.Y);
            }
            MapBackground.Path = Globals.World.MapBackground.Path;
            MapBackground.Position = MapPosition;
            CameraFrame.Position = CameraFramePosition + Globals.Camera.Position * (258.0f / MapSize.X);
        }

        public virtual void Draw()
        {
            MapSize = Globals.World.MapBackground.Dims;
            DrawEntityInMap(Globals.AllFriendlyEntity.Select(x => (Entity)x).ToList(), "2d\\UnitInMap");
            DrawEntityInMap(Globals.AllEnemyEntity.Select(x => (Entity)x).ToList(), "2d\\BadUnitInMap");
            DrawEntityInMap(Globals.AllFriendlyBuilder.Select(x => (Entity)x).ToList(), "2d\\UnitInMap");
            DrawEntityInMap(Globals.AllNeutralEntity.Select(x => (Entity)x).ToList(), "");
            CameraFrame.Draw();
            MapBackground.Draw();
        }

        public void DrawEntityInMap(List<Entity> entities, string path)
        {
            foreach (var entity in entities)
            {
                var entityInMap = new Basic2d(path, new Vector2(29, 815) +
                    new Vector2(entity.Position.X * (258.0f / MapSize.X), entity.Position.Y * (258.0f / MapSize.Y)),
                    new Vector2(entity.Dims.X * (258.0f / MapSize.X), entity.Dims.Y * (258.0f / MapSize.Y)), 0.29f);
                if (path == "")
                    entityInMap = new Basic2d(entity.Path, new Vector2(29, 815) +
                        new Vector2(entity.Position.X * (258.0f / MapSize.X), entity.Position.Y * (258.0f / MapSize.Y)),
                        new Vector2(entity.Dims.X * (258.0f / MapSize.X), entity.Dims.Y * (258.0f / MapSize.Y)), 0.29f);
                if (entity.Name == "Crystal")
                    entityInMap.Dims *= 2;
                if (path != "2d\\BadUnitInMap")
                    entityInMap.Draw();
                else if(CheckVision(entity.Position))
                        entityInMap.Draw();
            }
        }

        public bool CheckVision(Vector2 position)
        {
            foreach (var entity in Globals.AllFriendlyBuilder)
            {
                if ((entity.Position - position).Length() < entity.Vision)
                    return true;
            }
            foreach (var entity in Globals.AllFriendlyEntity)
            {
                if ((entity.Position - position).Length() < entity.Vision)
                    return true;
            }
            return false;
        }
    }
}
