using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Text;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public partial class ComposerLine
        {
            // Step 0. Initialize'

            // Step 1. Transformer
            public Vector2 StartingPoint0;
            public Vector2 StartingPoint1;

            public Vector2 Point0;
            public Vector2 Point1;

            // Step 2. Homography Matrix

            // Step 3. Matrix

            // Step 4. Host
            LineMatrix HostSourceNorm;
            PinchMatrix3x2 HostDestNorm;
            //public Matrix3x2 Host;

            // Step 6. Controller
            Vector2 Diff;
            LineControllerFoot Foot;
            LineController Controller;

            ControllerRadians Radians;

            readonly HostComposer Host;

            public ComposerLine(HostComposer host)
            {
                this.Host = host;
            }

            // ElongatePoint0
            public void FindHomography0()
            {
                if (this.HostSourceNorm.IsEmpty)
                {
                    this.Host.Host = Matrix3x2.Identity;
                }
                else
                {
                    this.HostDestNorm = new PinchMatrix3x2(this.HostSourceNorm, this.StartingPoint1, this.Point0);
                    if (this.HostDestNorm.IsEmpty)
                    {
                        this.Host.Host = Matrix3x2.Identity;
                    }
                    else
                    {
                        this.Host.Host = this.HostDestNorm;
                    }
                }
            }

            // ElongatePoint1
            public void FindHomography1()
            {
                if (this.HostSourceNorm.IsEmpty)
                {
                    this.Host.Host = Matrix3x2.Identity;
                }
                else
                {
                    this.HostDestNorm = new PinchMatrix3x2(this.HostSourceNorm, this.StartingPoint0, this.Point1);
                    if (this.HostDestNorm.IsEmpty)
                    {
                        this.Host.Host = Matrix3x2.Identity;
                    }
                    else
                    {
                        this.Host.Host = this.HostDestNorm;
                    }
                }
            }

            // MovePoint
            public void FindHomography()
            {
                if (this.HostSourceNorm.IsEmpty)
                {
                    this.Host.Host = Matrix3x2.Identity;
                }
                else
                {
                    this.HostDestNorm = new PinchMatrix3x2(this.HostSourceNorm, this.StartingPoint0, this.Point0, this.Point1);
                    if (this.HostDestNorm.IsEmpty)
                    {
                        this.Host.Host = Matrix3x2.Identity;
                    }
                    else
                    {
                        this.Host.Host = this.HostDestNorm;
                    }
                }
            }
        }

        #region Lines.Set
        partial class ComposerLine
        {
            public void SetTranslation(Vector2 translate)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translate);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y + this.Host.Host.M32);

                // Step 2. Homography Matrix
                // Step 3. Matrix
            }
            public void SetTranslation(IIndicator indicator, RowLineMode mode, Vector2 translate)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translate);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y + this.Host.Host.M32);

                // Step 2. Homography Matrix
                // Step 3. Matrix

                indicator.ChangeXY(this.Point0, this.Point1, mode);
            }
            public void SetTranslation(IIndicator indicator, ColumnLineMode mode, Vector2 translate)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translate);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y + this.Host.Host.M32);

                // Step 2. Homography Matrix
                // Step 3. Matrix

                indicator.ChangeXY(this.Point0, this.Point1, mode);
            }

            public void SetTranslationX(float translateX)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y);

                // Step 2. Homography Matrix
                // Step 3. Matrix
            }
            public void SetTranslationX(IIndicator indicator, RowLineMode mode, float translateX)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y);

                // Step 2. Homography Matrix
                // Step 3. Matrix

                indicator.ChangeXY(this.Point0, this.Point1, mode);
            }
            public void SetTranslationX(IIndicator indicator, ColumnLineMode mode, float translateX)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y);

                // Step 2. Homography Matrix
                // Step 3. Matrix

                indicator.ChangeXY(this.Point0, this.Point1, mode);
            }

            public void SetTranslationY(float translateY)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Host.M32);

                // Step 2. Homography Matrix
                // Step 3. Matrix
            }
            public void SetTranslationY(IIndicator indicator, RowLineMode mode, float translateY)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Host.M32);

                // Step 2. Homography Matrix
                // Step 3. Matrix

                indicator.ChangeXY(this.Point0, this.Point1, mode);
            }
            public void SetTranslationY(IIndicator indicator, ColumnLineMode mode, float translateY)
            {
                // Step 4. Host
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Host.M32);

                // Step 2. Homography Matrix
                // Step 3. Matrix

                indicator.ChangeXY(this.Point0, this.Point1, mode);
            }

            public void SetWidth(IIndicator indicator, RowLineMode mode, float value)
            {
                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                // Step 2. Homography Matrix
                // Step 3. Matrix
                //this.Find();

                // Step 4. Host
                switch (mode)
                {
                    case RowLineMode.Left:
                        this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 0);
                        this.Point1 = this.Controller.GetPoint1();
                        this.Host.Host = this.Controller.Multiply();
                        break;
                    case RowLineMode.Center:
                        this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 1);
                        this.Point0 = this.Controller.GetPoint0();
                        this.Point1 = this.Controller.GetPoint1();
                        this.Host.Host = this.Controller.Multiply();
                        break;
                    case RowLineMode.Right:
                        this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 2);
                        this.Point0 = this.Controller.GetPoint0();
                        this.Host.Host = this.Controller.Multiply();
                        break;
                    default:
                        break;
                }

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void SetHeight(IIndicator indicator, ColumnLineMode mode, float value)
            {
                // Step 1. Transformer
                //this.StartingPoint = this.Point;
                //this.Point = Vector2.Transform(this.StartingPoint, this.Host.Host);

                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                // Step 2. Homography Matrix
                // Step 3. Matrix
                //this.Find();

                // Step 4. Host
                switch (mode)
                {
                    case ColumnLineMode.Top:
                        this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 0);
                        this.Point1 = this.Controller.GetPoint1();
                        this.Host.Host = this.Controller.Multiply();
                        break;
                    case ColumnLineMode.Center:
                        this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 1);
                        this.Point0 = this.Controller.GetPoint0();
                        this.Point1 = this.Controller.GetPoint1();
                        this.Host.Host = this.Controller.Multiply();
                        break;
                    case ColumnLineMode.Bottom:
                        this.Controller = new LineController(value, this.StartingPoint0, this.StartingPoint1, 2);
                        this.Point0 = this.Controller.GetPoint0();
                        this.Host.Host = this.Controller.Multiply();
                        break;
                    default:
                        break;
                }

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void SetRotation(IIndicator indicator, float rotationAngleInDegrees)
            {
                // Step 4. Host
                this.Host.Host = indicator.CreateRotation(rotationAngleInDegrees);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Host);
                this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Host);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = this.StartingMatrix * this.Host.Host;
                //this.Invert();
            }
            public void SetRotation(IIndicator indicator, RowLineMode mode, float rotationAngleInDegrees)
            {
                // Step 4. Host
                this.Host.Host = indicator.CreateRotation(rotationAngleInDegrees);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Host);
                this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Host);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = this.StartingMatrix * this.Host.Host;
                //this.Invert();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void SetRotation(IIndicator indicator, ColumnLineMode mode, float rotationAngleInDegrees)
            {
                // Step 4. Host
                this.Host.Host = indicator.CreateRotation(rotationAngleInDegrees);

                // Step 1. Transformer
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Host);
                this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Host);

                // Step 3. Matrix
                //this.StartingMatrix = this.Matrix;
                //this.Matrix = this.StartingMatrix * this.Host.Host;
                //this.Invert();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
        }
        #endregion

        #region Lines.Transform
        partial class ComposerLine
        {
            public void CacheTranslation()
            {
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                //this.HostSourceNorm = new LineMatrix(this.StartingPoint0, this.StartingPoint1);
                this.Host.Host = Matrix3x2.Identity;
            }

            public void Translate(Vector2 startingPoint, Vector2 point)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
                this.T();
            }
            public void Translate(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
                this.T();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void Translate(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
                this.T();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void Translate(Vector2 translate)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translate);
                this.T();
            }
            public void Translate(IIndicator indicator, RowLineMode mode, Vector2 translate)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translate);
                this.T();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void Translate(IIndicator indicator, ColumnLineMode mode, Vector2 translate)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translate);
                this.T();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void Translate(float translateX, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, translateY);
                this.T();
            }
            public void Translate(IIndicator indicator, RowLineMode mode, float translateX, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, translateY);
                this.T();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void Translate(IIndicator indicator, ColumnLineMode mode, float translateX, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, translateY);
                this.T();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void TranslateX(float translateX)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);
                this.TX();
            }
            public void TranslateX(IIndicator indicator, RowLineMode mode, float translateX)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);
                this.TX();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void TranslateX(IIndicator indicator, ColumnLineMode mode, float translateX)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(translateX, 0f);
                this.TX();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void TranslateY(float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);
                this.TY();
            }
            public void TranslateY(IIndicator indicator, RowLineMode mode, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);
                this.TY();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void TranslateY(IIndicator indicator, ColumnLineMode mode, float translateY)
            {
                this.Host.Host = Matrix3x2.CreateTranslation(0f, translateY);
                this.TY();
                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            private void T()
            {
                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y + this.Host.Host.M32);
            }
            private void TX()
            {
                this.Point0 = new Vector2(this.StartingPoint0.X + this.Host.Host.M31, this.StartingPoint0.Y);
                this.Point1 = new Vector2(this.StartingPoint1.X + this.Host.Host.M31, this.StartingPoint1.Y);
            }
            private void TY()
            {
                this.Point0 = new Vector2(this.StartingPoint0.X, this.StartingPoint0.Y + this.Host.Host.M32);
                this.Point1 = new Vector2(this.StartingPoint1.X, this.StartingPoint1.Y + this.Host.Host.M32);
            }
        }
        #endregion

        #region Lines.Transform2
        partial class ComposerLine
        {
            public void CacheRotation(Vector2 point)
            {
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                //this.HostSourceNorm = new LineMatrix(this.StartingPoint0, this.StartingPoint1);
                this.Host.Host = Matrix3x2.Identity;

                this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1, point);
            }

            public void CacheElongation0()
            {
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.HostSourceNorm = new LineMatrix(this.StartingPoint1, this.StartingPoint0);
                this.Host.Host = Matrix3x2.Identity;

                this.Controller = new LineController(this.StartingPoint1, this.StartingPoint0);
            }

            public void CacheElongation1()
            {
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.HostSourceNorm = new LineMatrix(this.StartingPoint0, this.StartingPoint1);
                this.Host.Host = Matrix3x2.Identity;

                this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1);
            }

            public void CacheMovement()
            {
                this.StartingPoint0 = this.Point0;
                this.StartingPoint1 = this.Point1;

                this.HostSourceNorm = new LineMatrix(this.StartingPoint0, this.StartingPoint1);
                this.Host.Host = Matrix3x2.Identity;
            }

            public void Rotate(Vector2 point, float stepFrequency = float.NaN)
            {
                this.Radians = this.Controller.ToRadians(point, stepFrequency);
                this.Host.Host = this.Controller.Rotate(this.Radians);

                this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Host);
                this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Host);
            }
            public void Rotate(IIndicator indicator, RowLineMode mode, Vector2 point, float stepFrequency = float.NaN)
            {
                this.Radians = this.Controller.ToRadians(point, stepFrequency);
                this.Host.Host = this.Controller.Rotate(this.Radians);

                this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Host);
                this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Host);

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void Rotate(IIndicator indicator, ColumnLineMode mode, Vector2 point, float stepFrequency = float.NaN)
            {
                this.Radians = this.Controller.ToRadians(point, stepFrequency);
                this.Host.Host = this.Controller.Rotate(this.Radians);

                this.Point0 = Vector2.Transform(this.StartingPoint0, this.Host.Host);
                this.Point1 = Vector2.Transform(this.StartingPoint1, this.Host.Host);

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void ElongatePoint0(Vector2 startingPoint, Vector2 point)
            {
                this.Diff = startingPoint - this.StartingPoint0;
                this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, point, this.Diff);
                this.Point0 = this.Foot.Foot;

                this.FindHomography0();
            }
            public void ElongatePoint0(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
            {
                this.Diff = startingPoint - this.StartingPoint0;
                this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, point, this.Diff);
                this.Point0 = this.Foot.Foot;

                this.FindHomography0();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void ElongatePoint0(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
            {
                this.Diff = startingPoint - this.StartingPoint0;
                this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, point, this.Diff);
                this.Point0 = this.Foot.Foot;

                this.FindHomography0();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void ElongatePoint1(Vector2 startingPoint, Vector2 point)
            {
                this.Diff = startingPoint - this.StartingPoint1;
                this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, point, this.Diff);
                this.Point1 = this.Foot.Foot;

                this.FindHomography1();
            }
            public void ElongatePoint1(IIndicator indicator, RowLineMode mode, Vector2 startingPoint, Vector2 point)
            {
                this.Diff = startingPoint - this.StartingPoint1;
                this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, point, this.Diff);
                this.Point1 = this.Foot.Foot;

                this.FindHomography1();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void ElongatePoint1(IIndicator indicator, ColumnLineMode mode, Vector2 startingPoint, Vector2 point)
            {
                this.Diff = startingPoint - this.StartingPoint1;
                this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, point, this.Diff);
                this.Point1 = this.Foot.Foot;

                this.FindHomography1();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void MovePoint0(Vector2 point)
            {
                this.Point0 = point;

                this.FindHomography();
            }
            public void MovePoint0(IIndicator indicator, RowLineMode mode, Vector2 point)
            {
                this.Point0 = point;

                this.FindHomography();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void MovePoint0(IIndicator indicator, ColumnLineMode mode, Vector2 point)
            {
                this.Point0 = point;

                this.FindHomography();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }

            public void MovePoint1(Vector2 point)
            {
                this.Point1 = point;

                this.FindHomography();
            }
            public void MovePoint1(IIndicator indicator, RowLineMode mode, Vector2 point)
            {
                this.Point1 = point;

                this.FindHomography();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
            public void MovePoint1(IIndicator indicator, ColumnLineMode mode, Vector2 point)
            {
                this.Point1 = point;

                this.FindHomography();

                indicator.ChangeAll(this.Point0, this.Point1, mode);
            }
        }
        #endregion
    }
}