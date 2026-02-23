using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public ComposerSizeType ComposerSizeType(IndicatorKind kind)
        {
            return Indicator.ToComposerSizeType(this.SizeType, kind);
        }

        public void Reset(ChildRectTriangle item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(ChildSizeTriangle item)
        {
            Bounds bounds = new Bounds(item.Source.Width, item.Source.Height);
            this.Reset(bounds, item.Triangle, item.Matrix);
        }
        public void Reset(InvertibleRectTriangle item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(Curves.Path2 item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(Polylines.Path2 item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(Curves.Path3 item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(Polylines.Path3 item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(RectTriangle item) => this.Reset(item.SourceBounds, item.Triangle, item.Matrix);
        public void Reset(SizeTriangle item)
        {
            Bounds bounds = new Bounds(item.Source.Width, item.Source.Height);
            this.Reset(bounds, item.Triangle, item.Matrix);
        }
    }
}