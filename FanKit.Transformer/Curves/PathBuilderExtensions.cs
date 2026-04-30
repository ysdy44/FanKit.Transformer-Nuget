using System.Collections.Generic;

namespace FanKit.Transformer.Curves
{
    public static class PathBuilderExtensions
    {
        const bool Closed = true;
        const bool Open = false;

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

        public static void CreatePreviousPath(this IPathBuilder pathBuilder, ClosestPointer closest, ICanvasMatrix matrix)
        {
            // ?

            pathBuilder.BeginFigure(matrix.Transform(closest.Previous.Point));

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, matrix.Transform(closest.Previous), matrix.Transform(closest.Current));

            pathBuilder.EndFigure(Open);

            // return
        }

        public static void CreateNextPath(this IPathBuilder pathBuilder, ClosestPointer closest, ICanvasMatrix matrix)
        {
            // ?

            pathBuilder.BeginFigure(matrix.Transform(closest.Current.Point));

            AddBezier(pathBuilder, closest.PreviousIsSmooth, closest.NextIsSmooth, matrix.Transform(closest.Current), matrix.Transform(closest.Next));

            pathBuilder.EndFigure(Open);

            // return
        }
        #endregion

        #region Node
        public static void CreatePointPath(this IPathBuilder pathBuilder, List<Node> segment, bool isClosed)
        {
            // ?

            Node first = segment[0];
            pathBuilder.BeginFigure(first.Point);

            for (int i = 1; i < segment.Count; i++)
            {
                Node previous = segment[i - 1];
                Node next = segment[i];
                AddBezier(pathBuilder, true, true, previous, next);
            }

            if (isClosed)
            {
                Node last = segment[segment.Count - 1];
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
        public static void CreatePointPath(this IPathBuilder pathBuilder, List<Segment0> segment, bool isClosed)
        {
            // ?

            Segment0 first = segment[0];
            pathBuilder.BeginFigure(first.Point.Point);

            for (int i = 1; i < segment.Count; i++)
            {
                Segment0 previous = segment[i - 1];
                Segment0 next = segment[i];
                CreatePoint(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment0 last = segment[segment.Count - 1];
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
        public static void CreatePointPath(this IPathBuilder pathBuilder, List<Segment1> segment, bool isClosed)
        {
            // ?

            Segment1 first = segment[0];
            pathBuilder.BeginFigure(first.Point.Point);

            for (int i = 1; i < segment.Count; i++)
            {
                Segment1 previous = segment[i - 1];
                Segment1 next = segment[i];
                CreatePoint(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment1 last = segment[segment.Count - 1];
                CreatePoint(pathBuilder, last, first);

                pathBuilder.EndFigure(Closed);
            }
            else
            {
                pathBuilder.EndFigure(Open);
            }

            // return
        }

        public static void CreateActualPath(this IPathBuilder pathBuilder, List<Segment1> segment, bool isClosed)
        {
            // ?

            Segment1 first = segment[0];
            pathBuilder.BeginFigure(first.Actual.Point);

            for (int i = 1; i < segment.Count; i++)
            {
                Segment1 previous = segment[i - 1];
                Segment1 next = segment[i];
                CreateActual(pathBuilder, previous, next);
            }

            if (isClosed)
            {
                Segment1 last = segment[segment.Count - 1];
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
        public static void CreateRawPath(this IPathBuilder pathBuilder, IEnumerable<Figure2> figures)
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

        public static void CreateMapPath(this IPathBuilder pathBuilder, IEnumerable<Figure2> figures)
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
        public static void CreateRawPath(this IPathBuilder pathBuilder, IEnumerable<Figure3> figures)
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

        public static void CreateActualPath(this IPathBuilder pathBuilder, IEnumerable<Figure3> figures)
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