using FanKit.Transformer.Cache;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoHostComposer2 : HostComposer
    {
        public Vector2 ActualPoint;
        public Line2 ActualLine;
        public Box2 ActualBox;

        public void UpdateCanvas()
        {
            switch (this.SizeType)
            {
                case SizeType.Empty:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.Point:
                    this.ActualPoint = this.Point.Point;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.Line.Point0, this.Line.Point1);
                    this.ActualBox = default;
                    break;
                case SizeType.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.Line.Point0, this.Line.Point1);
                    this.ActualBox = default;
                    break;
                case SizeType.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box2(this.Panel.Triangle);
                    break;
                default:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
            }
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            switch (this.SizeType)
            {
                case SizeType.Empty:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.Point:
                    this.ActualPoint = matrix.Transform(this.Point.Point);
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.Line.Point0, this.Line.Point1, matrix);
                    this.ActualBox = default;
                    break;
                case SizeType.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.Line.Point0, this.Line.Point1, matrix);
                    this.ActualBox = default;
                    break;
                case SizeType.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box2(this.Panel.Triangle, matrix);
                    break;
                default:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
            }
        }
    }
}