using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    public readonly struct Angle
    {
        public readonly float X;
        public readonly float Y;
        public readonly float A;

        #region Constructors
        public Angle(Vector2 point)
        {
            X = point.X;
            Y = point.Y;
            A = (float)System.Math.Atan2(Y, X);
        }

        public Angle(Vector2 point, Vector2 centerPoint)
        {
            X = point.X - centerPoint.X;
            Y = point.Y - centerPoint.Y;
            A = (float)System.Math.Atan2(Y, X);
        }

        public Angle(Triangle triangle)
        {
            X = triangle.LeftBottom.X + triangle.LeftBottom.X - triangle.LeftTop.X - triangle.LeftTop.X;
            Y = triangle.LeftBottom.Y + triangle.LeftBottom.Y - triangle.LeftTop.Y - triangle.LeftTop.Y;
            A = (float)System.Math.Atan2(Y, X);
        }

        public Angle(Quadrilateral quad)
        {
            X = quad.RightBottom.X + quad.LeftBottom.X - quad.LeftTop.X - quad.RightTop.X;
            Y = quad.RightBottom.Y + quad.LeftBottom.Y - quad.LeftTop.Y - quad.RightTop.Y;
            A = (float)System.Math.Atan2(Y, X);
        }
        #endregion Constructors

        #region Public Static Operators
        public static implicit operator float(Angle a)
        {
            return a.A;
        }
        #endregion Public Static Operators
    }
}