using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectTriangle1 : RectTriangle
    {
        public Matrix3x2 ActualMatrix;
        public Box1 ActualBox;

        public DemoRectTriangle1(Bounds source, Triangle destination)
        {
            this.UpdateAll(source, destination);
        }

        public DemoRectTriangle1(Bounds source, Matrix3x2 matrix)
        {
            this.Initialize(source, matrix);
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix3x2.Identity;

            this.ActualBox = new Box1(this.Destination);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.HomographyMatrix;

            this.ActualBox = new Box1(this.Destination);
        }

        public void ResetCanvas(ICanvasMatrix canvasMatrix)
        {
            this.ActualMatrix = canvasMatrix.Matrix;

            this.ActualBox = new Box1(this.Destination, canvasMatrix);
        }

        public void UpdateCanvas(ICanvasMatrix canvasMatrix)
        {
            this.ActualMatrix = Math.Transform(this.HomographyMatrix, canvasMatrix.Matrix);

            this.ActualBox = new Box1(this.Destination, canvasMatrix);
        }
    }
}