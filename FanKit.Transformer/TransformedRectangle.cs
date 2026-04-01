using System.Numerics;

namespace FanKit.Transformer
{
    public readonly struct TransformedRectangle
    {
        private readonly Rectangle X;
        private readonly Rectangle Y;

        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        #region Constructors
        public TransformedRectangle(Vector2 position)
        {
            this.X = new Rectangle(position);
            this.Y = new Rectangle(position);

            this.LeftTop = position;
            this.RightTop = position;
            this.LeftBottom = position;
            this.RightBottom = position;
        }

        public TransformedRectangle(float x, float y, float width, float height)
        {
            this.X = new Rectangle
            {
                X = x,
                Y = 0f,
                Width = width,
                Height = 0f,
            };
            this.Y = new Rectangle
            {
                X = 0f,
                Y = y,
                Width = 0f,
                Height = height,
            };

            this.LeftTop = new Vector2(x, y);
            this.RightTop = new Vector2(x + width, y);
            this.LeftBottom = new Vector2(x, y + height);
            this.RightBottom = new Vector2(x + width, y + height);
        }

        public TransformedRectangle(float x, float y, float width, float height, Matrix3x2 matrix)
        {
            this.X = new Rectangle
            {
                X = x * matrix.M11,
                Y = y * matrix.M21,
                Width = width * matrix.M11,
                Height = height * matrix.M21,
            };
            this.Y = new Rectangle
            {
                X = x * matrix.M12,
                Y = y * matrix.M22,
                Width = width * matrix.M12,
                Height = height * matrix.M22,
            };

            this.LeftTop = new Vector2(this.X.X + this.X.Y + matrix.M31,
                this.Y.X + this.Y.Y + matrix.M32);
            this.RightTop = new Vector2(this.X.X + this.X.Width + this.X.Y + matrix.M31,
                this.Y.X + this.Y.Width + this.Y.Y + matrix.M32);
            this.LeftBottom = new Vector2(this.X.X + this.X.Y + this.X.Height + matrix.M31,
                this.Y.X + this.Y.Y + this.Y.Height + matrix.M32);
            this.RightBottom = new Vector2(this.X.X + this.X.Width + this.X.Y + this.X.Height + matrix.M31,
                this.Y.X + this.Y.Width + this.Y.Y + this.Y.Height + matrix.M32);
        }

        public TransformedRectangle(Rectangle rect)
        {
            this.X = new Rectangle
            {
                X = rect.X,
                Y = 0f,
                Width = rect.Width,
                Height = 0f,
            };
            this.Y = new Rectangle
            {
                X = 0f,
                Y = rect.Y,
                Width = 0f,
                Height = rect.Height,
            };

            this.LeftTop = new Vector2(rect.X, rect.Y);
            this.RightTop = new Vector2(rect.X + rect.Width, rect.Y);
            this.LeftBottom = new Vector2(rect.X, rect.Y + rect.Height);
            this.RightBottom = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
        }

        public TransformedRectangle(Rectangle rect, Matrix3x2 matrix)
        {
            this.X = new Rectangle
            {
                X = rect.X * matrix.M11,
                Y = rect.Y * matrix.M21,
                Width = rect.Width * matrix.M11,
                Height = rect.Height * matrix.M21,
            };
            this.Y = new Rectangle
            {
                X = rect.X * matrix.M12,
                Y = rect.Y * matrix.M22,
                Width = rect.Width * matrix.M12,
                Height = rect.Height * matrix.M22,
            };

            this.LeftTop = new Vector2(this.X.X + this.X.Y + matrix.M31,
                this.Y.X + this.Y.Y + matrix.M32);
            this.RightTop = new Vector2(this.X.X + this.X.Width + this.X.Y + matrix.M31,
                this.Y.X + this.Y.Width + this.Y.Y + matrix.M32);
            this.LeftBottom = new Vector2(this.X.X + this.X.Y + this.X.Height + matrix.M31,
                this.Y.X + this.Y.Y + this.Y.Height + matrix.M32);
            this.RightBottom = new Vector2(this.X.X + this.X.Width + this.X.Y + this.X.Height + matrix.M31,
                this.Y.X + this.Y.Width + this.Y.Y + this.Y.Height + matrix.M32);
        }
        #endregion Constructors

        #region Public Instance Methods
        public Vector2[] To3Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.LeftBottom,
        };
        public Vector2[] To4Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.RightBottom,
            this.LeftBottom,
        };
        public Bounds ToBounds() => new Bounds
        {
            Left = System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X)),
            Top = System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y)),
            Right = System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X)),
            Bottom = System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y)),
        };
        public Triangle ToTriangle() => new Triangle
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
        };
        public Quadrilateral ToQuadrilateral() => new Quadrilateral
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
            RightBottom = this.RightBottom,
        };
        #endregion Public Instance Methods

        #region Public Static Operators
        #endregion Public Static Operators
    }
}