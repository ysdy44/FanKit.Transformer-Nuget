using System.Numerics;

namespace FanKit.Transformer
{
    public readonly struct FreeTransformedSize
    {
        private readonly float XWidth;
        private readonly float XHeight;

        private readonly float YWidth;
        private readonly float YHeight;

        private readonly float ZWidth;
        private readonly float ZHeight;

        //private readonly Vector3 A;
        private readonly Vector3 B;
        private readonly Vector3 C;
        private readonly Vector3 D;

        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        #region Constructors
        public FreeTransformedSize(Vector2 position)
        {
            this.XWidth = 0f;
            this.XHeight = 0f;

            this.YWidth = 0f;
            this.YHeight = 0f;

            this.ZWidth = 0f;
            this.ZHeight = 0f;

            //this.A = new Vector3
            //{
            //    X = 0f,
            //    Y = 0f,
            //    Z = 1f,
            //};
            this.B = new Vector3
            {
                X = 0f,
                Y = 0f,
                Z = 1f,
            };
            this.C = new Vector3
            {
                X = 0f,
                Y = 0f,
                Z = 1f,
            };
            this.D = new Vector3
            {
                X = 0f,
                Y = 0f,
                Z = 1f,
            };

            this.LeftTop = position;
            this.RightTop = position;
            this.LeftBottom = position;
            this.RightBottom = position;
        }

        public FreeTransformedSize(float width, float height)
        {
            this.XWidth = width;
            this.XHeight = 0f;

            this.YWidth = 0f;
            this.YHeight = height;

            this.ZWidth = 0f;
            this.ZHeight = 0f;

            //this.A = new Vector3
            //{
            //    X = 0f,
            //    Y = 0f,
            //    Z = 1f,
            //};
            this.B = new Vector3
            {
                X = width,
                Y = 0f,
                Z = 1f,
            };
            this.C = new Vector3
            {
                X = 0f,
                Y = height,
                Z = 1f,
            };
            this.D = new Vector3
            {
                X = width,
                Y = height,
                Z = 1f,
            };

            this.LeftTop = Vector2.Zero;
            this.RightTop = new Vector2(width, 0f);
            this.LeftBottom = new Vector2(0f, height);
            this.RightBottom = new Vector2(width, height);
        }

        public FreeTransformedSize(float width, float height, Matrix4x4 matrix)
        {
            this.XWidth = matrix.M11 * width;
            this.XHeight = matrix.M21 * height;

            this.YWidth = matrix.M12 * width;
            this.YHeight = matrix.M22 * height;

            this.ZWidth = matrix.M14 * width;
            this.ZHeight = matrix.M24 * height;

            //this.A = new Vector3
            //{
            //    X = matrix.M41,
            //    Y = matrix.M42,
            //    Z = matrix.M44,
            //};
            this.B = new Vector3
            {
                X = XWidth + matrix.M41,
                Y = YWidth + matrix.M42,
                Z = ZWidth + matrix.M44,
            };
            this.C = new Vector3
            {
                X = XHeight + matrix.M41,
                Y = YHeight + matrix.M42,
                Z = ZHeight + matrix.M44,
            };
            this.D = new Vector3
            {
                X = XWidth + XHeight + matrix.M41,
                Y = YWidth + YHeight + matrix.M42,
                Z = ZWidth + ZHeight + matrix.M44,
            };

            //this.LeftTop = new Vector2(this.A.X / this.A.Z, this.A.Y / this.A.Z);
            this.LeftTop = new Vector2(matrix.M41 / matrix.M44, matrix.M42 / matrix.M44);
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