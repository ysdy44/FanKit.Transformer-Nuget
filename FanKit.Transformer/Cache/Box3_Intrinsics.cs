using System.Numerics;

namespace FanKit.Transformer.Cache
{
    partial struct Box3
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

        // Handle Sides
        public readonly float HorizontalX;
        public readonly float HorizontalY;
        public readonly float HorizontalLengthSquared;

        public readonly float HorizontalLength;
        public readonly float HandleHorizontalX;
        public readonly float HandleHorizontalY;

        public readonly float VerticalX;
        public readonly float VerticalY;
        public readonly float VerticalLengthSquared;

        public readonly float VerticalLength;
        public readonly float HandleVerticalX;
        public readonly float HandleVerticalY;

        public readonly Vector2 HandleLeft;
        public readonly Vector2 HandleTop;
        public readonly Vector2 HandleRight;
        public readonly Vector2 HandleBottom;

        // Handle Corners
        public readonly float DecreaseX;
        public readonly float DecreaseY;
        public readonly float DecreaseLengthSquared;

        public readonly float DecreaseLength;
        public readonly float HandleDecreaseX;
        public readonly float HandleDecreaseY;

        public readonly float IncreaseX;
        public readonly float IncreaseY;
        public readonly float IncreaseLengthSquared;

        public readonly float IncreaseLength;
        public readonly float HandleIncreaseX;
        public readonly float HandleIncreaseY;

        public readonly Vector2 HandleLeftTop;
        public readonly Vector2 HandleRightTop;
        public readonly Vector2 HandleLeftBottom;
        public readonly Vector2 HandleRightBottom;

        #region Constructors
        private Box3(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, float handleLength)
         : this(leftTop,
               rightTop,
               leftBottom,
               new Vector2(rightTop.X + leftBottom.X - leftTop.X,
                   rightTop.Y + leftBottom.Y - leftTop.Y), // Triangle -> Transformer
               handleLength)
        {
        }

        private Box3(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, Vector2 rightBottom, float handleLength)
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

            // Handle Sides
            this.HorizontalX = this.CenterRight.X - this.CenterLeft.X;
            this.HorizontalY = this.CenterRight.Y - this.CenterLeft.Y;
            this.HorizontalLengthSquared = this.HorizontalX * this.HorizontalX + this.HorizontalY * this.HorizontalY;

            this.HorizontalLength = (float)System.Math.Sqrt(this.HorizontalLengthSquared);
            this.HandleHorizontalX = handleLength * this.HorizontalX / this.HorizontalLength;
            this.HandleHorizontalY = handleLength * this.HorizontalY / this.HorizontalLength;

            this.VerticalX = this.CenterBottom.X - this.CenterTop.X;
            this.VerticalY = this.CenterBottom.Y - this.CenterTop.Y;
            this.VerticalLengthSquared = this.VerticalX * this.VerticalX + this.VerticalY * this.VerticalY;

            this.VerticalLength = (float)System.Math.Sqrt(this.VerticalLengthSquared);
            this.HandleVerticalX = handleLength * this.VerticalX / this.VerticalLength;
            this.HandleVerticalY = handleLength * this.VerticalY / this.VerticalLength;

            this.HandleLeft = new Vector2(this.CenterLeft.X - this.HandleHorizontalX,
                this.CenterLeft.Y - this.HandleHorizontalY);
            this.HandleTop = new Vector2(this.CenterTop.X - this.HandleVerticalX,
                this.CenterTop.Y - this.HandleVerticalY);
            this.HandleRight = new Vector2(this.CenterRight.X + this.HandleHorizontalX,
                this.CenterRight.Y + this.HandleHorizontalY);
            this.HandleBottom = new Vector2(this.CenterBottom.X + this.HandleVerticalX,
                this.CenterBottom.Y + this.HandleVerticalY);

            // Handle Corners
            this.DecreaseX = this.RightBottom.X - this.LeftTop.X;
            this.DecreaseY = this.RightBottom.Y - this.LeftTop.Y;
            this.DecreaseLengthSquared = this.DecreaseX * this.DecreaseX + this.DecreaseY * this.DecreaseY;

            this.DecreaseLength = (float)System.Math.Sqrt(this.DecreaseLengthSquared);
            this.HandleDecreaseX = handleLength * this.DecreaseX / this.DecreaseLength;
            this.HandleDecreaseY = handleLength * this.DecreaseY / this.DecreaseLength;

            this.IncreaseX = this.RightTop.X - this.LeftBottom.X;
            this.IncreaseY = this.RightTop.Y - this.LeftBottom.Y;
            this.IncreaseLengthSquared = this.IncreaseX * this.IncreaseX + this.IncreaseY * this.IncreaseY;

            this.IncreaseLength = (float)System.Math.Sqrt(this.IncreaseLengthSquared);
            this.HandleIncreaseX = handleLength * this.IncreaseX / this.IncreaseLength;
            this.HandleIncreaseY = handleLength * this.IncreaseY / this.IncreaseLength;

