using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Compute
{
    public abstract partial class AffineTriangle
    {
        internal Triangle StartingTriangle;
        internal Triangle Triangle;

        internal Matrix3x2 DestNorm;

        internal Matrix3x2 StartingMatrix;
        internal Matrix3x2 Matrix;

        internal Matrix3x2 Host;

        internal abstract void Find();

        #region Triangles.Initialize
        internal void UD(Triangle destination)
        {
            this.StartingTriangle = this.Triangle = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Triangles.Set
        internal void ST0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);

            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);

            indicator.ChangeX(this.Triangle, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);

            indicator.ChangeY(this.Triangle, mode);
        }

        internal void SF0(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;

            indicator.ChangeAll(this.Triangle, mode);
        }

        internal void SW(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateWidth(this.StartingTriangle, mode, value, keepRatio);

            this.Find();

            indicator.ChangeXYWH(this.Triangle, mode);
        }
        internal void SH(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateHeight(this.StartingTriangle, mode, value, keepRatio);

            this.Find();

            indicator.ChangeXYWH(this.Triangle, mode);
        }

        internal void SR(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            this.Host = indicator.CreateRotation(rotationAngleInDegrees);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        internal void SS(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum)
        {
            this.StartingTriangle = this.Triangle;
            this.Triangle = indicator.CreateSkew(this.StartingTriangle, mode, skewAngleInDegrees, minimum, maximum);

            this.Find();

            indicator.ChangeXYWHRS(this.Triangle, mode);
        }
        #endregion

        #region Triangles.Transform
        internal void CT()
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;
        }

        internal void CF()
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;
        }

        internal void TD0(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        internal void TD1(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void T0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
        }
        internal void T1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        internal void TXY1(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TX0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        internal void TX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TY0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        internal void TY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void F(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
        }
        #endregion
    }
}