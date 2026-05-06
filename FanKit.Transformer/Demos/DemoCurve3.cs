using FanKit.Transformer.Cache;
using FanKit.Transformer.Curves;
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoCurve3 : Path3
    {
        public bool IsEditable;
        public bool IsSelected;
        public Box0 ActualBox;

        public float StrokeWidth = 4f;
        public float ActualStrokeWidth = 4f;

        public DemoCurve3(List<Figure3> figures) : base(figures)
        {
        }

        public DemoCurve3(List<Figure3> figures, Matrix3x2 homographyMatrix) : base(figures, homographyMatrix)
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
                    Segment3 segment = figure.Segments[i];
                    figure.Segments[i] = new Segment3
                    {
                        Actual = segment.Map,
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = segment.IsChecked,
                        IsSmooth = segment.IsSmooth,
                        Starting = segment.Starting,
                        Raw = segment.Raw,
                        Map = segment.Map,
                        //Actual = item.Actual,
                    };
                }
            }
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            this.ActualStrokeWidth = canvasMatrix.Scale(this.StrokeWidth);
            this.ActualBox = new Box0(this.Destination, canvasMatrix);

            foreach (Figure3 figure in this.Figures)
            {
                for (int i = 0; i < figure.Segments.Count; i++)
                {
                    Segment3 segment = figure.Segments[i];
                    figure.Segments[i] = new Segment3
                    {
                        Actual = canvasMatrix.Transform(segment.Map),
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = segment.IsChecked,
                        IsSmooth = segment.IsSmooth,
                        Starting = segment.Starting,
                        Raw = segment.Raw,
                        Map = segment.Map,
                        //Actual = item.Actual,
                    };
                }
            }
        }
    }
}