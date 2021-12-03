using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;

namespace TcGame
{
    public class Bullet : StaticActor
    {
        public static Vector2f Up = new Vector2f(0.0f, -1.0f);

        public Vector2f Forward = new Vector2f(0.0f, 0.0f);
        public float Speed = 500.0f;

        /** Controla si tenemos dirección asignada o no */
        private bool mHaveDirection = false;

        public Bullet()
        {
            Sprite = new Sprite(Resources.Texture("Textures/Bullet"));
            Center();
        }

        public override void Update(float dt)
        {

            if (!mHaveDirection) setDirection();

            Rotation = MathUtil.AngleWithSign(Forward, Up);
            Position += Forward * Speed * dt;

            CheckScreenLimits();
            CheckAsteroidCollision();
        }

        /**
         * Método para establecer dirección según la posicón del raton
         */
        private void setDirection()
        {
            mHaveDirection = true;
            Forward = (Engine.Get.MousePos - Position).Normal();
        }

        protected void CheckScreenLimits()
        {
            var Bounds = GetGlobalBounds();
            var ScreenSize = Engine.Get.Window.Size;

            if ((Bounds.Left > ScreenSize.X) ||
                (Bounds.Left + Bounds.Width < 0.0f) ||
                (Bounds.Top + Bounds.Width < 0.0f) ||
                (Bounds.Top > ScreenSize.Y))
            {
                Destroy();
            }
        }

        protected void CheckAsteroidCollision()
        {
            var asteroids = Engine.Get.Scene.GetAll<Asteroid>();
            foreach (Asteroid a in asteroids)
            {
                var toAsteroid = a.WorldPosition - WorldPosition;
                if (toAsteroid.Size() < 50.0f)
                {
                    if (a.readyToDie()) a.Destroy();
                    else a.getHit();
                    Destroy();
                }
            }
        }
    }
}

