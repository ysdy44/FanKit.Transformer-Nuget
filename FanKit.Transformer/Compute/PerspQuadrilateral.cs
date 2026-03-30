using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Compute
{
    public abstract partial class PerspQuadrilateral
    {
        internal Quadrilateral StartingQuadrilateral;
        internal Quadrilateral Quadrilateral;

        internal Matrix4x4 StartingMatrix;
        internal Matrix4x4 Matrix;

        internal Matrix3x2 Host;

        internal FreeTransformController Controller;

        internal abstract void Find();

        #region Quadrilaterals.Initialize
        internal void UD(Quadrilateral destination)
        {
            this.StartingQuadrilateral = this.Quadrilateral = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Quadrilaterals.FreeTransform
        internal void CFF(FreeTransformMode mode)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;

            this.Controller = new FreeTransformController(this.Quadrilateral, mode, 8f);
        }

        internal void M0(Vector2 point)
        {
            this.Quadrilateral = this.StartingQuadrilateral.MovePoint(this.Controller.PointKind, point);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        internal void M1(Vector2 point)
        {
            this.Quadrilateral = this.Controller.MovePointOfConvexQuadrilateral(this.StartingQuadrilateral, point);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Quadrilaterals.Set
        internal void ST0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);

            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);

            indicator.ChangeX(this.Quadrilateral, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);

            indicator.ChangeY(this.Quadrilateral, mode);
        }

        internal void SF0(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Transform(this.StartingQuadrilateral, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = Quadrilateral.Transform(this.StartingQuadrilateral, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);

            indicator.ChangeAll(this.Quadrilateral, mode);
        }

        internal void SW(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = indicator.CreateWidth(this.StartingQuadrilateral, mode, value, keepRatio);

            this.Find();

            indicator.ChangeXYWH(this.Quadrilateral, mode);
        }
        internal void SH(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = indicator.CreateHeight(this.StartingQuadrilateral, mode, value, keepRatio);

            this.Find();

            indicator.ChangeXYWH(this.Quadrilateral, mode);
        }

        internal void SR(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        {
            this.Host = indicator.CreateRotation(rotationAngleInDegrees);

            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = this.StartingQuadrilateral * this.Host;

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);

            indicator.ChangeXYWHRS(this.Quadrilateral, mode);
        }
        internal void SS(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.Quadrilateral = indicator.CreateSkew(this.StartingQuadrilateral, mode, skewAngleInDegrees, minimum, maximum);

            this.Find();

            indicator.ChangeXYWHRS(this.Quadrilateral, mode);
        }
        #endregion

        #region Quadrilaterals.Transform
        internal void CT()
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;
        }

        internal void CF()
        {
            this.StartingQuadrilateral = this.Quadrilateral;
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
            indicator.ChangeXY(this.Quadrilateral, mode);
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
            indicator.ChangeXY(this.Quadrilateral, mode);
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
            indicator.ChangeXY(this.Quadrilateral, mode);
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
            indicator.ChangeXY(this.Quadrilateral, mode);
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
            indicator.ChangeXY(this.Quadrilateral, mode);
        }

        internal void F(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.Quadrilateral = this.StartingQuadrilateral * this.Host;
            this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
        }

        private void T()
        {
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
        }
        private void TX()
        {
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
        }
        private void TY()
        {
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
        }
        #endregion
    }
}