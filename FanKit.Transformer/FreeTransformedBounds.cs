using System.Numerics;

namespace FanKit.Transformer
{
    public readonly struct FreeTransformedBounds
    {
        public readonly Bounds X;
        public readonly Bounds Y;
        public readonly Bounds Z;

        public readonly Vector3 A;
        public readonly Vector3 B;
        public readonly Vector3 C;
        public readonly Vector3 D;

        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        #region Constructors
        public FreeTransformedBounds(Vector2 position)
        {
            this.X = new Bounds
            {
                Left = position.X,
                Top = 0f,
                Right = position.X,
                Bottom = 0f,
            };
            this.Y = new Bounds
            {
                Left = 0f,
                Top = position.Y,
                Right = 0f,
                Bottom = position.Y,
            };
            this.Z = new Bounds
            {
                Left = 0f,
                Top = 0f,
                Right = 0f,
                Bottom = 0f,
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

        public FreeTransformedBounds(float left, float top, float right, float bottom)
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
            this.Z = new Bounds
            {
                Left = 0f,
                Top = 0f,
                Right = 0f,
                Bottom = 0f,
            };

            this.A = new Vector3
            {
                X = left,
                Y = top,
                Z = 1f,
            };
            this.B = new Vector3
            {
                X = right,
                Y = top,
                Z = 1f,
            };
            this.C = new Vector3
            {
                X = left,
                Y = bottom,
                Z = 1f,
            };
            this.D = new Vector3
            {
                X = right,
                Y = bottom,
                Z = 1f,
            };

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.LeftBottom = new Vector2(left, bottom);
            this.RightBottom = new Vector2(right, bottom);
        }

        public FreeTransformedBounds(float left, float top, float right, float bottom, Matrix4x4 matrix)
        {
            this.X = new Bounds
            {
                Left = matrix.M11 * left,
                Top = matrix.M21 * top,
                Right = matrix.M11 * right,
                Bottom = matrix.M21 * bottom,
            };
            this.Y = new Bounds
            {
                Left = matrix.M12 * left,
                Top = matrix.M22 * top,
                Right = matrix.M12 * right,
                Bottom = matrix.M22 * bottom,
            };
            this.Z = new Bounds
            {
                Left = matrix.M14 * left,
                Top = matrix.M24 * top,
                Right = matrix.M14 * right,
                Bottom = matrix.M24 * bottom,
            };

            this.A = new Vector3
            {
                X = this.X.Left + this.X.Top + matrix.M41,
                Y = this.Y.Left + this.Y.Top + matrix.M42,
                Z = this.Z.Left + this.Z.Top + matrix.M44,
            };
            this.B = new Vector3
            {
                X = this.X.Right + this.X.Top + matrix.M41,
                Y = this.Y.Right + this.Y.Top + matrix.M42,
                Z = this.Z.Right + this.Z.Top + matrix.M44,
            };
            this.C = new Vector3
            {
                X = this.X.Left + this.X.Bottom + matrix.M41,
                Y = this.Y.Left + this.Y.Bottom + matrix.M42,
                Z = this.Z.Left + this.Z.Bottom + matrix.M44,
            };
            this.D = new Vector3
            {
                X = this.X.Right + this.X.Bottom + matrix.M41,
                Y = this.Y.Right + this.Y.Bottom + matrix.M42,
                Z = this.Z.Right + this.Z.Bottom + matrix.M44,
            };

            this.LeftTop = new Vector2(this.A.X / this.A.Z, this.A.Y / this.A.Z);
            this.RightTop = new Vector2(this.B.X / this.B.Z, this.B.Y / this.B.Z);
            this.LeftBottom = new Vector2(this.C.X / this.C.Z, this.C.Y / this.C.Z);
            this.RightBottom = new Vector2(this.D.X / this.D.Z, this.D.Y / this.D.Z);
        }

        public FreeTransformedBounds(Bounds bounds)
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
            this.Z = new Bounds
            {
                Left = 0f,
                Top = 0f,
                Right = 0f,
                Bottom = 0f,
            };

            this.A = new Vector3
            {
                X = bounds.Left,
                Y = bounds.Top,
                Z = 1f,
            };
            this.B = new Vector3
            {
                X = bounds.Right,
                Y = bounds.Top,
                Z = 1f,
            };
            this.C = new Vector3
            {
                X = bounds.Left,
                Y = bounds.Bottom,
                Z = 1f,
            };
            this.D = new Vector3
            {
                X = bounds.Right,
                Y = bounds.Bottom,
                Z = 1f,
            };

            this.LeftTop = new Vector2(bounds.Left, bounds.Top);
            this.RightTop = new Vector2(bounds.Right, bounds.Top);
            this.LeftBottom = new Vector2(bounds.Left, bounds.Bottom);
            this.RightBottom = new Vector2(bounds.Right, bounds.Bottom);
        }

        public FreeTransformedBounds(Bounds bounds, Matrix4x4 matrix)
        {
            this.X = new Bounds
            {
                Left = matrix.M11 * bounds.Left,
                Top = matrix.M21 * bounds.Top,
                Right = matrix.M11 * bounds.Right,
                Bottom = matrix.M21 * bounds.Bottom,
            };
            this.Y = new Bounds
            {
                Left = matrix.M12 * bounds.Left,
                Top = matrix.M22 * bounds.Top,
                Right = matrix.M12 * bounds.Right,
                Bottom = matrix.M22 * bounds.Bottom,
            };
            this.Z = new Bounds
            {
                Left = matrix.M14 * bounds.Left,
                Top = matrix.M24 * bounds.Top,
                Right = matrix.M14 * bounds.Right,
                Bottom = matrix.M24 * bounds.Bottom,
            };

            this.A = new Vector3
            {
                X = this.X.Left + this.X.Top + matrix.M41,
                Y = this.Y.Left + this.Y.Top + matrix.M42,
                Z = this.Z.Left + this.Z.Top + matrix.M44,
            };
            this.B = new Vector3
            {
                X = this.X.Right + this.X.Top + matrix.M41,
                Y = this.Y.Right + this.Y.Top + matrix.M42,
                Z = this.Z.Right + this.Z.Top + matrix.M44,
            };
            this.C = new Vector3
            {
                X = this.X.Left + this.X.Bottom + matrix.M41,
                Y = this.Y.Left + this.Y.Bottom + matrix.M42,
                Z = this.Z.Left + this.Z.Bottom + matrix.M44,
            };
            this.D = new Vector3
            {
                X = this.X.Right + this.X.Bottom + matrix.M41,
                Y = this.Y.Right + this.Y.Bottom + matrix.M42,
                Z = this.Z.Right + this.Z.Bottom + matrix.M44,
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