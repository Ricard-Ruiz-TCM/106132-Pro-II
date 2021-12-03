using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class Asteroid : StaticActor
    {
        public float RotationSpeed = 20.0f;
        public float Speed = 200.0f;
        public Vector2f Forward = new Vector2f(1.0f, 0.0f);

        /** Controla si el asteroide puede ser dsetruidor */
        private bool mReadyToDie;

        /** Gestiona indirectametne el sprite del asteroide */
        private int mHP;

        /** Controla si esta vivo o no el objeto para ser objetivo de un cochete */
        private bool mAlive;

        public Asteroid()
        {
            mReadyToDie = false; mHP = 0; mAlive = true;
            Sprite = new Sprite(Resources.Texture("Textures/Asteroid00"));
            Center();
            OnDestroy += OnAsteroidDestroyed;
        }

        public override void Update(float dt)
        {
            Position += Forward * Speed * dt;
            Rotation += RotationSpeed * dt;
            MyGame.ResolveLimits(this);
        }

        void OnAsteroidDestroyed(Actor obj)
        {
            var hud = Engine.Get.Scene.GetFirst<HUD>();
            if (hud != null)
            {
                hud.Points += 100;
            }
            mAlive = false;
            var explosion = Engine.Get.Scene.Create<Explosion>();
            explosion.WorldPosition = WorldPosition;
        }

        /**
         * Método para recibir un balazo, actualiza el sprite y comprueba si peude ser destruidor
         */
        public void getHit()
        {
            mHP++;
            Sprite = new Sprite(Resources.Texture("Textures/Asteroid0" + mHP.ToString()));
            if (mHP == 2)
            {
                mReadyToDie = true;
            }
        }

        /**
         * Método setter de mReadyToDie
         * 
         * @return bool -> mReadyToDie
         */
        public bool readyToDie()
        {
            return mReadyToDie;
        }

        /**
         * Método setter de mAlive
         * 
         * @return bool -> mAlive
         */
        public bool isAlive()
        {
            return mAlive;
        }
    }
}
