using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace TcGame
{

    /** Clase para gestionar el fondo y los elementos del mismo */
    public class Background : StaticActor
    {

        /**
         * COLL_TYPE
         * 
         * Tipo de colision, si es el suelo, una caja o la parte superior o inferior de una platafomra
         */
        public enum COLL_TYPE
        {
            GROUND, LEFT_WALL, RIGHT_WALL, LOWER_PLAT
        }

        /** Contenedor para la configuración de cada collision box */
        private List<COLL_TYPE> mCollisionType;

        /** controla si hay armas para coger del escenario */
        private Weapon mWeaponSpot1, mWeaponSpot2;

        /** Tiempo para volver a iniciar una weapon */
        private float mNextWP1, mNextWP2;

        /** Constructor */
        public Background()
        {
            mLayer = ELayer.Background;

            Sprite = new Sprite(Resources.Texture("Textures/Background"));

            mCollisionType = new List<COLL_TYPE>();

            setCollisions();

            // Iniciamos dos armas aleatorias
            spawnWeapon(1); spawnWeapon(2);

            // Creamos los 3 tps
            Teleport tp0 = MyGame.Instance.getScene().Create<Teleport>(new Vector2f(608.0f, 473.0f));
            tp0.init(0, new Vector2f(65.0f, 73.0f), new Vector2f(1210.0f, 60.0f));

            Teleport tp1 = MyGame.Instance.getScene().Create<Teleport>(new Vector2f(30.0f, 98.0f));
            tp1.init(1, new Vector2f(640.0f, 448.0f), new Vector2f(0.0f, 0.0f));

            Teleport tp2 = MyGame.Instance.getScene().Create<Teleport>(new Vector2f(1175.0f, 83.0f));
            tp2.init(2, new Vector2f(640.0f, 448.0f), new Vector2f(0.0f, 0.0f));

        }

        /** Override del método Update de Actor */
        public override void Update(float dt)
        {
            base.Update(dt);

            // Update del spawn de las 2 armas
            if (!mWeaponSpot1.isAlive())
            {
                mNextWP1 -= dt;
                if (mNextWP1 <= 0.0f) spawnWeapon(1);
            }

            if (!mWeaponSpot2.isAlive())
            {
                mNextWP2 -= dt;
                if (mNextWP2 <= 0.0f) spawnWeapon(2);
            }

        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            /*for (int i = 0; i < mCollisionBox.Count; i++)
            {
                Color clr = Color.Green;

                if (mCollisionType[i] == COLL_TYPE.GROUND) clr = Color.Cyan;
                if (mCollisionType[i] == COLL_TYPE.LEFT_WALL) clr = Color.Magenta;
                if (mCollisionType[i] == COLL_TYPE.RIGHT_WALL) clr = Color.Red;
                if (mCollisionType[i] == COLL_TYPE.LOWER_PLAT) clr = Color.White;

                MyGame.Instance.getDM().Box(mCollisionBox[i], clr);
            }*/
        }

        /**
        * Método para reiniciar las weapons
        * 
        * @param int i -> si es la 1 o la 2
        */
        public void spawnWeapon(int i)
        {
            if (i == 1)
            {
                mWeaponSpot1 = MyGame.Instance.getScene().Create<Weapon>(new Vector2f(100.0f, 40.0f));
                mWeaponSpot1.init(mWeaponSpot1.Position.Y);
                mNextWP1 = 5.0f;
            }
            else
            {
                mWeaponSpot2 = MyGame.Instance.getScene().Create<Weapon>(new Vector2f(1100.0f, 35.0f));
                mWeaponSpot2.init(mWeaponSpot2.Position.Y);
                mNextWP2 = 5.0f;
            }
        }

        /** Establece las colisiones del fondo */
        private void setCollisions()
        {
            mCollisionBox.Add(new FloatRect(187, 123, 15, 104));
            mCollisionType.Add(COLL_TYPE.LEFT_WALL);

            mCollisionBox.Add(new FloatRect(180, 422, 15, 98));
            mCollisionType.Add(COLL_TYPE.LEFT_WALL);

            mCollisionBox.Add(new FloatRect(243, 512, 15, 148));
            mCollisionType.Add(COLL_TYPE.LEFT_WALL);

            mCollisionBox.Add(new FloatRect(679, 496, 15, 98));
            mCollisionType.Add(COLL_TYPE.LEFT_WALL);

            mCollisionBox.Add(new FloatRect(737, 592, 15, 68));
            mCollisionType.Add(COLL_TYPE.LEFT_WALL);

            mCollisionBox.Add(new FloatRect(497, 609, 15, 49));
            mCollisionType.Add(COLL_TYPE.RIGHT_WALL);

            mCollisionBox.Add(new FloatRect(582, 499, 15, 113));
            mCollisionType.Add(COLL_TYPE.RIGHT_WALL);

            mCollisionBox.Add(new FloatRect(1018, 511, 15, 148));
            mCollisionType.Add(COLL_TYPE.RIGHT_WALL);

            mCollisionBox.Add(new FloatRect(1080, 419, 15, 78));
            mCollisionType.Add(COLL_TYPE.RIGHT_WALL);

            mCollisionBox.Add(new FloatRect(1073, 109, 15, 198));
            mCollisionType.Add(COLL_TYPE.RIGHT_WALL);

            mCollisionBox.Add(new FloatRect(0, 220, 200, 15));
            mCollisionType.Add(COLL_TYPE.LOWER_PLAT);

            mCollisionBox.Add(new FloatRect(1075, 295, 205, 15));
            mCollisionType.Add(COLL_TYPE.LOWER_PLAT);

            mCollisionBox.Add(new FloatRect(200, 160, 210, 15));
            mCollisionType.Add(COLL_TYPE.LOWER_PLAT);

            mCollisionBox.Add(new FloatRect(265, 645, 750, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);
            
            mCollisionBox.Add(new FloatRect(0, 412, 190, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(190, 502, 65, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(500, 600, 80, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(585, 490, 105, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(1020, 502, 65, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(1085, 412, 190, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(1085, 410, 190, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(690, 582, 60, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(0, 113, 200, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(200, 147, 210, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

            mCollisionBox.Add(new FloatRect(1075, 100, 205, 15));
            mCollisionType.Add(COLL_TYPE.GROUND);

        }

        /**
         * Método que devuelve el tipo de colision
         * 
         * @param int col   -> Posición en la mCollisionBox que hace referencia al tipo
         * 
         * @return COLL_TYPE    -> mCollisionType[col]
         */
        public COLL_TYPE getCollisionType(int col)
        {
            return mCollisionType[col];
        }

    }
}