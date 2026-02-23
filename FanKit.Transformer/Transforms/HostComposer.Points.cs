using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public partial class ComposerPoint
        {
            // Step 0. Initialize

            // Step 1. Transformer
            public Vector2 StartingPoint;

            public Vector2 Point;

            // Step 2. Homography Matrix

            // Step 3. Matrix

            // Step 4. Host

            // Step 6. Controller

            readonly HostComposer Host;

            public ComposerPoint(HostComposer host)
            {
                this.Host = host;
            }
        }

        #region Points.Set
        partial class ComposerPoint
        {
            public void SetTranslation(Vector2 translate)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translate);

                // Step 1. Transformer
                this.StartingPoint = this.Point;
                this.Point = Math.Translate(this.StartingPoint, this.Host.Host.M31, this.Host.Host.M32);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.Host.M31);
                //this.Invert();
            }
            public void SetTranslation(IIndicator indicator, Vector2 translate)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translate);

                // Step 1. Transformer
                this.StartingPoint = this.Point;
                this.Point = Math.Translate(this.StartingPoint, this.Host.Host.M31, this.Host.Host.M32);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.Host.M31);
                //this.Invert();

                indicator.ChangeAll(this.Point);
            }

            public void SetTranslationX(float translateX)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);

                // Step 1. Transformer
                this.StartingPoint = this.Point;
                this.Point = Math.TranslateX(this.StartingPoint, this.Host.Host.M31);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.Host.M31);
                //this.Invert();
            }
            public void SetTranslationX(IIndicator indicator, float translateX)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);

                // Step 1. Transformer
                this.StartingPoint = this.Point;
                this.Point = Math.TranslateX(this.StartingPoint, this.Host.Host.M31);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.Host.M31);
                //this.Invert();

                indicator.ChangeAll(this.Point);
            }

            public void SetTranslationY(float translateY)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);

                // Step 1. Transformer
                this.StartingPoint = this.Point;
                this.Point = Math.TranslateY(this.StartingPoint, this.Host.Host.M32);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.Host.M32);
                //this.Invert();
            }
            public void SetTranslationY(IIndicator indicator, float translateY)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);

                // Step 1. Transformer
                this.StartingPoint = this.Point;
                this.Point = Math.TranslateY(this.StartingPoint, this.Host.Host.M32);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.Host.M32);
                //this.Invert();

                indicator.ChangeAll(this.Point);
            }
        }
        #endregion

        #region Points.Transform
        partial class ComposerPoint
        {
            public void CacheTranslation()
            {
                this.StartingPoint = this.Point;
            }

            public void Translate(Vector2 startingPoint, Vector2 point)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
                this.T();
            }
            public void Translate(IIndicator indicator, Vector2 startingPoint, Vector2 point)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
                this.T();
                indicator.ChangeAll(this.Point);
            }

            public void Translate(Vector2 translate)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translate);
                this.T();
            }
            public void Translate(IIndicator indicator, Vector2 translate)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translate);
                this.T();
                indicator.ChangeAll(this.Point);
            }

            public void Translate(float translateX, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, translateY);
                this.T();
            }
            public void Translate(IIndicator indicator, float translateX, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, translateY);
                this.T();
                indicator.ChangeAll(this.Point);
            }

            public void TranslateX(float translateX)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);
                this.TX();
            }
            public void TranslateX(IIndicator indicator, float translateX)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);
                this.TX();
                indicator.ChangeAll(this.Point);
            }

            public void TranslateY(float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);
                this.TY();
            }
            public void TranslateY(IIndicator indicator, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);
                this.TY();
                indicator.ChangeAll(this.Point);
            }

            private void T()
            {
                this.Point = new Vector2(this.StartingPoint.X + this.Host.Host.M31, this.StartingPoint.Y + this.Host.Host.M32);
                //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.Host.M31, this.Host.Host.M32);
                //this.Invert();
            }
            private void TX()
            {
                this.Point = new Vector2(this.StartingPoint.X + this.Host.Host.M31, this.StartingPoint.Y);
                //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.Host.M31, 0f);
                //this.Invert();
            }
            private void TY()
            {
                this.Point = new Vector2(this.StartingPoint.X, this.StartingPoint.Y + this.Host.Host.M32);
                //this.Matrix = Math.TranslateX(this.StartingMatrix, 0f, this.Host.Host.M32);
                //this.Invert();
            }
        }
        #endregion
    }
}