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

        /*
        public void Initialize(Bounds source)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = Matrix3x2.Identity;
            //this.InverseMatrix = Matrix3x2.Identity;

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = default;
            this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);
        }

        public void Initialize(Bounds source, Matrix3x2 matrix)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = matrix;
            this.Invert();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void Extend(Bounds source)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
        
            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds;
        }
         */

        public void Reset()
        {
            // Step 0. Initialize
            this.Count = 0;
            this.SizeType = SizeType.Empty;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        public void Reset(Vector2 point)
        {
            // Step 0. Initialize
            this.Count = 1;
            this.SizeType = SizeType.Point;

            // Step 1. Transformer
            this.Point.StartingPoint = this.Point.Point = point;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        public void Reset(Triangle triangle)
        {
            // Step 0. Initialize
            this.Count = 1;
            this.SizeType = SizeType.Panel;

            // Step 1. Transformer
            this.Panel.StartingTriangle = this.Panel.Triangle = triangle;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }
    }
}