using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostLine0 : HostLine
    {
        public Line0 ActualLine;

        public DemoHostLine0()
        {
        }

        public void UpdateCanvas()
        {
            this.ActualLine = new Line0(this.Point0, this.Point1);
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            this.ActualLine = new Line0(this.Point0, this.Point1, canvasMatrix);
        }
    }
}