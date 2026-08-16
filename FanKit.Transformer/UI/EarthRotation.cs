using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct EarthRotation
    {
        readonly Matrix4x4 Y;
        readonly Matrix4x4 X;
        readonly Matrix4x4 Z;
        readonly Matrix4x4 M;

        public EarthRotation(Vector3 radians)
        {
            this.Y = Matrix4x4.CreateRotationY(radians.Y);
            this.X = Matrix4x4.CreateRotationX(radians.X);
            this.Z = Matrix4x4.CreateRotationZ(radians.Z);
            this.M = this.Y * this.X * this.Z;
        }

        public Vector3 RotateUnitVector(Vector3 unitVector)
        {
            return Vector3.Transform(unitVector, this.M);
        }
    }
}