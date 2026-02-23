using FanKit.Transformer.Cache;
using FanKit.Transformer.Polylines;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoPolyline2 : Path2
    {
        public bool IsEditable;
        public bool IsSelected;
        public Box0 ActualBox;

        public float StrokeWidth = 4f;

        public DemoPolyline2(List<Figure2> items) : base(items)
        {
        }

        public DemoPolyline2(List<Figure2> items, Matrix3x2 matrix) : base(items, matrix)
        {
        }

        public void RectChoose(Bounds bounds)
        {
            this.IsSelected = bounds.Contains(this.Triangle);
        }

        public void UpdateCanvas()
        {
            this.ActualBox = new Box0(this.Triangle);
        }
    }
}