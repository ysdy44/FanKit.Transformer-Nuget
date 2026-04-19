using System.Numerics;
using static FanKit.Transformer.Input.MatrixPair;
using Translation = System.Numerics.Vector4;
using Vector2 = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    // Translation
    public partial class CanvasTransformer0 : ICanvasMatrix, ICanvasInverseMatrix
    {
        float tx; // Translate X
        float ty; // Translate Y

        // 2
        float sx; // Starting X
        float sy; // Starting Y
        Translation t = new Translation();

        // 2
        MatrixPair p2 = new MatrixPair
        {
            mat = Matrix3x2.Identity,
            inv = Matrix3x2.Identity,
        };

        public float PositionX => t.X;
        public float PositionY => t.Y;

        public Matrix3x2 Matrix => p2.mat;
        public Matrix3x2 InverseMatrix => p2.inv;

        public float OriginX => p2.mat.M31;
        public float OriginY => p2.mat.M32;

        public void Fit(Vector2 translation)
        {
            t = Translate(translation.X, translation.Y);

            p2 = Pair(t);
        }

        public float Scale(float value)
        {
            return value;
        }
        public Vector2 Scale(Vector2 value)
        {
            return value;
        }

        public Vector2 Transform(Vector2 position)
        {
            return new Vector2(
                position.X + t.X,
                position.Y + t.Y);
        }
        public Vector2 Transform(float xPosition, float yPosition)
        {
            return new Vector2(
                xPosition + t.X,
                yPosition + t.Y);
        }
        public Node Transform(Node node)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X + t.X,
                    node.Point.Y + t.Y),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X + t.X,
                    node.LeftControlPoint.Y + t.Y),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X + t.X,
                    node.RightControlPoint.Y + t.Y),
            };
        }
        public Triangle Transform(Triangle triangle)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X + t.X,
                    triangle.LeftTop.Y + t.Y),
                RightTop = new Vector2(
                    triangle.RightTop.X + t.X,
                    triangle.RightTop.Y + t.Y),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X + t.X,
                    triangle.LeftBottom.Y + t.Y),
            };
        }
        public Quadrilateral Transform(Quadrilateral quad)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    quad.LeftTop.X + t.X,
                    quad.LeftTop.Y + t.Y),
                RightTop = new Vector2(
                    quad.RightTop.X + t.X,
                    quad.RightTop.Y + t.Y),
                LeftBottom = new Vector2(
                    quad.LeftBottom.X + t.X,
                    quad.LeftBottom.Y + t.Y),
                RightBottom = new Vector2(
                    quad.RightBottom.X + t.X,
                    quad.RightBottom.Y + t.Y),
            };
        }
        public Bounds TransformSize(float width, float height)
        {
            return new Bounds
            {
                Left = t.X,
                Top = t.Y,
                Right = width + t.X,
                Bottom = height + t.Y,
            };
        }

        public Matrix3x2 TransformTranslation(Vector2 translate) => new Matrix3x2
        {
            // First row
            M11 = 1f,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = 1f,

            // Third row
            M31 = translate.X + t.X,
            M32 = translate.Y + t.Y,
        };
        public Matrix3x2 TransformTranslation(float translateX, float translateY) => new Matrix3x2
        {
            // First row
            M11 = 1f,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = 1f,

            // Third row
            M31 = translateX + t.X,
            M32 = translateY + t.Y,
        };

        public float InverseScale(float value)
        {
            return value;
        }
        public Vector2 InverseScale(Vector2 value)
        {
            return value;
        }

        public Vector2 InverseTransform(Vector2 position)
        {
            return new Vector2(
                position.X + t.Z,
                position.Y + t.W);
        }
        public Vector2 InverseTransform(float xPosition, float yPosition)
        {
            return new Vector2(
                xPosition + t.Z,
                yPosition + t.W);
        }
        public Node InverseTransform(Node node)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X + t.Z,
                    node.Point.Y + t.W),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X + t.Z,
                    node.LeftControlPoint.Y + t.W),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X + t.Z,
                    node.RightControlPoint.Y + t.W),
            };
        }
        public Triangle InverseTransform(Triangle triangle)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X + t.Z,
                    triangle.LeftTop.Y + t.W),
                RightTop = new Vector2(
                    triangle.RightTop.X + t.Z,
                    triangle.RightTop.Y + t.W),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X + t.Z,
                    triangle.LeftBottom.Y + t.W),
                //RightBottom = new Vector2(
                //    triangle.RightBottomX + t.rx,
                //    triangle.RightBottomY + t.ry),
            };
        }
        public Quadrilateral InverseTransform(Quadrilateral quad)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    quad.LeftTop.X + t.Z,
                    quad.LeftTop.Y + t.W),
                RightTop = new Vector2(
                    quad.RightTop.X + t.Z,
                    quad.RightTop.Y + t.W),
                LeftBottom = new Vector2(
                    quad.LeftBottom.X + t.Z,
                    quad.LeftBottom.Y + t.W),
                RightBottom = new Vector2(
                    quad.RightBottom.X + t.Z,
                    quad.RightBottom.Y + t.W),
            };
        }
        public Bounds InverseTransformSize(float width, float height)
        {
            return new Bounds
            {
                Left = t.Z,
                Top = t.W,
                Right = width + t.Z,
                Bottom = height + t.W,
            };
        }

        public void CacheMove(CanvasMoveUnits unit)
        {
            tx = 0f;
            ty = 0f;

            sx = t.X;
            sy = t.Y;
        }

        public void Move(Vector2 startingPoint, Vector2 point, CanvasMoveUnits unit)
        {
            switch (unit)
            {
                case CanvasMoveUnits.Normal:
                    tx = point.X - startingPoint.X;
                    ty = point.Y - startingPoint.Y;
                    M();
                    break;
                case CanvasMoveUnits.Thumbnail:
                    tx = startingPoint.X - point.X;
                    ty = startingPoint.Y - point.Y;
                    M();
                    break;
                default:
                    break;
            }
        }

        public void Move(Vector2 translate, CanvasMoveUnits unit)
        {
            tx = translate.X;
            ty = translate.Y;
            M();
        }

        private void M()
        {
            t = Translate(sx + tx, sy + ty);

            p2 = Pair(t);
        }
    }
}