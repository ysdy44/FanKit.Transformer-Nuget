using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoSizeQuadrilateral : SizeQuadrilateral
    {
        public Matrix4x4 ActualMatrix;
        public Box0 ActualBox;

        public DemoSizeQuadrilateral(float sourceWidth, float sourceHeight, Quadrilateral destination)
        {
            this.UpdateAll(sourceWidth, sourceHeight, destination);
        }
        public DemoSizeQuadrilateral(float sourceWidth, float sourceHeight, Matrix4x4 matrix)
        {
            this.Initialize(sourceWidth, sourceHeight, matrix);
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix4x4.Identity;

            this.ActualBox = new Box0(this.Quadrilateral);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.Matrix;

            this.ActualBox = new Box0(this.Quadrilateral);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = matrix.Matrix.ToMatrix3x3();

            this.ActualBox = new Box0(this.Quadrilateral, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.Matrix, matrix.Matrix);

            this.ActualBox = new Box0(this.Quadrilateral, matrix);
        }
    }
}