using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using DenWild.World;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static System.Net.Mime.MediaTypeNames;

namespace GameProject.Source.GamePlay.world.Entities.FriendlyEntities
{
    public record Builder : FriendlyEntity
    {
        readonly static Vector2 MineButtonPosition = new Vector2(1647, 888);
        readonly static Vector2 MineButtonSize = new Vector2(69, 68);
        readonly static Vector2 BaseButtonPosition = new Vector2(1577, 888);
        readonly static Vector2 BaseButtonSize = new Vector2(69, 68);
        public FriendlyBuilding BuildingForConstruction, BuildingUnderConstruction;
        public Builder(string path, Vector2 position,
            Vector2 dims, float height) : base(path, position, dims, height)
        {
            AttackRange = 100;
            Damage = 50;
            EntitySpeed = 5;
            Hp = 500;
            MaxHp = 500;
            CreateTime = 60;
            Name = "Builder";
            Vision = 400;
            BuildingForConstruction = null;
            BuildingUnderConstruction = null;
            permissionToAttack = true;
            SwitchingPositions = new List<Vector2>();
            MotionVectors = new List<Vector2>();
        }

        public override void Update()
        {
            base.Update();
            if(BuildingUnderConstruction != null)
                StartBuild();
            if (BuildingForConstruction != null)
                EndConstruction();
        }
        public void EndConstruction()
        {
            if (Globals.Control.CheckOneLeftClick() && !CheckBuildColision(BuildingForConstruction, this) &&
                BuildingForConstruction.Name == "Base" && Globals.Pragmanit >= 5000)
            {
                BuildingUnderConstruction = BuildingForConstruction;
                GoToBuild();
            }
            else if (Globals.Control.CheckOneLeftClick() && BuildingForConstruction.Name == "Miner" &&
                CheckMinerPosition() && Globals.Pragmanit >= 2000)
            {
                BuildingUnderConstruction = BuildingForConstruction;
                GoToBuild();
            }
            if (Globals.ChoiceEntity.selectedEntity.Contains(this) && Globals.Control.CheckOneRightClick())
            {
                BuildingUnderConstruction = null;
                BuildingForConstruction = null;
                Globals.Control.CheckChoiceEntity = true;
            }
        }

        public bool CheckMinerPosition()
        {
            var mouseState = Mouse.GetState();
            foreach(var entity in Globals.AllNeutralEntity)
            {
                if ((entity.Position - mouseState.Position.ToVector2() - Globals.Camera.Position).Length() <
                    entity.Dims.X / 2)
                    return true;
            }
            return false;
        }

        public void GoToBuild()
        {
            var switchingPosition = BuildingForConstruction.Position;
            SwitchingPositions = new List<Vector2> { switchingPosition };
            var motion = new Vector2(switchingPosition.X - Position.X,
                switchingPosition.Y - Position.Y);
            var distance = motion.Length();
            MotionVectors = new List<Vector2> { new Vector2(motion.X / distance, motion.Y / distance) };
            Globals.Control.CheckChoiceEntity = true;
        }

        public void StartBuild()
        {
            if((Position - BuildingUnderConstruction.Position).Length() < BuildingUnderConstruction.Dims.X / 3)
            {
                BuildingUnderConstruction.Hp = BuildingUnderConstruction.MaxHp * 0.2f;
                BuildingUnderConstruction.Color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
                BuildingUnderConstruction.Transparency = 0.2f;
                Globals.AllEntity.Add(BuildingUnderConstruction);
                Globals.AllFriendlyBuilder.Add(BuildingUnderConstruction);
                Globals.Pragmanit -= BuildingUnderConstruction.Price;
                BuildingUnderConstruction = null;
                BuildingForConstruction = null;
                CheckToKill = true;
                Globals.Control.CheckChoiceEntity = true;
                Globals.Control.CheckBuild = false;
            }
        }

        public static bool CheckBuildColision(Entity checkEntity, Entity builder)
        {
            foreach (var entity in Globals.AllEntity)
            {
                if (entity != builder && ((entity.Position - checkEntity.Position).Length() <
                    (entity.Dims.X + checkEntity.Dims.X) / 2))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Draw()
        {
            base.Draw();
            if (BuildingForConstruction != null)
                DrawBuildPosition();
        }

        public void DrawBuildPosition()
        {
            if(BuildingUnderConstruction == null)
            {
                BuildingForConstruction.Position = Mouse.GetState().Position.ToVector2() + Globals.Camera.Position;
                BuildingForConstruction.Color = new Color(0.5f, 1, 0.5f, 0.5f);
                if (BuildingForConstruction.Name == "Base" && CheckBuildColision(BuildingForConstruction, this))
                    BuildingForConstruction.Color = new Color(1, 0.5f, 0.5f, 0.5f);
                if (BuildingForConstruction.Name == "Miner")
                {
                    BuildingForConstruction.Position = GetNearestMiner(BuildingForConstruction);
                    if(!CheckMinerPosition())
                        BuildingForConstruction.Color = new Color(1, 0.5f, 0.5f, 0.5f);
                }
            }
            BuildingForConstruction.Draw();
        }

        public Vector2 GetNearestMiner(FriendlyBuilding build)
        {
            var result = build.Position;
            foreach (var entity in Globals.AllNeutralEntity)
            {
                if ((build.Position - entity.Position).Length() < entity.Dims.X / 2)
                    return entity.Position;
            }
            return result;
        }
    }
}
