using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class ChildSizeBounds
    {
        // Step 0. Initialize
        //public int Count;
        public SizeSource Source;

        // Step 1. Transformer
        //Bounds TransformedBounds;
        public Bounds StartingBounds;
        public Bounds Bounds;

        // Step 2. Homography Matrix
        Matrix2x2 DestNorm;

        // Step 3. Matrix
        public Matrix2x2 StartingMatrix;
        public Matrix2x2 Matrix;
        //public Matrix2x2 InverseMatrix;

        // Step 4. Host
        //InvertibleMatrix2x2 HostSourceNorm;
        //Matrix2x2 HostDestNorm;
        Matrix2x2 Host;
        //float HostTranslateX => this.Host.TranslateX;
        //float HostTranslateY => this.Host.TranslateY;
        //Matrix2x2 HostMatrix => this.Host;

        // Step 6. Controller
        //CropController Controller;

        //ControllerRadians Radians;

        /*
        void Invert()
        {
            Matrix2x2.Invert(this.Matrix, out this.InverseMatrix);
        }
         */

        void Find()
        {
            this.DestNorm = this.Bounds.Normalize();
            this.Matrix = this.Source.Map(this.DestNorm);
            //this.Invert();
        }

        /*
        void FindHomography()
        {
            this.HostDestNorm = this.Bounds.Normalize();
            this.Host = this.HostSourceNorm * this.HostDestNorm;
        }
         */

        public void Initialize(SizeSource source)
        {
            // Step 0. Initialize
            this.Source = source;

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = Matrix2x2.Identity;
            //this.InverseMatrix = Matrix2x2.Identity;

            // Step 4. Host
            this.Host = Matrix2x2.Identity;

            // Step 1. Transformer
            //this.TransformedBounds = default;
            this.StartingBounds = this.Bounds = new Bounds(this.Source.Width, this.Source.Height);
        }

        public void Initialize(SizeSource source, Matrix2x2 matrix)
        {
            // Step 0. Initialize
            this.Source = source;

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = matrix;
            //this.Invert();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;

            // Step 1. Transformer
            //this.TransformedBounds = default;
            this.StartingBounds = this.Bounds = new Bounds(this.Source.Width, this.Source.Height, this.Matrix);
        }

        public void Extend(SizeSource source)
        {
            // Step 0. Initialize
            this.Source = source;

            // Step 4. Host
            this.Host = Matrix2x2.Identity;

            // Step 1. Transformer
            //this.TransformedBounds = default;
            this.StartingBounds = this.Bounds = new Bounds(this.Source.Width, this.Source.Height, this.Matrix);
        }

        public void Reset()
        {
            // Step 0. Initialize
            //this.Count = 0;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;
        }

        public void Reset(Bounds bounds)
        {
            // Step 0. Initialize
            //this.Count = 1;

            // Step 1. Transformer
            this.StartingBounds = this.Bounds = bounds;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;
        }

        #region Bounds.Set
        public void SetTranslation(Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.TranslateX, this.Host.TranslateY);
            //this.Invert();
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.TranslateX, this.Host.TranslateY);
            //this.Invert();

            indicator.ChangeXY(this.Bounds, mode);
        }

        public void SetTranslationX(float translateX)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.TranslateX);
            //this.Invert();
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.TranslateX);
            //this.Invert();

            indicator.ChangeX(this.Bounds, mode);
        }

        public void SetTranslationY(float translateY)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.TranslateY);
            //this.Invert();
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.TranslateY);
            //this.Invert();

            indicator.ChangeY(this.Bounds, mode);
        }

        public void SetTransform(Matrix2x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix2x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeAll(this.Bounds, mode);
        }

        public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateWidth(this.StartingBounds, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateHeight(this.StartingBounds, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }

        /*
        public void SetRotation(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            // Step 4. Host
            this.Host = indicator.CreateRotation(rotationAngleInDegrees);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeXYWHRS(this.Bounds, mode);
        }
        public void SetSkew(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        {
            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateSkew(this.StartingBounds, mode, skewAngleInDegrees, minimum, maximum);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWHRS(this.Bounds, mode);
        }
         */
        #endregion

        #region Bounds.Transform
        public void CacheTranslation()
        {
            this.StartingBounds = this.Bounds;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            this.Host = Matrix2x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingBounds = this.Bounds;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            this.Host = Matrix2x2.Identity;
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix2x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix2x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        public void Translate(Vector2 translate)
        {
            this.Host = Matrix2x2.CreateTranslation(translate);
            this.T();
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix2x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        public void Translate(float translateX, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeXY(this.Bounds, mode);
        }

        public void TranslateX(float translateX)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix2x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeXY(this.Bounds, mode);
        }

        public void TranslateY(float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix2x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeXY(this.Bounds, mode);
        }

        public void Transform(Matrix2x2 matrix)
        {
            this.Host = matrix;

            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }

        private void T()
        {
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.TranslateX, this.Host.TranslateY);
            //this.Invert();
        }
        private void TX()
        {
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.TranslateX);
            //this.Invert();
        }
        private void TY()
        {
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.TranslateY);
            //this.Invert();
        }
        #endregion
    }
}