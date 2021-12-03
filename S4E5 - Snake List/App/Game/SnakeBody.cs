using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Snake
{
    public class SnakeBody : Actor
    {
        // Actor de referencia para las nuvas posiciones
        private Actor mFather;

        public SnakeBody(Actor father)
        {
            Texture = new Texture("Data/body.png"); base.Center();
            mFather = father;
        }

        public void Update()
        {
            // Actualizamos nuestra posición y rotación con la de nustro padre
            Position = mFather.Position;
            Rotation = mFather.Rotation;
        }

    }
}