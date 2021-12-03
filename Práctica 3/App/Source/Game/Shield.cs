using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System;

namespace TcGame
{
    public class Shield : AnimatedActor
    {

        /** Controla si esta activo o no */
        private bool mActive;

        /** Control de los tiempos del escudo */
        private float mTimeActive = 5.0f;
        private const float mBaseTimeActive = 5.0f;
        private const float mGrowShrinkTime = 0.2f;

        public Shield()
        {
            AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/Shield"), 3, 2);
            Center();
        }

        public override void Update(float dt)
        {
            if (mActive)
            {
                if (mTimeActive >= (mBaseTimeActive - mGrowShrinkTime))
                {
                    Scale += new Vector2f(0.08f, 0.08f);
                }
                if (mTimeActive <= mGrowShrinkTime)
                {
                    Scale -= new Vector2f(0.08f, 0.08f);
                }
                if (mTimeActive <= 0.0f)
                {
                    mActive = false;
                }
                mTimeActive -= dt;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (mActive) base.Draw(target, states);
        }

        /**
         * Método getter de mActive
         * 
         * @return bool -> mActive
         */
        public bool isActive()
        {
            return mActive;
        }

        /**
         * Método para activar el escudo
         */
        public void active()
        {
            mActive = true;
            Scale = new Vector2f(0.0f, 0.0f);
            mTimeActive = mBaseTimeActive;
        }
    }
}