using System;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public static class MathUtil
    {
        public static float DEG2RAD = (float)(Math.PI / 180.0f);
        public static float RAD2DEG = (float)(180.0f / Math.PI);

        /// <summary>
        /// Vector magnitude
        /// </summary>
        public static float Size(this Vector2f vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        /// <summary>
        /// Squared vector magnitude
        /// </summary>
        public static float SizeSquared(this Vector2f vector)
        {
            return (vector.X * vector.X + vector.Y * vector.Y);
        }

        /// <summary>
        /// Returns the normalized vector (unit vector)
        /// </summary>
        public static Vector2f Normal(this Vector2f vector)
        {
            Vector2f result = vector;

            float size = vector.Size();
            if (size > 0.0f)
            {
                result.X /= size;
                result.Y /= size;
            }

            return result;
        }

        /// <summary>
        /// Dot product in range [-1, 1]
        /// </summary>
        public static float Dot(Vector2f lhs, Vector2f rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// Returns the angle in degrees between lhs and rhs (unsigned)
        /// </summary>
        public static float Angle(Vector2f lhs, Vector2f rhs)
        {
            return (float)Math.Acos(Dot(lhs.Normal(), rhs.Normal())) * RAD2DEG;
        }

        /// <summary>
        /// Returns the angle in degrees between lhs and rhs (signed)
        /// </summary>
        public static float AngleWithSign(Vector2f lhs, Vector2f rhs)
        {
            return (float)Math.Acos(Dot(lhs.Normal(), rhs.Normal())) * RAD2DEG * Sign(lhs, rhs);
        }

        /// <summary>
        /// Returns the cross product -1 or 1 with normalize Vectors
        /// </summary>
        public static float Cross(Vector2f lhs, Vector2f rhs)
        {
            return (lhs.X * rhs.Y) - (lhs.Y * rhs.X);
        }

        // <summary>
        /// Returns the sign [-1 or 1] with normalize Vectors
        /// </summary>
        public static float Sign(Vector2f lhs, Vector2f rhs)
        {
            return (Cross(lhs, rhs) <= 0.0f) ? 1.0f : -1.0f;
        }

        /// <summary>
        /// Rotate v angle degrees
        /// </summary>
        public static Vector2f Rotate(this Vector2f v, float angle)
        {
            float sin0 = (float)Math.Sin(angle * DEG2RAD);
            float cos0 = (float)Math.Cos(angle * DEG2RAD);

            if (cos0 * cos0 < 0.001f * 0.001f)
                cos0 = 0.0f;

            Vector2f result = new Vector2f();
            result.X = cos0 * v.X - sin0 * v.Y;
            result.Y = sin0 * v.X + cos0 * v.Y;
            return result;
        }

        public static float Clamp(float c, float A, float B)
        {
            return (float)Math.Max(Math.Min(c, B), A);
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        /// <param name="A">Initial pos</param>
        /// <param name="B">Target pos</param>
        /// <param name="d">Percentage in range [0.0,1.0]</param>
        public static Vector2f Lerp(Vector2f A, Vector2f B, float d)
        {
            d = Clamp(d, 0.0f, 1.0f);
            return B * d + A * (1.0f - d);
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        /// <param name="A">Initial value</param>
        /// <param name="B">Target value</param>
        /// <param name="d">Percentage in range [0.0,1.0]</param>
        public static float Lerp(float A, float B, float d)
        {
            d = Clamp(d, 0.0f, 1.0f);
            return B * d + A * (1.0f - d);
        }
    }
}
