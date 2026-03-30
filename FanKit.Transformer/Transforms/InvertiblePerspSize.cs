using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public class InvertiblePerspSize
    {
        readonly PerspSize Core = new PerspSize();

        public Matrix4x4 HomographyMatrix => this.Core.m;
        public void FindHomography(Quadrilateral source, float destinationWidth, float destinationHeight) => this.Core.FindHomography(source, destinationWidth, destinationHeight);
    }
}