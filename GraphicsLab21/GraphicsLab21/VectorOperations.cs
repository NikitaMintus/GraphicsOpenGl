using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLab21
{
    class VectorOperations
    {
        public static Vector Diff(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static float GetLength(Vector a)
        {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        }

        public static float CalculateCos(Vector a, Vector b)
        {
            float cos = (ScalarMultiplication(a, b)) / (GetLength(a) * GetLength(b));
            return cos;
        }

        public static Vector Cross(Vector a, Vector b)
        {
            float resX = a.Y * b.Z - a.Z * b.Y;
            float resY = a.Z * b.X - a.X * b.Y;
            float resZ = a.X * b.Y - a.Y * b.X;

            return new Vector(resX, resY, resZ);
        }

        public static float ScalarMultiplication(Vector a, Vector b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

       

        public static Vector GetTriangleNormal(Vector a, Vector b, Vector c)
        {
            Vector edge1 = Diff(b, a);
            Vector edge2 = Diff(c, a);
            Vector normal = Cross(edge1, edge2);
            return normal;
        }

        public static Vector GetNormal(float u, float v, float deltaU, float deltaV, float a, float b, float p)
        {
            float dxU = 0, dyU = 0, dzU = 0;
            float dxV = 0, dyV = 0, dzV = 0;
            dxU = (v * (float)Math.Cos(u + deltaU) - v * (float)Math.Cos(u)) / deltaU;
            dyU = (v * (float)Math.Sin(u + deltaU) - v * (float)Math.Sin(u)) / deltaU;
            dzU = (b * (u + deltaU) + a * (float)Math.Sin(p * (u + deltaU)));
            Vector vec1 = new Vector(dxU, dyU, dzU);

            dxV = ((v + deltaV) * (float)Math.Cos(u) - v * (float)Math.Cos(u)) / deltaV;
            dyV = ((v + deltaV) * (float)Math.Sin(u) - v * (float)Math.Sin(u)) / deltaV;
            dzV = 0.0f;
            Vector vec2 = new Vector(dxV, dyV, dzV);

            Vector normal = Cross(vec1, vec2);

            return normal;
        }
    }
}
