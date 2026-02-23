using FanKit.Transformer.Cache;
using FanKit.Transformer.Curves;
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoCurve1 : Path1
    {
        public Box0 ActualBox;

        public float StrokeWidth = 4f;
        public float ActualStrokeWidth = 4f;

        public DemoCurve1(List<Segment1> items) : base(items)
        {
        }

        public void UpdateCanvas()
        {
            this.ActualStrokeWidth = this.StrokeWidth;
            this.ActualBox = new Box0(this.Triangle);

            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 segment = this.Data[i];
                this.Data[i] = new Segment1
                {
                    Actual = segment.Point,
                    // C# 9.0 : var a = item with { ... }

                    IsChecked = segment.IsChecked,
                    IsSmooth = segment.IsSmooth,
                    Starting = segment.Starting,
                    Point = segment.Point,
                    //Actual = item.Actual,
                };
            }
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualStrokeWidth = matrix.Scale(this.StrokeWidth);
            this.ActualBox = new Box0(this.Triangle, matrix);

            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 segment = this.Data[i];
                this.Data[i] = new Segment1
                {
                    Actual = matrix.Transform(segment.Point),
                    // C# 9.0 : var a = item with { ... }

                    IsChecked = segment.IsChecked,
                    IsSmooth = segment.IsSmooth,
                    Starting = segment.Starting,
                    Point = segment.Point,
                    //Actual = item.Actual,
                };
            }
        }
    }
}