using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostTriangle3 : HostTriangle
    {
        public Box3 ActualBox;

        public void UpdateCanvas()
        {
            switch (this.Count)
            {
                case 0:
                    this.ActualBox = default;
                    break;
                default:
                    this.ActualBox = new Box3(this.Destination);
                    break;
            }
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            switch (this.Count)
            {
                case 0:
                    this.ActualBox = default;
                    break;
                default:
                    this.ActualBox = new Box3(this.Destination, matrix);
                    break;
            }
        }
    }
}