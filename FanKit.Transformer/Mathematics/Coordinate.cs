using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public struct Coordinate
    {
        public Vector2 Translate;

        public float ScaleFactor;

        public float InverseScaleFactor;

        public Matrix3x2 Matrix
        {
            get
            {
                return new Matrix3x2(ScaleFactor, 0f, 0f, ScaleFactor, // Scale
                    Translate.X, Translate.Y); // Translate
            }
        }

        //public static Coordinate Animation(Coordinate form, Coordinate to, float amout)
        public Coordinate(Coordinate form, Coordinate to, float amout)
        {
            if (amout <= 0f)
            {
                Translate = form.Translate;
                ScaleFactor = form.ScaleFactor;
                InverseScaleFactor = form.InverseScaleFactor;
            }
            else if (amout >= 1f)
            {
                Translate = to.Translate;
                ScaleFactor = to.ScaleFactor;
                InverseScaleFactor = to.InverseScaleFactor;
            }
            else
            {
                var r = 1f - amout;

                Translate = new Vector2
                {
                    X = form.Translate.X * r + to.Translate.X * amout,
                    Y = form.Translate.Y * r + to.Translate.Y * amout,
                };
                ScaleFactor = form.ScaleFactor * r + to.ScaleFactor * amout;
                InverseScaleFactor = form.InverseScaleFactor * r + to.InverseScaleFactor * amout;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Transform(float x, float y)
        {
            return new Vector2(
                x * ScaleFactor + Translate.X,
                y * ScaleFactor + Translate.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Transform(Vector2 position)
        {
            return new Vector2(
                position.X * ScaleFactor + Translate.X,
                position.Y * ScaleFactor + Translate.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node Transform(Node node)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X * ScaleFactor + Translate.X,
                    node.Point.Y * ScaleFactor + Translate.Y),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X * ScaleFactor + Translate.X,
                    node.LeftControlPoint.Y * ScaleFactor + Translate.Y),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X * ScaleFactor + Translate.X,
                    node.RightControlPoint.Y * ScaleFactor + Translate.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle Transform(Triangle triangle)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X * ScaleFactor + Translate.X,
                    triangle.LeftTop.Y * ScaleFactor + Translate.Y),
                RightTop = new Vector2(
                    triangle.RightTop.X * ScaleFactor + Translate.X,
                    triangle.RightTop.Y * ScaleFactor + Translate.Y),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X * ScaleFactor + Translate.X,
                    triangle.LeftBottom.Y * ScaleFactor + Translate.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral Transform(Quadrilateral quad)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    quad.LeftTop.X * ScaleFactor + Translate.X,
                    quad.LeftTop.Y * ScaleFactor + Translate.Y),
                RightTop = new Vector2(
                    quad.RightTop.X * ScaleFactor + Translate.X,
                    quad.RightTop.Y * ScaleFactor + Translate.Y),
                LeftBottom = new Vector2(
                    quad.LeftBottom.X * ScaleFactor + Translate.X,
                    quad.LeftBottom.Y * ScaleFactor + Translate.Y),
                RightBottom = new Vector2(
                    quad.RightBottom.X * ScaleFactor + Translate.X,
                    quad.RightBottom.Y * ScaleFactor + Translate.Y),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 Transform(Matrix3x2 matrix)
        {
            return Math.Transform(matrix, Translate, ScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4 Transform(Matrix4x4 matrix)
        {
            return Math.Transform(matrix, Translate, ScaleFactor);
        }
    }
}