namespace FanKit.Transformer.Demos
{
    public class DemoStrokeWidth
    {
        public float Value;

        public float ActualValue;

        public void UpdateCanvas()
        {
            this.ActualValue = this.Value;
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualValue = matrix.Scale(this.Value);
        }
    }
}