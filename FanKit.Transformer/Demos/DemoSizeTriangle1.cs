using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoSizeTriangle1 : SizeTriangle
    {
        public Matrix3x2 ActualMatrix;
        public Box1 ActualBox;

        public DemoSizeTriangle1(SizeSource source, Triangle triangle)
        {
            this.Source = source;
            this.Triangle = triangle;
            this.Reset();
        }
        public DemoSizeTriangle1(SizeSource source, Matrix3x2 matrix)
        {
            this.Source = source;
            this.Matrix = matrix;
            this.Triangle = new Triangle(source.Width, source.Height, matrix);
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