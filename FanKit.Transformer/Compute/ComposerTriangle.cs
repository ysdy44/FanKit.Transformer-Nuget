using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Compute
{
    internal partial class ComposerTriangle
    {
        internal Triangle StartingTriangle;
        internal Triangle Triangle;

        internal Triangle Destination => this.Triangle;

        InvertibleMatrix3x2 HostSourceNorm;
        Matrix3x2 HostDestNorm;

        TransformController Controller;

        ControllerRadians Radians;

        readonly M3x2 Host;

        internal ComposerTriangle(M3x2 host)
        {
            this.Host = host;
        }

        void FindHomography()
        {
            this.HostDestNorm = this.Triangle.Normalize();
            this.Host.Matrix = this.HostSourceNorm.BidiAffine(this.HostDestNorm);
        }

        #region Triangles.Set
        internal void ST0(Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.Matrix.M31, this.Host.Matrix.M32);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.Matrix.M31, this.Host.Matrix.M32);

            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.Matrix.M31);
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.Matrix.M31);

            indicator.ChangeX(this.Triangle, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.Matrix.M32);
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.Matrix.M32);

            indicator.ChangeY(this.Triangle, mode);
        }

        internal void SF0(Matrix3x2 matrix)
        {
            this.Host.Matrix = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host.Matrix);
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host.Matrix = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host.Matrix);

            indicator.ChangeAll(this.Triangle, mode);
        }

        internal void SW(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateWidth(this.StartingTriangle, mode, value, keepRatio);

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }
        internal void SH(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateHeight(this.StartingTriangle, mode, value, keepRatio);

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }

        internal void SR(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            this.Host.Matrix = indicator.CreateRotation(rotationAngleInDegrees);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host.Matrix);

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        internal void SS(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        {
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateSkew(this.StartingTriangle, mode, skewAngleInDegrees, minimum, maximum);

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.FindHomography();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        #endregion

        #region Triangles.Transform
        internal void CT()
        {
            this.StartingTriangle = this.Triangle;

            this.Host.Matrix = Matrix3x2.Identity;
        }

        internal void CF()
        {
            this.StartingTriangle = this.Triangle;

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host.Matrix = Matrix3x2.Identity;
        }

        internal void TD0(Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        internal void TD1(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void T0(Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
        }
        internal void T1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        internal void TXY1(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TX0(float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        internal void TX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TY0(float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        internal void TY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host.Matrix = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void F(Matrix3x2 matrix)
        {
            this.Host.Matrix = matrix;

            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host.Matrix);
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.Matrix.M31, this.Host.Matrix.M32);
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.Matrix.M31);
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.Matrix.M32);
        }
        #endregion

        #region Triangles.Transform2
        internal void CR(Vector2 point)
        {
            this.StartingTriangle = this.Triangle;

            this.Host.Matrix = Matrix3x2.Identity;

            this.Controller = new TransformController(this.Triangle, point);
        }

        internal void CF(TransformMode mode)
        {
            this.StartingTriangle = this.Triangle;

            this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host.Matrix = Matrix3x2.Identity;

            this.Controller = new TransformController(this.Triangle, mode);
        }

        internal void R0(Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host.Matrix = this.Controller.Rotate(this.Radians);
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host.Matrix);
        }
        internal void R1(IIndicator indicator, BoxMode mode, Vector2 point, float stepFrequency = float.NaN)
        {
            this.Radians = this.Controller.ToRadians(point, stepFrequency);

            this.Host.Matrix = this.Controller.Rotate(this.Radians);
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host.Matrix);

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }

        internal void TWH0(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.FindHomography();
        }
        internal void TWH1(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.FindHomography();

            indicator.ChangeXYWH(this.Triangle, mode);
        }

        internal void TS0(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.FindHomography();
        }
        internal void TS1(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.Triangle = this.Controller.Transform(this.StartingTriangle, point, keepRatio, centeredScaling);

            this.FindHomography();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        #endregion
    }
}