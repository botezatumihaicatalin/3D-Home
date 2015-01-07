using System;

namespace Home3d
{
    public class Vertex3
    {
        public Vertex3()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        public Vertex3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public void Add(Vertex3 other)
        {
            X += other.X;
            Y += other.Y;
            Z += other.Z;
        }
        public void Substract(Vertex3 other)
        {
            X -= other.X;
            Y -= other.Y;
            Z -= other.Z;
        }

        public void Multiply(double scalar)
        {
            X *= scalar;
            Y *= scalar;
            Z *= scalar;
        }

        public void Divide(double scalar)
        {
            X /= scalar;
            Y /= scalar;
            Z /= scalar;
        }

        public Vertex3 Normalize()
        {
            var length = Length();
            return new Vertex3(X / length, Y / length, Z / length);
        }
        public double Length()
        {
            return Math.Sqrt(X*X + Y*Y + Z*Z);
        }
        public Vertex3 CrossProduct(Vertex3 other)
        {
            return new Vertex3(Y * other.Z - Z * other.Y , Z * other.X - X * other.Z , X * other.Y - Y * other.X);
        }
    }
}
