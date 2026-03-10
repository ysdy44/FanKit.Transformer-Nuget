using FanKit.Transformer.Cache;
using FanKit.Transformer.Polylines;
using FanKit.Transformer.Transforms;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoPolyline0 : Path0
    {
        public Box0 ActualBox;

        public float StrokeWidth = 4f;

        public DemoPolyline0(List<Segment0> items) : base(items)
        {
        }

        public void UpdateCanvas()
        {
            this.ActualBox = new Box0(this.Destination);
        }
    }
}