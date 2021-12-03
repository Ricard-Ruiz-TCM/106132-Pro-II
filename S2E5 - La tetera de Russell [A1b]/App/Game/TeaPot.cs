using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TeteraDeRusell
{
	public class TeaPot : Sprite
	{
		private float speed, t;

		// Variable para determinar un segundo Origin para la rotación del objeto
		private Vector2f mSubOrigin;

		// Variable de estado para determinar si rotamos partiendo del sol (centro pantalla) o tierra
		// true -> tierra
		// false -> sol
		private bool mSunOrEarth;

		public TeaPot (float x, float y)
		{
			speed = 95.0f; t = 0.0f;
			// Establecemos mSubOrigin a centro de la pantala y le ordenamos sguir al sol
			mSubOrigin = new Vector2f(WINDOW.W / 2, WINDOW.H / 2); mSunOrEarth = false;
			Position = new Vector2f (x, y);
			Texture = new Texture("Data/TeaPot.png");
			Origin = new Vector2f(GetLocalBounds().Width, GetLocalBounds().Height) / 2.0f;
		}

		public void Update(float dt){
			Rotation += speed * dt * 5.0f; t += dt * 1.25f;
			// Utilizamos moviento de sinus y cosinus para la rotación teniendo encuenta el mSubOrigin
			Position = new Vector2f(mSubOrigin.X + (float)Math.Cos(t * 2) * speed, mSubOrigin.Y + (float)Math.Sin(t * 2) * speed);
		}

		// swapRotation
		// *************
		// Método para cambiar el estado de rotación, de sol (centro pantalla) a tierra (objeto en movimiento)
		// En el caso de seguir a sol, establecemos el mSubOrigin y aumentamos la velocidad
		public void swapRotation()
        {
			mSunOrEarth = !mSunOrEarth;
			speed = 65.0f;
			if (!mSunOrEarth)
			{
				mSubOrigin.X = WINDOW.W / 2.0f; mSubOrigin.Y = WINDOW.H / 2.0f;
				speed += 30.0f;
			}
        }

		// followEarth
		// ************
		// @return bool mSunOrEarth
		// @see TeaPot.mSunOrEarth
		// Método para comprobar si seguimos a la tierra o no segun la variable mSunOrEarth
		public bool followEarth()
        {
			return mSunOrEarth;
        }

		// updateEarthPosition
		// ********************
		// @param Vector2f EarthPosition
		// Método para actualizar la posición el mSubOrigin con la posición de la tierra
		public void updateEarthPosition(Vector2f earthPosition)
        {
			mSubOrigin = earthPosition;
        }

	}
}

