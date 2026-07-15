using FanKit.Transformer.Cache;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoHostComposer3 : HostComposer
    {
        public Vector2 ActualPoint;
        public Line3 ActualLine;
        public Box3 ActualBox;

        public DemoHostComposer3()
        {
        }

        public void UpdateCanvas()
        {
            switch (this.PointsDistribution)
            {
                case ComposerPointsDistribution.Empty:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.Point:
                    this.ActualPoint = this.PointPoint;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line3(this.LinePoint0, this.LinePoint1);
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line3(this.LinePoint0, this.LinePoint1);
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box3(this.PanelDestination);
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
            switch (this.PointsDistribution)
            {
                case ComposerPointsDistribution.Empty:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.Point:
                    this.ActualPoint = canvasMatrix.Transform(this.PointPoint);
                    this.ActualLine = default;
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.RowLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line3(this.LinePoint0, this.LinePoint1, canvasMatrix);
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.ColumnLine:
                    this.ActualPoint = default;
                    this.ActualLine = new Line3(this.LinePoint0, this.LinePoint1, canvasMatrix);
                    this.ActualBox = default;
                    break;
                case ComposerPointsDistribution.Panel:
                    this.ActualPoint = default;
                    this.ActualLine = default;
                    this.ActualBox = new Box3(this.PanelDestination, canvasMatrix);
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