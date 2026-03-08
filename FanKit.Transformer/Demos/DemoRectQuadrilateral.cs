using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectQuadrilateral : RectQuadrilateral
    {
        public Matrix4x4 ActualMatrix;
        public Box0 ActualBox;

        public DemoRectQuadrilateral(Bounds source, Quadrilateral destination)
        {
            this.UpdateAll(source, destination);
        }
        public DemoRectQuadrilateral(Bounds source, Matrix4x4 matrix)
        {
            this.Initialize(source, matrix);
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