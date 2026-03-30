using FanKit.Transformer.Cache;
using FanKit.Transformer.Compute;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Figure = FanKit.Transformer.Polylines.Figure2;
using Segment = FanKit.Transformer.Polylines.Segment2;

namespace FanKit.Transformer.Polylines
{
    public partial class Path2 : PathInvertibleTriangle
    {
        public Bounds SourceBounds { get; private set; }
        public Rectangle SourceRect { get; private set; }
        RectMatrix SourceNormalize;

        TransformedBounds TransformedBounds;

        Matrix3x2 DestNorm;
        public Triangle Destination => this.Triangle;

        public Matrix3x2 HomographyMatrix => this.Matrix;
        public Matrix3x2 HomographyInverseMatrix => this.InverseMatrix;

        public float TranslationX => this.Host.M31;
        public float TranslationY => this.Host.M32;
        public Matrix3x2 TransformMatrix => this.Host;

        void Invert()
        {
            Matrix3x2.Invert(this.Matrix, out this.InverseMatrix);
        }

        internal override void Find()
        {
            this.DestNorm = this.Triangle.Normalize();
            this.Matrix = this.SourceNormalize.Affine(this.DestNorm);
            this.Invert();
        }

        #region Triangles.Initialize
        private void BeginExtend()
        {
            this.SourceBounds = Bounds.Infinity;
        }
        private void Extend(Vector2 point)
        {
            this.SourceBounds = Bounds.Union(this.SourceBounds, point);
        }
        private void EndExtend()
        {
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void Reset(Triangle destination)
        {
            this.RT(destination);
        }
        #endregion

        #region Triangles.SelectedItems
        public List<Figure> Data;

        public PathSetting Setting { get; } = new PathSetting();

        public Path2(List<Figure> items)
        {
            this.Data = items;
            this.Matrix = Matrix3x2.Identity;
            this.InverseMatrix = Matrix3x2.Identity;

            this.BeginExtend();
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    figure.Data[i] = new Segment
                    {
                        Map = item.Raw,
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        //Map = item.Map,
                    };
                    this.Extend(item.Raw);
                }
            }

            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }
        public Path2(List<Figure> items, Matrix3x2 matrix)
        {
            this.Data = items;
            this.Matrix = matrix;
            this.Invert();

            this.BeginExtend();
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    figure.Data[i] = new Segment
                    {
                        Map = Vector2.Transform(item.Raw, this.Matrix),
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        //Map = item.Map,
                    };
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }

