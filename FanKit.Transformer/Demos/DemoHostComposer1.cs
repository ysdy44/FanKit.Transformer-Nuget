using FanKit.Transformer.Cache;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoHostComposer1 : HostComposer
    {
        public Vector2 ActualPoint;
        public Line1 ActualLine;
        public Box1 ActualBox;

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
                    this.ActualPoint = this.PointPoint;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line1(this.LinePoint0, this.LinePoint1);
                    this.ActualBox = default;
                    break;
                case SizeType.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line1(this.LinePoint0, this.LinePoint1);
                    this.ActualBox = default;
                    break;
                case SizeType.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box1(this.PanelDestination);
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
                    this.ActualPoint = matrix.Transform(this.PointPoint);
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line1(this.LinePoint0, this.LinePoint1, matrix);
                    this.ActualBox = default;
                    break;
                case SizeType.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line1(this.LinePoint0, this.LinePoint1, matrix);
                    this.ActualBox = default;
                    break;
                case SizeType.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box1(this.PanelDestination, matrix);
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