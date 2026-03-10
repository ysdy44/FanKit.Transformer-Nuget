using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class HostComposer
    {
        // Step 0. Initialize
        public int Count { get; private set; }
        public SizeType SizeType { get; private set; }
        Bounds SourceBounds;

        // Step 1. Transformer

        // Step 2. Homography Matrix
        //Matrix3x2 DestNorm;

        // Step 3. Matrix
        //Matrix3x2 StartingMatrix;
        //Matrix3x2 Matrix;
        //Matrix3x2 InverseMatrix;
        //public Matrix3x2 HomographyMatrix => this.Matrix;
        //public Matrix3x2 HomographyInverseMatrix => this.InverseMatrix;

        // Step 4. Host
        Matrix3x2 Host;
        public float TranslationX => this.Host.M31;
        public float TranslationY => this.Host.M32;
        public Matrix3x2 TransformMatrix => this.Host;

        // Step 6. Controller

        public readonly ComposerPoint Point;
        public readonly ComposerLine Line;
        public readonly ComposerTriangle Panel;

        public HostComposer()
        {
            Point = new ComposerPoint(this);
            Line = new ComposerLine(this);
            Panel = new ComposerTriangle(this);
        }
    }
}