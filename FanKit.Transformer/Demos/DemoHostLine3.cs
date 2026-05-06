using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostLine3 : HostLine
    {
        public Line3 ActualLine;

        public DemoHostLine3()
        {
        }

        public void UpdateCanvas()
        {
            this.ActualLine = new Line3(this.Point0, this.Point1);
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            this.ActualLine = new Line3(this.Point0, this.Point1, canvasMatrix);
        }
    }
}