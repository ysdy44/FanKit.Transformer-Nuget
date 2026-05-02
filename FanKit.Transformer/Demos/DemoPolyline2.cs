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

        public DemoPolyline2(List<Figure2> figures) : base(figures)
        {
        }

        public DemoPolyline2(List<Figure2> figures, Matrix3x2 homographyMatrix) : base(figures, homographyMatrix)
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