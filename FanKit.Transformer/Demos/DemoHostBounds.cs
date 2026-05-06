using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostBounds : HostBounds
    {
        public Box1 ActualBox;

        public DemoHostBounds()
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
                    this.ActualBox = new Box1(this.Destination);
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
                    this.ActualBox = new Box1(this.Destination, canvasMatrix);
                    break;
            }
        }
    }
}