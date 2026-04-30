using FanKit.Transformer.Cache;
using FanKit.Transformer.Polylines;
using FanKit.Transformer.Transforms;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoPolyline1 : Path1
    {
        public Box0 ActualBox;

        public float StrokeWidth = 4f;
        public float ActualStrokeWidth = 4f;

        public DemoPolyline1(List<Segment1> segments) : base(segments)
        {
        }

        public DemoPolyline1(List<Segment1> segments, Matrix3x2 matrix) : base(segments, matrix)
        {
        }

        public void UpdateCanvas()
        {
            this.ActualStrokeWidth = this.StrokeWidth;
            this.ActualBox = new Box0(this.Destination);

            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment1 item = this.Segments[i];
                this.Segments[i] = new Segment1
                {
                    Actual = item.Point,
                    // C# 9.0 : var a = item with { ... }

                    IsChecked = item.IsChecked,
                    Starting = item.Starting,
                    Point = item.Point,
                    //Actual = item.Actual,
                };
            }
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualStrokeWidth = matrix.Scale(this.StrokeWidth);
            this.ActualBox = new Box0(this.Destination, matrix);

            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment1 item = this.Segments[i];
                this.Segments[i] = new Segment1
                {
                    Actual = matrix.Transform(item.Point),
                    // C# 9.0 : var a = item with { ... }

                    IsChecked = item.IsChecked,
                    Starting = item.Starting,
                    Point = item.Point,
                    //Actual = item.Actual,
                };
            }
        }
    }
}