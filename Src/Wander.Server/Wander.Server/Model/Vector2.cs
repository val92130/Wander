using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    public class Vector2
    {
        private double _x, _y;
        public Vector2(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Vector2()
        {
            _x = 0;
            _y = 0;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public override string ToString()
        {
            return String.Format("Vector2 X:{0}, Y:{1}", this.X, this.Y);
        }

        public override bool Equals(object obj)
        {

            Vector2 t = obj as Vector2;
            return (t.X == this.X && t.Y == this.Y);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            return hash;
        }

        public static Vector2 operator *(Vector2 first, Vector2 second)
        {
            return new Vector2(first.X*second.X , first.Y*second.Y);
        }

        public static Vector2 operator + (Vector2 first, Vector2 second)
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
            double lgt = Length(v);
            return new Vector2(v.X / lgt, v.Y / lgt);
        }
    }
}