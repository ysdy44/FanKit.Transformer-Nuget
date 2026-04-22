using static FanKit.Transformer.Input.MatrixPair;
using Translation = System.Numerics.Vector4;
using Vector2 = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    partial class CanvasTransformer3
    {
        public void Push2(Vector2 startingPoint, Vector2 point, float rotateDistance = 720f)
        {
            float angular = point.X - startingPoint.X;
            //float scale = point.Y - startingPoint.Y;
            r = Rotate(sr + angular / rotateDistance);
            //S = Scales(StartingScale + scale / distance);
            O(startingPoint);
        }

        public void Push3(Vector2 startingPoint, Vector2 point, float scaleDistance = 512f, float rotateDistance = 720f)
        {
            float angular = point.X - startingPoint.X;
            float scale = point.Y - startingPoint.Y;
            r = Rotate(sr + angular / rotateDistance);
            s = Scales(ss + scale / scaleDistance);
            O(startingPoint);
        }

        public void Pinch2(float startingRotationAngle, float rotationAngle, Vector2 centerPoint)
        {
            r = Rotate(sr + rotationAngle - startingRotationAngle);
            //S = Scales(StartingScale * radius / startingRadius);
            O(centerPoint);
        }

        public void Pinch3(Pivot3 startingPivot, Pivot3 pivot, Vector2 centerPoint)
        {
            r = Rotate(sr + pivot.RotationAngle - startingPivot.RotationAngle);
            s = Scales(ss * pivot.Radius / startingPivot.Radius);
            O(centerPoint);
        }

        public void SetRotation(float rotationAngle, Vector2 centerPoint)
        {
            cc = p3.R(centerPoint);
            r = Rotate(rotationAngle);

            O(centerPoint);
        }

        public void TwistCounterclockwise(Vector2 centerPoint, float deltaRotationAngle = Constants.DegreesToRadians)
        {
            cc = p3.R(centerPoint);
            r = Rotate(r.X - deltaRotationAngle);

            O(centerPoint);
        }

        public void TwistClockwise(Vector2 centerPoint, float deltaRotationAngle = Constants.DegreesToRadians)
        {
            cc = p3.R(centerPoint);
            r = Rotate(r.X + deltaRotationAngle);

            O(centerPoint);
        }

        private void O(Vector2 cp)
        {
            t = new Translation();

            // Donald Knuth: Premature optimization is the root of all evil
            p1 = new MatrixPair(r, c);
            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);

            vc = p3.T(cc);
            t = Translate(cp.X - vc.X, cp.Y - vc.Y);

            p1 = new MatrixPair(r, c);
            p2 = Pair(t, s, c);
            p3 = Pair(p1, p2);
        }
    }
}