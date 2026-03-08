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
        public int Count;
        public SizeType SizeType;
        public Bounds SourceBounds;

        // Step 1. Transformer

        // Step 2. Homography Matrix
        //Matrix3x2 DestNorm;

        // Step 3. Matrix
        //public Matrix3x2 StartingMatrix;
        //public Matrix3x2 Matrix;
        //public Matrix3x2 InverseMatrix;

        // Step 4. Host
        Matrix3x2 Host;
        public float HostTranslateX => this.Host.M31;
        public float HostTranslateY => this.Host.M32;
        public Matrix3x2 HostMatrix => this.Host;

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