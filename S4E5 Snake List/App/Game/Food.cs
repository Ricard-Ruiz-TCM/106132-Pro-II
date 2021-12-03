using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Snake
{
    public class Food : AnimatedSprite
    {

        public Food() : base(new Texture("Data/food.png"), 4, 1)
        {
            FrameTime = 1.0f;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public void newPos(RenderTarget rt)
        {
            Random r = new Random();
            Position = new Vector2f(r.Next(16, (int)rt.Size.X - 16), r.Next(128, (int)rt.Size.Y - 16));
            FrameTime = r.Next(10, 100) / 100.0f;
        }


    }
}

