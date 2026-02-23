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

        public void Reset(ChildRectBounds item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        public void Reset(ChildSizeBounds item)
        {
            Bounds bounds = new Bounds(item.Source.Width, item.Source.Height);
            this.Reset(bounds, item.Bounds, item.Matrix);
        }
        //public void Reset(InvertibleRectBounds item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        //public void Reset(Curves.Path2 item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        //public void Reset(Polylines.Path2 item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        //public void Reset(Curves.Path3 item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        //public void Reset(Polylines.Path3 item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        public void Reset(RectBounds item) => this.Reset(item.SourceBounds, item.Bounds, item.Matrix);
        public void Reset(SizeBounds item)
        {
            Bounds bounds = new Bounds(item.Source.Width, item.Source.Height);
            this.Reset(bounds, item.Bounds, item.Matrix);
        }
    }
}