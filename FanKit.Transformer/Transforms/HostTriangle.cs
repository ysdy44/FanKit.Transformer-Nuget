using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Text;

namespace FanKit.Transformer.Transforms
{
    public partial class HostTriangle
    {
        // Step 0. Initialize
        public int Count;
        Bounds SourceBounds;

        // Step 1. Transformer
        //TransformedBounds TransformedBounds;
        public Triangle StartingTriangle;
        public Triangle Triangle;

        // Step 2. Homography Matrix
        //Matrix3x2 DestNorm;

        // Step 3. Matrix
        //public Matrix3x2 StartingMatrix;
        //public Matrix3x2 Matrix;
        //public Matrix3x2 InverseMatrix;

        // Step 4. Host
        InvertibleMatrix3x2 HostSourceNorm;
        Matrix3x2 HostDestNorm;
        Matrix3x2 Host;
        public float HostTranslateX => this.Host.M31;
        public float HostTranslateY => this.Host.M32;
        public Matrix3x2 HostMatrix => this.Host;

        // Step 6. Controller
        TransformController Controller;

        ControllerRadians Radians;

        //void Invert()
        //{
        //    Matrix3x2.Invert(this.Matrix, out this.InverseMatrix);
        //}

        //void Find()
        //{
        //    this.DestNorm = this.Triangle.Normalize();
        //    this.Matrix = this.Source * this.DestNorm;
        //    this.Invert();
        //}

        void FindHomography()
        {
            this.HostDestNorm = this.Triangle.Normalize();
            this.Host = this.HostSourceNorm.BidiAffine(this.HostDestNorm);
        }

        #region Triangles.Reset
        public void Reset(Bounds source, Triangle triangle, Matrix3x2 matrix)
        {
            // Step 0. Initialize
            this.Count = 1;
            //this.SizeType = IndicatorSizeType.Transform;
            this.SourceBounds = source;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle = triangle;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = matrix;
        }

        public void BeginExtend()
        {
            this.Count = 0;

            this.SourceBounds = Bounds.Infinity;
        }

        public void Extend(Bounds bounds)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.SourceBounds = Bounds.Infinity;

                    this.Eb(bounds);

                    this.StartingTriangle = this.Triangle = new Triangle(bounds);

                    this.Host = Matrix3x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(this.Triangle);

                    this.Eb(bounds);

                    this.Host = Matrix3x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.Eb(bounds);

                    this.Host = Matrix3x2.Identity;
                    break;
            }
        }

        public void Extend(Triangle triangle)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(triangle);

                    this.StartingTriangle = this.Triangle = triangle;

                    this.Host = Matrix3x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.E3(this.Triangle);

                    this.E3(triangle);

                    this.Host = Matrix3x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.E3(triangle);

                    this.Host = Matrix3x2.Identity;
                    break;
            }
        }

        public void Extend(Quadrilateral quad)
        {
            switch (this.Count)
            {
                case 0:
                    this.Count = 1;
                    this.SourceBounds = Bounds.Infinity;

                    this.E4(quad);

                    this.StartingTriangle = this.Triangle = quad.ToTriangle();

                    this.Host = Matrix3x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.Extend(this.Triangle);

                    this.E4(quad);

                    this.Host = Matrix3x2.Identity;
                    break;
                default:
                    this.Count++;

                    this.E4(quad);

                    this.Host = Matrix3x2.Identity;
                    break;
            }
        }

        private void Ex(Vector2 point)
        {
            if (this.SourceBounds.Left > point.X) this.SourceBounds.Left = point.X;
            if (this.SourceBounds.Top > point.Y) this.SourceBounds.Top = point.Y;
            if (this.SourceBounds.Right < point.X) this.SourceBounds.Right = point.X;
            if (this.SourceBounds.Bottom < point.Y) this.SourceBounds.Bottom = point.Y;
        }
        private void Eb(Bounds bounds)
        {
            if (this.SourceBounds.Left > bounds.Left) this.SourceBounds.Left = bounds.Left;
            if (this.SourceBounds.Top > bounds.Top) this.SourceBounds.Top = bounds.Top;
            if (this.SourceBounds.Right < bounds.Left) this.SourceBounds.Right = bounds.Left;
            if (this.SourceBounds.Bottom < bounds.Top) this.SourceBounds.Bottom = bounds.Top;

            if (this.SourceBounds.Left > bounds.Right) this.SourceBounds.Left = bounds.Right;
            if (this.SourceBounds.Top > bounds.Bottom) this.SourceBounds.Top = bounds.Bottom;
            if (this.SourceBounds.Right < bounds.Right) this.SourceBounds.Right = bounds.Right;
            if (this.SourceBounds.Bottom < bounds.Bottom) this.SourceBounds.Bottom = bounds.Bottom;
        }
        private void E3(Triangle triangle)
        {
            this.Ex(triangle.LeftTop);
            this.Ex(triangle.RightTop);
            this.Ex(triangle.LeftBottom);

            this.Ex(new Vector2(triangle.RightTop.X + triangle.LeftBottom.X - triangle.LeftTop.X,
                triangle.RightTop.Y + triangle.LeftBottom.Y - triangle.LeftTop.Y)); // Triangle -> Transformer
        }
        private void E4(Quadrilateral quad)
        {
            this.Ex(quad.LeftTop);
            this.Ex(quad.RightTop);
            this.Ex(quad.LeftBottom);

            this.Ex(quad.RightBottom);
        }

        public void EndExtend()
        {
            switch (this.Count)
            {
                case 0:
                    // Step 0. Initialize
                    //this.SizeType = IndicatorSizeType.Empty;
                    break;
                case 1:
                    // Step 0. Initialize
                    //this.SizeType = IndicatorSizeType.Transform;
                    break;
                default:
                    // Step 0. Initialize
                    //this.SizeType = IndicatorSizeType.Transform;
                    //this.SourceBounds = new Bounds(items);

                    // Step 1. Transformer
                    this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
            }
        }
        #endregion

        #region Triangles.Set
        public void SetTranslation(Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
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
            //this.Find();

            // Step 4. Host
            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateHeight(this.StartingTriangle, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.FindHomography();

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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
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
            //this.Find();

            // Step 4. Host
            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.FindHomography();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        #endregion

        #region Triangles.Transform
        public void CacheTranslation()
        {
            this.StartingTriangle = this.Triangle;
            //this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingTriangle = this.Triangle;
            //this.StartingMatrix = this.Matrix;

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
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
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();
        }
        #endregion

        #region Triangles.Transform2
        public void CacheRotation(Vector2 point)
        {
            this.StartingTriangle = this.Triangle;
            //this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;

            this.Controller = new TransformController(this.Triangle, point);
        }

        public void CacheTransform(TransformMode mode)
        {
            this.StartingTriangle = this.Triangle;
            //this.StartingMatrix = this.Matrix;

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;

            this.Controller = new TransformController(this.Triangle, mode);
        }

        public void Rotate(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }
        public void Rotate(IIndicator indicator, BoxMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();
        }
        public void TransformSize(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }

        public void TransformSkew(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();
        }
        public void TransformSkew(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        #endregion
    }
}