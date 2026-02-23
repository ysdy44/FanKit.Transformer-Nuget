using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectTriangleLayer : ChildRectTriangle
    {
        public bool IsSelected;
        public Matrix3x2 ActualMatrix;
        public Box0 ActualBox;

        public DemoRectTriangleLayer(Bounds source, Triangle triangle)
        {
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
            this.Triangle = triangle;
            this.Reset();
        }
        public DemoRectTriangleLayer(Bounds source, Matrix3x2 matrix)
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

        public void RectChoose(Bounds bounds)
        {
            this.IsSelected = bounds.Contains(this.Triangle);
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