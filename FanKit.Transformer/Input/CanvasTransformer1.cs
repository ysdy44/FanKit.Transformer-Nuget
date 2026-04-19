using FanKit.Transformer.Mathematics;
using System.Numerics;
using static FanKit.Transformer.Input.MatrixPair;
using Scaler = System.Numerics.Vector2;
using Translation = System.Numerics.Vector4;
using Vector2 = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    // Scale
    // Translation
    public partial class CanvasTransformer1 : ICanvasMatrix, ICanvasInverseMatrix
    {
        float tx; // Translate X
        float ty; // Translate Y

        // 2
        float ss; // Starting Scale
        Scaler s = empty;

        float sx; // Starting X
        float sy; // Starting Y
        Translation t = new Translation();

        Vector2 cc; // CanvasCenter
        Vector2 vc; // VirtualCenter

        // 2
        MatrixPair p2 = new MatrixPair
        {
            mat = Matrix3x2.Identity,
            inv = Matrix3x2.Identity,
        };

        public float ScaleFactor
        {
            get => s.X;
            set
            {
                s = Scales(value);

                p2 = Pair(t, s);
            }
        }

        public float InverseScaleFactor
        {
            get => s.Y;
            set
            {
                s = Scales(1f / value, value);

                p2 = Pair(t, s);
            }
        }

        public float PositionX => t.X;
        public float PositionY => t.Y;

        public Matrix3x2 Matrix => p2.mat;
        public Matrix3x2 InverseMatrix => p2.inv;

        public float OriginX => p2.mat.M31;
        public float OriginY => p2.mat.M32;

        public void Fit(Coordinate coord)
        {
            s = Scales(coord.ScaleFactor, coord.InverseScaleFactor);
            t = Translate(coord.Translate.X, coord.Translate.Y);

            p2 = Pair(t, s);
        }

        public float Scale(float value)
        {
            return value * s.X;
        }
        public Vector2 Scale(Vector2 value)
        {
            return value * s.X;
        }

        public Vector2 Transform(Vector2 position)
        {
            return new Vector2(
                position.X * s.X + t.X,
                position.Y * s.X + t.Y);
        }
        public Vector2 Transform(float xPosition, float yPosition)
        {
            return new Vector2(
                xPosition * s.X + t.X,
                yPosition * s.X + t.Y);
        }
        public Node Transform(Node node)
        {
            return new Node
            {
                Point = new Vector2(
                    node.Point.X * s.X + t.X,
                    node.Point.Y * s.X + t.Y),
                LeftControlPoint = new Vector2(
                    node.LeftControlPoint.X * s.X + t.X,
                    node.LeftControlPoint.Y * s.X + t.Y),
                RightControlPoint = new Vector2(
                    node.RightControlPoint.X * s.X + t.X,
                    node.RightControlPoint.Y * s.X + t.Y),
            };
        }
        public Triangle Transform(Triangle triangle)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    triangle.LeftTop.X * s.X + t.X,
                    triangle.LeftTop.Y * s.X + t.Y),
                RightTop = new Vector2(
                    triangle.RightTop.X * s.X + t.X,
                    triangle.RightTop.Y * s.X + t.Y),
                LeftBottom = new Vector2(
                    triangle.LeftBottom.X * s.X + t.X,
                    triangle.LeftBottom.Y * s.X + t.Y),
            };
        }
        public Quadrilateral Transform(Quadrilateral quad)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    quad.LeftTop.X * s.X + t.X,
                    quad.LeftTop.Y * s.X + t.Y),
                RightTop = new Vector2(
                    quad.RightTop.X * s.X + t.X,
                    quad.RightTop.Y * s.X + t.Y),
                LeftBottom = new Vector2(
                    quad.LeftBottom.X * s.X + t.X,
                    quad.LeftBottom.Y * s.X + t.Y),
                RightBottom = new Vector2(
                    quad.RightBottom.X * s.X + t.X,
                    quad.RightBottom.Y * s.X + t.Y),
            };
        }
        public Bounds TransformSize(float width, float height)
        {
            return new Bounds
            {
                Left = t.X,
                Top = t.Y,
                Right = width * s.X + t.X,
                Bottom = height * s.X + t.Y,
            };
        }

        public Matrix3x2 TransformTranslation(Vector2 translate) => new Matrix3x2
        {
            // First row
            M11 = s.X,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = s.Y,

            // Third row
            M31 = s.X * translate.X + t.X,
            M32 = s.Y * translate.Y + t.Y,
        };
        public Matrix3x2 TransformTranslation(float translateX, float translateY) => new Matrix3x2
        {
            // First row
            M11 = s.X,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = s.Y,

            // Third row
            M31 = s.X * translateX + t.X,
            M32 = s.Y * translateY + t.Y,
        };

        public float InverseScale(float value)
        {
            return value * s.Y;
        }
        public Vector2 InverseScale(Vector2 value)
        {
            return value * s.Y;
        }

        public Vector2 InverseTransform(Vector2 position)
        {
            return new Vector2(
                (position.X + t.Z) * s.Y,
                (position.Y + t.W) * s.Y);
        }
        public Vector2 InverseTransform(float xPosition, float yPosition)
        {
            return new Vector2(
                (xPosition + t.Z) * s.Y,
                (yPosition + t.W) * s.Y);
        }
        public Node InverseTransform(Node node)
        {
            return new Node
            {
                Point = new Vector2(
                    (node.Point.X + t.Z) * s.Y,
                    (node.Point.Y + t.W) * s.Y),
                LeftControlPoint = new Vector2(
                    (node.LeftControlPoint.X + t.Z) * s.Y,
                    (node.LeftControlPoint.Y + t.W) * s.Y),
                RightControlPoint = new Vector2(
                    (node.RightControlPoint.X + t.Z) * s.Y,
                    (node.RightControlPoint.Y + t.W) * s.Y),
            };
        }
        public Triangle InverseTransform(Triangle triangle)
        {
            return new Triangle
            {
                LeftTop = new Vector2(
                    (triangle.LeftTop.X + t.Z) * s.Y,
                    (triangle.LeftTop.Y + t.W) * s.Y),
                RightTop = new Vector2(
                    (triangle.RightTop.X + t.Z) * s.Y,
                    (triangle.RightTop.Y + t.W) * s.Y),
                LeftBottom = new Vector2(
                    (triangle.LeftBottom.X + t.Z) * s.Y,
                    (triangle.LeftBottom.Y + t.W) * s.Y),
                //RightBottom = new Vector2(
                //    (triangle.RightBottomX + t.rx) * s.inv,
                //    (triangle.RightBottomY + t.ry) * s.inv),
            };
        }
        public Quadrilateral InverseTransform(Quadrilateral quad)
        {
            return new Quadrilateral
            {
                LeftTop = new Vector2(
                    (quad.LeftTop.X + t.Z) * s.Y,
                    (quad.LeftTop.Y + t.W) * s.Y),
                RightTop = new Vector2(
                    (quad.RightTop.X + t.Z) * s.Y,
                    (quad.RightTop.Y + t.W) * s.Y),
                LeftBottom = new Vector2(
                    (quad.LeftBottom.X + t.Z) * s.Y,
                    (quad.LeftBottom.Y + t.W) * s.Y),
                RightBottom = new Vector2(
                    (quad.RightBottom.X + t.Z) * s.Y,
                    (quad.RightBottom.Y + t.W) * s.Y),
            };
        }
        public Bounds InverseTransformSize(float width, float height)
        {
            return new Bounds
            {
                Left = t.Z * s.Y,
                Top = t.W * s.Y,
                Right = (width + t.Z) * s.Y,
                Bottom = (height + t.W) * s.Y,
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
                    M0();
                    break;
                case CanvasMoveUnits.Thumbnail:
                    tx = startingPoint.X - point.X;
                    ty = startingPoint.Y - point.Y;
                    M1();
                    break;
                default:
                    break;
            }
        }

        public void Move(Vector2 translate, CanvasMoveUnits unit)
        {
            tx = translate.X;
            ty = translate.Y;

            switch (unit)
            {
                case CanvasMoveUnits.Normal: M0(); break;
                case CanvasMoveUnits.Thumbnail: M1(); break;
                default: break;
            }
        }

        private void M0()
        {
            t = Translate(sx + tx, sy + ty);

            p2 = Pair(t, s);
        }

        private void M1()
        {
            //Vector2 translate = new Vector2(tx, ty);
            //translate = Math.Scale(translate, p2.mat);
            //t = Translate(sx + translate.X, sy + translate.Y);
            t = Translate(sx + tx * s.X,
                sy + ty * s.X);

            p2 = Pair(t, s);
        }

        public void CachePush(Vector2 startingCenterPoint)
        {
            tx = 0f;
            ty = 0f;

            ss = s.X;
            cc = p2.R(startingCenterPoint);
        }

        public void Push1(Vector2 startingPoint, Vector2 point, float scaleDistance = 512f)
        {
            //tx = point.X - startingPoint.X; // Angular
            ty = point.Y - startingPoint.Y; // Scale
            //R = Rotate(StartingRadians + tx / rotateDistance);
            s = Scales(ss + ty / scaleDistance);
            P(startingPoint);
        }

        public void CachePinch(Vector2 startingCenterPoint)
        {
            tx = 0f;
            ty = 0f;

            ss = s.X;
            cc = p2.R(startingCenterPoint);
        }

        public void Pinch1(float startingRadius, float radius, Vector2 centerPoint)
        {
            //R = Rotate(StartingRadians + angular - startingAngular);
            s = Scales(ss * radius / startingRadius);
            P(centerPoint);
        }

        public void ZoomIn(Vector2 centerPoint)
        {
            Z(centerPoint, 1.1f);
        }

        public void ZoomOut(Vector2 centerPoint)
        {
            Z(centerPoint, 1 / 1.1f);
        }

        private void Z(Vector2 cp, float ss)
        {
            s = Scales(s.X * ss);
            t = Translate(
                (t.X - cp.X) * ss + cp.X,
                (t.Y - cp.Y) * ss + cp.Y);

            p2 = Pair(t, s);
        }

        private void P(Vector2 cp)
        {
            t = new Translation();

            // Donald Knuth: Premature optimization is the root of all evil
            p2 = Pair(t, s);

            vc = p2.T(cc);
            t = Translate(cp.X - vc.X, cp.Y - vc.Y);

            p2 = Pair(t, s);
        }
    }
}