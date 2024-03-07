﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monoroids.Core.Services;

public class RenderService : IGameService
{
    public readonly GraphicsDeviceManager Graphics;

    private readonly SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;
    private SortedList<int, IList<IRenderable>> _layers = new();

    public RenderService(GraphicsDeviceManager graphics)
    {
        Graphics = graphics ?? throw new System.ArgumentNullException(nameof(graphics));
        _spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
    }

    public void Initialize()
    {
        _sceneManager = GameServicesManager.Instance.GetService<SceneManager>();
    }

    public void Step(GameTime gameTime)
    {
        BuildLayers(_sceneManager.Current.Root, _layers);
    }

    public void Render()
    {
        Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach (var layer in _layers.Values)
        { 
            _spriteBatch.Begin();

            foreach (var renderable in layer)
                renderable.Render(_spriteBatch);

            _spriteBatch.End();
        }            
    }

    private static void BuildLayers(GameObject node, SortedList<int, IList<IRenderable>> layers)
    {
        if (null == node || !node.Enabled)
            return;

        foreach (var component in node.Components)
            if (component is IRenderable renderable &&
                component.Initialized &&
                !renderable.Hidden)
            {
                if (!layers.ContainsKey(renderable.LayerIndex))
                    layers.Add(renderable.LayerIndex, new List<IRenderable>());
                layers[renderable.LayerIndex].Add(renderable);
            }

        foreach (var child in node.Children)
            BuildLayers(child, layers);
    }
}
