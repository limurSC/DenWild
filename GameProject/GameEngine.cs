using DenWild.Source.Engine;
using DenWild.Source.GamePlay;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay;
using GameProject.Source.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameProject;

public class GameEngine : Game
{
    private GraphicsDeviceManager _graphics;
    private int Weight, Height;
    public UI Interface;
    public static Menu Menu;

    public GameEngine()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Weight = 920;
        Height = 1080;
        Weight = 1920;
        Height = 1080;
        Globals.Camera.ViewportWidth = Weight;
        Globals.Camera.ViewportHeight = Height;
        _graphics.IsFullScreen = true;
        _graphics.PreferredBackBufferWidth = Weight;
        _graphics.PreferredBackBufferHeight = Height;
        _graphics.ApplyChanges();
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.Content = Content;
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        Globals.Audio = new Audio();
        Menu = new Menu();
        Globals.Control = new Control();
        Interface = new UI();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || MenuState.CheckExitGame)
            Exit();

        // TODO: Add your update logic here
        Menu.Update();
        if (!MenuState.CheckGameMenu && !MenuState.CheckWinGame)
        { 
            Globals.World.Update();
            Interface.Update();
            Globals.Control.Update();
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.ForestGreen);

        // TODO: Add your drawing code here

        Globals.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
        null, null, null, null, Globals.Camera.TranslationMatrix);

        if (MenuState.CheckStartGame)
            Globals.World.Draw();

        Globals.SpriteBatch.End();
        Globals.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
        null, null, null, null);

        Menu.Draw();
        if (MenuState.CheckStartGame)
            Interface.Draw();

        Globals.SpriteBatch.End();

        base.Draw(gameTime);
    }
}