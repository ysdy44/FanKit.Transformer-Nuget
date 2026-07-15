using FanKit.Transformer.Indicators;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public void ResetByPoint(Vector2 point)
        {
            this.Count = 1;
            this.PointsDistribution = ComposerPointsDistribution.Point;

            this.Point.StartingPoint = this.Point.Point = point;

            this.Host.Matrix = Matrix3x2.Identity;
        }

        public void UnionByPoint(Node node) { this.Count++; this.Ex(node.Point); }

        public void UnionByPoint(Polylines.Segment0 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point); } }
        public void UnionByPoint(Polylines.Segment1 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point); } }
        public void UnionByPoint(Polylines.Segment2 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map); } }
        public void UnionByPoint(Polylines.Segment3 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map); } }

        public void UnionByPoint(Curves.Segment0 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point.Point); } }
        public void UnionByPoint(Curves.Segment1 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point.Point); } }
        public void UnionByPoint(Curves.Segment2 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map.Point); } }
        public void UnionByPoint(Curves.Segment3 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map.Point); } }

        public void EndUnionByPoints()
        {
            switch (this.Count)
            {
                case 0:
                    this.PointsDistribution = ComposerPointsDistribution.Empty;
                    break;
                case 1:
                    this.PointsDistribution = ComposerPointsDistribution.Point;
                    break;
                default:
                    this.PointsDistribution = this.SourceBounds.IsWidthZero ?
                        this.SourceBounds.IsHeightZero ? ComposerPointsDistribution.Point : ComposerPointsDistribution.ColumnLine :
                        this.SourceBounds.IsHeightZero ? ComposerPointsDistribution.RowLine : ComposerPointsDistribution.Panel;
                    break;
            }

            float cx = (this.SourceBounds.Left + this.SourceBounds.Right) / 2f;
            float cy = (this.SourceBounds.Top + this.SourceBounds.Bottom) / 2f;

            switch (this.PointsDistribution)
            {
                case ComposerPointsDistribution.Empty:
                    this.SourceBounds = Bounds.Infinity;

                    this.Panel.StartingTriangle = this.Panel.Triangle = Triangle.Empty;

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case ComposerPointsDistribution.Point:
                    this.Point.Point = new Vector2(cx, cy);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case ComposerPointsDistribution.RowLine:
                    this.Line.Point0 = new Vector2(this.SourceBounds.Left, cy);
                    this.Line.Point1 = new Vector2(this.SourceBounds.Right, cy);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case ComposerPointsDistribution.ColumnLine:
                    this.Line.Point0 = new Vector2(cx, this.SourceBounds.Top);
                    this.Line.Point1 = new Vector2(cx, this.SourceBounds.Bottom);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case ComposerPointsDistribution.Panel:
                    this.Panel.StartingTriangle = this.Panel.Triangle = new Triangle(this.SourceBounds);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                default:
                    break;
            }
        }
    }
}