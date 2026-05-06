using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoSizeTriangle1 : SizeTriangle
    {
        public Matrix3x2 ActualMatrix;
        public Box1 ActualBox;

        public DemoSizeTriangle1(float sourceWidth, float sourceHeight, Triangle destination)
        {
            this.UpdateAll(sourceWidth, sourceHeight, destination);
        }

        public DemoSizeTriangle1(float sourceWidth, float sourceHeight, Matrix3x2 matrix)
        {
            this.Initialize(sourceWidth, sourceHeight, matrix);
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