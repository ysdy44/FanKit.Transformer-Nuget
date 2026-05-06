namespace FanKit.Transformer.Demos
{
    public class DemoStrokeWidth
    {
        public float Value;

        public float ActualValue;

        public DemoStrokeWidth()
        {
        }

        public void UpdateCanvas()
        {
            this.ActualValue = this.Value;
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            this.ActualValue = canvasMatrix.Scale(this.Value);
        }
    }
}