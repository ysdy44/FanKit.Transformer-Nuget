using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using System.Numerics;

namespace FanKit.Transformer.Demos
{
    public class DemoQuadrilateralSize : QuadrilateralSize
    {
        public Matrix4x4 ActualDestinationMatrix;
        public Box0 ActualSourceBox;

        public Quadrilateral ActualDestBox;

        public DemoQuadrilateralSize(float destinationWidth, float destinationHeight)
        {
            this.Initialize(destinationWidth, destinationHeight);
        }
        public DemoQuadrilateralSize(float destinationWidth, float destinationHeight, Quadrilateral source)
        {
            this.UpdateAll(destinationWidth, destinationHeight, source);
        }

        public void ResetCanvas()
        {
            this.ActualDestinationMatrix = Matrix4x4.Identity;
            this.ActualSourceBox = new Box0(this.Source);

            this.ActualDestBox = new Quadrilateral(0f, 0f, this.DestinationWidth, this.DestinationHeight);
        }

        public void UpdateCanvas()
        {
            this.ActualDestinationMatrix = this.HomographyMatrix;
            this.ActualSourceBox = new Box0(this.Source);

            this.ActualDestBox = new Quadrilateral(0f, 0f, this.DestinationWidth, this.DestinationHeight);
        }

        public void ResetCanvas(ICanvasMatrix matrix)
        {
            this.ActualDestinationMatrix = matrix.Matrix.ToMatrix3x3();
            this.ActualSourceBox = new Box0(this.Source, matrix);

            this.ActualDestBox = new Quadrilateral(this.DestinationWidth, this.DestinationHeight, matrix.Matrix);
        }

        public void UpdateCanvas(ICanvasMatrix matrix)
        {
            this.ActualDestinationMatrix = Math.Transform(this.HomographyMatrix, matrix.Matrix);
            this.ActualSourceBox = new Box0(this.Source, matrix);

            this.ActualDestBox = new Quadrilateral(this.DestinationWidth, this.DestinationHeight, matrix.Matrix);
        }
    }
}