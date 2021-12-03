using SFML.Graphics;
using System.Collections.Generic;

namespace TcGame
{

    /** Clase para actores que tienen una o varias animaciones */
    public class AnimatedActor : Actor
    {

        /** Lista contenedora de todas las animaciones */
        protected List<AnimatedSprite> mAnimations;

        /** Indicador de animación actual */
        protected int mAnimation;

        /** Constructor */
        public AnimatedActor() : base() 
        {
            mAnimations = new List<AnimatedSprite>();
        }

        /** Override del método Update de Actor */
        public override void Update(float dt)
        {
            base.Update(dt);

            if (mAnimations != null)
            {
                mAnimations[mAnimation].Update(dt);
            }
        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            if (mAnimations != null)
            {
                states.Transform *= Transform;
                target.Draw(mAnimations[mAnimation], states);
            }
        }

        /** Override del método GetLocalBounds de Actor */
        protected override FloatRect GetLocalBounds()
        {
            return (mAnimations != null) ? mAnimations[mAnimation].GetLocalBounds() : base.GetLocalBounds();
        }
    }
}