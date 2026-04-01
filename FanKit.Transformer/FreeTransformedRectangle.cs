using System.Numerics;

namespace FanKit.Transformer
{
    public readonly struct FreeTransformedRectangle
    {
        private readonly Rectangle X;
        private readonly Rectangle Y;
        private readonly Rectangle Z;

        private readonly Vector3 A;
        private readonly Vector3 B;
        private readonly Vector3 C;
        private readonly Vector3 D;

        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        #region Constructors
        public FreeTransformedRectangle(Vector2 position)
        {
            this.X = new Rectangle
            {
                X = position.X,
                Y = 0f,
                Width = 0f,
                Height = 0f,
            };
            this.Y = new Rectangle
            {
                X = 0f,
                Y = position.Y,
                Width = 0f,
                Height = 0f,
            };
            this.Z = new Rectangle
            {
                X = 0f,
                Y = 0f,
                Width = 0f,
                Height = 0f,
            };

            this.A = new Vector3
            {
                X = position.X,
                Y = position.Y,
                Z = 1f,
            };
            this.B = new Vector3
            {
                X = position.X,
                Y = position.Y,
                Z = 1f,
            };
            this.C = new Vector3
            {
                X = position.X,
                Y = position.Y,
                Z = 1f,
            };
            this.D = new Vector3
            {
                X = position.X,
                Y = position.Y,
                Z = 1f,
            };

            this.LeftTop = new Vector2(position.X, position.Y);
            this.RightTop = new Vector2(position.X, position.Y);
            this.LeftBottom = new Vector2(position.X, position.Y);
            this.RightBottom = new Vector2(position.X, position.Y);
        }

        public FreeTransformedRectangle(float x, float y, float width, float height)
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
            this.Z = new Rectangle
            {
                X = 0f,
                Y = 0f,
                Width = 0f,
                Height = 0f,
            };

            this.A = new Vector3
            {
                X = x,
                Y = y,
                Z = 0f,
            };
            this.B = new Vector3
            {
                X = x + width,
                Y = y,
                Z = 0f,
            };
            this.C = new Vector3
            {
                X = x,
                Y = y + height,
                Z = 0f,
            };
            this.D = new Vector3
            {
                X = x + width,
                Y = y + height,
                Z = 0f
            };

            this.LeftTop = new Vector2(this.A.X, this.A.Y);
            this.RightTop = new Vector2(this.B.X, this.B.Y);
            this.LeftBottom = new Vector2(this.C.X, this.C.Y);
            this.RightBottom = new Vector2(this.D.X, this.D.Y);
        }

        public FreeTransformedRectangle(float x, float y, float width, float height, Matrix4x4 matrix)
        {
            this.X = new Rectangle
            {
                X = matrix.M11 * x,
                Y = matrix.M21 * y,
                Width = matrix.M11 * width,
                Height = matrix.M21 * height,
            };
            this.Y = new Rectangle
            {
                X = matrix.M12 * x,
                Y = matrix.M22 * y,
                Width = matrix.M12 * width,
                Height = matrix.M22 * height,
            };
            this.Z = new Rectangle
            {
                X = matrix.M14 * x,
                Y = matrix.M24 * y,
                Width = matrix.M14 * width,
                Height = matrix.M24 * height,
            };

            this.A = new Vector3
            {
                X = this.X.X + this.X.Y + matrix.M41,
                Y = this.Y.X + this.Y.Y + matrix.M42,
                Z = this.Z.X + this.Z.Y + matrix.M44,
            };
            this.B = new Vector3
            {
                X = this.X.X + this.X.Width + this.X.Y + matrix.M41,
                Y = this.Y.X + this.Y.Width + this.Y.Y + matrix.M42,
                Z = this.Z.X + this.Z.Width + this.Z.Y + matrix.M44,
            };
            this.C = new Vector3
            {
                X = this.X.X + this.X.Y + this.X.Height + matrix.M41,
                Y = this.Y.X + this.Y.Y + this.Y.Height + matrix.M42,
                Z = this.Z.X + this.Z.Y + this.Z.Height + matrix.M44,
            };
            this.D = new Vector3
            {
                X = this.X.X + this.X.Width + this.X.Y + this.X.Height + matrix.M41,
                Y = this.Y.X + this.Y.Width + this.Y.Y + this.Y.Height + matrix.M42,
                Z = this.Z.X + this.Z.Width + this.Z.Y + this.Z.Height + matrix.M44,
            };

            this.LeftTop = new Vector2(this.A.X / this.A.Z, this.A.Y / this.A.Z);
            this.RightTop = new Vector2(this.B.X / this.B.Z, this.B.Y / this.B.Z);
            this.LeftBottom = new Vector2(this.C.X / this.C.Z, this.C.Y / this.C.Z);
            this.RightBottom = new Vector2(this.D.X / this.D.Z, this.D.Y / this.D.Z);
        }

