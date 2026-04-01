using FanKit.Transformer.Indicators;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public void Reset(Vector2 point)
        {
            this.Count = 1;
            this.SizeType = SizeType.Point;

            this.Point.StartingPoint = this.Point.Point = point;

            this.Host.Matrix = Matrix3x2.Identity;
        }

        public void Extend(Node segment) { this.Count++; this.Ex(segment.Point); }

        public void Extend(Polylines.Segment0 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point); } }
        public void Extend(Polylines.Segment1 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point); } }
        public void Extend(Polylines.Segment2 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map); } }
        public void Extend(Polylines.Segment3 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map); } }

        public void Extend(Curves.Segment0 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point.Point); } }
        public void Extend(Curves.Segment1 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Point.Point); } }
        public void Extend(Curves.Segment2 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map.Point); } }
        public void Extend(Curves.Segment3 segment) { if (segment.IsChecked) { this.Count++; this.Ex(segment.Map.Point); } }

        public void EndExtendByPoints()
        {
            switch (this.Count)
            {
                case 0:
                    this.SizeType = SizeType.Empty;
                    break;
                case 1:
                    this.SizeType = SizeType.Point;
                    break;
                default:
                    this.SizeType = this.SourceBounds.IsWidthZero ?
                        this.SourceBounds.IsHeightZero ? SizeType.Point : SizeType.ColumnLine :
                        this.SourceBounds.IsHeightZero ? SizeType.RowLine : SizeType.Panel;
                    break;
            }

            float cx = (this.SourceBounds.Left + this.SourceBounds.Right) / 2f;
            float cy = (this.SourceBounds.Top + this.SourceBounds.Bottom) / 2f;

            switch (this.SizeType)
            {
                case SizeType.Empty:
                    this.SourceBounds = Bounds.Infinity;

                    this.Panel.StartingTriangle = this.Panel.Triangle = Triangle.Empty;

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case SizeType.Point:
                    this.Point.Point = new Vector2(cx, cy);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case SizeType.RowLine:
                    this.Line.Point0 = new Vector2(this.SourceBounds.Left, cy);
                    this.Line.Point1 = new Vector2(this.SourceBounds.Right, cy);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case SizeType.ColumnLine:
                    this.Line.Point0 = new Vector2(cx, this.SourceBounds.Top);
                    this.Line.Point1 = new Vector2(cx, this.SourceBounds.Bottom);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                case SizeType.Panel:
                    this.Panel.StartingTriangle = this.Panel.Triangle = new Triangle(this.SourceBounds);

                    this.Host.Matrix = Matrix3x2.Identity;
                    break;
                default:
                    break;
            }
        }
    }
}