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

            this.ActualBox = new Box0(this.Destination);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.HomographyMatrix;

            this.ActualBox = new Box0(this.Destination);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = matrix.Matrix.ToMatrix3x3();

            this.ActualBox = new Box0(this.Destination, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.HomographyMatrix, matrix.Matrix);

            this.ActualBox = new Box0(this.Destination, matrix);
        }
    }
}