using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    public readonly struct NodeSmooth
    {
        readonly float px;
        readonly float py;
        readonly float pls;
        readonly float pd;

        readonly float nx;
        readonly float ny;
        readonly float nls;
        readonly float nd;

        readonly float x;
        readonly float y;
        readonly float s;

        readonly float j0;
        readonly float j1;

        // ---------------- Curve ---------------- //
        readonly float p;
        readonly float value;

        readonly float footX;
        readonly float footY;

        readonly float vx;
        readonly float vy;

        readonly float ls;
        // ---------------- Curve ---------------- //

        readonly float invNorm;
        readonly float normX;
        readonly float normY;

        public readonly Vector2 LeftVector;
        public readonly Vector2 RightVector;
        public readonly Node Node;

        public NodeSmooth(Vector2 previous, Vector2 current, Vector2 next)
        {
            // Distance of Previous
            px = current.X - previous.X;
            py = current.Y - previous.Y;
            pls = px * px + py * py;
            pd = (float)System.Math.Sqrt(pls) / Constants.PI;

            // Distance of Next
            nx = current.X - next.X;
            ny = current.Y - next.Y;
            nls = nx * nx + ny * ny;
            nd = (float)System.Math.Sqrt(nls) / Constants.PI;

            x = previous.X - next.X;
            y = previous.Y - next.Y;
            s = x * x + y * y;

            j0 = x * py;
            j1 = y * px;

            if (j0 == j1)
            {
                // ---------------- Curve ---------------- //
                p = 0f;
                value = 0f;

                footX = 0f;
                footY = 0f;

                vx = 0f;
                vy = 0f;

                ls = 0f;
                // ---------------- Curve ---------------- //

                // Normalize
                invNorm = 1.0f / (float)System.Math.Sqrt(s);
                normX = x * invNorm;
                normY = y * invNorm;

                LeftVector = new Vector2
                {
                    X = pd * normX,
                    Y = pd * normY,
                };
                RightVector = new Vector2
                {
                    X = -nd * normX,
                    Y = -nd * normY,
                };
            }
            else
            {
                // Foot Point
                p = px * x + py * y;
                value = p / s;

                footX = previous.X + x * value;
                footY = previous.Y + y * value;

                if (j0 < j1)
                {
                    vx = footX - current.X;
                    vy = footY - current.Y;
                }
                else
                {
                    vx = current.X - footX;
                    vy = current.Y - footY;
                }

                // Normalize
                ls = vx * vx + vy * vy;
                invNorm = 1.0f / (float)System.Math.Sqrt(ls);
                normX = vx * invNorm;
                normY = vy * invNorm;

                LeftVector = new Vector2
                {
                    X = pd * normY,
                    Y = -pd * normX,
                };
                RightVector = new Vector2
                {
                    X = -nd * normY,
                    Y = nd * normX,
                };
            }

            Node = new Node
            {
                Point = current,
                LeftControlPoint = current + LeftVector,
                RightControlPoint = current + RightVector,
            };
        }
    }
}