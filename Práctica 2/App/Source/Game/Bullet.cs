using SFML.Graphics;
using SFML.System;

using System;

namespace TcGame
{
    public class Bullet : StaticActor
    {
        /**
         * BULLET_TYPE
         * 
         * BULLET_TANK  -> Proyectil lanzado por los objetos Tank
         * BULLET_PLANE -> Proyectil lanzado por el objeto Plane
         */
        public enum BULLET_TYPE
        {
            BULLET_TANK, BULLET_PLANE
        }

        private float mSpeed;
        private float mTimeAlive;
        private BULLET_TYPE mType;

        /** Constructor */
        public Bullet() : base() 
        {
            mSpeed = 6.0f; 
            mTimeAlive = 5.0f; 
            mCollisionBox = new FloatRect(0.0f, 0.0f, 0.0f, 0.0f);
        }

        /**
         * Método para iniciar un objeto Bullet según el objeto que lo instancie
         * 
         * @param type      -> Tipo de proyectil
         * @param position  -> Posición inicial de la bala
         */
        public void load(BULLET_TYPE type, Vector2f position)
        {
            if (type == BULLET_TYPE.BULLET_PLANE)
            {
                mSpeed *= -1;
                Sprite = new Sprite(Resources.Texture("Textures/Bullets/PlaneBullet"));
                
            } 
            else
            {
                Sprite = new Sprite(Resources.Texture("Textures/Bullets/TankBullet"));
            }

            mType = type;
            
            Position = new Vector2f(position.X - GetLocalBounds().Width / 2.0f, position.Y);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            
            Position = new Vector2f(Position.X, Position.Y + mSpeed);

            updateCollisionBox();

            mTimeAlive -= dt;
            if (mTimeAlive <= 0.0f)
            {
                Destroy();
            }

        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
        }

        /**
         * Método para actualizar la caja de colisión teniendo en cuenta GetGlobalBounds();
         */
        private void updateCollisionBox()
        {
            mCollisionBox = GetGlobalBounds();
            if (mType == BULLET_TYPE.BULLET_PLANE)
            {
                mCollisionBox.Top += mCollisionBox.Height / 3.0f; mCollisionBox.Left += mCollisionBox.Width / 2.5f; mCollisionBox.Width /= 6.0f; mCollisionBox.Height /= 4.0f;
            }
        }

        /**
         * Método para recuperar el tipo de balla
         * 
         * @return BULLET_TYPE  -> Variable mType
         */
        public BULLET_TYPE getType()
        {
            return (BULLET_TYPE)mType;
        }
    }
}
