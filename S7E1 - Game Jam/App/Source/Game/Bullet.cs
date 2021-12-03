using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace TcGame
{
    public class Bullet : StaticActor
    {

        /** variable de tipo del arma*/
        private short mType;

        /** Varaible de contro lde velocidad */
        private float mSpeed;

        /** Variable para determinar si la bala existe */
        private bool mAlive;
        private float mTimeAlive;

        /** Daño pro defecto de la bala */
        private float mDamage;

        List<Sprite> mSprites;

        /** Constructor */
        public Bullet() : base()
        {
        }

        /**
         * Método para iniciar la bala
         * 
         * @param Vector2 pos               -> Posicion inicla de la bala
         * @param short  bulletType       -> Tipo de la bala
         * @param float dir                 -> Dirección de la bala (-1.0f (izquierda) || 1.0f (derecha))
         * @param float damage              -> Daño que tendrá la bala al impactar
         */
        public void init(Vector2f pos, short bulletType, float dir, float damage)
        {
            Position = new Vector2f(pos.X + (75.0f * dir), pos.Y - 20.0f);
            mType = (short)bulletType;
            Scale = new Vector2f(Scale.X * dir, Scale.Y);
            mSpeed = 500.0f * dir;
            mDamage = damage;
            mTimeAlive = 3.0f;
            mAlive = true;

            mSprites = new List<Sprite>();

            setSprites();

            setCollision();
            updateCollision();
        }

        /** Override del método Update de Actor */
        public override void Update(float dt)
        {
            base.Update(dt);

            mTimeAlive -= dt;

            if (mTimeAlive <= 0.0f)
            {
                mAlive = false; Destroy();
            }

            if (mAlive) checkCollision();

            // Actualizamos posicion
            Position += new Vector2f(mSpeed, 0.0f) * dt;
        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        { 
            if ((mType != (short)Weapon.WEAPON_TYPE.FIST) && (mType != (short)Weapon.WEAPON_TYPE.SWORD) && (mAlive)) base.Draw(target, states);
        }

        private void setSprites()
        {
            mSprites.Add(new Sprite(Resources.Texture("Elements/Bullets/fist")));
            mSprites.Add(new Sprite(Resources.Texture("Elements/Bullets/sword")));
            mSprites.Add(new Sprite(Resources.Texture("Elements/Bullets/flecha")));
            mSprites.Add(new Sprite(Resources.Texture("Elements/Bullets/bala")));

            Sprite = mSprites[mType];
        }

        /**
         * Método para establecer las cajas de collisiones
         */
        private void setCollision()
        {
            mCollisionBox.Add(GetLocalBounds());
        }

        /**
         * Método para comprobar las colisiones con todos los objetos 
         */
        private void checkCollision()
        {
            updateCollision();
            List<PlayerEntity> players = MyGame.Instance.getScene().GetAll<PlayerEntity>();
            for (int i = 0; i < players.Count; i++)
            {
                if (getCollisionBox().Intersects(players[i].getCollisionBox()))
                {
                    if (players[i].isReady()){
                        players[i].damageMe(mDamage);
                        if (players.Count > 1) players[(i == 0 ? 1 : 0)].upgrade();
                        mAlive = false; Destroy();
                    }
                }
            }
            Background bg = MyGame.Instance.getScene().GetFirst<Background>();

            for (int i = 0; i < bg.getCollisionAmount(); i++)
            {
                if (getCollisionBox().Intersects(bg.getCollisionBox(i)))
                {
                    mAlive = false; Destroy();
                }
            }

        }
    }
}