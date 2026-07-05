using FanKit.Transformer.Controllers;
using System.Collections.Generic;

namespace FanKit.Transformer.Curves
{
    public static partial class PathBuilderExtensions
    {
        const bool Closed = true;
        const bool Open = false;

        // Radians
        const float PI = Constants.PI;
        const float PITwice = Constants.PITwice;
        const float PIOver2 = Constants.PIOver2;

        const float R360 = Constants.PI + Constants.PI;
        const float R270 = Constants.PI + Constants.PIOver2;
        const float R180 = Constants.PI;
        const float R90 = Constants.PIOver2;
        const float R0 = 0f;

        private static void CreatePoint(IPathBuilder pathBuilder, Segment0 previous, Segment0 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Point, next.Point);

        private static void CreatePoint(IPathBuilder pathBuilder, Segment1 previous, Segment1 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Point, next.Point);

        private static void CreateActual(IPathBuilder pathBuilder, Segment1 previous, Segment1 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Actual, next.Actual);

        private static void CreateRaw(IPathBuilder pathBuilder, Segment2 previous, Segment2 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Raw, next.Raw);

        private static void CreateMap(IPathBuilder pathBuilder, Segment2 previous, Segment2 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Map, next.Map);

        private static void CreateRaw(IPathBuilder pathBuilder, Segment3 previous, Segment3 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Raw, next.Raw);

        private static void CreateActual(IPathBuilder pathBuilder, Segment3 previous, Segment3 next)
            => AddBezier(pathBuilder, previous.IsSmooth, next.IsSmooth, previous.Actual, next.Actual);

        private static void AddBezier(IPathBuilder pathBuilder, bool previousIsSmooth, bool nextIsSmooth, Node previousPoint, Node nextPoint)
        {
            if (nextIsSmooth)
            {
                if (previousIsSmooth)
                    pathBuilder.AddCubicBezier(previousPoint.RightControlPoint, nextPoint.LeftControlPoint, nextPoint.Point);
                else
                    pathBuilder.AddQuadraticBezier(nextPoint.LeftControlPoint, nextPoint.Point);
            }
            else
            {
                if (previousIsSmooth)
                    pathBuilder.AddQuadraticBezier(previousPoint.RightControlPoint, nextPoint.Point);
                else
                    pathBuilder.AddLine(nextPoint.Point);
            }
        }

        #region ClosestPointer
        public static void CreatePreviousPath(this IPathBuilder pathBuilder, ClosestPointer closest)
        {
            // ?

            pathBuilder.BeginFigure(closest.Previous.Point);

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, closest.Previous, closest.Current);

            pathBuilder.EndFigure(Open);

            // return
        }

        public static void CreateNextPath(this IPathBuilder pathBuilder, ClosestPointer closest)
        {
            // ?

            pathBuilder.BeginFigure(closest.Current.Point);

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, closest.Current, closest.Next);

            pathBuilder.EndFigure(Open);

