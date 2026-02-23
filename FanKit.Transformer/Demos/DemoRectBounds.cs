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

        public DemoRectBounds(Bounds source, Bounds bounds)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Bounds = bounds;
            this.Reset();
        }
        public DemoRectBounds(Bounds source, Matrix2x2 matrix)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Matrix = matrix;

            this.Bounds = new Bounds(this.Source.Rect, matrix);
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