            this.HandleLeftTop = new Vector2(this.LeftTop.X - this.HandleDecreaseX,
                this.LeftTop.Y - this.HandleDecreaseY);
            this.HandleRightTop = new Vector2(this.RightTop.X + this.HandleIncreaseX,
                this.RightTop.Y + this.HandleIncreaseY);
            this.HandleRightBottom = new Vector2(this.RightBottom.X + this.HandleDecreaseX,
                this.RightBottom.Y + this.HandleDecreaseY);
            this.HandleLeftBottom = new Vector2(this.LeftBottom.X - this.HandleIncreaseX,
                this.LeftBottom.Y - this.HandleIncreaseY);
        }

        public Box3(Bounds bounds, float handleLength = 32f)
            : this(new Vector2(bounds.Left, bounds.Top),
                  new Vector2(bounds.Right, bounds.Top),
                  new Vector2(bounds.Left, bounds.Bottom),
                  new Vector2(bounds.Right, bounds.Bottom),
                  handleLength)
        {
        }

        public Box3(Bounds bounds, Matrix3x2 matrix, float handleLength = 32f)
            : this(Mathematics.Math.Transform(bounds.Left, bounds.Top, matrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Top, matrix),
                  Mathematics.Math.Transform(bounds.Left, bounds.Bottom, matrix),
                  Mathematics.Math.Transform(bounds.Right, bounds.Bottom, matrix),
                  handleLength)
        {
        }

        public Box3(Bounds bounds, ICanvasMatrix matrix, float handleLength = 32f)
            : this(matrix.Transform(bounds.Left, bounds.Top),
                  matrix.Transform(bounds.Right, bounds.Top),
                  matrix.Transform(bounds.Left, bounds.Bottom),
                  matrix.Transform(bounds.Right, bounds.Bottom),
                  handleLength)
        {
        }

        public Box3(Bounds bounds, ICanvasInverseMatrix inverseMatrix, float handleLength = 32f)
            : this(inverseMatrix.InverseTransform(bounds.Left, bounds.Top),
                  inverseMatrix.InverseTransform(bounds.Right, bounds.Top),
                  inverseMatrix.InverseTransform(bounds.Left, bounds.Bottom),
                  inverseMatrix.InverseTransform(bounds.Right, bounds.Bottom),
                  handleLength)
        {
        }

        public Box3(Triangle triangle, float handleLength = 32f)
            : this(triangle.LeftTop,
                  triangle.RightTop,
                  triangle.LeftBottom,
                  handleLength)
        {
        }

        public Box3(Triangle triangle, Matrix3x2 matrix, float handleLength = 32f)
            : this(Vector2.Transform(triangle.LeftTop, matrix),
                  Vector2.Transform(triangle.RightTop, matrix),
                  Vector2.Transform(triangle.LeftBottom, matrix),
                  handleLength)
        {
        }

        public Box3(Triangle triangle, ICanvasMatrix matrix, float handleLength = 32f)
            : this(matrix.Transform(triangle.LeftTop),
                  matrix.Transform(triangle.RightTop),
                  matrix.Transform(triangle.LeftBottom),
                  handleLength)
        {
        }

        public Box3(Triangle triangle, ICanvasInverseMatrix inverseMatrix, float handleLength = 32f)
            : this(inverseMatrix.InverseTransform(triangle.LeftTop),
                  inverseMatrix.InverseTransform(triangle.RightTop),
                  inverseMatrix.InverseTransform(triangle.LeftBottom),
                  handleLength)
        {
        }

        public Box3(Quadrilateral quad, float handleLength = 32f)
            : this(quad.LeftTop,
                  quad.RightTop,
                  quad.LeftBottom,
                  quad.RightBottom,
                  handleLength)
        {
        }

        public Box3(Quadrilateral quad, Matrix3x2 matrix, float handleLength = 32f)
            : this(Vector2.Transform(quad.LeftTop, matrix),
                  Vector2.Transform(quad.RightTop, matrix),
                  Vector2.Transform(quad.LeftBottom, matrix),
                  Vector2.Transform(quad.RightBottom, matrix),
                  handleLength)
        {
        }

        public Box3(Quadrilateral quad, ICanvasMatrix matrix, float handleLength = 32f)
            : this(matrix.Transform(quad.LeftTop),
                  matrix.Transform(quad.RightTop),
                  matrix.Transform(quad.LeftBottom),
                  matrix.Transform(quad.RightBottom),
                  handleLength)
        {
        }

        public Box3(Quadrilateral quad, ICanvasInverseMatrix inverseMatrix, float handleLength = 32f)
            : this(inverseMatrix.InverseTransform(quad.LeftTop),
                  inverseMatrix.InverseTransform(quad.RightTop),
                  inverseMatrix.InverseTransform(quad.LeftBottom),
                  inverseMatrix.InverseTransform(quad.RightBottom),
                  handleLength)
        {
        }
        #endregion Constructors

        #region Public Static Operators
        public static implicit operator Triangle(Box3 box)
        {
            return new Triangle
            {
                LeftTop = box.LeftTop,
                RightTop = box.RightTop,
                LeftBottom = box.LeftBottom,
            };
        }

        public static implicit operator Quadrilateral(Box3 box)
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