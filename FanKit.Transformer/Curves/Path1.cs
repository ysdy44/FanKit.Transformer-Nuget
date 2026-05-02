using FanKit.Transformer.Cache;
using FanKit.Transformer.Compute;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Segment = FanKit.Transformer.Curves.Segment1;

namespace FanKit.Transformer.Curves
{
    public partial class Path1 : PathTriangle, IGetFigure
    {
        public Bounds SourceBounds { get; private set; }
        Bounds b;
        public Rectangle SourceRect { get; private set; }

        public Triangle Destination => this.Triangle;

        public float TranslationX => this.Host.M31;
        public float TranslationY => this.Host.M32;
        public Matrix3x2 TransformMatrix => this.Host;

        #region Triangles.Initialize
        private void Extend()
        {
            this.SourceBounds = Bounds.Infinity;

            for (int i = 1; i < this.Segments.Count; i++)
            {
                Segment previous = this.Segments[i - 1];
                Segment next = this.Segments[i];

                this.b = Segment.Extend(previous, next);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }

            if (this.IsClosed)
            {
                Segment first = this.Segments[0];
                Segment last = this.Segments[this.Segments.Count - 1];

                this.b = Segment.Extend(last, first);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }
        }
        private void EndExtend()
        {
            this.SourceRect = new Rectangle(this.SourceBounds);

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void Reset(Triangle destination) => this.RT(destination);
        #endregion

        #region Triangles.SelectedItems
        public bool IsClosed;

        public readonly List<Segment> Segments;

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Segments.Count;
        public int GetChecksCount() => this.Segments.Count(GetIsChecked);
        private static bool GetIsChecked(Segment item) => item.IsChecked;

        public Path1(List<Segment> segments)
        {
            this.Segments = segments;

            this.Extend();

            this.SourceRect = new Rectangle(this.SourceBounds);

            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void Complete()
        {
            this.Extend();

            this.EndExtend();
        }

        public void Select(int segmentIndex)
        {
            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];

                if (item.IsChecked)
                {
                    if (segmentIndex != i)
                    {
                        this.Segments[i] = new Segment
                        {
                            IsChecked = false,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
                else
                {
                    if (segmentIndex == i)
                    {
                        this.Segments[i] = new Segment
                        {
                            IsChecked = true,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
            }
        }

        public void DeselectAll()
        {
            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];

                if (item.IsChecked)
                {
                    this.Segments[i] = new Segment
                    {
                        IsChecked = false,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }
        }

        public void RectChooseItems(Bounds bounds)
        {
            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];

                if (bounds.ContainsPoint(item.Point.Point))
                {
                    if (item.IsChecked is false)
                    {
                        this.Segments[i] = new Segment
                        {
                            IsChecked = true,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
                else
                {
                    if (item.IsChecked)
                    {
                        this.Segments[i] = new Segment
                        {
                            IsChecked = false,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
            }
        }
        #endregion

        #region Triangles.SelectedItems.Set
        public void SetTranslationSelectedItems(Vector2 translate) => this.STSI0(translate);
        public void SetTranslationSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate) => this.STSI1(indicator, mode, translate);

        public void SetTranslationXSelectedItems(float translateX) => this.STXSI0(translateX);
        public void SetTranslationXSelectedItems(IIndicator indicator, BoxMode mode, float translateX) => this.STXSI1(indicator, mode, translateX);

        public void SetTranslationYSelectedItems(float translateY) => this.STYSI0(translateY);
        public void SetTranslationYSelectedItems(IIndicator indicator, BoxMode mode, float translateY) => this.STYSI1(indicator, mode, translateY);

        public void SetTransformSelectedItems(Matrix3x2 matrix) => this.SFSI0(matrix);
        public void SetTransformSelectedItems(IIndicator indicator, BoxMode mode, Matrix3x2 matrix) => this.SFSI1(indicator, mode, matrix);

        internal override void TranslateRaw()
        {
            bool has = false;

            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Translate(item.Point, this.TranslationX, this.TranslationY);
                    this.Segments[i] = new Segment
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }

        internal override void TransformMap()
        {
            bool has = false;

            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Transform(item.Point, this.TransformMatrix);
                    this.Segments[i] = new Segment
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Set.Index
        public void SetTranslation(Vector2 translate, Vector2 point, int segmentIndex) => this.ST0(translate, point, segmentIndex);
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate, Vector2 point, int segmentIndex) => this.ST1(indicator, mode, translate, point, segmentIndex);

        public void SetTranslation(float translateX, float translateY, Vector2 point, int segmentIndex) => this.STXY0(translateX, translateY, point, segmentIndex);
        public void SetTranslation(IIndicator indicator, BoxMode mode, float translateX, float translateY, Vector2 point, int segmentIndex) => this.STXY1(indicator, mode, translateX, translateY, point, segmentIndex);

        internal override void SI(Vector2 point, int segmentIndex)
        {
            Segment segment = this.Segments[segmentIndex];
            Node node = segment.Point.MovePoint(point);

            this.Segments[segmentIndex] = new Segment
            {
                IsChecked = true,
                Point = node,
                // C# 9.0 : var a = segment with { ... }

                //IsChecked = segment.IsChecked,
                IsSmooth = segment.IsSmooth,
                Starting = segment.Starting,
                //Point = segment.Point,
                Actual = segment.Actual,
            };

            this.Extend();

            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Transform
        public void CacheTranslationSelectedItems() => this.CTSI();

        public void CacheTransformSelectedItems() => this.CFSI();

        public void TranslateSelectedItems(Vector2 startingPoint, Vector2 point) => this.TDSI0(startingPoint, point);
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point) => this.TDSI1(indicator, mode, startingPoint, point);

        public void TranslateSelectedItems(Vector2 translate) => this.TSI0(translate);
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate) => this.TSI1(indicator, mode, translate);

        public void TranslateSelectedItems(float translateX, float translateY) => this.TXYSI0(translateX, translateY);
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, float translateX, float translateY) => this.TXYSI1(indicator, mode, translateX, translateY);

        public void TranslateXSelectedItems(float translateX) => this.TXSI0(translateX);
        public void TranslateXSelectedItems(IIndicator indicator, BoxMode mode, float translateX) => this.TXSI1(indicator, mode, translateX);

        public void TranslateYSelectedItems(float translateY) => this.TYSI0(translateY);
        public void TranslateYSelectedItems(IIndicator indicator, BoxMode mode, float translateY) => this.TYSI1(indicator, mode, translateY);

        public void TransformSelectedItems(Matrix3x2 matrix) => this.FSI(matrix);

        internal override void CacheRaw()
        {
            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];
                if (item.IsChecked)
                {
                    this.Segments[i] = new Segment
                    {
                        IsChecked = true,
                        Starting = item.Point,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        //Starting = item.Starting,
                        Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }
        }

        internal override void CacheMap()
        {
            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];
                if (item.IsChecked)
                {
                    this.Segments[i] = new Segment
                    {
                        IsChecked = true,
                        Starting = item.Point,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        //Starting = item.Starting,
                        Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }
        }

        internal override void TranslateStarting()
        {
            bool has = false;

            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Translate(item.Starting, this.TranslationX, this.TranslationY);
                    this.Segments[i] = new Segment
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }

        internal override void TransformStarting()
        {
            bool has = false;

            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment item = this.Segments[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Transform(item.Starting, this.TransformMatrix);
                    this.Segments[i] = new Segment
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }
        #endregion

        #region Triangles.Set
        public void SetTranslation(Vector2 translate) => this.ST0(translate);
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate) => this.ST1(indicator, mode, translate);

        public void SetTranslationX(float translateX) => this.STX0(translateX);
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX) => this.STX1(indicator, mode, translateX);

        public void SetTranslationY(float translateY) => this.STY0(translateY);
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY) => this.STY1(indicator, mode, translateY);

        public void SetTransform(Matrix3x2 matrix) => this.SF0(matrix);
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix) => this.SF1(indicator, mode, matrix);
        #endregion

        #region Triangles.Transform
        public void CacheTranslation() => this.CT();

        public void CacheTransform() => this.CF();

        public void Translate(Vector2 startingPoint, Vector2 point) => this.TD0(startingPoint, point);
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point) => this.TD1(indicator, mode, startingPoint, point);

        public void Translate(Vector2 translate) => this.T0(translate);
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate) => this.T1(indicator, mode, translate);

        public void Translate(float translateX, float translateY) => this.TXY0(translateX, translateY);
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY) => this.TXY1(indicator, mode, translateX, translateY);

        public void TranslateX(float translateX) => this.TX0(translateX);
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX) => this.TX1(indicator, mode, translateX);

        public void TranslateY(float translateY) => this.TY0(translateY);
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY) => this.TY1(indicator, mode, translateY);

        public void Transform(Matrix3x2 matrix) => this.F(matrix);
        #endregion
    }
}