using System;

namespace IsoMap
{
    public class Vector2
    {
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }

    public class Coordinates
    {
        public Vector2 CartesianToIsometric(Vector2 v_in)
        {
            int vx = v_in.X - v_in.Y;
            int vy = (v_in.X + v_in.Y) / 2;

            return new Vector2(vx, vy);
        }

        public Vector2 IsometricToCartesian(Vector2 v_in)
        {
            int vx = (2 * v_in.Y + v_in.X) / 2;
            int vy = (2 * v_in.Y - v_in.X) / 2;

            return new Vector2(vx, vy);
        }
    }
}
