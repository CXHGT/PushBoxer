using System;
using System.Collections.Generic;
using System.Text;

namespace PushBoxer
{
    public struct Point2 : IEquatable<Point2>
    {
        public int X;
        public int Y;
        public Point2(int X = 0, int Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }

        public Point2(Vector2 vector2)
        {
            this.X = (int)vector2.X;
            this.Y = (int)vector2.Y;
        }
        public override bool Equals(object obj)
        {
            return obj is Point2 && this.Equals((Point2)obj);
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }
        public bool Equals(Point2 other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        public static bool operator ==(Point2 p1, Point2 p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Point2 p1, Point2 p2)
        {
            return !p1.Equals(p2);
        }
        public static Point2 operator +(Point2 p1, Point2 p2)
        {
            return new Point2(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Point2 operator -(Point2 p)
        {
            return new Point2(-p.X, -p.Y);
        }
        public static Point2 operator -(Point2 p1, Point2 p2)
        {
            return new Point2(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
}
