using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Snake
{
    public class Actor : Sprite
    {
        public Vector2f Forward = new Vector2f(1.0f, 0.0f);
        public float Speed = 0.0f;

        public void Center()
        {
            Origin = new Vector2f(GetLocalBounds().Width / 2.0f, GetLocalBounds().Height / 2.0f);
        }

        public virtual void Update(float dt)
        {
            Vector2f Velocity = Forward * Speed;
            Position += Velocity * dt;
        }
    }
}

