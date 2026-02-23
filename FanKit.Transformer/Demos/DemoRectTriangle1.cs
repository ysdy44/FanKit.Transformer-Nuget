using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectTriangle1 : RectTriangle
    {
        public Matrix3x2 ActualMatrix;
        public Box1 ActualBox;

        public DemoRectTriangle1(Bounds source, Triangle triangle)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Triangle = triangle;
            this.Reset();
        }
        public DemoRectTriangle1(Bounds source, Matrix3x2 matrix)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Matrix = matrix;

            TransformedRectangle rect = new TransformedRectangle(this.Source.Rect, matrix);
            this.Triangle = new Triangle
            {
                LeftTop = rect.LeftTop,
                RightTop = rect.RightTop,
                LeftBottom = rect.LeftBottom,
            };
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix3x2.Identity;

            this.ActualBox = new Box1(this.Triangle);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.Matrix;

            this.ActualBox = new Box1(this.Triangle);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = matrix.Matrix;

            this.ActualBox = new Box1(this.Triangle, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.Matrix, matrix.Matrix);

            this.ActualBox = new Box1(this.Triangle, matrix);
        }
    }
}