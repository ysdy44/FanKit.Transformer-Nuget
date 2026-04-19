using FanKit.Transformer.Mathematics;
using System.Numerics;
using static FanKit.Transformer.Input.MatrixPair;
using Rotation = System.Numerics.Vector2;
using Scaler = System.Numerics.Vector2;
using Translation = System.Numerics.Vector4;
using Vector2 = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    // Center
    // Rotate
    // Scale
    // Translation
    public partial class CanvasTransformer3 : ICanvasMatrix, ICanvasInverseMatrix
    {
        float tx; // Translate X
        float ty; // Translate Y

        // 1
        Translation c; // Center

        float sr; // Starting Radians
        Rotation r = new Rotation();

        // 2
        float ss; // Starting Scale
        Scaler s = empty;

        float sx; // Starting X
        float sy; // Starting Y
        Translation t = new Translation();

        Vector2 cc; // CanvasCenter
        Vector2 vc; // VirtualCenter

        // Thumb
        // Matrix3x2 mat;
        float s11;
        float s21;
        float s12;
        float s22;

        // 1
        MatrixPair p1;
        // 2
        MatrixPair p2;
        // 3
        MatrixPair p3;

        public float CenterX => c.X;
        public float CenterY => c.Y;

        public float Rotation
        {
            get => r.X;
            set
            {
                r = Rotate(value);

                p1 = new MatrixPair(r, c);
                p3 = Pair(p1, p2);
            }
        }

        public float ScaleFactor
        {
            get => s.X;
            set
            {
                s = Scales(value);

                p2 = Pair(t, s, c);
                p3 = Pair(p1, p2);
            }
        }

        public float InverseScaleFactor
        {
            get => s.Y;
            set
            {
                s = Scales(1f / value, value);

                p2 = Pair(t, s, c);
                p3 = Pair(p1, p2);
            }
        }

        public float PositionX => t.X;
        public float PositionY => t.Y;

        public Matrix3x2 Matrix => p3.mat;
        public Matrix3x2 InverseMatrix => p3.inv;

        public float OriginX => p2.mat.M31;
        public float OriginY => p2.mat.M32;

        public CanvasTransformer3(float centerX, float centerY) => SetCenter(centerX, centerY);

        public void SetCenter(float centerX, float centerY)
        {
            c = Translate(centerX, centerY);

            p1 = new MatrixPair(r, c);
            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }

        public void Fit(Coordinate coord)
        {
            r = new Rotation();
            s = Scales(coord.ScaleFactor, coord.InverseScaleFactor);
            t = Translate(coord.Translate.X, coord.Translate.Y);

            p1 = new MatrixPair(r, c);
            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }

        public float Scale(float value) => value * s.X;
        public Vector2 Scale(Vector2 value) => value * s.X;

        public Vector2 Transform(Vector2 position) => p3.T(position);
        public Vector2 Transform(float xPosition, float yPosition) => p3.T(xPosition, yPosition);
        public Node Transform(Node node) => node * p3.mat;
        public Triangle Transform(Triangle triangle) => triangle * p3.mat;
        public Quadrilateral Transform(Quadrilateral quad) => quad * p3.mat;
        public Quadrilateral TransformSize(float width, float height) => new Quadrilateral(width, height, p3.mat);

        public Matrix3x2 TransformTranslation(Vector2 translate) => Math.TransformTranslation(translate, this.p3.mat);
        public Matrix3x2 TransformTranslation(float translateX, float translateY) => Math.TransformTranslation(translateX, translateY, this.p3.mat);

        public float InverseScale(float value) => value * s.Y;
        public Vector2 InverseScale(Vector2 value) => value * s.Y;

        public Vector2 InverseTransform(Vector2 position) => p3.R(position);
        public Vector2 InverseTransform(float xPosition, float yPosition) => p3.R(xPosition, yPosition);
        public Node InverseTransform(Node node) => node * p3.inv;
        public Triangle InverseTransform(Triangle triangle) => triangle * p3.inv;
        public Quadrilateral InverseTransform(Quadrilateral quad) => quad * p3.inv;
        public Quadrilateral InverseTransformSize(float width, float height) => new Quadrilateral(width, height, p3.inv);

        public void CacheMove(CanvasMoveUnits unit)
        {
            tx = 0f;
            ty = 0f;

            switch (unit)
            {
                case CanvasMoveUnits.Normal:
                    sx = t.X;
                    sy = t.Y;
                    break;
                case CanvasMoveUnits.Thumbnail:
                    sx = t.X;
                    sy = t.Y;

                    //mat = p3.mat;
                    s11 = p3.mat.M11;
                    s21 = p3.mat.M21;
                    s12 = p3.mat.M12;
                    s22 = p3.mat.M22;
                    break;
                default:
                    break;
            }
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

            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }

        private void M1()
        {
            //Vector2 translate = new Vector2(tx, ty);
            //translate = Math.Scale(translate, mat);
            //t = Translate(sx + translate.X, sy + translate.Y);
            t = Translate(sx + tx * s11 + ty * s21,
                sy + tx * s12 + ty * s22);

            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }

        public void CachePush(Vector2 startingCenterPoint)
        {
            tx = 0f;
            ty = 0f;

            sr = r.X;
            ss = s.X;
            cc = p3.R(startingCenterPoint);
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

            sr = r.X;
            ss = s.X;
            cc = p3.R(startingCenterPoint);
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
                (t.X + c.X - cp.X) * ss + cp.X - c.X,
                (t.Y + c.Y - cp.Y) * ss + cp.Y - c.Y);

            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }

        private void P(Vector2 cp)
        {
            t = new Translation();

            // Donald Knuth: Premature optimization is the root of all evil
            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);

            vc = p3.T(cc);
            t = Translate(cp.X - vc.X, cp.Y - vc.Y);

            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }
    }
}