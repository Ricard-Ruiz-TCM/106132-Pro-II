using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class Fish : AnimatedActor
    {
        private Vector2f Forward = new Vector2f(0.0f, -1.0f);
        private float Speed = 20.0f;
        private Vector2f dest = new Vector2f(Engine.Get.Window.Size.X / 4.0f * 3.0f, Engine.Get.Window.Size.Y / 3.0f);
        private float d = 0.0f;
        private Vector2f stPos;

        float totalTime = 0.0f;

        public Fish()
        {
            Texture t = new Texture("Data/Textures/fish.png");
            AnimatedSprite = new AnimatedSprite(t, 2, 1);
            AnimatedSprite.Loop = true;
            Position = new Vector2f(Engine.Get.Window.Size.X / 4.0f * 1.0f, Engine.Get.Window.Size.Y / 3.0f);
            stPos = new Vector2f(Engine.Get.Window.Size.X / 4.0f * 1.0f, Engine.Get.Window.Size.Y / 3.0f);
            Engine.Get.Window.KeyPressed += HandleKeyPressed;
        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {

        }

        public override void Draw(RenderTarget rt, RenderStates rs)
        {
            AnimatedSprite.Draw(rt, rs);
        }

        public override void Update(float dt)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                stPos = new Vector2f(Position.X, Position.Y);
                dest = new Vector2f((Engine.Get.Window.Size.X / 4.0f) * 1.0f, Engine.Get.Window.Size.Y / 3.0f);
                Forward = new Vector2f(-1.0f, 0.0f);
                d = 0.0f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                stPos = new Vector2f(Position.X, Position.Y);
                dest = new Vector2f(Engine.Get.Window.Size.X / 4.0f * 3.0f, Engine.Get.Window.Size.Y / 3.0f);
                Forward = new Vector2f(1.0f, 0.0f);
                d = 0.0f;
            }

            //===== AQUI ======== en Función de la velocidad ======
            //d += 0.1f * dt;

            //===== AQUI ======== en Función de la velocidad ======
            //Vector2f recorrido = dest - Position;
            //d += Speed * dt / recorrido.Size();

            //===== AQUI ======== en Función del Tiempo la velocidad ======
            //d = (float)Math.Sin(Engine.Get.Time * 0.9f) * 0.5f + 0.5f;

            //===== AQUI ======== 5 seconds with linear velocity ======
            //d += dt / 5;

            //===== AQUI ======== 5 seconds with Sin ======
            d = (float)Math.Sin(Engine.Get.Time * 0.5f) * 0.5f + 0.5f;

            Position = MathUtil.Lerp(stPos, dest, d);

            MyGame.ResolveLimits(this);
            CheckCollision();
        }

        private void CheckCollision()
        {

        }

        void OnShipDestroy(Actor obj)
        {
            Engine.Get.Window.KeyPressed -= HandleKeyPressed;
        }


    }
}

