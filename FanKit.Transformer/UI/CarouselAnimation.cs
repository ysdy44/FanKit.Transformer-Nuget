using System;

namespace FanKit.Transformer.UI
{
    public sealed class CarouselAnimation
    {
        static readonly TimeSpan TotalTime = TimeSpan.FromMilliseconds(300);
        long Time;

        readonly float TotalTimeF = TotalTime.Ticks;
        float TimeF;

        // 0.0~1.0
        float Scale;
        float Reverse;
        float Amout;

        public float Form { get; private set; }
        public float To { get; private set; }
        public float Value { get; private set; }

        public void Reset(float offset, CarouselItem2 item)
        {
            Time = 0;

            Scale = 1f;
            Reverse = 1f;
            Amout = 0f;

            Form = offset;
            To = offset - item.Raw;
            Value = offset;
        }

        public void Reset(float form, float to)
        {
            Time = 0;

            Scale = 1f;
            Reverse = 1f;
            Amout = 0f;

            Form = form;
            To = to;
            Value = form;
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