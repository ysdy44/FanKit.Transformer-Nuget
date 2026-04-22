using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    public readonly struct ControllerRadians
    {
        readonly Angle r2;

        readonly float rn; // Numerator
        readonly int rq; // Quotient
        public readonly float Radians; // Subtract

        internal ControllerRadians(TransformController r, Vector2 point, float stepFrequency)
        {
            r2 = new Angle(point, r.c.c);

            if (float.IsNaN(stepFrequency))
            {
                rn = default;
                rq = default;
                Radians = r2 - r.r1;
            }
            else
            {
                rn = stepFrequency / 2f + r.r0 + r2 - r.r1;
                rq = (int)(rn / stepFrequency);
                Radians = rq * stepFrequency - r.r0;
            }
        }

        internal ControllerRadians(LineController r, Vector2 point, float stepFrequency)
        {
            r2 = new Angle(point, r.c);

            if (float.IsNaN(stepFrequency))
            {
                rn = default;
                rq = default;
                Radians = r2 - r.r1;
            }
            else
            {
                rn = stepFrequency / 2f + r.r0 + r2 - r.r1;
                rq = (int)(rn / stepFrequency);
                Radians = rq * stepFrequency - r.r0;
            }
        }
    }
}