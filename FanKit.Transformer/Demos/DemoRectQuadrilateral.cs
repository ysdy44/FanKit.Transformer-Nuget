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

        public DemoRectQuadrilateral(Bounds source, Quadrilateral quad)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Quadrilateral = quad;
            this.Reset();
        }
        public DemoRectQuadrilateral(Bounds source, Matrix4x4 matrix)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Matrix = matrix;

            FreeTransformedRectangle rect = new FreeTransformedRectangle(this.Source.Rect, matrix);
            this.Quadrilateral = new Quadrilateral
            {
                LeftTop = rect.LeftTop,
                RightTop = rect.RightTop,
                LeftBottom = rect.LeftBottom,
                RightBottom = rect.RightBottom,
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