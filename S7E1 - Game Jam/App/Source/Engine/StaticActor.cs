using SFML.System;
using SFML.Graphics;


namespace TcGame
{
    /** Clase para actores que son estaticos (no tienen animaciones) */
    public class StaticActor : Actor
    {

        /** Sprite a dibujar del objeto statico*/
        protected Sprite Sprite;

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            if (Sprite != null)
            {
                states.Transform *= Transform;
                target.Draw(Sprite, states);
            }
        }

        /** Override del método GetLocalBounds de Actor */
        protected override FloatRect GetLocalBounds()
        {
            return (Sprite != null) ? Sprite.GetLocalBounds() : new FloatRect();
        }
    }
}