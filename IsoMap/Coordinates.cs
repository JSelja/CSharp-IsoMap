using System;

namespace IsoMap
{
    public class Vector2
    {
        // Describes a point in 2D space, using X and Y.
        // Two parameter constructor, automatically set to cartesian coordinates.
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
            IsCartesian = true;
        }

        // Three parameter, constructor.
        public Vector2(int x, int y, bool isCartesian)
        {
            X = x;
            Y = y;
            IsCartesian = isCartesian;
        }

        public int X { get; }

        public int Y { get; }

        public bool IsCartesian { get; }
    }

    public class Coordinates
    {
        public Vector2 ToIsometric(Vector2 v_in)
        {
            if (v_in.IsCartesian)
            {
                int vx = v_in.X - v_in.Y;
                int vy = (v_in.X + v_in.Y) / 2;

                return new Vector2(vx, vy, false);
            } else
            {
                return v_in;
            }
        }

        public Vector2 ToCartesian(Vector2 v_in)
        {
            if (!v_in.IsCartesian)
            {
                int vx = (2 * v_in.Y + v_in.X) / 2;
                int vy = (2 * v_in.Y - v_in.X) / 2;

                return new Vector2(vx, vy, true);
            } else
            {
                return v_in;
            }
        }
    }
}
