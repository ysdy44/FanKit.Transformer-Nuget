using FanKit.Transformer.Indicators;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public void Reset(Vector2 point)
        {
            // Step 0. Initialize
            this.Count = 1;
            this.SizeType = SizeType.Point;

            // Step 1. Transformer
            this.Point.StartingPoint = this.Point.Point = point;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
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
                    // Step 0. Initialize
                    this.SizeType = SizeType.Empty;
                    break;
                case 1:
                    // Step 0. Initialize
                    this.SizeType = SizeType.Point;
                    break;
                default:
                    // Step 0. Initialize
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
                case SizeType.Point:
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
                case SizeType.RowLine:
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
                case SizeType.ColumnLine:
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
                case SizeType.Panel:
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