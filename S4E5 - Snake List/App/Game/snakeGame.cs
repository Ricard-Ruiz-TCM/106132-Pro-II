using System;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;
using System.Collections.Generic;
using SFML.System;

namespace Snake
{
    public class snakeGame : Game
    {
        private RenderWindow window;
        private Snake sn;
        private HUD h;
        private Vector2f mousePos;

        // Variable para contar 1000 ms (1 sgundo)
        public float mMS;
        // Variable para contar hasta 5 segundos
        public float mFiveSeconds;

        // Lista de comida para la serpiente
        List<Food> mFood;

        // Variable de sonido para la acción de comer
        Sound mEatSound;

        // Lista que gestiona el cuerpo de la serpiente
        List<SnakeBody> mSnakeBody;

        public void Init()
        {
            VideoMode videoMode = new VideoMode(640, 480);
            window = new RenderWindow(videoMode, "snakeGame");
            h = new HUD();

            // Cremos la lista y rellenamos con 25 elementos
            mFood = new List<Food>();
            for (int i = 0; i < 25; i++) addFood();

            // Cargamos el sonido y lo dejamos listo
            mEatSound = new Sound(new SoundBuffer("Data/eat.wav")); mEatSound.Volume *= 0.25f;

            sn = new Snake(window.Size.X / 2.0f, window.Size.Y / 2.0f);

            // Creamos la llista y rellenamos con 1 elemento
            mSnakeBody = new List<SnakeBody>();
            mSnakeBody.Add(new SnakeBody(sn));

            window.MouseMoved += HandleMouseMove;
        }

        public void HandleMouseMove(object sender, MouseMoveEventArgs e)
        {
            mousePos = new Vector2f(e.X, e.Y);
        }

        public void DeInit()
        {
            window.Dispose();
        }

        public void Update(float dt)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                window.Close();
            }
            window.DispatchEvents();

            // Actualizmoas cada elemento del mSnakeBody
            // se actualiza al rever por que nos interesa ir cogiendo la nueva posición antes de sobre escribirla
            // el último elemento es la cabeza de la serpiente
            for (int i = mSnakeBody.Count - 1; i >= 0; i--)
            {
                mSnakeBody[i].Update();
            }

            sn.Update(dt, mousePos);

            // Actualizamos cada elementos de la lista mFoods
            for (int i = 0; i < mFood.Count; i++) 
            {
                mFood[i].Update(dt);
                // Comprobamos si ha colisionado con la snake
                if (sn.GetGlobalBounds().Intersects(mFood[i].GetGlobalBounds()))
                {
                    eatFood(i); i--;
                }
            }

            h.Update(dt);

            // Summos el dt a los timers y hacemos las comprobaciones
            mMS += dt; mFiveSeconds += dt;
            if (mMS >= 1.0f)
            {
                addFood(); mMS = 0.0f;
                if (mFiveSeconds >= 4.0f) { h.newPosAnimation(); }
                if (mFiveSeconds >= 5.0f) { newFoodPos(); mFiveSeconds = 0.0f; }
            }

        }

        public void Draw()
        {
            window.Clear();

            // Pintamos cada elemento de mFood
            for (int i = 0; i < mFood.Count; i++)
            {
                window.Draw(mFood[i]);
            }

            // Pintamos cada elemento de mSnakeBody
            for (int i = mSnakeBody.Count - 1; i >= 0; i--)
            {
                window.Draw(mSnakeBody[i]);
            }

            window.Draw(h);
            window.Draw(sn);
            window.Display();
        }

        public bool IsAlive()
        {
            return window.IsOpen;
        }

        public void HandleKeyPressed(object sender, KeyEventArgs ee) { }

        public void MousePressed(object sender, MouseButtonEventArgs ee) { }

        // eatFood
        // ********
        // @param int i Posición de la lista mFood que tenemos que gestionar
        // Método para añadir puntuación y elminar el elemento de la lista mFoods
        // Reproducirá el sonido de comida
        // Añadirá un nuevo elemento al cuerpo de la serpiente
        public void eatFood(int i)
        {
            mEatSound.Stop();
            mEatSound.Play();
            h.ScoreAdd();
            mFood.RemoveAt(i);
            mSnakeBody.Add(new SnakeBody(mSnakeBody[mSnakeBody.Count - 1]));
        }

        // addFood
        // ********
        // Método para añadir un elemento de comida a la lista mFoods y dale una posición aleatoria
        public void addFood()
        {
            mFood.Add(new Food()); mFood[mFood.Count - 1].newPos(window);
        }

        // newFoodPos
        // ***********
        // Método para cambiar la posición de todos los elementos de la lista mFoods
        // Notificará al hud que se realizará está mecánica
        public void newFoodPos()
        {
            for(int i = 0; i < mFood.Count; i++)
            {
                mFood[i].newPos(window);
            }
        }
    }
}

