using SFML.Graphics;
using SFML.System;

namespace TcGame
{
    public class Life : StaticActor
    {
        public Life()
        {
            Sprite = new Sprite(Resources.Texture("Textures/Life"));
            Center();
        }
    }
}

