using SFML.Graphics;
using SFML.System;
using System;

namespace TcGame
{
    public class Ovni : Enemy
    {

        /**
         * STATE
         * 
         * PATROLLING       -> Enemigo en comportamiento de patruya
         * REACHING_PERSON  -> Enemigo buscnado un objetivo 
         * CAPTURING_PERSON -> Enemigo capturando a su objetivo
         */
        private enum STATE
        {
            PATROLLING, REACHING_PERSON, CAPTURING_PERSON
        }

        private float mSpeed;
        private STATE mState;

        private Person mTarget;
        private float mTimeCapturing;

        private bool mGoingNorth;

        private bool mCanCapture;

        /** Constructor */
        public Ovni() : base()
        {
            Layer = ELayer.Front;

            mState = STATE.PATROLLING;
            mTimeCapturing = 3.0f;

            mSpeed = -0.85f;
            mGoingNorth = true;

            mCanCapture = true;

            Sprite = new Sprite(Resources.Texture("Textures/Enemies/Ovni0" + new Random().Next(1, 5).ToString()));

            Center();

            OnDestroy += setFree;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            updateCollisionBox();

            switch (mState)
            {
                case STATE.PATROLLING:
                    patrollingMovement();
                    if (selectTarget())
                    {
                        mState = STATE.REACHING_PERSON;
                    }
                    break;
                case STATE.REACHING_PERSON:
                    movingToTarget();
                    if (mTarget.isAlive())
                    {
                        if (getCollisionBox().Intersects(mTarget.getCollisionBox()))
                        {
                            mState = STATE.CAPTURING_PERSON;
                        }
                    }
                    else mState = STATE.PATROLLING;
                    break;
                case STATE.CAPTURING_PERSON:
                    movingToTarget();
                    mTimeCapturing -= dt;
                    if ((mTarget.isAlive()) && (mCanCapture))
                    {
                        mTarget.capturing();
                    }
                    if (mTimeCapturing <= 0.0f)
                    {
                        mState = STATE.PATROLLING;
                        mTimeCapturing = 3.0f;
                    }
                    break;
                default: break;
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
            mCollisionBox.Top += mCollisionBox.Height / 4.0f; mCollisionBox.Left += mCollisionBox.Width / 10.0f; mCollisionBox.Width /= 1.3f; mCollisionBox.Height /= 5.0f;
        }

        /**
         * Método que gestiona el movimiento de patrolla
         */
        private void patrollingMovement()
        {

            Position = new Vector2f(Position.X, Position.Y + mSpeed);

            if (Position.Y >= 768.0f) mGoingNorth = true;
            if (Position.Y <= 0.0f)   mGoingNorth = false;

            if (mGoingNorth) mSpeed = -3.0f;
            else mSpeed = 3.0f;
        }

        /**
         * Método que gestiona el movimiento hacia el target
         */
        private void movingToTarget()
        {
            mSpeed = 3.0f;

            if (Position.X < mTarget.Position.X) Position = new Vector2f(Position.X + mSpeed, Position.Y);
            if (Position.X > mTarget.Position.X) Position = new Vector2f(Position.X - mSpeed, Position.Y);

            /** Si estamos capturando a una al objetivo, nos ponemos 50.0f px por encima */
            if (Position.Y < mTarget.Position.Y - (mState == STATE.CAPTURING_PERSON ? 50.0f : 0.0f)) Position = new Vector2f(Position.X, Position.Y + mSpeed);
            if (Position.Y > mTarget.Position.Y - (mState == STATE.CAPTURING_PERSON ? 50.0f : 0.0f)) Position = new Vector2f(Position.X, Position.Y - mSpeed);

        }

        /**
         * Método para selecionar un objetivo
         * 
         * @return bool -> true si ha encontrado objetivo | false si no hay objetivo disponible
         */
        private bool selectTarget()
        {
            mTarget = MyGame.Instance.Scene.GetRandom<Person>();

            if (mTarget != null)
            {
                return (mTarget.isAlive()) ? true : false;
            }
            return false;

        }

        /**
         * Método para librear al objetivo Person que esta siendo capturado
         * 
         * @param actor -> ?
         */
        private void setFree(Actor actor)
        {
            mCanCapture = false;
            if (mState == STATE.CAPTURING_PERSON)
            {
                if (!mTarget.isAlive())
                {
                    mTarget.free();
                }
            }
        }

    }
}
