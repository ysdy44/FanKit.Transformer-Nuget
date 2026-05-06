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

        public Box0(Bounds bounds, Matrix3x2 canvasMatrix)
            : this(Mathematics.Math.Transform(bounds.Left, bounds.Top, canvasMatrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Top, canvasMatrix),
                  Mathematics.Math.Transform(bounds.Left, bounds.Bottom, canvasMatrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Bottom, canvasMatrix))
        {
        }

        public Box0(Bounds bounds, ICanvasMatrix canvasMatrix)
            : this(canvasMatrix.Transform(bounds.Left, bounds.Top),
                  canvasMatrix.Transform(bounds.Right, bounds.Top),
                  canvasMatrix.Transform(bounds.Left, bounds.Bottom),
                  canvasMatrix.Transform(bounds.Right, bounds.Bottom))
        {
        }

        public Box0(Bounds bounds, ICanvasInverseMatrix inverseMatrix)
            : this(inverseMatrix.InverseTransform(bounds.Left, bounds.Top),
                  inverseMatrix.InverseTransform(bounds.Right, bounds.Top),
                  inverseMatrix.InverseTransform(bounds.Left, bounds.Bottom),
                  inverseMatrix.InverseTransform(bounds.Right, bounds.Bottom))
        {
        }

        public Box0(Triangle triangle)
            : this(triangle.LeftTop,
                  triangle.RightTop,
                  triangle.LeftBottom)
        {
        }

        public Box0(Triangle triangle, Matrix3x2 canvasMatrix)
            : this(Vector2.Transform(triangle.LeftTop, canvasMatrix),
                  Vector2.Transform(triangle.RightTop, canvasMatrix),
                  Vector2.Transform(triangle.LeftBottom, canvasMatrix))
        {
        }

        public Box0(Triangle triangle, ICanvasMatrix canvasMatrix)
            : this(canvasMatrix.Transform(triangle.LeftTop),
                  canvasMatrix.Transform(triangle.RightTop),
                  canvasMatrix.Transform(triangle.LeftBottom))
        {
        }

        public Box0(Triangle triangle, ICanvasInverseMatrix inverseMatrix)
            : this(inverseMatrix.InverseTransform(triangle.LeftTop),
                  inverseMatrix.InverseTransform(triangle.RightTop),
                  inverseMatrix.InverseTransform(triangle.LeftBottom))
        {
        }

        public Box0(Quadrilateral quad)
            : this(quad.LeftTop,
                  quad.RightTop,
                  quad.LeftBottom,
                  quad.RightBottom)
        {
        }

        public Box0(Quadrilateral quad, Matrix3x2 canvasMatrix)
            : this(Vector2.Transform(quad.LeftTop, canvasMatrix),
                  Vector2.Transform(quad.RightTop, canvasMatrix),
                  Vector2.Transform(quad.LeftBottom, canvasMatrix),
                  Vector2.Transform(quad.RightBottom, canvasMatrix))
        {
        }

        public Box0(Quadrilateral quad, ICanvasMatrix canvasMatrix)
            : this(canvasMatrix.Transform(quad.LeftTop),
                  canvasMatrix.Transform(quad.RightTop),
                  canvasMatrix.Transform(quad.LeftBottom),
                  canvasMatrix.Transform(quad.RightBottom))
        {
        }

        public Box0(Quadrilateral quad, ICanvasInverseMatrix inverseMatrix)
            : this(inverseMatrix.InverseTransform(quad.LeftTop),
                  inverseMatrix.InverseTransform(quad.RightTop),
                  inverseMatrix.InverseTransform(quad.LeftBottom),
                  inverseMatrix.InverseTransform(quad.RightBottom))
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