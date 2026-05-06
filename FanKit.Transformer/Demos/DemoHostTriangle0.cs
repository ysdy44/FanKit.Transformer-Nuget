using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostTriangle0 : HostTriangle
    {
        public Box0 ActualBox;

        public DemoHostTriangle0()
        {
        }

        public void UpdateCanvas()
        {
            switch (this.Count)
            {
                case 0:
                    this.ActualBox = default;
                    break;
                default:
                    this.ActualBox = new Box0(this.Destination);
                    break;
            }
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            switch (this.Count)
            {
                case 0:
                    this.ActualBox = default;
                    break;
                default:
                    this.ActualBox = new Box0(this.Destination, canvasMatrix);
                    break;
            }
        }
    }
}