        public FreeTransformedRectangle(Rectangle rect)
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
            this.Z = new Rectangle
            {
                X = 0f,
                Y = 0f,
                Width = 0f,
                Height = 0f,
            };

            this.A = new Vector3
            {
                X = rect.X,
                Y = rect.Y,
                Z = 0f,
            };
            this.B = new Vector3
            {
                X = rect.X + rect.Width,
                Y = rect.Y,
                Z = 0f,
            };
            this.C = new Vector3
            {
                X = rect.X,
                Y = rect.Y + rect.Height,
                Z = 0f,
            };
            this.D = new Vector3
            {
                X = rect.X + rect.Width,
                Y = rect.Y + rect.Height,
                Z = 0f
            };

            this.LeftTop = new Vector2(this.A.X, this.A.Y);
            this.RightTop = new Vector2(this.B.X, this.B.Y);
            this.LeftBottom = new Vector2(this.C.X, this.C.Y);
            this.RightBottom = new Vector2(this.D.X, this.D.Y);
        }

        public FreeTransformedRectangle(Rectangle rect, Matrix4x4 matrix)
        {
            this.X = new Rectangle
            {
                X = matrix.M11 * rect.X,
                Y = matrix.M21 * rect.Y,
                Width = matrix.M11 * rect.Width,
                Height = matrix.M21 * rect.Height,
            };
            this.Y = new Rectangle
            {
                X = matrix.M12 * rect.X,
                Y = matrix.M22 * rect.Y,
                Width = matrix.M12 * rect.Width,
                Height = matrix.M22 * rect.Height,
            };
            this.Z = new Rectangle
            {
                X = matrix.M14 * rect.X,
                Y = matrix.M24 * rect.Y,
                Width = matrix.M14 * rect.Width,
                Height = matrix.M24 * rect.Height,
            };

            this.A = new Vector3
            {
                X = this.X.X + this.X.Y + matrix.M41,
                Y = this.Y.X + this.Y.Y + matrix.M42,
                Z = this.Z.X + this.Z.Y + matrix.M44,
            };
            this.B = new Vector3
            {
                X = this.X.X + this.X.Width + this.X.Y + matrix.M41,
                Y = this.Y.X + this.Y.Width + this.Y.Y + matrix.M42,
                Z = this.Z.X + this.Z.Width + this.Z.Y + matrix.M44,
            };
            this.C = new Vector3
            {
                X = this.X.X + this.X.Y + this.X.Height + matrix.M41,
                Y = this.Y.X + this.Y.Y + this.Y.Height + matrix.M42,
                Z = this.Z.X + this.Z.Y + this.Z.Height + matrix.M44,
            };
            this.D = new Vector3
            {
                X = this.X.X + this.X.Width + this.X.Y + this.X.Height + matrix.M41,
                Y = this.Y.X + this.Y.Width + this.Y.Y + this.Y.Height + matrix.M42,
                Z = this.Z.X + this.Z.Width + this.Z.Y + this.Z.Height + matrix.M44,
            };

            this.LeftTop = new Vector2(this.A.X / this.A.Z, this.A.Y / this.A.Z);
            this.RightTop = new Vector2(this.B.X / this.B.Z, this.B.Y / this.B.Z);
            this.LeftBottom = new Vector2(this.C.X / this.C.Z, this.C.Y / this.C.Z);
            this.RightBottom = new Vector2(this.D.X / this.D.Z, this.D.Y / this.D.Z);
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