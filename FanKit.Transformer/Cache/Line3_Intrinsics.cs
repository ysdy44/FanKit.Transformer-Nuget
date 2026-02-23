using System.Numerics;

namespace FanKit.Transformer.Cache
{
    public readonly partial struct Line3
    {
        // Line
        public readonly Vector2 Point0;
        public readonly Vector2 Point1;

        // Sides
        readonly float CenterX;
        readonly float CenterY;
        public readonly Vector2 Center;

        // Handle
        public readonly float X;
        public readonly float Y;
        public readonly float LengthSquared;

        public readonly float Length;
        public readonly float HandleX;
        public readonly float HandleY;

        // Handle Sides
        public readonly Vector2 Handle;

        // Handle Corners
        public readonly Vector2 Handle0;
        public readonly Vector2 Handle1;

        #region Constructors
        public Line3(Vector2 point0, Vector2 point1, float handleLength = 32f)
        {
            // Line
            this.Point0 = point0;
            this.Point1 = point1;

            // Sides
            this.CenterX = this.Point0.X + this.Point1.X;
            this.CenterY = this.Point0.Y + this.Point1.Y;
            this.Center = new Vector2(this.CenterX / 2f, this.CenterY / 2f);

            // Handle
            this.X = this.Point0.X - this.Point1.X;
            this.Y = this.Point0.Y - this.Point1.Y;
            this.LengthSquared = this.X * this.X + this.Y * this.Y;

            this.Length = (float)System.Math.Sqrt(this.LengthSquared);
            this.HandleX = handleLength * this.X / this.Length;
            this.HandleY = handleLength * this.Y / this.Length;

            // Handle Sides
            this.Handle = new Vector2(this.Center.X - this.HandleY,
                this.Center.Y + this.HandleX);

            // Handle Corners
            this.Handle0 = new Vector2(this.Point0.X + this.HandleX,
                this.Point0.Y + this.HandleY);
            this.Handle1 = new Vector2(this.Point1.X - this.HandleX,
                this.Point1.Y - this.HandleY);
        }

        public Line3(Vector2 point0, Vector2 point1, ICanvasMatrix matrix, float handleLength = 32f)
        {
            // Line
            this.Point0 = matrix.Transform(point0);
            this.Point1 = matrix.Transform(point1);

            // Sides
            this.CenterX = this.Point0.X + this.Point1.X;
            this.CenterY = this.Point0.Y + this.Point1.Y;
            this.Center = new Vector2(this.CenterX / 2f, this.CenterY / 2f);

            // Handle
            this.X = this.Point0.X - this.Point1.X;
            this.Y = this.Point0.Y - this.Point1.Y;
            this.LengthSquared = this.X * this.X + this.Y * this.Y;

            this.Length = (float)System.Math.Sqrt(this.LengthSquared);
            this.HandleX = handleLength * this.X / this.Length;
            this.HandleY = handleLength * this.Y / this.Length;

            // Handle Sides
            this.Handle = new Vector2(this.Center.X - this.HandleY,
                this.Center.Y + this.HandleX);

            // Handle Corners
            this.Handle0 = new Vector2(this.Point0.X + this.HandleX,
                this.Point0.Y + this.HandleY);
            this.Handle1 = new Vector2(this.Point1.X - this.HandleX,
                this.Point1.Y - this.HandleY);
        }
        #endregion Constructors
    }
}