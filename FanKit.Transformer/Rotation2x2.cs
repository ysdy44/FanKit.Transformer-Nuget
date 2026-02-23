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
    }
}