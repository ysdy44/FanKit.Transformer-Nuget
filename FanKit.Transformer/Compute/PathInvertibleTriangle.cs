using FanKit.Transformer.Cache;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformer.Compute
{
    public abstract partial class PathInvertibleTriangle
    {
        internal Triangle StartingTriangle;
        internal Triangle Triangle;

        internal Matrix3x2 StartingMatrix;
        internal Matrix3x2 Matrix;
        internal Matrix3x2 InverseMatrix;

        internal Matrix3x2 Host;

        void Invert()
        {
            Matrix3x2.Invert(this.Matrix, out this.InverseMatrix);
            this.RawToMap();
        }

        internal abstract void Find();

        #region Triangles.Initialize
        internal void RT(Triangle destination)
        {
            this.StartingTriangle = this.Triangle = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Triangles.SelectedItems
        internal abstract void RawToMap();
        #endregion

        #region Triangles.SelectedItems.Set
        internal void STSI0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));

            this.TranslateRaw();
        }
        internal void STSI1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));

            this.TranslateRaw();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STXSI0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateRaw();
        }
        internal void STXSI1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateRaw();
            indicator.ChangeX(this.Triangle, mode);
        }

        internal void STYSI0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateRaw();
        }
        internal void STYSI1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateRaw();
            indicator.ChangeY(this.Triangle, mode);
        }

        internal void SFSI0(Matrix3x2 matrix)
        {
            this.Host = matrix * this.InverseMatrix;

            this.TransformMap();
        }
        internal void SFSI1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix * this.InverseMatrix;

            this.TransformMap();
            indicator.ChangeAll(this.Triangle, mode);
        }

        internal abstract void TranslateRaw();

        internal abstract void TransformMap();
        #endregion

        #region Triangles.SelectedItems.Set.Index
        internal void ST0(Vector2 translate, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));
            SI(point, index);
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));
            SI(point, index);
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STXY0(float translateX, float translateY, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));
            SI(point, index);
        }
        internal void STXY1(IIndicator indicator, BoxMode mode, float translateX, float translateY, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));
            SI(point, index);
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal abstract void SI(Vector2 point, int index);
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
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(point.X - startingPoint.X, point.Y - startingPoint.Y, this.InverseMatrix));
            this.TranslateStarting();
        }
        internal void TDSI1(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(point.X - startingPoint.X, point.Y - startingPoint.Y, this.InverseMatrix));
            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TSI0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));

            this.TranslateStarting();
        }
        internal void TSI1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TXYSI0(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));

            this.TranslateStarting();
        }
        internal void TXYSI1(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TXSI0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateStarting();
        }
        internal void TXSI1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void TYSI0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateStarting();
        }
        internal void TYSI1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void FSI(Matrix3x2 matrix)
        {
            this.Host = matrix * this.InverseMatrix;

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

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            this.Invert();
        }
        internal void ST1(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            this.Invert();

            indicator.ChangeXY(this.Triangle, mode);
        }

        internal void STX0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            this.Invert();
        }
        internal void STX1(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            this.Invert();

            indicator.ChangeX(this.Triangle, mode);
        }

        internal void STY0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            this.Invert();
        }
        internal void STY1(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            this.Invert();

            indicator.ChangeY(this.Triangle, mode);
        }

        internal void SF0(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            this.Invert();
        }
        internal void SF1(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            this.Invert();

            indicator.ChangeAll(this.Triangle, mode);
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
            this.Invert();
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            this.Invert();
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            this.Invert();
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            this.Invert();
        }
        #endregion
    }
}