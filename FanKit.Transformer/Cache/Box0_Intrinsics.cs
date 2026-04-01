using System.Numerics;

namespace FanKit.Transformer.Cache
{
    partial struct Box0
    {
        // Corners
        public readonly Vector2 LeftTop;
        public readonly Vector2 RightTop;
        public readonly Vector2 LeftBottom;
        public readonly Vector2 RightBottom;

        #region Constructors
        private Box0(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom)
         : this(leftTop,
               rightTop,
               leftBottom,
               new Vector2(rightTop.X + leftBottom.X - leftTop.X,
                   rightTop.Y + leftBottom.Y - leftTop.Y)) // Triangle -> Transformer
        {
        }

        private Box0(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, Vector2 rightBottom)
        {
            // Corners
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.LeftBottom = leftBottom;
            this.RightBottom = rightBottom;
        }

        public Box0(Bounds bounds)
            : this(new Vector2(bounds.Left, bounds.Top),
                  new Vector2(bounds.Right, bounds.Top),
                  new Vector2(bounds.Left, bounds.Bottom),
                  new Vector2(bounds.Right, bounds.Bottom))
        {
        }

        public Box0(Bounds bounds, Matrix3x2 matrix)
            : this(Mathematics.Math.Transform(bounds.Left, bounds.Top, matrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Top, matrix),
                  Mathematics.Math.Transform(bounds.Left, bounds.Bottom, matrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Bottom, matrix))
        {
        }

        public Box0(Bounds bounds, ICanvasMatrix matrix)
            : this(matrix.Transform(bounds.Left, bounds.Top),
                  matrix.Transform(bounds.Right, bounds.Top),
                  matrix.Transform(bounds.Left, bounds.Bottom),
                  matrix.Transform(bounds.Right, bounds.Bottom))
        {
        }

        public Box0(Bounds bounds, ICanvasInverseMatrix matrix)
            : this(matrix.InverseTransform(bounds.Left, bounds.Top),
                  matrix.InverseTransform(bounds.Right, bounds.Top),
                  matrix.InverseTransform(bounds.Left, bounds.Bottom),
                  matrix.InverseTransform(bounds.Right, bounds.Bottom))
        {
        }

        public Box0(Triangle triangle)
            : this(triangle.LeftTop,
                  triangle.RightTop,
                  triangle.LeftBottom)
        {
        }

        public Box0(Triangle triangle, Matrix3x2 matrix)
            : this(Vector2.Transform(triangle.LeftTop, matrix),
                  Vector2.Transform(triangle.RightTop, matrix),
                  Vector2.Transform(triangle.LeftBottom, matrix))
        {
        }

        public Box0(Triangle triangle, ICanvasMatrix matrix)
            : this(matrix.Transform(triangle.LeftTop),
                  matrix.Transform(triangle.RightTop),
                  matrix.Transform(triangle.LeftBottom))
        {
        }

        public Box0(Triangle triangle, ICanvasInverseMatrix matrix)
            : this(matrix.InverseTransform(triangle.LeftTop),
                  matrix.InverseTransform(triangle.RightTop),
                  matrix.InverseTransform(triangle.LeftBottom))
        {
        }

        public Box0(Quadrilateral quad)
            : this(quad.LeftTop,
                  quad.RightTop,
                  quad.LeftBottom,
                  quad.RightBottom)
        {
        }

        public Box0(Quadrilateral quad, Matrix3x2 matrix)
            : this(Vector2.Transform(quad.LeftTop, matrix),
                  Vector2.Transform(quad.RightTop, matrix),
                  Vector2.Transform(quad.LeftBottom, matrix),
                  Vector2.Transform(quad.RightBottom, matrix))
        {
        }

        public Box0(Quadrilateral quad, ICanvasMatrix matrix)
            : this(matrix.Transform(quad.LeftTop),
                  matrix.Transform(quad.RightTop),
                  matrix.Transform(quad.LeftBottom),
                  matrix.Transform(quad.RightBottom))
        {
        }

        public Box0(Quadrilateral quad, ICanvasInverseMatrix matrix)
            : this(matrix.InverseTransform(quad.LeftTop),
                  matrix.InverseTransform(quad.RightTop),
                  matrix.InverseTransform(quad.LeftBottom),
                  matrix.InverseTransform(quad.RightBottom))
        {
        }
        #endregion Constructors

        #region Public Static Operators
        public static implicit operator Triangle(Box0 box)
        {
            return new Triangle
            {
                LeftTop = box.LeftTop,
                RightTop = box.RightTop,
                LeftBottom = box.LeftBottom,
            };
        }

        public static implicit operator Quadrilateral(Box0 box)
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