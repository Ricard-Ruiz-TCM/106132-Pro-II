using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace TcGame
{
    /** Clase Base para las entidades del juego */
    public abstract class Actor : Transformable, Drawable
    {
        /** 
         * ELayer
         * 
         * Enum para indicar el orden de pintado
         */
        public enum ELayer
        {
            Background,
            Back,
            Middle,
            Front,
            HUD
        }

        /** Referente al layer de pintado */
        protected ELayer mLayer;

        /** Lista contenedora de todas las collisiones del objeto segun estado */
        protected List<FloatRect> mCollisionBox;

        /** definición del método CallBack que ejecutara el objeto al destruirse */
        public Action<Actor> OnDestroy;

        /** Constructor */
        protected Actor() 
        {
            mCollisionBox = new List<FloatRect>();
        }

        /**
         * Método virtual para hacer el "update" del objeto
         * 
         * @param dt -> delta time 
         */
        public virtual void Update(float dt)
        {
        }

        /**
         * Método virtual para hacer el "render" del objeto
         * 
         * @param target -> ?
         * @param states -> ?
         */
        public virtual void Draw(RenderTarget target, RenderStates states) { }

        /**
         * Método para dibujar todas las cajas de colission
         * 
         * @param target -> ?
         * @param states -> ?
         * @param color -> Color
         */
        protected void drawCollisions(RenderTarget target, RenderStates states, Color color)
        {
            for(int i = 0; i < mCollisionBox.Count; i++)
            {
                MyGame.Instance.getDM().Box(mCollisionBox[i], color);
            }
        }

        /**
         * Método para centrar el origin del objeto al centro
         */
        protected void Center()
        {
            Origin = new Vector2f(GetLocalBounds().Width, GetLocalBounds().Height) / 2.0f;
        }

        /**
         * Método para recuperar GetLocalBounds
         * 
         * @return FloatRect    -> Nuevo objeto FloatRect
         */
        protected virtual FloatRect GetLocalBounds()
        {
            return new FloatRect();
        }

        /**
         * Método que devuelve la posicion y dimensiones del objeto
         * 
         * @return FloatRect    -> Transform de GetLocalBounds()
         */
        protected FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(GetLocalBounds());
        }

        /**
         * Método "destructor" del objeto
         */
        public void Destroy()
        {
            MyGame.Instance.getScene().Destroy(this);
        }

        /**
         * Método para utilizar una caja de collisón diferente a la proporcionada por GetGlobalBounds()
         * 
         * @param pos           -> posicion en la lista de caja de colisiones (suele estar relacionado con el estado)
         * 
         * @return FloatRect    -> mCollisionBox[state]
         */
        public virtual FloatRect getCollisionBox(int pos = 0)
        {
            return mCollisionBox[pos];
        }

        /**
         * Método para devolver la cantidad de cajas de colsiion de un objeto
         * 
         * @return int  -> mCollisionBox.Count
         */
        public int getCollisionAmount()
        {
            return mCollisionBox.Count;
        }

        /**
         * Método para actualizar las collisiones según el GetGlobalBounds()
         */
        protected void updateCollision()
        {
            for (int i = 0; i < mCollisionBox.Count; i++)
            {
                mCollisionBox[i] = GetGlobalBounds();
            }
        }

        /**
         * Getter de mLayer
         * 
         * @return ELayer   -> mLayer
         */
        public ELayer getLayer()
        {
            return mLayer;
        }
    }
}
