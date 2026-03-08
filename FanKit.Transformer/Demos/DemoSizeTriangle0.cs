using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoSizeTriangle0 : SizeTriangle
    {
        public Matrix3x2 ActualMatrix;
        public Box0 ActualBox;

        public DemoSizeTriangle0(float sourceWidth, float sourceHeight, Triangle destination)
        {
            this.UpdateAll(sourceWidth, sourceHeight, destination);
        }
        public DemoSizeTriangle0(float sourceWidth, float sourceHeight, Matrix3x2 matrix)
        {
            this.Initialize(sourceWidth, sourceHeight, matrix);
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix3x2.Identity;

            this.ActualBox = new Box0(this.Triangle);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.Matrix;

            this.ActualBox = new Box0(this.Triangle);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = matrix.Matrix;

            this.ActualBox = new Box0(this.Triangle, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.Matrix, matrix.Matrix);

            this.ActualBox = new Box0(this.Triangle, matrix);
        }
    }
}