using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoSizeBounds : SizeBounds
    {
        public Matrix3x2 ActualMatrix;
        public Box1 ActualBox;

        public DemoSizeBounds(float sourceWidth, float sourceHeight, Bounds destination)
        {
            this.UpdateAll(sourceWidth, sourceHeight, destination);
        }
        public DemoSizeBounds(float sourceWidth, float sourceHeight, Matrix2x2 matrix)
        {
            this.Initialize(sourceWidth, sourceHeight, matrix);
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix3x2.Identity;

            this.ActualBox = new Box1(this.Bounds);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.Matrix.ToMatrix3x2();

            this.ActualBox = new Box1(this.Bounds);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = matrix.Matrix;

            this.ActualBox = new Box1(this.Bounds, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.Matrix, matrix.Matrix);

            this.ActualBox = new Box1(this.Bounds, matrix);
        }
    }
}