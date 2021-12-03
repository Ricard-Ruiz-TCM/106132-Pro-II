using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TcGame
{
    public class Background : StaticActor
    {
        private Vector2f offset;
        private Texture texture;

        public float Speed = 30.0f;

        public Background()
        {
            Layer = ELayer.Background;

            texture = Resources.Texture("Textures/Background");
            texture.Repeated = true;

            rePositionBackground();
            
            Sprite = new Sprite(texture);
            /** Doblamos la texture.Size.Y en el tamaño de dibujado del sprite para poder hacer scroll vertical con la Texture.Repeated en true */
            Sprite.TextureRect = new IntRect(0, 0, (int)texture.Size.X, (int)texture.Size.Y * 2);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            
            offset -= new Vector2f(0.0f, dt * Speed / texture.Size.Y);
            Position -= offset * 2.0f;

            if (Position.Y >= -(texture.Size.Y - MyGame.Instance.Window.Size.Y))
            {
                rePositionBackground();
            }
        }

        /** Método para ajustar el fondo a la posición incial */
        private void rePositionBackground()
        {
            Position = new Vector2f(-1.0f, (-texture.Size.Y * 2.0f + MyGame.Instance.Window.Size.Y));
        }
    }
}
