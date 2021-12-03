using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace TcGame
{


    public class Teleport : StaticActor
    {

        // 0 -> centro
        // 1 -> izq
        // 2 -> derecha
        public int side;

        public Vector2f destination;
        public Vector2f destination2;

        private Sprite mHalo;

        private int mHaloHeight;

        // tiempo sobre el tp para activarlo
        float durationTp = 1.5f;

        float durationSwap = 3.0f;

        bool colision;

        private Sprite side1;
        private Sprite side2;

        /** Constructor */
        public Teleport() : base()
        {
            mLayer = ELayer.Front;
            colision = false;
            Scale = new Vector2f(0.5f, 0.5f);

            mHalo = new Sprite(Resources.Texture("Elements/halo_tp"));

            mHalo.Scale = new Vector2f(1.0f, -1.0f);
        }

        public void init(int sde, Vector2f destin1, Vector2f destin2)
        {
            side = sde;
            destination = destin1;
            destination2 = destin2;
            mHalo.Position = new Vector2f(Position.X, Position.Y + 10.0f);

            mHaloHeight = mHalo.TextureRect.Height;
            mHalo.TextureRect = new IntRect(mHalo.TextureRect.Left, mHalo.TextureRect.Height, mHalo.TextureRect.Width, 0);

            setSprite();
            setCollision();
        }

        /** Override del método Update de Actor */
        public override void Update(float dt)
        {
            base.Update(dt);

            if (colision) durationTp -= dt;

            if (side == 0)
            {
                durationSwap -= dt;
                if (durationSwap <= 0.0f) swapDestination();
            }

            if (colision)
            {
                mHalo.TextureRect = new IntRect(mHalo.TextureRect.Left, mHalo.TextureRect.Top,  mHalo.TextureRect.Width, mHalo.TextureRect.Height + 1);
            }

            checkCollision();
        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            //drawCollisions(target, states, Color.Black);
            if (colision) mHalo.Draw(target, states);
        }

        public void swapDestination()
        {
            durationSwap = 3.0f;
            if (Sprite.Equals(side1))
            {
                Sprite = side2;
            }
            else
            {
                Sprite = side1;
            }
        }

        private void setSprite()
        {
            side1 = new Sprite(Resources.Texture("Elements/tp_1"));
            side2 = new Sprite(Resources.Texture("Elements/tp_2"));

            if (side == 1)
            {
                Sprite = side1;
            }
            else if (side == 2)
            {
                Sprite = side2;
            }
            else if (side == 0)
            {
                if (new Random().Next(0, 2) == 0) Sprite = side1;
                else Sprite = side2;
            }
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
            colision = false;
            List<PlayerEntity> players = MyGame.Instance.getScene().GetAll<PlayerEntity>();
            for (int i = 0; i < players.Count; i++)
            {
                if (getCollisionBox().Intersects(players[i].getCollisionBox()))
                {
                    colision = true; 
                    if (durationTp <= 0)
                    {
                        durationTp = 1.5f;
                        mHalo.TextureRect = new IntRect(mHalo.TextureRect.Left, mHalo.TextureRect.Top, mHalo.TextureRect.Width, 0);
                        if (side == 0)
                        {
                            if (Sprite.Equals(side1))
                            {
                                players[i].Position = destination;
                            }
                            else
                            {
                                players[i].Position = destination2;
                            }
                        } else
                        {
                            players[i].Position = destination;
                        }
                    }
                }
            }
            if ((!colision) && (durationTp < 1.5f))
            {
                durationTp = 1.5f;
                mHalo.TextureRect = new IntRect(mHalo.TextureRect.Left, mHalo.TextureRect.Top, mHalo.TextureRect.Width, 0);
            }
        }

    }
}