        public void Complete()
        {
            this.BeginExtend();
            foreach (Figure items in this.Data)
            {
                foreach (Segment item in items.Data)
                {
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }

        public void Select(int index1, int index2)
        {
            for (int j = 0; j < this.Data.Count; j++)
            {
                Figure figure = this.Data[j];

                if (index1 == j)
                {
                    for (int i = 0; i < figure.Data.Count; i++)
                    {
                        Segment item = figure.Data[i];

                        if (item.IsChecked)
                        {
                            if (index2 != i)
                            {
                                figure.Data[i] = new Segment
                                {
                                    IsChecked = false,
                                    // C# 9.0 : var a = item with { ... }

                                    //IsChecked = item.IsChecked,
                                    Starting = item.Starting,
                                    Raw = item.Raw,
                                    Map = item.Map,
                                };
                            }
                        }
                        else
                        {
                            if (index2 == i)
                            {
                                figure.Data[i] = new Segment
                                {
                                    IsChecked = true,
                                    // C# 9.0 : var a = item with { ... }

                                    //IsChecked = item.IsChecked,
                                    Starting = item.Starting,
                                    Raw = item.Raw,
                                    Map = item.Map,
                                };
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < figure.Data.Count; i++)
                    {
                        Segment item = figure.Data[i];
                        if (item.IsChecked)
                        {
                            figure.Data[i] = new Segment
                            {
                                IsChecked = false,
                                // C# 9.0 : var a = item with { ... }

                                //IsChecked = item.IsChecked,
                                Starting = item.Starting,
                                Raw = item.Raw,
                                Map = item.Map,
                            };
                        }
                    }
                }
            }
        }

        public void DeselectAll()
        {
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];

                    if (item.IsChecked)
                    {
                        figure.Data[i] = new Segment
                        {
                            IsChecked = false,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            Starting = item.Starting,
                            Raw = item.Raw,
                            Map = item.Map,
                        };
                    }
                }
            }
        }

        public void RectChooseItems(Bounds bounds)
        {
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];

                    if (bounds.ContainsPoint(item.Map))
                    {
                        if (item.IsChecked is false)
                        {
                            figure.Data[i] = new Segment
                            {
                                IsChecked = true,
                                // C# 9.0 : var a = item with { ... }

                                //IsChecked = item.IsChecked,
                                Starting = item.Starting,
                                Raw = item.Raw,
                                Map = item.Map,
                            };
                        }
                    }
                    else
                    {
                        if (item.IsChecked)
                        {
                            figure.Data[i] = new Segment
                            {
                                IsChecked = false,
                                // C# 9.0 : var a = item with { ... }

                                //IsChecked = item.IsChecked,
                                Starting = item.Starting,
                                Raw = item.Raw,
                                Map = item.Map,
                            };
                        }
                    }
                }
            }
        }

        internal override void RawToMap()
        {
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    figure.Data[i] = new Segment
                    {
                        Map = Vector2.Transform(item.Raw, this.Matrix),
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        //Map = item.Map,
                    };
                }
            }

            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }
        #endregion

        #region Triangles.SelectedItems.Set
        public void SetTranslationSelectedItems(Vector2 translate)
        {
            this.STSI0(translate);
        }
        public void SetTranslationSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.STSI1(indicator, mode, translate);
        }

        public void SetTranslationXSelectedItems(float translateX)
        {
            this.STXSI0(translateX);
        }
        public void SetTranslationXSelectedItems(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.STXSI1(indicator, mode, translateX);
        }

        public void SetTranslationYSelectedItems(float translateY)
        {
            this.STYSI0(translateY);
        }
        public void SetTranslationYSelectedItems(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.STYSI1(indicator, mode, translateY);
        }

        public void SetTransformSelectedItems(Matrix3x2 matrix)
        {
            this.SFSI0(matrix);
        }
        public void SetTransformSelectedItems(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.SFSI1(indicator, mode, matrix);
        }

        internal override void TranslateRaw()
        {
            this.BeginExtend();
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        Vector2 p = new Vector2(item.Raw.X + this.TranslationX, item.Raw.Y + this.TranslationY);
                        figure.Data[i] = new Segment
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Vector2.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }

        internal override void TransformMap()
        {
            this.BeginExtend();
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        Vector2 p = Vector2.Transform(item.Map, this.TransformMatrix);
                        figure.Data[i] = new Segment
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Vector2.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Set.Index
        public void SetTranslation(Vector2 translate, Vector2 point, int index)
        {
            this.ST0(translate, point, index);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate, Vector2 point, int index)
        {
            this.ST1(indicator, mode, translate, point, index);
        }

        public void SetTranslation(float translateX, float translateY, Vector2 point, int index)
        {
            this.STXY0(translateX, translateY, point, index);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, float translateX, float translateY, Vector2 point, int index)
        {
            this.STXY1(indicator, mode, translateX, translateY, point, index);
        }

        internal override void SI(Vector2 point, int index)
        {
            foreach (Figure figure in this.Data)
            {
                Segment item = figure.Data[index];

                figure.Data[index] = new Segment
                {
                    IsChecked = true,
                    Map = point,
                    Raw = Vector2.Transform(point, this.InverseMatrix),
                    // C# 9.0 : var a = item with { ... }

                    //IsChecked = item.IsChecked,
                    Starting = item.Starting,
                    //Raw = item.Raw,
                    //Map = item.Map,
                };
            }

            this.BeginExtend();
            foreach (Figure items in this.Data)
            {
                foreach (Segment item in items.Data)
                {
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Transform
        public void CacheTranslationSelectedItems()
        {
            this.CTSI();
        }

        public void CacheTransformSelectedItems()
        {
            this.CFSI();
        }

        public void TranslateSelectedItems(Vector2 startingPoint, Vector2 point)
        {
            this.TDSI0(startingPoint, point);
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.TDSI1(indicator, mode, startingPoint, point);
        }

        public void TranslateSelectedItems(Vector2 translate)
        {
            this.TSI0(translate);
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.TSI1(indicator, mode, translate);
        }

        public void TranslateSelectedItems(float translateX, float translateY)
        {
            this.TXYSI0(translateX, translateY);
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.TXYSI1(indicator, mode, translateX, translateY);
        }

        public void TranslateXSelectedItems(float translateX)
        {
            this.TXSI0(translateX);
        }
        public void TranslateXSelectedItems(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.TXSI1(indicator, mode, translateX);
        }

        public void TranslateYSelectedItems(float translateY)
        {
            this.TYSI0(translateY);
        }
        public void TranslateYSelectedItems(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.TYSI1(indicator, mode, translateY);
        }

        public void TransformSelectedItems(Matrix3x2 matrix)
        {
            this.FSI(matrix);
        }

        internal override void CacheRaw()
        {
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        figure.Data[i] = new Segment
                        {
                            IsChecked = true,
                            Starting = item.Raw,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            //Starting = item.Starting,
                            Raw = item.Raw,
                            Map = item.Map,
                        };
                    }
                }
            }
        }

        internal override void CacheMap()
        {
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        figure.Data[i] = new Segment
                        {
                            IsChecked = true,
                            Starting = item.Map,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            //Starting = item.Starting,
                            Raw = item.Raw,
                            Map = item.Map,
                        };
                    }
                }
            }
        }

        internal override void TranslateStarting()
        {
            this.BeginExtend();
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        Vector2 p = Math.Translate(item.Starting, this.TranslationX, this.TranslationY);
                        figure.Data[i] = new Segment
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Vector2.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }

        internal override void TransformStarting()
        {
            this.BeginExtend();
            foreach (Figure figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        Vector2 p = Vector2.Transform(item.Starting, this.TransformMatrix);
                        figure.Data[i] = new Segment
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Vector2.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                    this.Extend(item.Raw);
                }
            }
            this.EndExtend();
        }
        #endregion

        #region Triangles.Set
        public void SetTranslation(Vector2 translate)
        {
            this.ST0(translate);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.ST1(indicator, mode, translate);
        }

        public void SetTranslationX(float translateX)
        {
            this.STX0(translateX);
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.STX1(indicator, mode, translateX);
        }

        public void SetTranslationY(float translateY)
        {
            this.STY0(translateY);
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.STY1(indicator, mode, translateY);
        }

        public void SetTransform(Matrix3x2 matrix)
        {
            this.SF0(matrix);
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.SF1(indicator, mode, matrix);
        }
        #endregion

        #region Triangles.Transform
        public void CacheTranslation()
        {
            this.CT();
        }

        public void CacheTransform()
        {
            this.CF();
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.TD0(startingPoint, point);
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.TD1(indicator, mode, startingPoint, point);
        }

        public void Translate(Vector2 translate)
        {
            this.T0(translate);
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.T1(indicator, mode, translate);
        }

        public void Translate(float translateX, float translateY)
        {
            this.TXY0(translateX, translateY);
        }
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.TXY1(indicator, mode, translateX, translateY);
        }

        public void TranslateX(float translateX)
        {
            this.TX0(translateX);
        }
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.TX1(indicator, mode, translateX);
        }

        public void TranslateY(float translateY)
        {
            this.TY0(translateY);
        }
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.TY1(indicator, mode, translateY);
        }

        public void Transform(Matrix3x2 matrix)
        {
            this.F(matrix);
        }
        #endregion
    }
}