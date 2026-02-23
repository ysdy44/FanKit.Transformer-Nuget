using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct FlyoutPathData
    {
        public readonly Vector2 StartPoint; // PathFigure StartPoint

        public readonly Vector2 Line0Point; // Top Line0 Point
        public readonly Vector2 Line1Point; // Top Line1 Point
        public readonly Vector2 Line2Point; // Right Line Point
        public readonly Vector2 Line3Point; // Bottom Line Point
        public readonly Vector2 Line4Point; // Left Line Point

        public readonly Vector2 ArcSize;

        public readonly Vector2 Arc0Point; // Right-Top Arc Point
        public readonly Vector2 Arc1Point; // Right-Bottom Arc Point
        public readonly Vector2 Arc2Point; // Left-Bottom Arc Point
        public readonly Vector2 Arc3Point; // Left-Top Arc Point

        public readonly Vector2 Bezier0Point1; // Top Bezier0 Point1
        public readonly Vector2 Bezier0Point2; // Top Bezier0 Point2
        public readonly Vector2 Bezier0Point3; // Top Bezier0 Point3

        public readonly Vector2 Bezier1Point1; // Top Bezier1 Point1
        public readonly Vector2 Bezier1Point2; // Top Bezier1 Point2
        public readonly Vector2 Bezier1Point3; // Top Bezier1 Point3

        public FlyoutPathData(FlyoutLocation location, FlyoutMetrics metrics, float arrowRadius = 4, float tailRadius = 6)
        {
            this.ArcSize = new Vector2(location.CornerRadius, location.CornerRadius);

            switch (metrics.Mode)
            {
                case FlyoutPlacementMode.Left:
                    this.StartPoint = new Vector2(metrics.Body.Right, metrics.Inner.Top);

                    this.Line0Point = metrics.TailLeft;

                    // --------------------- Bezier --------------------- //

                    this.Bezier0Point1 = new Vector2(metrics.TailLeft.X, metrics.TailLeft.Y + tailRadius);

                    this.Bezier0Point2 = new Vector2(metrics.Arrow.X, metrics.Arrow.Y - arrowRadius);
                    this.Bezier0Point3 = metrics.Arrow;
                    this.Bezier1Point1 = new Vector2(metrics.Arrow.X, metrics.Arrow.Y + arrowRadius);

                    this.Bezier1Point2 = new Vector2(metrics.TailRight.X, metrics.TailRight.Y - tailRadius);

                    this.Bezier1Point3 = metrics.TailRight;

                    // --------------------- Arc and Line --------------------- //

                    this.Line1Point = new Vector2(metrics.Body.Right, metrics.Inner.Bottom);

                    this.Arc0Point = new Vector2(metrics.Inner.Right, metrics.Body.Bottom);

                    this.Line2Point = new Vector2(metrics.Inner.Left, metrics.Body.Bottom);
                    this.Arc1Point = new Vector2(metrics.Body.Left, metrics.Inner.Bottom);

                    this.Line3Point = new Vector2(metrics.Body.Left, metrics.Inner.Top);
                    this.Arc2Point = new Vector2(metrics.Inner.Left, metrics.Body.Top);

                    this.Line4Point = new Vector2(metrics.Inner.Right, metrics.Body.Top);
                    this.Arc3Point = new Vector2(metrics.Body.Right, metrics.Inner.Top);
                    break;
                case FlyoutPlacementMode.Right:
                    this.StartPoint = new Vector2(metrics.Body.Left, metrics.Inner.Bottom);

                    this.Line0Point = metrics.TailRight;

                    // --------------------- Bezier --------------------- //

                    this.Bezier0Point1 = new Vector2(metrics.TailRight.X, metrics.TailRight.Y - tailRadius);

                    this.Bezier0Point2 = new Vector2(metrics.Arrow.X, metrics.Arrow.Y + arrowRadius);
                    this.Bezier0Point3 = metrics.Arrow;
                    this.Bezier1Point1 = new Vector2(metrics.Arrow.X, metrics.Arrow.Y - arrowRadius);

                    this.Bezier1Point2 = new Vector2(metrics.TailLeft.X, metrics.TailLeft.Y + tailRadius);

                    this.Bezier1Point3 = metrics.TailLeft;

                    // --------------------- Arc and Line --------------------- //

                    this.Line1Point = new Vector2(metrics.Body.Left, metrics.Inner.Top);

                    this.Arc0Point = new Vector2(metrics.Inner.Left, metrics.Body.Top);

                    this.Line2Point = new Vector2(metrics.Inner.Right, metrics.Body.Top);
                    this.Arc1Point = new Vector2(metrics.Body.Right, metrics.Inner.Top);

                    this.Line3Point = new Vector2(metrics.Body.Right, metrics.Inner.Bottom);
                    this.Arc2Point = new Vector2(metrics.Inner.Right, metrics.Body.Bottom);

                    this.Line4Point = new Vector2(metrics.Inner.Left, metrics.Body.Bottom);
                    this.Arc3Point = new Vector2(metrics.Body.Left, metrics.Inner.Bottom);
                    break;
                case FlyoutPlacementMode.Top:
                    this.StartPoint = new Vector2(metrics.Inner.Right, metrics.Body.Bottom);

                    this.Line0Point = metrics.TailRight;

                    // --------------------- Bezier --------------------- //

                    this.Bezier0Point1 = new Vector2(metrics.TailRight.X - tailRadius, metrics.TailRight.Y);

                    this.Bezier0Point2 = new Vector2(metrics.Arrow.X + arrowRadius, metrics.Arrow.Y);
                    this.Bezier0Point3 = metrics.Arrow;
                    this.Bezier1Point1 = new Vector2(metrics.Arrow.X - arrowRadius, metrics.Arrow.Y);

                    this.Bezier1Point2 = new Vector2(metrics.TailLeft.X + tailRadius, metrics.TailLeft.Y);

                    this.Bezier1Point3 = metrics.TailLeft;

                    // --------------------- Arc and Line --------------------- //

                    this.Line1Point = new Vector2(metrics.Inner.Left, metrics.Body.Bottom);

                    this.Arc0Point = new Vector2(metrics.Body.Left, metrics.Inner.Bottom);

                    this.Line2Point = new Vector2(metrics.Body.Left, metrics.Inner.Top);
                    this.Arc1Point = new Vector2(metrics.Inner.Left, metrics.Body.Top);

                    this.Line3Point = new Vector2(metrics.Inner.Right, metrics.Body.Top);
                    this.Arc2Point = new Vector2(metrics.Body.Right, metrics.Inner.Top);

                    this.Line4Point = new Vector2(metrics.Body.Right, metrics.Inner.Bottom);
                    this.Arc3Point = new Vector2(metrics.Inner.Right, metrics.Body.Bottom);
                    break;
                case FlyoutPlacementMode.Bottom:
                    this.StartPoint = new Vector2(metrics.Inner.Left, metrics.Body.Top);

                    this.Line0Point = metrics.TailLeft;

                    // --------------------- Bezier --------------------- //

                    this.Bezier0Point1 = new Vector2(metrics.TailLeft.X + tailRadius, metrics.TailLeft.Y);

                    this.Bezier0Point2 = new Vector2(metrics.Arrow.X - arrowRadius, metrics.Arrow.Y);
                    this.Bezier0Point3 = metrics.Arrow;
                    this.Bezier1Point1 = new Vector2(metrics.Arrow.X + arrowRadius, metrics.Arrow.Y);

                    this.Bezier1Point2 = new Vector2(metrics.TailRight.X - tailRadius, metrics.TailRight.Y);

                    this.Bezier1Point3 = metrics.TailRight;

                    // --------------------- Arc and Line --------------------- //

                    this.Line1Point = new Vector2(metrics.Inner.Right, metrics.Body.Top);

                    this.Arc0Point = new Vector2(metrics.Body.Right, metrics.Inner.Top);

                    this.Line2Point = new Vector2(metrics.Body.Right, metrics.Inner.Bottom);
                    this.Arc1Point = new Vector2(metrics.Inner.Right, metrics.Body.Bottom);

                    this.Line3Point = new Vector2(metrics.Inner.Left, metrics.Body.Bottom);
                    this.Arc2Point = new Vector2(metrics.Body.Left, metrics.Inner.Bottom);

                    this.Line4Point = new Vector2(metrics.Body.Left, metrics.Inner.Top);
                    this.Arc3Point = new Vector2(metrics.Inner.Left, metrics.Body.Top);
                    break;
                default:
                    this.StartPoint =
                    this.Line0Point =
                    this.Bezier0Point1 = new Vector2(metrics.Inner.Left, metrics.Body.Top);

                    this.Bezier0Point2 =
                    this.Bezier0Point3 =
                    this.Bezier1Point1 = new Vector2(metrics.Body.X + metrics.Body.Width / 2, metrics.Body.Top);

                    this.Bezier1Point2 =
                    this.Bezier1Point3 =
                    this.Line1Point = new Vector2(metrics.Inner.Right, metrics.Body.Top);

                    // Copy from Bottom
                    this.Arc0Point = new Vector2(metrics.Body.Right, metrics.Inner.Top);

                    this.Line2Point = new Vector2(metrics.Body.Right, metrics.Inner.Bottom);
                    this.Arc1Point = new Vector2(metrics.Inner.Right, metrics.Body.Bottom);

                    this.Line3Point = new Vector2(metrics.Inner.Left, metrics.Body.Bottom);
                    this.Arc2Point = new Vector2(metrics.Body.Left, metrics.Inner.Bottom);

                    this.Line4Point = new Vector2(metrics.Body.Left, metrics.Inner.Top);
                    this.Arc3Point = new Vector2(metrics.Inner.Left, metrics.Body.Top);
                    break;
            }
        }
    }
}