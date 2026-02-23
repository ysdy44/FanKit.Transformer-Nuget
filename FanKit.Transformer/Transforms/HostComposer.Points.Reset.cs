using FanKit.Transformer.Indicators;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
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
                    // Step 0. Initialize
                    this.SizeType = IndicatorSizeType.Empty;
                    break;
                case 1:
                    // Step 0. Initialize
                    this.SizeType = IndicatorSizeType.Point;
                    break;
                default:
                    // Step 0. Initialize
                    this.SizeType = this.SourceBounds.IsWidthZero ?
                        this.SourceBounds.IsHeightZero ? IndicatorSizeType.Point : IndicatorSizeType.ColumnLine :
                        this.SourceBounds.IsHeightZero ? IndicatorSizeType.RowLine : IndicatorSizeType.Panel;
                    break;
            }

            float cx = (this.SourceBounds.Left + this.SourceBounds.Right) / 2f;
            float cy = (this.SourceBounds.Top + this.SourceBounds.Bottom) / 2f;

            switch (this.SizeType)
            {
                case IndicatorSizeType.Empty:
                    // Step 0. Initialize
                    this.SourceBounds = Bounds.Infinity;

                    // Step 1. Transformer
                    this.Panel.StartingTriangle = this.Panel.Triangle = Triangle.Empty;

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
                case IndicatorSizeType.Point:
                    // Step 0. Initialize
                    //this.SourceBounds = Bounds.Empty;

                    // Step 1. Transformer
                    this.Point.Point = new Vector2(cx, cy);
                    //this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
                case IndicatorSizeType.RowLine:
                    // Step 0. Initialize
                    //this.SourceBounds = Bounds.Empty;

                    // Step 1. Transformer
                    this.Line.Point0 = new Vector2(this.SourceBounds.Left, cy);
                    this.Line.Point1 = new Vector2(this.SourceBounds.Right, cy);
                    //this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
                case IndicatorSizeType.ColumnLine:
                    // Step 0. Initialize
                    //this.SourceBounds = Bounds.Empty;

                    // Step 1. Transformer
                    this.Line.Point0 = new Vector2(cx, this.SourceBounds.Top);
                    this.Line.Point1 = new Vector2(cx, this.SourceBounds.Bottom);
                    //this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
                case IndicatorSizeType.Panel:
                    //this.SourceBounds = Bounds.Empty;

                    // Step 1. Transformer
                    this.Panel.StartingTriangle = this.Panel.Triangle = new Triangle(this.SourceBounds);

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
                default:
                    break;
            }
        }
    }
}