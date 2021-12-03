using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;

namespace TcGame
{
    public class Actor : Transformable, Drawable
    {
        public Action<Actor> OnDestroy;

        private Actor newParent;
        private Actor parent;
        private List<Actor> Children = new List<Actor>();

        public enum ELayer
        {
            Background,
            Back,
            Middle,
            Front,
            FX,
            HUD
        }

        public ELayer Layer { get; set; }

        public Transform WorldTransform
        {
            get
            {
                return (Parent != null) ? Transform * Parent.WorldTransform : Transform;
            }
        }

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

        public Actor Parent
        {
            get { return parent; }
            set { newParent = value; }
        }

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

        protected Actor()
        {
            Layer = ELayer.Back;
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
    }
}
