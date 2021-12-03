using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using SFML.Audio;

namespace TcGame
{
    /// <summary>
    /// Base class for the entities of the game
    /// </summary>
    public abstract class Actor : Transformable, Drawable
    {
        /// <summary>
        /// Draw Order
        /// </summary>
        public enum ELayer
        {
            Background,
            Back,
            Middle,
            Front,
            HUD
        }

        /// <summary>
        /// Order in that this actor will be drawn
        /// </summary>
        public ELayer Layer { get; set; }

        public FloatRect mCollisionBox;

        /// <summary>
        /// This action is launched when this actor is destroyed
        /// </summary>
        public Action<Actor> OnDestroy;

        protected Actor()
        {
            Layer = ELayer.Back;
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {

        }

        public void Center()
        {
            Origin = new Vector2f(GetLocalBounds().Width, GetLocalBounds().Height) / 2.0f;
        }

        public virtual FloatRect GetLocalBounds()
        {
            return new FloatRect();
        }

        public FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(GetLocalBounds());
        }

        /// <summary>
        /// The actor will be removed from scene
        /// </summary>
        public void Destroy()
        {
            
            MyGame.Instance.Scene.Destroy(this);
        }

        /**
         * Método para utilizar una caja de collisón diferente a la proporcionada por GetGlobalBounds()
         * 
         * @return FloatRect    -> mCollisionBox
         */
        public FloatRect getCollisionBox()
        {
            return mCollisionBox;
        }
    }
}
