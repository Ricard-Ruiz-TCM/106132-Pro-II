using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class Explosion : AnimatedActor
    {
        private float m_LifeTime = 4.0f;

        public Explosion()
        {
            Layer = ELayer.FX;

            AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/FX/Explosion"), 4, 1);
            AnimatedSprite.Loop = false;
            AnimatedSprite.FrameTime = 0.2f;
            m_LifeTime = AnimatedSprite.FrameTime * 3.0f;
            Center();
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            Position += new Vector2f(0.0f, 30.0f * dt);

            m_LifeTime -= dt;
            if (m_LifeTime < 0.0f)
            {
                Destroy();
            }
        }
    }
}

