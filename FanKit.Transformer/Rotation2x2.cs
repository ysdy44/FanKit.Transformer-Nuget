using System.Numerics;

namespace FanKit.Transformer
{
    public readonly struct Rotation2x2
    {
        public readonly Matrix3x2 Matrix;
        public readonly float C; // M11
        public readonly float S; // M12

        public Rotation2x2(float radians)
        {
            this.Matrix = Matrix3x2.CreateRotation(radians);
            this.C = this.Matrix.M11;
            this.S = this.Matrix.M12;
        }

        public Vector2 Transform(Vector2 position)
        {
            return new Vector2(
                position.X * this.C - position.Y * this.S,
                position.X * this.S + position.Y * this.C);
        }

        public Vector2 Transform(float x, float y)
        {
            return new Vector2(
                x * this.C - y * this.S,
                x * this.S + y * this.C);
        }

        internal Vector2 Normalize()
        {
            return new Vector2(C, S);
        }

        internal Vector2 T1(Vector2 position) // Transform 1
        {
            return new Vector2(
                -position.X * this.C - position.Y * this.S,
                -position.X * this.S + position.Y * this.C);
        }

        internal Vector2 T2(Vector2 position) // Transform 2
        {
            return new Vector2(
                position.X * this.C + position.Y * this.S,
                position.X * this.S - position.Y * this.C);
        }

        internal Vector2 T3(Vector2 position) // Transform 3
        {
            return new Vector2(
                -position.X * this.C + position.Y * this.S,
                -position.X * this.S - position.Y * this.C);
        }

        internal Vector2 NP() // Normalize Point
        {
            return new Vector2(-this.C, -this.S);
        }

        internal Vector2 NCP(float sclae) // Normalize Control Point
        {
            return new Vector2(
                -this.S * sclae - this.C, // Reflection.X + Point.X
                this.C * sclae - this.S); // Reflection.Y + Point.Y
        }
    }
}