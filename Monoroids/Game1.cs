﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monoroids.Core.Services;

namespace Monoroids;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    
    private SceneManager _sceneManager;
    private RenderService _renderService;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;            
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.ApplyChanges();

        _renderService = new RenderService(_graphics);
        _renderService.SetLayerConfig((int)RenderLayers.Background, new RenderLayerConfig
        {
            SamplerState = SamplerState.LinearWrap
        });
        GameServicesManager.Instance.AddService(_renderService);

        _sceneManager = new SceneManager();
        GameServicesManager.Instance.AddService(_sceneManager);

        GameServicesManager.Instance.AddService(new CollisionService(new Point(64, 64)));

        GameServicesManager.Instance.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _sceneManager.AddScene(GameStuff.Scenes.SceneNames.Welcome, new GameStuff.Scenes.PreGameScene(this, "Blazeroids!"));
        _sceneManager.AddScene(GameStuff.Scenes.SceneNames.Play, new GameStuff.Scenes.PlayScene(this));
        _sceneManager.AddScene(GameStuff.Scenes.SceneNames.GameOver, new GameStuff.Scenes.PreGameScene(this, "Game Over!"));
        
        _sceneManager.SetCurrentScene(GameStuff.Scenes.SceneNames.Welcome);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        GameServicesManager.Instance.Step(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _renderService.Render();

        base.Draw(gameTime);
    }
}