using FanKit.Transformer.Cache;
using FanKit.Transformer.Curves;
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoCurve2 : Path2
    {
        public bool IsEditable;
        public bool IsSelected;
        public Box0 ActualBox;

        public float StrokeWidth = 4f;

        public DemoCurve2(List<Figure2> items) : base(items)
        {
        }

        public DemoCurve2(List<Figure2> items, Matrix3x2 matrix) : base(items, matrix)
        {
        }

        public void RectChoose(Bounds bounds)
        {
            this.IsSelected = bounds.Contains(this.Destination);
        }

        public void UpdateCanvas()
        {
            this.ActualBox = new Box0(this.Destination);
        }
    }
}