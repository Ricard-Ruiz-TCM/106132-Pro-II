using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System;

namespace TcGame
{
    public class Rocket : Bullet
    {

        /** Velocidad de rotación */
        private float mRotation = 90.0f;

        /** Target del Asteroid */
        private Asteroid mTarget;

        /** Controla si tenemos objetivo o no */
        private bool mHaveTarget;

        public Rocket()
        {
            Speed = 300.0f;
            mHaveTarget = false;

            Sprite = new Sprite(Resources.Texture("Textures/Rocket"));
            Center();

            var flame = Engine.Get.Scene.Create<Flame>(this);
            flame.Position = Origin + new Vector2f(0.0f, 40.0f);

        }

        public override void Update(float dt)
        {

            Rotation = MathUtil.AngleWithSign(Forward, Up);
            Position += Forward * Speed * dt;

            if (mHaveTarget) moveToTarget(dt);
            else findTarget();

            CheckScreenLimits();
            CheckAsteroidCollision();
        }

        /**
         * Método para actualizar posición hacia el objetivo o declara que no hay objetivo
         * 
         * @param float dt  -> deltaTime
         */
        private void moveToTarget(float dt)
        {
            if (mTarget != null)
            {
                if (mTarget.isAlive())
                {
                    Vector2f dif = (mTarget.Position - Position).Normal();
                    if (MathUtil.AngleWithSign(Forward, dif) < 0.0f) Rotation += mRotation * dt;
                    if (MathUtil.AngleWithSign(Forward, dif) > 0.0f) Rotation -= mRotation * dt;
                    Forward = Up.Rotate(Rotation);
                }
                else
                {
                    mHaveTarget = false;
                }
            }
            else
            {
                mHaveTarget = false;
            }
        }

        /**
         * Método para encotnrar un objetivo
         * Recupear todos los Asteroids y filtra por distancia, encontrando el más cercano
         */
        private void findTarget()
        {
            List<Asteroid> asteroids = Engine.Get.Scene.GetAll<Asteroid>();
            float distance = 10000.0f;
            foreach (Asteroid a in asteroids)
            {
                if ((a.distance(Position)) < distance)
                {
                    mTarget = a;
                    distance = a.distance(Position);
                    mHaveTarget = true;
                }
            }
        }
    }
}