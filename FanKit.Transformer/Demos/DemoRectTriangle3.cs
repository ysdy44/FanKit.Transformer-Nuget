using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectTriangle3 : RectTriangle
    {
        public Matrix3x2 ActualMatrix;
        public Box3 ActualBox;

        public DemoRectTriangle3(Bounds source, Triangle destination)
        {
            this.UpdateAll(source, destination);
        }
        public DemoRectTriangle3(Bounds source, Matrix3x2 matrix)
        {
            this.Initialize(source, matrix);
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix3x2.Identity;

            this.ActualBox = new Box3(this.Triangle);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.Matrix;

            this.ActualBox = new Box3(this.Triangle);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = matrix.Matrix;

            this.ActualBox = new Box3(this.Triangle, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.Matrix, matrix.Matrix);

            this.ActualBox = new Box3(this.Triangle, matrix);
        }
    }
}