using FanKit.Transformer.Compute;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class HostComposer
    {
        public int Count { get; private set; }
        public SizeType SizeType { get; private set; }
        Bounds SourceBounds;

        public float TranslationX => this.Host.Matrix.M31;
        public float TranslationY => this.Host.Matrix.M32;
        public Matrix3x2 TransformMatrix => this.Host.Matrix;

        readonly M3x2 Host;
        readonly ComposerPoint Point;
        readonly ComposerLine Line;
        readonly ComposerTriangle Panel;

        public HostComposer()
        {
            this.Host = new M3x2();
            this.Point = new ComposerPoint(this.Host);
            this.Line = new ComposerLine(this.Host);
            this.Panel = new ComposerTriangle(this.Host);
        }
    }
}