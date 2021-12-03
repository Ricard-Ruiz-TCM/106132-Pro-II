using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Snake
{
    public class Snake : Actor
    {
        public Snake(float _x, float _y)
        {
            Texture = new Texture("Data/player.png");
            base.Center();
            Position = new Vector2f(_x, _y);
        }

        public void Update(float dt, Vector2f mousePos)
        {
            base.Update(dt);

            Vector2f dist = mousePos - Position;

            Forward = dist.Normal();
            Speed = dist.Size() * 2.0f;
            if (Forward.X > 0)
            {
                Rotation = (float)Math.Atan((Forward.Y) / Forward.X) * MathUtil.RAD2DEG;
            }
            else
            {
                Rotation = 180 + (float)Math.Atan((Forward.Y) / Forward.X) * MathUtil.RAD2DEG;
            }

        }

    }
}

