using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public double X;
        public double Y;
        public Vector2(double X = 0, double Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector2(Point2 point2)
        {
            this.X = point2.X;
            this.Y = point2.Y;
        }
        public override bool Equals(object obj)
        {
            return obj is Vector2 && this.Equals((Vector2)obj);
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }
        public bool Equals(Vector2 other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !v1.Equals(v2);
        }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
    }
}
