using FanKit.Transformer.Cache;
using FanKit.Transformer.Transforms;

namespace FanKit.Transformer.Demos
{
    public class DemoHostTriangle0 : HostTriangle
    {
        public Box0 ActualBox;

        public void UpdateCanvas()
        {
            switch (this.Count)
            {
                case 0:
                    this.ActualBox = default;
                    break;
                default:
                    this.ActualBox = new Box0(this.Triangle);
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
                    this.ActualBox = new Box0(this.Triangle, matrix);
                    break;
            }
        }
    }
}