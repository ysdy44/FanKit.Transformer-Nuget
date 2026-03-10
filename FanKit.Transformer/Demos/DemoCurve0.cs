using FanKit.Transformer.Cache;
using FanKit.Transformer.Curves;
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoCurve0 : Path0
    {
        public Box0 ActualBox;

        public float StrokeWidth = 4f;

        public DemoCurve0(List<Segment0> items) : base(items)
        {
        }

        public void UpdateCanvas()
        {
            this.ActualBox = new Box0(this.Destination);
        }
    }
}