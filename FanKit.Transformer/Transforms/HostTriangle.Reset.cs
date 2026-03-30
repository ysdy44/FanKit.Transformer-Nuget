using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
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
            this.Count = 0;

            this.Host.Matrix = Matrix3x2.Identity;
        }

        public void Reset(Triangle destination)
        {
            this.Count = 1;

            this.Panel.StartingTriangle = this.Panel.Triangle = destination;

            this.Host.Matrix = Matrix3x2.Identity;
        }

        public void Reset(ChildRectTriangle item) => this.Reset(item.SourceBounds, item.Destination, item.HomographyMatrix);
        public void Reset(ChildSizeTriangle item)
        {
            Bounds bounds = new Bounds(item.SourceWidth, item.SourceHeight);
            this.Reset(bounds, item.Destination, item.HomographyMatrix);
        }
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