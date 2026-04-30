using FanKit.Transformer.Cache;
using FanKit.Transformer.Polylines;
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoPolyline3 : Path3
    {
        public bool IsEditable;
        public bool IsSelected;
        public Box0 ActualBox;

        public float StrokeWidth = 4f;
        public float ActualStrokeWidth = 4f;

        public DemoPolyline3(List<Figure3> figures) : base(figures)
        {
        }

        public DemoPolyline3(List<Figure3> figures, Matrix3x2 matrix) : base(figures, matrix)
        {
        }

        public void RectChoose(Bounds bounds)
        {
            this.IsSelected = bounds.Contains(this.Destination);
        }

        public void UpdateCanvas()
        {
            this.ActualStrokeWidth = this.StrokeWidth;
            this.ActualBox = new Box0(this.Destination);

            foreach (Figure3 figure in this.Figures)
            {
                for (int i = 0; i < figure.Segments.Count; i++)
                {
                    Segment3 item = figure.Segments[i];
                    figure.Segments[i] = new Segment3
                    {
                        Actual = item.Map,
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        Map = item.Map,
                        //Actual = item.Actual,
                    };
                }
            }
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualStrokeWidth = matrix.Scale(this.StrokeWidth);
            this.ActualBox = new Box0(this.Destination, matrix);

            foreach (Figure3 figure in this.Figures)
            {
                for (int i = 0; i < figure.Segments.Count; i++)
                {
                    Segment3 item = figure.Segments[i];
                    figure.Segments[i] = new Segment3
                    {
                        Actual = matrix.Transform(item.Map),
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        Map = item.Map,
                        //Actual = item.Actual,
                    };
                }
            }
        }
    }
}