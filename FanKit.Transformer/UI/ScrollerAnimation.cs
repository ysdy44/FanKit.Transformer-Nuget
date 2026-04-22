using System;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public sealed class ScrollerAnimation
    {
        static readonly TimeSpan TotalTime = TimeSpan.FromMilliseconds(300);
        long Time;

        readonly float TotalTimeF = TotalTime.Ticks;
        float TimeF;

        // 0.0~1.0
        float Scale;
        float Reverse;
        float Amout;

        public float StartingY { get; private set; }
        public ScrollerDirection Direction { get; private set; }

        public Vector2 Form { get; private set; }
        public Vector2 To { get; private set; }
        public Vector2 Value { get; private set; }

        public void Reset(Vector2 form, ScrollerBounds bounds, float startingY)
        {
            Time = 0;
            TimeF = 0f;

            Scale = 1f;
            Reverse = 1f;
            Amout = 0f;

            StartingY = startingY;
            Direction = bounds.EndDirection(form.X);

            switch (Direction)
            {
                case ScrollerDirection.None:
                    Form = form;
                    To = new Vector2(bounds.Left, startingY);
                    Value = form;
                    break;
                case ScrollerDirection.PageUp:
                    Form = form;
                    To = new Vector2(bounds.Right, startingY);
                    Value = form;
                    break;
                default:
                    break;
            }
        }

        public bool Update(TimeSpan elapsedTime)
        {
            Time += elapsedTime.Ticks;
            TimeF = Time;

            if (TimeF > TotalTimeF)
                return false;

            Scale = 1f - Time / TotalTimeF;
            Reverse = Scale * Scale;
            Amout = 1f - Reverse;

            Value = Reverse * Form + Amout * To;
            return true;
        }
    }
}