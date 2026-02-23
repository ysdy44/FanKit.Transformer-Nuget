using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    // Foot Point
    internal readonly struct Distance4
    {
        internal readonly float hx, hy; // Horizontal lineVector
        internal readonly float hds, rds; // Distance's Squared pointLineA

        // Line Point 0
        // Line Point 1
        internal Distance4(float rx, float ry, float lx, float ly)
        {
            hx = rx - lx;
            hy = ry - ly;

            hds = hx * hx + hy * hy;
            rds = rx * hx + ry * hy;
        }

        // Scale
        // Ratio Scale
        internal float S(Vector2 p)
        {
            float pds = p.X * hx + p.Y * hy;
            return (rds - pds - pds) / (hds * 2); // sds
        }

        // Scale / 2
        // Center Scale
        internal float S2(Vector2 p)
        {
            float pds = p.X * hx + p.Y * hy;
            return (rds - pds - pds) / hds; // sds * 2
        }

        // Scale / 4
        // Ratio Center Scale
        internal float S4(Vector2 p)
        {
            float pds = p.X * hx + p.Y * hy;
            return (rds + rds - pds * 4) / hds; // sds * 4
        }
    }
}