using SFML.Graphics;
using SFML.System;
using System;
using SFML.Audio;

namespace TcGame
{
    public class Tank : Enemy
    {

        private float mSpeed;
        private int mDestructionOffset;

        private float mReload;

        /** Constructor */
        public Tank() : base()
        {
            Random r = new Random();
            mSpeed = 0.8f;
            mDestructionOffset = 75;
            mReload = -0.1f;
            Sprite = new Sprite(Resources.Texture("Textures/Enemies/Tank0" + r.Next(1, 3)));
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            updateCollisionBox();

            Position = new SFML.System.Vector2f(Position.X, Position.Y + mSpeed);

            if (mReload <= 0.0f) shoot();
            else mReload -= dt;

            if (Position.Y > MyGame.Instance.Window.Size.Y + mDestructionOffset)
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
            mCollisionBox.Top += mCollisionBox.Height / 10.0f; mCollisionBox.Left += mCollisionBox.Width / 10.0f; mCollisionBox.Width /= 1.2f; mCollisionBox.Height /= 1.3f;
        }

        /**
         * Método para disparar la bala del tank
         */
        private void shoot()
        {
            Sound tanke = Resources.Sound("Audio/disparo_tanke");
            tanke.Volume = 50;
            tanke.Play();

            Bullet bullet; mReload = 3.0f;
            bullet = MyGame.Instance.Scene.Create<Bullet>();
            bullet.load(Bullet.BULLET_TYPE.BULLET_TANK, new Vector2f(Position.X + GetLocalBounds().Width / 2.0f, Position.Y + 25.0f));
        }

    }
}
