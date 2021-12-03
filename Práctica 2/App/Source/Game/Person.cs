using System;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;

namespace TcGame
{
    public class Person : AnimatedActor
    {

        private bool mAlive;
        private float mSpeed;
        private bool mCaptured;
        private float mTimeCaptured;

        private Sound mAcapturingSound;

        /** Constructor */
        public Person() : base()
        {
            Random r = new Random();

            mSpeed = 1.0f;
            mAlive = true;
            mCaptured = false;
            mTimeCaptured = 3.0f;

            AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/People/People0" + r.Next(1, 4).ToString()), 2, 1);

            mAcapturingSound = Resources.Sound("Audio/abducir");
            mAcapturingSound.Volume = 55;

            if (r.Next(0, 2) == 0) Scale = new Vector2f(Scale.X * -1, Scale.Y);

            Center();
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            movement(dt);

            updateCollisionBox();

            if ((Position.Y > MyGame.Instance.Window.Size.Y) || (mTimeCaptured <= 0.0f))
            {
                mAlive = false;
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
            mCollisionBox.Top += mCollisionBox.Height / 3.5f; mCollisionBox.Left += mCollisionBox.Width / 3.5f; mCollisionBox.Width /= 2.5f; mCollisionBox.Height /= 3.0f;
        }

        /**
         * Método para calcular el movimiento del objeto Person
         * También ejecuta la "animación" de ser abuducido
         * 
         * @param dt    -> tempo entre frames
         */
        private void movement(float dt)
        {
            if (!mCaptured)
            {
                Position = new Vector2f(Position.X, Position.Y + mSpeed);
            }
            else
            {
                mTimeCaptured -= dt;
                Rotation += mSpeed * 2.0f;
            }
        }

        /** 
         * Método para comparobar si el objeto Person está "vivo"
         * 
         * @return bool     -> mAlive
         */
        public bool isAlive()
        {
            return mAlive;
        }

        /**
         * Método para decirle al objeto Peson que está siendo capturado
         */
        public void capturing()
        {
            mAlive = false;
            mCaptured = true;
            mAcapturingSound.Play();
            MyGame.Instance.mHUD.captured();
        }

        /**
         * Método para "matar" al objeto Person y no poder ser objetivo de otros enemigos
         */
        public void rescued()
        {
            mAlive = false;
            Destroy();
            MyGame.Instance.mHUD.rescued();
        }

        /**
         * Método para reinicar un objeto Person que esta siendo capturado por un ovni pero el ovni es druido en el moment
         */
        public void free()
        {
            mAlive = true;
            Rotation = 0.0f;
            mCaptured = false;
            mAcapturingSound.Stop();
            MyGame.Instance.mHUD.captured(-1);
        }

    }
}