            // return
        }

        public static void CreatePreviousPath(this IPathBuilder pathBuilder, ClosestPointer closest, ICanvasMatrix canvasMatrix)
        {
            // ?

            pathBuilder.BeginFigure(canvasMatrix.Transform(closest.Previous.Point));

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, canvasMatrix.Transform(closest.Previous), canvasMatrix.Transform(closest.Current));

            pathBuilder.EndFigure(Open);

            // return
        }

        public static void CreateNextPath(this IPathBuilder pathBuilder, ClosestPointer closest, ICanvasMatrix canvasMatrix)
        {
            // ?

            pathBuilder.BeginFigure(canvasMatrix.Transform(closest.Current.Point));

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, canvasMatrix.Transform(closest.Current), canvasMatrix.Transform(closest.Next));

            pathBuilder.EndFigure(Open);

            // return
        }
        #endregion

        #region Node
        public static void CreatePath(this IPathBuilder pathBuilder, List<Node> segments, bool isClosed)
        {
            CreatePointPath(pathBuilder, segments, isClosed);
        }

        private static void CreatePointPath(IPathBuilder pathBuilder, List<Node> segments, bool isClosed)
        {
            // ?

            Node first = segments[0];
            pathBuilder.BeginFigure(first.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Node previous = segments[i - 1];
                Node next = segments[i];
                AddBezier(pathBuilder, true, true, previous, next);
            }

            if (isClosed)
            {
                Node last = segments[segments.Count - 1];
                AddBezier(pathBuilder, true, true, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }
        #endregion

        #region Segment0
        public static void CreatePath(this IPathBuilder pathBuilder, List<Segment0> figures, bool isClosed)
        {
            CreatePointPath(pathBuilder, figures, isClosed);
        }

        private static void CreatePointPath(IPathBuilder pathBuilder, List<Segment0> segments, bool isClosed)
        {
            // ?

            Segment0 first = segments[0];
            pathBuilder.BeginFigure(first.Point.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Segment0 previous = segments[i - 1];
                Segment0 next = segments[i];
                CreatePoint(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment0 last = segments[segments.Count - 1];
                CreatePoint(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }
        #endregion

        #region Segment1
        public static void CreatePath(this IPathBuilder pathBuilder, NodePointUnits unit, List<Segment1> segments, bool isClosed)
        {
            switch (unit)
            {
                case NodePointUnits.Normal:
                    CreatePointPath(pathBuilder, segments, isClosed);
                    break;
                case NodePointUnits.Actual:
                    CreateActualPath(pathBuilder, segments, isClosed);
                    break;
                default:
                    break;
            }
        }

        private static void CreatePointPath(IPathBuilder pathBuilder, List<Segment1> segments, bool isClosed)
        {
            // ?

            Segment1 first = segments[0];
            pathBuilder.BeginFigure(first.Point.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Segment1 previous = segments[i - 1];
                Segment1 next = segments[i];
                CreatePoint(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment1 last = segments[segments.Count - 1];
                CreatePoint(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }

        private static void CreateActualPath(IPathBuilder pathBuilder, List<Segment1> segments, bool isClosed)
        {
            // ?

            Segment1 first = segments[0];
            pathBuilder.BeginFigure(first.Actual.Point);

            for (int i = 1; i < segments.Count; i++)
            {
                Segment1 previous = segments[i - 1];
                Segment1 next = segments[i];
                CreateActual(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment1 last = segments[segments.Count - 1];
                CreateActual(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }
        #endregion

        #region Segment2
        public static void CreatePath(this IPathBuilder pathBuilder, NodePointUnits unit, IEnumerable<Figure2> figures)
        {
            switch (unit)
            {
                case NodePointUnits.Normal:
                    CreateRawPath(pathBuilder, figures);
                    break;
                case NodePointUnits.Actual:
                    CreateMapPath(pathBuilder, figures);
                    break;
                default:
                    break;
            }
        }

        private static void CreateRawPath(IPathBuilder pathBuilder, IEnumerable<Figure2> figures)
        {
            // ?

            foreach (Figure2 figure in figures)
            {
                Segment2 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Raw.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment2 previous = figure.Segments[i - 1];
                    Segment2 next = figure.Segments[i];
                    CreateRaw(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment2 last = figure.Segments[figure.Segments.Count - 1];
                    CreateRaw(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }

        private static void CreateMapPath(IPathBuilder pathBuilder, IEnumerable<Figure2> figures)
        {
            // ?

            foreach (Figure2 figure in figures)
            {
                Segment2 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Map.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment2 previous = figure.Segments[i - 1];
                    Segment2 next = figure.Segments[i];
                    CreateMap(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment2 last = figure.Segments[figure.Segments.Count - 1];
                    CreateMap(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }
        #endregion

        #region Segment3
        public static void CreatePath(this IPathBuilder pathBuilder, NodePointUnits unit, IEnumerable<Figure3> figures)
        {
            switch (unit)
            {
                case NodePointUnits.Normal:
                    CreateRawPath(pathBuilder, figures);
                    break;
                case NodePointUnits.Actual:
                    CreateActualPath(pathBuilder, figures);
                    break;
                default:
                    break;
            }
        }

        private static void CreateRawPath(IPathBuilder pathBuilder, IEnumerable<Figure3> figures)
        {
            // ?

            foreach (Figure3 figure in figures)
            {
                Segment3 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Raw.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment3 previous = figure.Segments[i - 1];
                    Segment3 next = figure.Segments[i];
                    CreateRaw(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment3 last = figure.Segments[figure.Segments.Count - 1];
                    CreateRaw(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }

        private static void CreateActualPath(IPathBuilder pathBuilder, IEnumerable<Figure3> figures)
        {
            // ?

            foreach (Figure3 figure in figures)
            {
                Segment3 first = figure.Segments[0];
                pathBuilder.BeginFigure(first.Actual.Point);

                for (int i = 1; i < figure.Segments.Count; i++)
                {
                    Segment3 previous = figure.Segments[i - 1];
                    Segment3 next = figure.Segments[i];
                    CreateActual(pathBuilder, previous, next);
                }

                if (figure.IsClosed)
                {
                    Segment3 last = figure.Segments[figure.Segments.Count - 1];
                    CreateActual(pathBuilder, last, first);

                    pathBuilder.EndFigure(Closed);
                }
                else
                {
                    pathBuilder.EndFigure(Open);
                }
            }

            // return
        }
        #endregion
    }
}