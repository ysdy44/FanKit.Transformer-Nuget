using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer
{
    partial struct Rectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        #region Constructors
        public Rectangle(Vector2 point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Width = 0f;
            this.Height = 0f;
        }

        public Rectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Rectangle(float width, float height)
        {
            this.X = 0f;
            this.Y = 0f;
            this.Width = width;
            this.Height = height;
        }

        public Rectangle(Vector2 point1, Vector2 point2)
        {
            this.X = System.Math.Min(point1.X, point2.X);
            this.Y = System.Math.Min(point1.Y, point2.Y);

            this.Width = System.Math.Abs(point1.X - point2.X);
            this.Height = System.Math.Abs(point1.Y - point2.Y);
        }

        public Rectangle(Bounds bounds)
        {
            this.X = System.Math.Min(bounds.Left, bounds.Right);
            this.Y = System.Math.Min(bounds.Top, bounds.Bottom);

            this.Width = System.Math.Abs(bounds.Left - bounds.Right);
            this.Height = System.Math.Abs(bounds.Top - bounds.Bottom);
        }
        #endregion Constructors

        #region Public Instance Methods
        public bool Equals(Rectangle other)
        {
            return
                this.X == other.X &&
                this.Y == other.Y &&
                this.Width == other.Width &&
                this.Height == other.Height;
        }
        #endregion Public Instance Methods

        #region Public Static Operators
        public static Rectangle operator +(Rectangle left, Vector2 right)
        {
            return new Rectangle
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Width = left.Width,
                Height = left.Height,
            };
        }

        public static Rectangle operator +(Vector2 left, Rectangle right)
        {
            return new Rectangle
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Width = right.Width,
                Height = right.Height,
            };
        }

        public static Rectangle operator +(Rectangle left, Rectangle right)
        {
            return new Rectangle
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Width = left.Width + right.Width,
                Height = left.Height + right.Height,
            };
        }

        public static Rectangle operator -(Rectangle left, Vector2 right)
        {
            return new Rectangle
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Width = left.Width,
                Height = left.Height,
            };
        }

        public static Rectangle operator -(Vector2 left, Rectangle right)
        {
            return new Rectangle
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Width = right.Width,
                Height = right.Height,
            };
        }

        public static Rectangle operator -(Rectangle left, Rectangle right)
        {
            return new Rectangle
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Width = left.Width - right.Width,
                Height = left.Height - right.Height,
            };
        }

        public static TransformedRectangle operator *(Rectangle left, Matrix3x2 right)
        {
            return new TransformedRectangle(left, right);
        }

        public static FreeTransformedRectangle operator *(Rectangle left, Matrix4x4 right)
        {
            return new FreeTransformedRectangle(left, right);
        }

        public static Rectangle operator *(Rectangle left, Vector2 right)
        {
            return new Rectangle
            {
                X = left.X,
                Y = left.Y,
                Width = left.Width * right.X,
                Height = left.Height * right.Y,
            };
        }

        public static Rectangle operator *(Rectangle left, float right)
        {
            return new Rectangle
            {
                X = left.X,
                Y = left.Y,
                Width = left.Width * right,
                Height = left.Height * right,
            };
        }

        public static Rectangle operator *(float left, Rectangle right)
        {
            return new Rectangle
            {
                X = right.X,
                Y = right.Y,
                Width = left * right.Width,
                Height = left * right.Height,
            };
        }

        public static Rectangle operator /(Rectangle left, Vector2 right)
        {
            return new Rectangle
            {
                X = left.X,
                Y = left.Y,
                Width = left.Width / right.X,
                Height = left.Height / right.Y,
            };
        }

        public static Rectangle operator /(Rectangle left, float right)
        {
            return new Rectangle
            {
                X = left.X,
                Y = left.Y,
                Width = left.Width / right,
                Height = left.Height / right,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle operator -(Rectangle value)
        {
            return new Rectangle
            {
                X = -value.X,
                Y = -value.Y,
                Width = -value.Width,
                Height = -value.Height,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }
        #endregion Public Static Operators
    }
}