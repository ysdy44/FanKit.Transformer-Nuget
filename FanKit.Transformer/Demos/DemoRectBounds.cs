using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectBounds : RectBounds
    {
        public Matrix3x2 ActualMatrix;
        public Box1 ActualBox;

        public DemoRectBounds(Bounds source, Bounds destination)
        {
            this.UpdateAll(source, destination);
        }

        public DemoRectBounds(Bounds source, Matrix2x2 matrix)
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
            this.ActualMatrix = this.HomographyMatrix.ToMatrix3x2();

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