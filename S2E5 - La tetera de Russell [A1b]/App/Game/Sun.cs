using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TeteraDeRusell
{
	public class Sun : Sprite
	{
		private float speed;

		public Sun (float x, float y)
		{
			speed = 100.0f;
			Position = new Vector2f (x, y);
			Texture = new Texture("Data/Sun.png");
			Origin = new Vector2f(GetLocalBounds().Width, GetLocalBounds().Height) / 2.0f;
		}

		public void Update(float dt){
			Rotation += speed * dt * 1.5f;
		}
	}
}

