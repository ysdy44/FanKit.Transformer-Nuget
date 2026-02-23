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

        public DemoCurve3(List<Figure3> items) : base(items)
        {
        }

        public DemoCurve3(List<Figure3> items, Matrix3x2 matrix) : base(items, matrix)
        {
        }

        public void RectChoose(Bounds bounds)
        {
            this.IsSelected = bounds.Contains(this.Triangle);
        }

        public void UpdateCanvas()
        {
            this.ActualStrokeWidth = this.StrokeWidth;
            this.ActualBox = new Box0(this.Triangle);

            foreach (Figure3 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment3 segment = figure.Data[i];
                    figure.Data[i] = new Segment3
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

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualStrokeWidth = matrix.Scale(this.StrokeWidth);
            this.ActualBox = new Box0(this.Triangle, matrix);

            foreach (Figure3 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment3 segment = figure.Data[i];
                    figure.Data[i] = new Segment3
                    {
                        Actual = matrix.Transform(segment.Map),
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