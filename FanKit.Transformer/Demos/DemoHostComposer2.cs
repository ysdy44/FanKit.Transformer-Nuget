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

        public DemoHostComposer2()
        {
        }

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
                    this.ActualLine = new Line2(this.LinePoint0, this.LinePoint1);
                    this.ActualBox = default;
                    break;
                case SizeType.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.LinePoint0, this.LinePoint1);
                    this.ActualBox = default;
                    break;
                case SizeType.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box2(this.PanelDestination);
                    break;
                default:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
            }
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            switch (this.SizeType)
            {
                case SizeType.Empty:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.Point:
                    this.ActualPoint = canvasMatrix.Transform(this.PointPoint);
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case SizeType.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.LinePoint0, this.LinePoint1, canvasMatrix);
                    this.ActualBox = default;
                    break;
                case SizeType.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line2(this.LinePoint0, this.LinePoint1, canvasMatrix);
                    this.ActualBox = default;
                    break;
                case SizeType.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box2(this.PanelDestination, canvasMatrix);
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