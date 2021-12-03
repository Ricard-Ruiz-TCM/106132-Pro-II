using System;
using SFML.Window;
using SFML.System;

namespace Snake
{
	public static class MathUtil
	{
		public static float DEG2RAD = (float)(Math.PI / 180.0f);
		public static float RAD2DEG = (float)(180.0f / Math.PI);

		public static float Size(this Vector2f vector)
		{
			return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
		}

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
	}
}
