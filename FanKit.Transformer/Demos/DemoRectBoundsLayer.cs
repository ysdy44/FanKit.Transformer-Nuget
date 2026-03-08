using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoRectBoundsLayer : ChildRectBounds
    {
        public bool IsSelected;
        public Matrix3x2 ActualMatrix;
        public Box0 ActualBox;

        public DemoRectBoundsLayer(Bounds source, Bounds destination)
        {
            this.UpdateAll(source, destination);
        }
        public DemoRectBoundsLayer(Bounds source, Matrix2x2 matrix)
        {
            this.Initialize(source, matrix);
        }

        public void RectChoose(Bounds bounds)
        {
            this.IsSelected = bounds.Contains(this.Bounds);
        }

        public void ResetCanvas()
        {
            this.ActualMatrix = Matrix3x2.Identity;

            this.ActualBox = new Box0(this.Bounds);
        }

        public void UpdateCanvas()
        {
            this.ActualMatrix = this.Matrix.ToMatrix3x2();

            this.ActualBox = new Box0(this.Bounds);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = this.Matrix.ToMatrix3x2();

            this.ActualBox = new Box0(this.Bounds, matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualMatrix = Math.Transform(this.Matrix, matrix.Matrix);

            this.ActualBox = new Box0(this.Bounds, matrix);
        }
    }
}