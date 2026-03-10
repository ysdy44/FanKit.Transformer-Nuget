using FanKit.Transformer.Indicators;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostTriangle
    {
        public TransformerSizeType TransformerSizeType(IndicatorKind kind)
        {
            return Indicator.ToTransformerSizeType(this.Count, kind);
        }

        public void Reset()
        {
            // Step 0. Initialize
            this.Count = 0;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        public void Reset(Triangle triangle)
        {
            // Step 0. Initialize
            this.Count = 1;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle = triangle;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        public void Reset(ChildRectTriangle item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(ChildSizeTriangle item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
        public void Reset(InvertibleRectTriangle item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(Curves.Path2 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(Polylines.Path2 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(Curves.Path3 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(Polylines.Path3 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(RectTriangle item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(SizeTriangle item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
    }
}