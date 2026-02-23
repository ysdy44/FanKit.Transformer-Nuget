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

        public DemoSizeQuadrilateral(SizeSource source, Quadrilateral quad)
        {
            this.Source = source;
            this.Quadrilateral = quad;
            this.Reset();
        }
        public DemoSizeQuadrilateral(SizeSource source, Matrix4x4 matrix)
        {
            this.Source = source;
            this.Matrix = matrix;

            FreeTransformedSize size = new FreeTransformedSize(source.Width, source.Height, matrix);
            this.Quadrilateral = new Quadrilateral
            {
                LeftTop = size.LeftTop,
                RightTop = size.RightTop,
                LeftBottom = size.LeftBottom,
                RightBottom = size.RightBottom,
            };
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