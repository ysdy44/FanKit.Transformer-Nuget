using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class RectQuadrilateral
    {
        // Step 0. Initialize
        //public int Count;
        public Bounds SourceBounds;
        public RectSource Source;

        // Step 1. Transformer
        FreeTransformedBounds TransformedBounds;
        public Quadrilateral StartingQuadrilateral;
        public Quadrilateral Quadrilateral;

        // Step 2. Homography Matrix
        PerspRectMatrix3x3 DestNorm;

        // Step 3. Matrix
        public Matrix4x4 StartingMatrix;
        public Matrix4x4 Matrix;
        //public Matrix4x4 InverseMatrix;

        // Step 4. Host
        //InvertibleMatrix3x2 HostSourceNorm;
        //Matrix3x2 HostDestNorm;
        Matrix3x2 Host;
        //float HostTranslateX => this.Host.M31;
        //float HostTranslateY => this.Host.M32;
        //Matrix3x2 HostMatrix => this.Host;

        // Step 6. Controller
        FreeTransformController Controller;

        public QuadrilateralPointKind PointKind => this.Controller.PointKind;

        /*
        void Invert()
        {
            Matrix4x4.Invert(this.Matrix, out this.InverseMatrix);
        }
         */

        void Find()
        {
            this.DestNorm = this.Source.ToPerspMatrix(this.Quadrilateral);
            this.Matrix = this.DestNorm;
            //this.Invert()
        }

        /*
        void FindHomography()
        {
            this.HostDestNorm = this.Quadrilateral.Normalize();
            this.Host = this.HostSourceNorm * this.HostDestNorm;
        }
         */

        public void Initialize(Bounds source)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = Matrix4x4.Identity;
            //this.InverseMatrix = Matrix4x4.Identity;

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = default;
            this.StartingQuadrilateral = this.Quadrilateral = new Quadrilateral(this.SourceBounds);
        }

        public void Initialize(Bounds source, Matrix4x4 matrix)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = matrix;
            //this.Invert();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new FreeTransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingQuadrilateral = this.Quadrilateral = this.TransformedBounds.ToQuadrilateral();
        }

        public void Extend(Bounds source)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new FreeTransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingQuadrilateral = this.Quadrilateral = this.TransformedBounds.ToQuadrilateral();
        }

        public void Reset()
        {
            // Step 0. Initialize
            //this.Count = 0;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        public void Reset(Quadrilateral quad)
        {
            // Step 0. Initialize
            //this.Count = 1;

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral = quad;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        #region Quadrilaterals.FreeTransform
        /*
        public void CacheRotation(Vector2 point)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;

            this.Controller = new Controller(this.Quadrilateral, point);
        }
         */

        public void CacheFreeTransform(FreeTransformMode mode)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;

            this.Controller = new FreeTransformController(this.Quadrilateral, mode, 8f);
        }

        /*
        public void Rotate(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Quadrilateral = this.StartingQuadrilateral * this.Host;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }
        public void Rotate(IIndicator indicator, IndicatorMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Quadrilateral = this.StartingQuadrilateral * this.Host;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeXYWHRS(this.Quadrilateral, mode);
        }
         */

        public void MovePoint(Vector2 point)
        {
            this.Quadrilateral = this.StartingQuadrilateral.MovePoint(this.Controller.PointKind, point);

            this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        public void MovePointOfConvexQuadrilateral(Vector2 point)
        {
            this.Quadrilateral = this.Controller.MovePointOfConvexQuadrilateral(this.StartingQuadrilateral, point);

            this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Quadrilaterals.Set
        public void SetTranslation(Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert()
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert()

            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        public void SetTranslationX(float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert()
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert()

            indicator.ChangeX(this.Quadrilateral, mode);
        }

        public void SetTranslationY(float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert()
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert()

            indicator.ChangeY(this.Quadrilateral, mode);
        }

        public void SetTransform(Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Transform(this.StartingQuadrilateral, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
            //this.Invert();
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Transform(this.StartingQuadrilateral, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
            //this.Invert();

            indicator.ChangeAll(this.Quadrilateral, mode);
        }

        public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = indicator.CreateWidth(this.StartingQuadrilateral, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWH(this.Quadrilateral, mode);
        }
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = indicator.CreateHeight(this.StartingQuadrilateral, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWH(this.Quadrilateral, mode);
        }

        public void SetRotation(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            // Step 4. Host
            this.Host = indicator.CreateRotation(rotationAngleInDegrees);

            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = this.StartingQuadrilateral * this.Host;

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
            //this.Invert()

            indicator.ChangeXYWHRS(this.Quadrilateral, mode);
        }
        public void SetSkew(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        {
            // Step 1. Transformer
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = indicator.CreateSkew(this.StartingQuadrilateral, mode, skewAngleInDegrees, minimum, maximum);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWHRS(this.Quadrilateral, mode);
        }
        #endregion

        #region Quadrilaterals.Transform
        public void CacheTranslation()
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        public void Translate(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        public void Translate(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        public void TranslateX(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        public void TranslateY(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        public void Transform(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.Quadrilateral = this.StartingQuadrilateral * this.Host;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
            //this.Invert()
        }

        private void T()
        {
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert()
        }
        private void TX()
        {
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert()
        }
        private void TY()
        {
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert()
        }
        #endregion
    }
}