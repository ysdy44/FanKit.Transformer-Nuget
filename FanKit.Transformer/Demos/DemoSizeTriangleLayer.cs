using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoSizeTriangleLayer : ChildSizeTriangle
    {
        public bool IsSelected;
        public Matrix3x2 ActualMatrix;
        public Box0 ActualBox;

        public DemoSizeTriangleLayer(float sourceWidth, float sourceHeight, Triangle destination)
        {
            this.UpdateAll(sourceWidth, sourceHeight, destination);
        }
        public DemoSizeTriangleLayer(float sourceWidth, float sourceHeight, Matrix3x2 matrix)
        {
            this.Initialize(sourceWidth, sourceHeight, matrix);
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