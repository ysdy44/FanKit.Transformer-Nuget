using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostLine1 : HostLine
    {
        public Line1 ActualLine;

        public void UpdateCanvas()
        {
            this.ActualLine = new Line1(this.Point0, this.Point1);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualLine = new Line1(this.Point0, this.Point1, matrix);
        }
    }
}