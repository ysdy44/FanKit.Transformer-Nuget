using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public struct Coordinate
    {
        public float TranslateX;

        public float TranslateY;

        public float ScaleFactor;

        public float InverseScaleFactor;

        public Matrix3x2 Matrix
        {
            get
            {
                return new Matrix3x2(ScaleFactor, 0f, 0f, ScaleFactor, // Scale
                    TranslateX, TranslateY); // Translate
            }
        }

        public Coordinate(Vector2 translate, float scaleFactor)
        {
            TranslateX = translate.X;
            TranslateY = translate.Y;
            ScaleFactor = scaleFactor;
            InverseScaleFactor = 1f / ScaleFactor;
        }

        public Coordinate(float translateX, float translateY, float scaleFactor)
        {
            TranslateX = translateX;
            TranslateY = translateY;
            ScaleFactor = scaleFactor;
            InverseScaleFactor = 1f / ScaleFactor;
        }

        //public static Coordinate Animation(Coordinate form, Coordinate to, float amount)
        public Coordinate(Coordinate form, Coordinate to, float amount)
        {
            if (amount <= 0f)
            {
                TranslateX = form.TranslateX;
                TranslateY = form.TranslateY;
                ScaleFactor = form.ScaleFactor;
                InverseScaleFactor = form.InverseScaleFactor;
            }
            else if (amount >= 1f)
            {
                TranslateX = to.TranslateX;
                TranslateY = to.TranslateY;
                ScaleFactor = to.ScaleFactor;
                InverseScaleFactor = to.InverseScaleFactor;
            }
            else
            {
                float r = 1f - amount;

                TranslateX = form.TranslateX * r + to.TranslateX * amount;
                TranslateY = form.TranslateY * r + to.TranslateY * amount;
                ScaleFactor = form.ScaleFactor * r + to.ScaleFactor * amount;
                InverseScaleFactor = form.InverseScaleFactor * r + to.InverseScaleFactor * amount;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Transform(Vector2 position)
        {
            return new Vector2(
                position.X * ScaleFactor + TranslateX,
                position.Y * ScaleFactor + TranslateY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Transform(float xPosition, float yPosition)
        {
            return new Vector2(
                xPosition * ScaleFactor + TranslateX,
                yPosition * ScaleFactor + TranslateY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Scale(Vector2 position)
        {
            return new Vector2(
                position.X * ScaleFactor,
                position.Y * ScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Scale(float xPosition, float yPosition)
        {
            return new Vector2(
                xPosition * ScaleFactor,
                yPosition * ScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 InverseTransform(Vector2 position)
        {
            return new Vector2(
                (position.X - TranslateX) * InverseScaleFactor,
                (position.Y - TranslateY) * InverseScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 InverseTransform(float xPosition, float yPosition)
        {
            return new Vector2(
                (xPosition - TranslateX) * InverseScaleFactor,
                (yPosition - TranslateY) * InverseScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 InverseScale(Vector2 position)
        {
            return new Vector2(
                position.X * InverseScaleFactor,
                position.Y * InverseScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 InverseScale(float xPosition, float yPosition)
        {
            return new Vector2(
                xPosition * InverseScaleFactor,
                yPosition * InverseScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node Transform(Node node)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X * ScaleFactor + TranslateX,
                    node.Point.Y * ScaleFactor + TranslateY),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X * ScaleFactor + TranslateX,
                    node.LeftControlPoint.Y * ScaleFactor + TranslateY),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X * ScaleFactor + TranslateX,
                    node.RightControlPoint.Y * ScaleFactor + TranslateY),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Triangle Transform(Triangle triangle)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X * ScaleFactor + TranslateX,
                    triangle.LeftTop.Y * ScaleFactor + TranslateY),
                RightTop = new Vector2(
                    triangle.RightTop.X * ScaleFactor + TranslateX,
                    triangle.RightTop.Y * ScaleFactor + TranslateY),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X * ScaleFactor + TranslateX,
                    triangle.LeftBottom.Y * ScaleFactor + TranslateY),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quadrilateral Transform(Quadrilateral quad)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    quad.LeftTop.X * ScaleFactor + TranslateX,
                    quad.LeftTop.Y * ScaleFactor + TranslateY),
                RightTop = new Vector2(
                    quad.RightTop.X * ScaleFactor + TranslateX,
                    quad.RightTop.Y * ScaleFactor + TranslateY),
                LeftBottom = new Vector2(
                    quad.LeftBottom.X * ScaleFactor + TranslateX,
                    quad.LeftBottom.Y * ScaleFactor + TranslateY),
                RightBottom = new Vector2(
                    quad.RightBottom.X * ScaleFactor + TranslateX,
                    quad.RightBottom.Y * ScaleFactor + TranslateY),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 Transform(Matrix3x2 matrix)
        {
            return Math.Transform(matrix, TranslateX, TranslateY, ScaleFactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4 Transform(Matrix4x4 matrix)
        {
            return Math.Transform(matrix, TranslateX, TranslateY, ScaleFactor);
        }
    }
}