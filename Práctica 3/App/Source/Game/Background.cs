using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class Background : StaticActor
    {
        private Texture texture;

        public Background()
        {
            var screenSize = Engine.Get.Window.Size;

            texture = Resources.Texture("Textures/Background");
            Sprite = new Sprite(texture);
            Sprite.TextureRect = new IntRect(0, 0, (int)screenSize.X, (int)screenSize.Y);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Texture = texture;
            base.Draw(target, states);
        }
    }
}

