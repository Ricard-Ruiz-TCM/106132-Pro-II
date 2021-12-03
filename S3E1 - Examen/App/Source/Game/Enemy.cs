using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

namespace TcGame
{
    public class Enemy : AnimatedActor
    {

        private static float MIN_SCALE = 0.7f;
        private static float MAX_SCALE = 0.8f;
        private static float SCALE_SPEED = 0.3f;

        private int m_ScaleDirection = 1;
        private Vector2f m_Forward;
        private float m_Speed;

        private Actor m_Target = null;

        public Enemy()
        {
            Layer = ELayer.Front;

            Texture t = Resources.Texture("Textures/Enemies/SpaceInvader_animated");
            AnimatedSprite = new AnimatedSprite(t, 2, 1);
            AnimatedSprite.Loop = true;
            AnimatedSprite.FrameTime = (Engine.Get.random.Next(0, 40) / 100f) + 0.1f;
            Scale = new Vector2f(MIN_SCALE, MIN_SCALE);

            Center();

            m_Forward = new Vector2f(0.0f, 1.0f);
            m_Speed = 100.0f;

            m_Target = Engine.Get.Scene.GetFirst<Tank>();
            Debug.Assert(m_Target != null);
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            m_Forward = (m_Target.Position - Position).Normal();

            Move(_dt);
            UpdateScale(_dt);
        }

        private void Move(float _dt)
        {
            Position += m_Forward * m_Speed * _dt;
        }

        private void UpdateScale(float _dt)
        {
            float scale = Scale.X + _dt * m_ScaleDirection * SCALE_SPEED;
            Scale = new Vector2f(scale, scale);
            if (scale > MAX_SCALE || scale < MIN_SCALE)
            {
                m_ScaleDirection *= -1;
            }
        }


    }
}

