using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Text;

namespace FanKit.Transformer.Transforms
{
    public partial class HostBounds
    {
        // Step 0. Initialize
        public int Count;
        Bounds SourceBounds;

        // Step 1. Transformer
        //Bounds TransformedBounds;
        public Bounds StartingBounds;
        public Bounds Bounds;

        // Step 2. Homography Matrix
        //Matrix2x2 DestNorm;

        // Step 3. Matrix
        //public Matrix2x2 StartingMatrix;
        //public Matrix2x2 Matrix;
        //public Matrix2x2 InverseMatrix;

        // Step 4. Host
        Matrix2x2 HostSourceNorm;
        Matrix2x2 HostDestNorm;
        Matrix2x2 Host;
        public float HostTranslateX => this.Host.TranslateX;
        public float HostTranslateY => this.Host.TranslateY;
        public Matrix2x2 HostMatrix => this.Host;

        // Step 6. Controller
        CropController Controller;

        //ControllerRadians Radians;

        //void Invert()
        //{
        //    Matrix2x2.Invert(this.Matrix, out this.InverseMatrix);
        //}

        //void Find()
        //{
        //    this.DestNorm = this.Bounds.Normalize();
        //    this.Matrix = this.Source * this.DestNorm;
        //    this.Invert();
        //}

        void FindHomography()
        {
            this.HostDestNorm = this.Bounds.Normalize();
            this.Host = this.HostSourceNorm.Map(this.HostDestNorm);
        }

        /*
        public void Initialize(Bounds source)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = Matrix2x2.Identity;
            //this.InverseMatrix = Matrix2x2.Identity;

            // Step 4. Host
            this.Host = Matrix2x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = default;
            this.StartingBounds = this.Bounds = this.SourceBounds;
        }

        public void Initialize(Bounds source, Matrix2x2 matrix)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix = matrix;
            //this.Invert();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingBounds = this.Bounds = this.TransformedBounds.ToBounds();
        }

        public void Extend(Bounds source)
        {
            // Step 0. Initialize
            this.SourceBounds = source;
            this.Source = new RectSource(this.SourceBounds);
        
            // Step 4. Host
            this.Host = Matrix2x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingBounds = this.Bounds = this.TransformedBounds;
        }
         */

        public void Reset()
        {
            // Step 0. Initialize
            this.Count = 0;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;
        }

        public void Reset(Bounds bounds)
        {
            // Step 0. Initialize
            this.Count = 1;

            // Step 1. Transformer
            this.StartingBounds = this.Bounds = bounds;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.Host = Matrix2x2.Identity;
        }

        #region Bounds.Reset
        public void Reset(Bounds source, Bounds bounds, Matrix2x2 matrix)
        {
            // Step 0. Initialize
            this.Count = 1;
            //this.SizeType = IndicatorSizeType.Panel;
            this.SourceBounds = source;

            // Step 1. Transformer
            this.StartingBounds = this.Bounds = bounds;

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

                    this.StartingBounds = this.Bounds = bounds;

                    this.Host = Matrix2x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.Eb(this.Bounds);

                    this.Eb(bounds);

                    this.Host = Matrix2x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.Eb(bounds);

                    this.Host = Matrix2x2.Identity;
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

                    this.StartingBounds = this.Bounds = new Bounds(triangle);

                    this.Host = Matrix2x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.Eb(this.Bounds);

                    this.E3(triangle);

                    this.Host = Matrix2x2.Identity;
                    break;
                case 2:
                default:
                    this.Count++;

                    this.E3(triangle);

                    this.Host = Matrix2x2.Identity;
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

                    this.StartingBounds = this.Bounds = quad.ToBounds();

                    this.Host = Matrix2x2.Identity;
                    break;
                case 1:
                    this.Count = 2;
                    this.SourceBounds = Bounds.Infinity;

                    this.Extend(this.Bounds);

                    this.E4(quad);

                    this.Host = Matrix2x2.Identity;
                    break;
                default:
                    this.Count++;

                    this.E4(quad);

                    this.Host = Matrix2x2.Identity;
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
                    //this.SizeType = IndicatorSizeType.Panel;
                    break;
                default:
                    // Step 0. Initialize
                    //this.SizeType = IndicatorSizeType.Panel;
                    //this.SourceBounds = new Bounds(items);

                    // Step 1. Transformer
                    this.StartingBounds = this.Bounds = this.SourceBounds;

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    //this.Find();

                    // Step 4. Host
                    this.Host = Matrix2x2.Identity;
                    break;
            }
        }
        #endregion

        #region Bounds.Set
        public void SetTranslation(Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix2x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
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
            //this.Find();

            // Step 4. Host
            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            // Step 1. Transformer
            this.StartingBounds = this.Bounds;
            this.Bounds = indicator.CreateHeight(this.StartingBounds, mode, value, keepRatio);

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();

            // Step 4. Host
            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.FindHomography();

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
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
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
            //this.Find();

            // Step 4. Host
            this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            this.FindHomography();

            indicator.ChangeXYWHRS(this.Bounds, mode);
        }
         */
        #endregion

        #region Bounds.Transform
        public void CacheTranslation()
        {
            this.StartingBounds = this.Bounds;
            //this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            this.Host = Matrix2x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingBounds = this.Bounds;
            //this.StartingMatrix = this.Matrix;

            this.HostSourceNorm = this.StartingBounds.Normalize();
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
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }

        private void T()
        {
            this.Bounds = Bounds.Translate(this.StartingBounds, this.Host.TranslateX, this.Host.TranslateY);
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();
        }
        private void TX()
        {
            this.Bounds = Bounds.TranslateX(this.StartingBounds, this.Host.TranslateX);
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();
        }
        private void TY()
        {
            this.Bounds = Bounds.TranslateY(this.StartingBounds, this.Host.TranslateY);
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();
        }
        #endregion

        #region Bounds.Transform2
        /*
        public void CacheRotation(Vector2 point)
        {
            this.StartingBounds = this.Bounds;
            //this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingBounds.ToInvertibleMatrix();
            this.Host = Matrix2x2.Identity;

            this.Controller = new CropController(this.Bounds, point);
        }
         */

        public void CacheTransform(CropMode mode)
        {
            this.StartingBounds = this.Bounds;
            //this.StartingMatrix = this.Matrix;

            this.HostSourceNorm = this.StartingBounds.Normalize();
            this.Host = Matrix2x2.Identity;

            this.Controller = new CropController(this.Bounds, mode);
        }

        /*
        public void Rotate(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
        }
        public void Rotate(IIndicator indicator, BoxMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host = this.Controller.Rotate(this.Radians);
            this.Bounds = Bounds.Transform(this.StartingBounds, this.Host);
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();

            indicator.ChangeXYWHRS(this.Bounds, mode);
        }
         */

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();
        }
        public void TransformSize(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();

            indicator.ChangeXYWH(this.Bounds, mode);
        }

        /*
        public void TransformSkew(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();
        }
        public void TransformSkew(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Bounds = this.Controller.Crop(this.StartingBounds, point, keepRatio, centeredScaling);

            //this.Find();

            this.FindHomography();

            indicator.ChangeXYWHRS(this.Bounds, mode);
        }
         */
        #endregion
    }
}