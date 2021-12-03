using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TeteraDeRusell
{
	public class Earth : Sprite
	{
		private float speed, t;

		public Earth (float x, float y)
		{
			speed = -150.0f; t = 0.0f;
			Position = new Vector2f (x, y);
			Texture = new Texture("Data/Earth.png");
			Origin = new Vector2f(GetLocalBounds().Width, GetLocalBounds().Height) / 2.0f;
		}

		public void Update(float dt){
			Rotation += speed * dt * 1.5f; t += dt * 0.9f;
			// Utilizamos moviento de sinus y cosinus para la rotación teniendo encuenta el centro de la pantalla
			Position = new Vector2f(WINDOW.W / 2.0f + (float)Math.Cos(t * 2) * speed, WINDOW.H / 2.0f + (float)Math.Sin(t * 2) * speed);
		}

		// getPosition
		// ************
		// @return Vector2f Position de la clase
		// Método para recuperar la posición X, Y, del objeto
		public Vector2f getPosition()
        {
			return Position;
        }
	}
}

