using System;

namespace Wander.Server.ClassLibrary.Model
{
    public struct Vector2
    {
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public override string ToString()
        {
            return string.Format("Vector2 X:{0}, Y:{1}", X, Y);
        }

        public override bool Equals(object obj)
        {
            var t = (Vector2) obj;
            return (t.X == X && t.Y == Y);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash*7) + X.GetHashCode();
            hash = (hash*7) + Y.GetHashCode();
            return hash;
        }

        public static Vector2 operator *(Vector2 first, Vector2 second)
        {
            return new Vector2(first.X*second.X, first.Y*second.Y);
        }

        public static Vector2 operator +(Vector2 first, Vector2 second)
        {
            return new Vector2(first.X + second.X, first.Y + second.Y);
        }

        public static double Length(Vector2 v)
        {
            return Math.Sqrt((v.X*v.X) + (v.Y*v.Y));
        }

        public double Length()
        {
            return Length(this);
        }

        public static Vector2 Normalize(Vector2 v)
        {
            var lgt = Length(v);
            return new Vector2(v.X/lgt, v.Y/lgt);
        }
    }
}