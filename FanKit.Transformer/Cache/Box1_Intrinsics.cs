using System.Numerics;

namespace FanKit.Transformer.Cache
{
    partial struct Box1
    {
        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        // Sides
        public readonly float SideLeftX;
        public readonly float SideLeftY;
        public readonly float SideLeftLengthSquared;

        public readonly float SideTopX;
        public readonly float SideTopY;
        public readonly float SideTopLengthSquared;

        public readonly float SideRightX;
        public readonly float SideRightY;
        public readonly float SideRightLengthSquared;

        public readonly float SideBottomX;
        public readonly float SideBottomY;
        public readonly float SideBottomLengthSquared;

        public readonly Vector2 CenterLeft;
        public readonly Vector2 CenterTop;
        public readonly Vector2 CenterRight;
        public readonly Vector2 CenterBottom;

        // Center
        public readonly Vector2 Center;

        #region Constructors
        private Box1(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom)
         : this(leftTop,
               rightTop,
               leftBottom,
               new Vector2(rightTop.X + leftBottom.X - leftTop.X,
                   rightTop.Y + leftBottom.Y - leftTop.Y)) // Triangle -> Transformer
        {
        }

        private Box1(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, Vector2 rightBottom)
        {
            // Corners
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.LeftBottom = leftBottom;
            this.RightBottom = rightBottom;

            // Sides
            this.SideLeftX = this.LeftBottom.X - this.LeftTop.X;
            this.SideLeftY = this.LeftBottom.Y - this.LeftTop.Y;
            this.SideLeftLengthSquared = this.SideLeftX * this.SideLeftX + this.SideLeftY * this.SideLeftY;

            this.SideTopX = this.LeftTop.X - this.RightTop.X;
            this.SideTopY = this.LeftTop.Y - this.RightTop.Y;
            this.SideTopLengthSquared = this.SideTopX * this.SideTopX + this.SideTopY * this.SideTopY;

            this.SideRightX = this.RightTop.X - this.RightBottom.X;
            this.SideRightY = this.RightTop.Y - this.RightBottom.Y;
            this.SideRightLengthSquared = this.SideRightX * this.SideRightX + this.SideRightY * this.SideRightY;

            this.SideBottomX = this.RightBottom.X - this.LeftBottom.X;
            this.SideBottomY = this.RightBottom.Y - this.LeftBottom.Y;
            this.SideBottomLengthSquared = this.SideBottomX * this.SideBottomX + this.SideBottomY * this.SideBottomY;

            this.CenterLeft = new Vector2((this.LeftTop.X + this.LeftBottom.X) / 2f,
                (this.LeftTop.Y + this.LeftBottom.Y) / 2f);
            this.CenterTop = new Vector2((this.LeftTop.X + this.RightTop.X) / 2f,
                (this.LeftTop.Y + this.RightTop.Y) / 2f);
            this.CenterRight = new Vector2((this.RightTop.X + this.RightBottom.X) / 2f,
                (this.RightTop.Y + this.RightBottom.Y) / 2f);
            this.CenterBottom = new Vector2((this.RightBottom.X + this.LeftBottom.X) / 2f,
                (this.RightBottom.Y + this.LeftBottom.Y) / 2f);

            // Center
            this.Center = new Vector2((this.LeftTop.X + this.RightTop.X + this.RightBottom.X + this.LeftBottom.X) / 2f,
                (this.LeftTop.Y + this.RightTop.Y + this.RightBottom.Y + this.LeftBottom.Y) / 2f);
        }

        public Box1(Bounds bounds)
            : this(new Vector2(bounds.Left, bounds.Top),
                  new Vector2(bounds.Right, bounds.Top),
                  new Vector2(bounds.Left, bounds.Bottom),
                  new Vector2(bounds.Right, bounds.Bottom))
        {
        }

        public Box1(Bounds bounds, Matrix3x2 matrix)
            : this(Mathematics.Math.Transform(bounds.Left, bounds.Top, matrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Top, matrix),
                  Mathematics.Math.Transform(bounds.Left, bounds.Bottom, matrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Bottom, matrix))
        {
        }

        public Box1(Bounds bounds, ICanvasMatrix matrix)
            : this(matrix.Transform(bounds.Left, bounds.Top),
                  matrix.Transform(bounds.Right, bounds.Top),
                  matrix.Transform(bounds.Left, bounds.Bottom),
                  matrix.Transform(bounds.Right, bounds.Bottom))
        {
        }

        public Box1(Bounds bounds, ICanvasInverseMatrix inverseMatrix)
            : this(inverseMatrix.InverseTransform(bounds.Left, bounds.Top),
                  inverseMatrix.InverseTransform(bounds.Right, bounds.Top),
                  inverseMatrix.InverseTransform(bounds.Left, bounds.Bottom),
                  inverseMatrix.InverseTransform(bounds.Right, bounds.Bottom))
        {
        }

        public Box1(Triangle triangle)
            : this(triangle.LeftTop,
                  triangle.RightTop,
                  triangle.LeftBottom)
        {
        }

        public Box1(Triangle triangle, Matrix3x2 matrix)
            : this(Vector2.Transform(triangle.LeftTop, matrix),
                  Vector2.Transform(triangle.RightTop, matrix),
                  Vector2.Transform(triangle.LeftBottom, matrix))
        {
        }

        public Box1(Triangle triangle, ICanvasMatrix matrix)
            : this(matrix.Transform(triangle.LeftTop),
                  matrix.Transform(triangle.RightTop),
                  matrix.Transform(triangle.LeftBottom))
        {
        }

        public Box1(Triangle triangle, ICanvasInverseMatrix inverseMatrix)
            : this(inverseMatrix.InverseTransform(triangle.LeftTop),
                  inverseMatrix.InverseTransform(triangle.RightTop),
                  inverseMatrix.InverseTransform(triangle.LeftBottom))
        {
        }

        public Box1(Quadrilateral quad)
            : this(quad.LeftTop,
                  quad.RightTop,
                  quad.LeftBottom,
                  quad.RightBottom)
        {
        }

        public Box1(Quadrilateral quad, Matrix3x2 matrix)
            : this(Vector2.Transform(quad.LeftTop, matrix),
                  Vector2.Transform(quad.RightTop, matrix),
                  Vector2.Transform(quad.LeftBottom, matrix),
                  Vector2.Transform(quad.RightBottom, matrix))
        {
        }

        public Box1(Quadrilateral quad, ICanvasMatrix matrix)
            : this(matrix.Transform(quad.LeftTop),
                  matrix.Transform(quad.RightTop),
                  matrix.Transform(quad.LeftBottom),
                  matrix.Transform(quad.RightBottom))
        {
        }

        public Box1(Quadrilateral quad, ICanvasInverseMatrix inverseMatrix)
            : this(inverseMatrix.InverseTransform(quad.LeftTop),
                  inverseMatrix.InverseTransform(quad.RightTop),
                  inverseMatrix.InverseTransform(quad.LeftBottom),
                  inverseMatrix.InverseTransform(quad.RightBottom))
        {
        }
        #endregion Constructors

        #region Public Static Operators
        public static implicit operator Triangle(Box1 box)
        {
            return new Triangle
            {
                LeftTop = box.LeftTop,
                RightTop = box.RightTop,
                LeftBottom = box.LeftBottom,
            };
        }

        public static implicit operator Quadrilateral(Box1 box)
        {
            return new Quadrilateral
            {
                LeftTop = box.LeftTop,
                RightTop = box.RightTop,
                LeftBottom = box.LeftBottom,
                RightBottom = box.RightBottom,
            };
        }
        #endregion Public Static Operators
    }
}