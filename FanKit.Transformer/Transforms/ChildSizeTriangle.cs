using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class ChildSizeTriangle
    {
        // Step 0. Initialize
        //public int Count;
        public SizeSource Source;

        // Step 1. Transformer
        //TransformedBounds TransformedBounds;
        public Triangle StartingTriangle;
        public Triangle Triangle;

        // Step 2. Homography Matrix
        Matrix3x2 DestNorm;

        // Step 3. Matrix
        public Matrix3x2 StartingMatrix;
        public Matrix3x2 Matrix;
        //public Matrix3x2 InverseMatrix;

        // Step 4. Host
        //InvertibleMatrix3x2 HostSourceNorm;
        //Matrix3x2 HostDestNorm;
        Matrix3x2 Host;
        //float HostTranslateX => this.Host.M31;
        //float HostTranslateY => this.Host.M32;
        //Matrix3x2 HostMatrix => this.Host;

        // Step 6. Controller
        //TransformController Controller;

        //ControllerRadians Radians;

        /*
        void Invert()
        {
            Matrix3x2.Invert(this.Matrix, out this.InverseMatrix);
        }
         */

        void Find()
        {
            this.DestNorm = this.Triangle.Normalize();
            this.Matrix = this.Source.Affine(this.DestNorm);
            //this.Invert();
        }

        /*
        void FindHomography()
        {
            this.HostDestNorm = this.Triangle.Normalize();
            this.Host = this.HostSourceNorm * this.HostDestNorm;
        }
         */

        public void Initialize(SizeSource source)
        {
            // Step 0. Initialize
            this.Source = source;

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = Matrix3x2.Identity;
            //this.InverseMatrix = Matrix3x2.Identity;

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            //this.TransformedBounds = default;
            this.StartingTriangle = this.Triangle = new Triangle(this.Source.Width, this.Source.Height);
        }

        public void Initialize(SizeSource source, Matrix3x2 matrix)
        {
            // Step 0. Initialize
            this.Source = source;

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = matrix;
            //this.Invert();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            //this.TransformedBounds = default;
            this.StartingTriangle = this.Triangle = new Triangle(this.Source.Width, this.Source.Height, this.Matrix);
        }

        public void Extend(SizeSource source)
        {
            // Step 0. Initialize
            this.Source = source;

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            //this.TransformedBounds = default;
            this.StartingTriangle = this.Triangle = new Triangle(this.Source.Width, this.Source.Height, this.Matrix);
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

        public void Reset(Triangle triangle)
        {
            // Step 0. Initialize
            //this.Count = 1;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle = triangle;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        #region Triangles.Set
        public void SetTranslation(Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();

            indicator.ChangeXY(this.Triangle, mode);
        }

        public void SetTranslationX(float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();

            indicator.ChangeX(this.Triangle, mode);
        }

        public void SetTranslationY(float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();

            indicator.ChangeY(this.Triangle, mode);
        }

        public void SetTransform(Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeAll(this.Triangle, mode);
        }

        public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateWidth(this.StartingTriangle, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateHeight(this.StartingTriangle, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }

        public void SetRotation(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            // Step 4. Host
            this.Host = indicator.CreateRotation(rotationAngleInDegrees);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        public void SetSkew(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        {
            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateSkew(this.StartingTriangle, mode, skewAngleInDegrees, minimum, maximum);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            //this.FindHomography();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        #endregion

        #region Triangles.Transform
        public void CacheTranslation()
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
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
            indicator.ChangeXY(this.Triangle, mode);
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
            indicator.ChangeXY(this.Triangle, mode);
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
            indicator.ChangeXY(this.Triangle, mode);
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
            indicator.ChangeXY(this.Triangle, mode);
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
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void Transform(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();
        }
        #endregion
    }
}