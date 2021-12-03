using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace TcGame
{
    public class Weapon : StaticActor
    {

        /**
         * WEAPON_TYPE
         * 
         * Define el tipo de arma
         */
        public enum WEAPON_TYPE
        {
            FIST, SWORD, CROSSBOW, REVOLVER
        }

        /** Daño del arma */
        private float mDamage;

        /** Determian si esta vivio el objeto */
        private bool mAlive;

        /** TIempo que ha estado vivo el objeto */
        private float mTotalTime;

        /** Tipo de arma */
        private short mType;

        private List<Texture> mWeaponTexures;

        private float mBasePosition;

        /** Constructor */
        public Weapon() : base()
        {
        }

        public void init(float y)
        {
            mBasePosition = y;

            mLayer = ELayer.Front;

            mAlive = true;
            mDamage = 5.0f;

            mWeaponTexures = new List<Texture>();

            createWeapon();

            setAnimations();

            setCollision();
        }

        /** 
         * Método para cargar unos valores básicos de arma
         */
        public void loadBasicWeapon()
        {
            mType = (short)WEAPON_TYPE.FIST;
        }

        /** Override del método Update de Actor */
        public override void Update(float dt)
        {
            base.Update(dt);

            mTotalTime += dt;
            Position = new Vector2f(Position.X, mBasePosition + (float)Math.Sin(mTotalTime * 4) * 15.0f);

            if (mAlive) checkCollision();
        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if ((mType != (short)WEAPON_TYPE.FIST) && (mAlive))
            {
                base.Draw(target, states);
                //drawCollisions(target, states, Color.Yellow);
            }
        }

        /** 
         * Método para crear el tipo de arma de forma aleatoria
         */
        private void createWeapon()
        {
            Random rng = new Random();

            mType = (short)rng.Next(1, 4);

            switch (mType)
            {
                case 1: // Cuchillo
                    mDamage = 15.0f;
                    break;
                case 2: // Ballesta
                    mDamage = 20.0f;
                    break;
                case 3: // Revolver
                    mDamage = 25.0f;
                    break;
                default:
                    loadBasicWeapon();
                    break;
            }
        }

        /** 
         * Método para establecer las animaciones
         */
        private void setAnimations()
        {
            mWeaponTexures.Add(Resources.Texture("Elements/Weapons/fist"));
            mWeaponTexures.Add(Resources.Texture("Elements/Weapons/espada"));
            mWeaponTexures.Add(Resources.Texture("Elements/Weapons/ballesta"));
            mWeaponTexures.Add(Resources.Texture("Elements/Weapons/pistola"));

            Sprite = new Sprite(mWeaponTexures[mType]);

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
                    if (players[i].pickUpWeapon(this))
                    {
                        mAlive = false; Destroy();
                    }
                }
            }
        }

        /**
         * Método get para mDamage
         * 
         * @return float    -> mDamage
         */
        public float getDamage()
        {
            return mDamage;
        }

        /**
        * Método para recupear el tipo de la bala
        * 
        * @return short   -> mType
        */
        public short getType()
        {
            return mType;
        }

        /**
         * Método para ocmprobar si esta viva
         * 
         * @return bool -> mAlive
         */
        public bool isAlive()
        {
            return mAlive;
        }

    }
}