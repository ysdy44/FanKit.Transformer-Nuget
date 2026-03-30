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
            this.Count = 0;

            this.Host.Matrix = Matrix2x2.Identity;
        }

        public void Reset(Bounds bounds)
        {
            this.Count = 1;

            this.Panel.StartingBounds = this.Panel.Bounds = bounds;

            this.Host.Matrix = Matrix2x2.Identity;
        }

        public void Reset(ChildRectBounds item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(ChildSizeBounds item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
        public void Reset(RectBounds item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(SizeBounds item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
    }
}