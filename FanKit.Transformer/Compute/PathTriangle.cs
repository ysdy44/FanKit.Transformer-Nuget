using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System;
using System.Numerics;
using System.Linq;
using FanKit.Transformer.Indicators;

namespace FanKit.Transformer.Compute
{
    public abstract partial class PathTriangle
    {
        internal TransformedBounds TransformedBounds;
        internal Triangle StartingTriangle;
        internal Triangle Triangle;

        internal Matrix3x2 Host;

        #region Triangles.Initialize
        internal void RT(Triangle destination)
        {
            this.StartingTriangle = this.Triangle = destination;

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Triangles.SelectedItems.Set
        internal void STSI0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateRaw();
        }
        internal void STSI1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateRaw();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STXSI0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateRaw();
        }
        internal void STXSI1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateRaw();
            indicator.ChangeX(this.Triangle, mode);
        }

        internal void STYSI0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateRaw();
        }
        internal void STYSI1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateRaw();
            indicator.ChangeY(this.Triangle, mode);
        }

        internal void SFSI0(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.TransformMap();
        }
        internal void SFSI1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.TransformMap();
            indicator.ChangeAll(this.Triangle, mode);
        }

        internal abstract void TranslateRaw();

        internal abstract void TransformMap();
        #endregion

        #region Triangles.SelectedItems.Set.Index
        internal void ST0(Vector2 translate, Vector2 point, int segmentIndex)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.SI(point, segmentIndex);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate, Vector2 point, int segmentIndex)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.SI(point, segmentIndex);
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STXY0(float translateX, float translateY, Vector2 point, int segmentIndex)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.SI(point, segmentIndex);
        }
        internal void STXY1(IIndicator indicator, BoxMode mode, float translateX, float translateY, Vector2 point, int segmentIndex)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.SI(point, segmentIndex);
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal abstract void SI(Vector2 point, int segmentIndex);
        #endregion

        #region Triangles.SelectedItems.Transform
        internal void CTSI()
        {
            this.CacheRaw();

            this.Host = Matrix3x2.Identity;
        }

        internal void CFSI()
        {
            this.CacheMap();

            this.Host = Matrix3x2.Identity;
        }

        internal void TDSI0(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.TranslateStarting();
        }
        internal void TDSI1(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TSI0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateStarting();
        }
        internal void TSI1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TXYSI0(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);

            this.TranslateStarting();
        }
        internal void TXYSI1(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TXSI0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateStarting();
        }
        internal void TXSI1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TYSI0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateStarting();
        }
        internal void TYSI1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void FSI(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.TransformStarting();
        }

        internal abstract void CacheRaw();

        internal abstract void CacheMap();

        internal abstract void TranslateStarting();

        internal abstract void TransformStarting();
        #endregion

        #region Triangles.Set
        internal void ST0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            indicator.ChangeX(this.Triangle, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            indicator.ChangeY(this.Triangle, mode);
        }

        internal void SF0(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            indicator.ChangeAll(this.Triangle, mode);
        }
        #endregion

        #region Triangles.Transform
        internal void CT()
        {
            this.StartingTriangle = this.Triangle;

            this.Host = Matrix3x2.Identity;
        }

        internal void CF()
        {
            this.StartingTriangle = this.Triangle;

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
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
        }
        #endregion
    }
}