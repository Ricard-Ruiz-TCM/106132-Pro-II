using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Minimal {
    // Tips for the exercise.
    //  - The window of the game will not close when clicking on the "Close" icon. 
    //    Your life will be happier if you check the example in the documentation
    //    that comes with source code of the SFML.NET
    //  - Random will be your friend: https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-5.0
    //  - SFML C++ examples: https://www.sfml-dev.org/tutorials/2.5/

    class MainClass {

        public static void Main(string[] args) {
            // -------------------------------------------------
            // Initialization of the game
            //  - Here we create the window
            //  - Load all the data we want in our game
            //  - Create the objects that will be alive during the whole game execution
            //  - Do initial calculations we don't want to execute every frame
            //  - ...

            // Ancho y Alto del video
            uint mVideoWidth = 640;
            uint mVideoHeight = 480;

            // Init y creación del video y ventana de render
            VideoMode mVideoMode = new VideoMode(mVideoWidth, mVideoHeight);
            RenderWindow mRenderWindow = new RenderWindow(mVideoMode, "Minimal SFML - Ricard Ruiz");

            // Cargamos la fuente
            Font mFont_fuente_ttf_ = new Font("data/fuente.ttf");

            // Creamos e inicializamos toods los textos
            Text mTextDvD = new Text("DvD", mFont_fuente_ttf_, 40);
            Text mTextCords = new Text("", mFont_fuente_ttf_, 14);

            // Posicionamos las cordenadas al 2% de margen superior izq.
            mTextCords.Position = new Vector2f(mVideoWidth * 0.02f, mVideoHeight * 0.02f);

            // Cargamos el fichero de audio y creamos Sonido
            SoundBuffer mSoundBuffer = new SoundBuffer("data/sound.wav");
            Sound mSound = new Sound(mSoundBuffer);

            // Creamos el Generador aleatorio
            Random mRandom = new Random();

            // Creamos e inicializamos el circulo 
            CircleShape mCircle = new CircleShape(35.0f);
            mCircle.Position = new Vector2f(mRandom.Next(0, (int)mVideoWidth - (int)(mCircle.Radius * 2)), mRandom.Next(0, (int)mVideoHeight- (int)(mCircle.Radius * 2)));

            // Creamos y rellenamos un array de 3 bytes aleatorios (0 - 255)
            byte[] mRGB = new byte[3]; mRandom.NextBytes(mRGB);

            // Coloreamos el circulo con los bytes aleatorios
            mCircle.FillColor = new Color(mRGB[0], mRGB[1], mRGB[2]);

            // Iniciamos variables de control de estado y posición para el circulo
            /**
             * mState
             * ---------
             * Controla el estado de movimiento del circulo, teniendo 4 estados de movimiento
             * 0 -> Movimiento Abajo Derecha
             * 1 -> Movimiento Arriba Derecha
             * 2 -> Movimineto Arriba Izquierda
             * 3 -> Movimiento Abajo Izquierda
             **/
            byte mState = (byte)mRandom.Next(0,3);
            Vector2f mPosition = mCircle.Position;
            float mSpeed = 0.1f;

            // Ejecutamos el sonido antes de iniciar le loo del juego
            mSound.Play();

            // -------------------------------------------------
            // Game loop

            while (mRenderWindow.IsOpen) {
                // 1.Read inputs
                // ...
                mRenderWindow.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) {
                    mRenderWindow.Close();
                }

                // 2. Game update
                // ...
                // Movemos el circulo acorde al estado
                switch (mState) {
                    case 0: // abajo derecha
                        mPosition.X += mSpeed;
                        mPosition.Y += mSpeed;
                        break;
                    case 1: // arriba derecha
                        mPosition.X += mSpeed;
                        mPosition.Y -= mSpeed;
                        break;
                    case 2: // arriba izquierda
                        mPosition.X -= mSpeed;
                        mPosition.Y -= mSpeed;
                        break;
                    case 3: // abajo izquierda
                        mPosition.X -= mSpeed;
                        mPosition.Y += mSpeed;
                        break;
                    default: break;
                }

                // Comprobamos y cambiamos estado si es necesario
                // Limite izquierdo
                if (mPosition.X < 0) {
                    if (mState == 3) mState = 0;
                    else if (mState == 2) mState = 1;
                }
                // Limite inferior
                if (mPosition.Y > mVideoHeight - (int)mCircle.Radius * 2.0f) {
                    if (mState == 0) mState = 1;
                    else if (mState == 3) mState = 2;
                }
                // Limite derecho
                if (mPosition.X > mVideoWidth - (int)mCircle.Radius * 2.0f) {
                    if (mState == 1) mState = 2;
                    else if (mState == 0) mState = 3;
                }
                // Limite Superior
                if (mPosition.Y < 0) { 
                    if (mState == 1) mState = 0;
                    else if (mState == 2) mState = 3;
                }

                // Actualizamos la posición del circulo y el texto 
                mTextDvD.Position = mCircle.Position = mPosition;

                // Actualizamos cordenadas del circulo
                mTextCords.DisplayedString = String.Format("X: {0}\nY: {1}", (int)mCircle.Position.X, (int)mCircle.Position.Y);

                // 3. Draw:
                //	3.1. Clear everything we drew during the last frame
                mRenderWindow.Clear();

                //	3.2. Call RenderWindow.Draw() on every Drawable object we want to show in the screen
                mRenderWindow.Draw(mCircle);
                mRenderWindow.Draw(mTextDvD);
                mRenderWindow.Draw(mTextCords);

                //	3.3. As a last step, we call RenderWindow.Display()
                mRenderWindow.Display();
            }

        }
    }
}
