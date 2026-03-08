using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoQuadrilateralRect : QuadrilateralRect
    {
        public Matrix4x4 ActualDestinationMatrix;
        public Box0 ActualSourceBox;

        public Quadrilateral ActualDestBox;

        public DemoQuadrilateralRect(float destWidth, float destHeight)
        {
            this.Initialize(destWidth, destHeight);
        }
        public DemoQuadrilateralRect(float destWidth, float destHeight, Quadrilateral source)
        {
            this.UpdateAll(destWidth, destHeight, source);
        }

        public void ResetCanvas()
        {
            this.ActualDestinationMatrix = Matrix4x4.Identity;
            this.ActualSourceBox = new Box0(this.Quadrilateral);

            this.ActualDestBox = new Quadrilateral(0f, 0f, this.DestinationWidth, this.DestinationHeight);
        }

        public void UpdateCanvas()
        {
            this.ActualDestinationMatrix = this.Matrix;
            this.ActualSourceBox = new Box0(this.Quadrilateral);

            this.ActualDestBox = new Quadrilateral(0f, 0f, this.DestinationWidth, this.DestinationHeight);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualDestinationMatrix = matrix.Matrix.ToMatrix3x3();
            this.ActualSourceBox = new Box0(this.Quadrilateral, matrix);

            this.ActualDestBox = new Quadrilateral(this.DestinationWidth, this.DestinationHeight, matrix.Matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualDestinationMatrix = Math.Transform(this.Matrix, matrix.Matrix);
            this.ActualSourceBox = new Box0(this.Quadrilateral, matrix);

            this.ActualDestBox = new Quadrilateral(this.DestinationWidth, this.DestinationHeight, matrix.Matrix);
        }
    }
}