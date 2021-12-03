using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace TeteraDeRusell
{

	// static class WINDOW
	// ********************
	// Clase estatica con 2 variables consantes para poder acceder a las dimensiones de la pantalla en cualquier lugar
	public static class WINDOW
    {
		public const int W = 640;
		public const int H = 480;
    }

	public class TeaGame : Game
	{
		private RenderWindow window;
		private Earth e;
		private Sun s;
		private TeaPot t;

		// Variables de texto
		private Font mFont;
		private Text mClickBait;

		public void Init(){
			// Window initialization
			VideoMode videoMode = new VideoMode(WINDOW.W, WINDOW.H);
			window = new RenderWindow(videoMode, "Russell TeaPot");

			// Cargamos la fuente y el texto
			mFont = new Font("Data/fuente.ttf");
			mClickBait = new Text("CLIQUE EN LA TETERA Y JAMAS PENSE QUE PASARIA ESTO...", mFont, 15);
			mClickBait.Position = new Vector2f(15.0f, 15.0f);

			// Declaración a mano
			s = new Sun(WINDOW.W / 4.0f * 2.0f, WINDOW.H / 2.0f);
			e = new Earth(WINDOW.W / 4.0f * 2.0f + s.Texture.Size.X * 1.5f, WINDOW.H/2.0f);
			t = new TeaPot(WINDOW.W / 4.0f * 2.0f + s.Texture.Size.X, WINDOW.H/2.0f);

			window.KeyPressed += HandleKeyPressed;
			window.MouseButtonPressed += MousePressed;
		}

		public void DeInit(){
			window.Dispose ();
		}

		public void Update( float dt){
			if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
			{
				window.Close();
			}
			window.DispatchEvents();

			// 
			if (t.followEarth())
            {
				t.updateEarthPosition(e.getPosition());
				mClickBait.Position = t.Position;
            }

			// Update a mano
			s.Update (dt);
			e.Update (dt);
			t.Update (dt);
		}

		public void Draw(){
			window.Clear ();
			//window.Draw(back);
			window.Draw(e);
			window.Draw(s);
			window.Draw(t);
			window.Draw(mClickBait);
			window.Display ();
		}

		public bool IsAlive()
		{
			return window.IsOpen;
		}

		public void HandleKeyPressed(object sender, KeyEventArgs e)
		{
		}

		public void MousePressed(object sender, MouseButtonEventArgs e)
		{
			if( e.Button == Mouse.Button.Left && t.GetGlobalBounds().Contains(e.X,e.Y)){
				t.swapRotation();
				if (t.followEarth())
                {
					mClickBait.DisplayedString = "ES LA LUNA";
                } else
                {
					mClickBait.DisplayedString = "CLIQUE EN LA TETERA Y JAMAS PENSE QUE PASARIA ESTO...";
					mClickBait.Position = new Vector2f(15.0f, 15.0f);

				}
			}
		}
	}
}

