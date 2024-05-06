using Microsoft.Xna.Framework;

namespace Snake;

public class Snake
{
    public Snake()
    {
        Head = new Segment();
    }

    public Direction Direction;

    public readonly Segment Head;

    public record Segment
    {
        public Vector2 Position;
        public Direction Direction;
        public Segment? Next = null;
    }
}