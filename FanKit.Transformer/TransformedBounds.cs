using System.Numerics;

namespace FanKit.Transformer
{
    public readonly struct TransformedBounds
    {
        private readonly Bounds X;
        private readonly Bounds Y;

        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        #region Constructors
        public TransformedBounds(Vector2 position)
        {
            this.X = new Bounds(position);
            this.Y = new Bounds(position);

            this.LeftTop = position;
            this.RightTop = position;
            this.LeftBottom = position;
            this.RightBottom = position;
        }

        public TransformedBounds(float left, float top, float right, float bottom)
        {
            this.X = new Bounds
            {
                Left = left,
                Top = 0f,
                Right = right,
                Bottom = 0f,
            };
            this.Y = new Bounds
            {
                Left = 0f,
                Top = top,
                Right = 0f,
                Bottom = bottom,
            };

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.LeftBottom = new Vector2(left, bottom);
            this.RightBottom = new Vector2(right, bottom);
        }

        public TransformedBounds(float left, float top, float right, float bottom, Matrix3x2 matrix)
        {
            this.X = new Bounds
            {
                Left = left * matrix.M11,
                Top = top * matrix.M21,
                Right = right * matrix.M11,
                Bottom = bottom * matrix.M21,
            };
            this.Y = new Bounds
            {
                Left = left * matrix.M12,
                Top = top * matrix.M22,
                Right = right * matrix.M12,
                Bottom = bottom * matrix.M22,
            };

            this.LeftTop = new Vector2(this.X.Left + this.X.Top + matrix.M31,
                this.Y.Left + this.Y.Top + matrix.M32);
            this.RightTop = new Vector2(this.X.Right + this.X.Top + matrix.M31,
                this.Y.Right + this.Y.Top + matrix.M32);
            this.LeftBottom = new Vector2(this.X.Left + this.X.Bottom + matrix.M31,
                this.Y.Left + this.Y.Bottom + matrix.M32);
            this.RightBottom = new Vector2(this.X.Right + this.X.Bottom + matrix.M31,
                this.Y.Right + this.Y.Bottom + matrix.M32);
        }

        public TransformedBounds(Bounds bounds)
        {
            this.X = new Bounds
            {
                Left = bounds.Left,
                Top = 0f,
                Right = bounds.Right,
                Bottom = 0f,
            };
            this.Y = new Bounds
            {
                Left = 0f,
                Top = bounds.Top,
                Right = 0f,
                Bottom = bounds.Bottom,
            };

            this.LeftTop = new Vector2(bounds.Left, bounds.Top);
            this.RightTop = new Vector2(bounds.Right, bounds.Top);
            this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
            this.RightBottom = new Vector2(bounds.Right, bounds.Bottom);
        }

        public TransformedBounds(Bounds bounds, Matrix3x2 matrix)
        {
            this.X = new Bounds
            {
                Left = bounds.Left * matrix.M11,
                Top = bounds.Top * matrix.M21,
                Right = bounds.Right * matrix.M11,
                Bottom = bounds.Bottom * matrix.M21,
            };
            this.Y = new Bounds
            {
                Left = bounds.Left * matrix.M12,
                Top = bounds.Top * matrix.M22,
                Right = bounds.Right * matrix.M12,
                Bottom = bounds.Bottom * matrix.M22,
            };

            this.LeftTop = new Vector2(this.X.Left + this.X.Top + matrix.M31,
                this.Y.Left + this.Y.Top + matrix.M32);
            this.RightTop = new Vector2(this.X.Right + this.X.Top + matrix.M31,
                this.Y.Right + this.Y.Top + matrix.M32);
            this.LeftBottom = new Vector2(this.X.Left + this.X.Bottom + matrix.M31,
                this.Y.Left + this.Y.Bottom + matrix.M32);
            this.RightBottom = new Vector2(this.X.Right + this.X.Bottom + matrix.M31,
                this.Y.Right + this.Y.Bottom + matrix.M32);
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