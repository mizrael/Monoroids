using System;
using Microsoft.Xna.Framework;
using Snake.Components;
using Solo;
using Solo.Services;

namespace Snake.Scenes;

public class PlayScene : Scene
{
    private RenderService _renderService;

    public PlayScene(Game game) : base(game)
    {
    }

    protected override void EnterCore()
    {
        _renderService = GameServicesManager.Instance.GetService<RenderService>();

        var board = new Board(64, 64);
        var boardObject = new GameObject();
        var boardRenderer = boardObject.Components.Add<BoardRenderer>();
        boardRenderer.Board = board;
        boardRenderer.LayerIndex = (int)RenderLayers.Background;

        var setTileSize = new Action(() =>
        {
            boardRenderer.TileSize = new Vector2(
                (float)_renderService.Graphics.PreferredBackBufferWidth / board.Width,
                (float)_renderService.Graphics.PreferredBackBufferHeight / board.Height
            );
        });
        setTileSize();
        _renderService.Graphics.DeviceReset += (s, e) => setTileSize();

        this.Root.AddChild(boardObject);
    }
}
