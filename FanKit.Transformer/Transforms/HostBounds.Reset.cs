using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostBounds
    {
        public CropperSizeType CropperSizeType(IndicatorKind kind)
        {
            return Indicator.ToCropperSizeType(this.Count, kind);
        }

        public void Reset()
        {
            // Step 0. Initialize
            this.Count = 0;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;
        }

        public void Reset(Bounds bounds)
        {
            // Step 0. Initialize
            this.Count = 1;

            // Step 1. Transformer
            this.StartingBounds = this.Bounds = bounds;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;
        }

        public void Reset(ChildRectBounds item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(ChildSizeBounds item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
        //public void Reset(InvertibleRectBounds item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        //public void Reset(Curves.Path2 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        //public void Reset(Polylines.Path2 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        //public void Reset(Curves.Path3 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        //public void Reset(Polylines.Path3 item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(RectBounds item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(SizeBounds item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
    }
}