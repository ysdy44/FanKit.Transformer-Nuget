using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public class InvertiblePerspectiveRect
    {
        readonly PerspRect Core = new PerspRect();

        public Matrix4x4 HomographyMatrix => this.Core.m;
        public void FindHomography(Quadrilateral source, float destinationWidth, float destinationHeight) => this.Core.FindHomography(source, destinationWidth, destinationHeight);
        public void FindHomography(Quadrilateral source, Rectangle destination) => this.Core.FindHomography(source, destination);
    }
}