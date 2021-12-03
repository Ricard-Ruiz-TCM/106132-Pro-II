using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;

namespace TcGame
{
    /// <summary>
    /// Base class for the entities of the game
    /// </summary>
    public class Actor : Transformable, Drawable
    {
        /// <summary>
        /// Event that is invoked when this actor is destroyed
        /// </summary>
        public Action<Actor> OnDestroy;

        private Actor newParent;
        private Actor parent;
        private List<Actor> Children = new List<Actor>();

        /// <summary>
        /// Returns the world transformation
        /// It's calculated when it's called
        /// </summary>
        public Transform WorldTransform
        {
            get
            {
                return (Parent != null) ? Transform * Parent.WorldTransform : Transform;
            }
        }

        /// <summary>
        /// Actor osition in world coordinates
        /// </summary>
        public Vector2f WorldPosition
        {
            get
            {
                return WorldTransform * Origin;
            }
            set
            {
                Position = (Parent != null) ? Parent.WorldTransform.GetInverse().TransformPoint(value) : value;
            }
        }

        /// <summary>
        /// Gets or sets the parent of this node in the scene graph
        /// </summary>
        public Actor Parent
        {
            get { return parent; }
            set { newParent = value; }
        }

        /// <summary>
        /// Resolve the parent relationship
        /// </summary>
        public void FixParent()
        {
            if (newParent != parent)
            {
                Vector2f prevWorldPosition = WorldPosition;

                if (parent != null)
                {
                    parent.Children.Remove(this);
                }

                if (newParent != null)
                {
                    newParent.Children.Add(this);
                }

                parent = newParent;

                WorldPosition = prevWorldPosition;
            }
        }

        /// <summary>
        /// An Actor must be created with Engine.Get.Scene.Create
        /// </summary>
        protected Actor()
        {

        }

        public virtual void UpdateRecursive(float dt)
        {
            Update(dt);
            Children.ForEach(x => x.UpdateRecursive(dt));
        }

        public virtual void DrawRecursive(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            Draw(target, states);
            Children.ForEach(x => x.DrawRecursive(target, states));
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
            return WorldTransform.TransformRect(GetLocalBounds());
        }

        public void Destroy()
        {
            Engine.Get.Scene.Destroy(this);
        }

        public void PlaySound(string soundName, float volume = 100.0f)
        {
            Engine.Get.SoundMgr.PlaySound(soundName, volume);
        }

        /**
         * Método para calcualr distancia entre el actor y un Vector2f
         * 
         * @param Vectro2f position -> Posición del otro elemento
         * 
         * @return float    -> Distancia
         */
        public float distance(Vector2f position)
        {
            return (float)Math.Sqrt(Math.Pow((Position.X - position.X), 2) + Math.Pow((Position.Y - position.Y), 2));
        }
    }
}
