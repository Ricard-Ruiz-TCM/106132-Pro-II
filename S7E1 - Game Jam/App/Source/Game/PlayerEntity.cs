using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using SFML.Audio;

namespace TcGame
{
    public class PlayerEntity : AnimatedActor
    {

        /**
         * STATE
         * 
         * Estado del jugador
         */
        public enum STATE
        {
            IDLE, RUN, JUMP, FALL, FIST, KICK, SHOT, DAMAGE
        }

        /** Variable qeu controla el estado */
        private short mState;

        /** Velocidad del jugador */
        private Vector2f mSpeed;

        /** Definimos la velocidad base del objeto */
        private float mBaseSpeed = 250.0f;

        /** Bools que controlan donde estamos en encarando al jugador */
        private bool mRight, mLeft;

        /** Bool que controlan las colisiones con el suelo */
        private bool mGrounding;

        /** Vida del objeto */
        private float mHP;

        /** Arma que lleva el objeto */
        private Weapon mWeapon;

        /** define quien soy? */
        private string mME;

        /** Nivel de poder del objeto */
        private int mPower;

        /** Cantidad de animaciones para cambiarla segun el nivel de poder */
        private int mTotalAnimations = 13;

        /** Tiempo en segudnos que dura las animaciones de daño */
        private float mTimeFistAnimation;
        private float mTimeKickAnimation;
        private float mTimeShotAnimation;

        /** Variable para guardar el otro player */
        private PlayerEntity mEnemy;

        /** Varaiable para determinar cuando vuelvo volvr a recibir daño */
        private float mTimeToGetDaamageAgain;

        private bool mAlive;

        /** Constructor */
        public PlayerEntity() : base()
        {
            mAlive = true;
        }

        /** 
         * Método para inciar el objeto
         * 
         * @param string me -> Si soy el p1 o el p2
         */
        public void init(string me, Vector2f pos)
        {
            mME = me;

            Position = pos;

            mPower = 0;
            mAlive = true;
            mHP = 100.0f;
            mGrounding = false;
            mLeft = mRight = false;
            mWeapon = new Weapon();
            mWeapon.init(0);
            mWeapon.loadBasicWeapon();
            changeState(STATE.FALL);
            mSpeed = new Vector2f(0.0f, 0.0f);
            Scale = new Vector2f(0.5f, 0.5f);

            setAnimations();

            setCollisions();

            updateCollision();

            MyGame.Instance.getHUD().updateHUD(mME, mHP, mPower);

        }

        /** Override del método Update de Actor */
        public override void Update(float dt)
        {
            if (mAlive)
            {
                base.Update(dt);

                mLeft = mRight = false;

                mTimeFistAnimation -= dt;
                mTimeKickAnimation -= dt;
                mTimeShotAnimation -= dt;
                mTimeToGetDaamageAgain -= dt;

                pollEvents();

                updateState();

                speedUpdate();

                checkCollision();

                // Encaramos al jugador
                if (mLeft) Scale = new Vector2f(-0.5f, 0.5f);
                if (mRight) Scale = new Vector2f(0.5f, 0.5f);

                if (Position.X <= 0.0f)
                {
                    Position = new Vector2f(1275.0f, Position.Y);
                    if (Position.Y <= 200) Position += new Vector2f(0.0f, -15.0f);
                }
                if (Position.X >= 1280.0f)
                {
                    Position = new Vector2f(15.0f, Position.Y);
                }

                // Actualizmos posición del objeto
                Position += mSpeed * dt;
            }
        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (mAlive)
            {
                if (mState == (short)STATE.DAMAGE)
                {
                    if (mTimeToGetDaamageAgain % 0.25f <= 0.1f)
                    {
                        base.Draw(target, states);
                    }
                }
                else
                {
                    base.Draw(target, states);
                }

                //drawCollisions(target, states, Color.Green);
                Center();
            }
        }

        public float getHp()
        {
            return mHP;
        }

        /**
         * Método para establecer las animaciones 
         */
        private void setAnimations()
        {
            // Nivel de poder 1

            for (int i = 1; i < 4; i++)
            {
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/IDDLE_ANIMATION/iddle_" + i), 2, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/IDDLE_ANIMATION/iddle_" + i + "_" + i), 2, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/RUN_ANIMATION/run_" + i), 5, 1));
                mAnimations[mAnimations.Count - 1].FrameTime = 0.10f;
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/RUN_ANIMATION/run_" + i + "_" + i), 5, 1));
                mAnimations[mAnimations.Count - 1].FrameTime = 0.10f;
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/JUMP_ANIMATION/jump_" + i), 1, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/FALL_ANIMATION/fall_" + i), 1, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/FIST_ANIMATION/fist_" + i), 2, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/FIST_ANIMATION/fist_" + i + "_" + i), 2, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/KICK_ANIMATION/kick_" + i), 3, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/KICK_ANIMATION/kick_" + i + "_" + i), 3, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/SHOT_ANIMATION/shot_" + i), 1, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/GET_DAMAGE_ANIMATION/damage_" + i), 1, 1));
                mAnimations.Add(new AnimatedSprite(Resources.Texture("Player/" + mME + "/GET_DAMAGE_ANIMATION/damage_" + i + "_" + i), 1, 1));

            }

        }

        /**
         * Método para establecer las cajas de collisiones
         */
        private void setCollisions()
        {
                mCollisionBox.Add(GetLocalBounds());
        }

        /**
         * Método para gestionar todo el input del teclado/mando del objeto
         */
        private void pollEvents()
        {

            if (mME == "P1")
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN)) jump();
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    mLeft = true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    mRight = true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN)) changeState(STATE.FIST);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.X))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN)) changeState(STATE.KICK);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.E))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN))
                    {
                        if (mWeapon != null)
                        {
                            if (mWeapon.getType() == mPower + 1)
                            {
                                changeState(STATE.SHOT);
                            }
                        }
                    }
                }
            }
            if (mME == "P2")
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.U))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN)) jump();
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.J))
                {
                    mLeft = true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.L))
                {
                    mRight = true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.M))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN)) changeState(STATE.FIST);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.N))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN)) changeState(STATE.KICK);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                {
                    if ((mState == (short)STATE.IDLE) || (mState == (short)STATE.RUN))
                    {
                        if (mWeapon != null)
                        {
                            if (mWeapon.getType() == mPower + 1)
                            {
                                changeState(STATE.SHOT);
                            }
                        }
                    }
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Numpad0))
            {
                mPower = 0; changeState((STATE)mState);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Numpad1))
            {
                mPower = 1; changeState((STATE)mState);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Numpad2))
            {
                mPower = 2; changeState((STATE)mState);
            }

        }

        /**
         * Método para corregir la diferencia de animacion segun arma
         * 
         * @param int plus      -> offset a sumar
         * @param bool weapon   -> si tenemos arma equipada den uestro poder o no
         */
        private void offSetAnimation(int plus, bool weapon)
        {
            mAnimation += plus + (weapon == true ? 1 : 0);
        }

        /**
         * Método para cambiar de estado al jugador
         * 
         * @param STATE newState    -> Nuevo estado
         */
        private void changeState(STATE newState)
        {

            // Actualizamos estado
            mState = (short)newState;
            mAnimation = mState + (mPower * mTotalAnimations);

            // Corregimos offset de animación y aplicamos según nuevo estado
            switch (newState)
            {
                case STATE.IDLE:
                    offSetAnimation(0, (mWeapon.getType() == mPower + 1));
                    break;
                case STATE.RUN:
                    offSetAnimation(1, (mWeapon.getType() == mPower + 1));
                    break;
                case STATE.JUMP:
                    offSetAnimation(2, false);
                    break;
                case STATE.FALL:
                    offSetAnimation(2, false);
                    break;
                case STATE.FIST:
                    hitEnemy();
                    mTimeFistAnimation = 0.4f;
                    offSetAnimation(2, (mWeapon.getType() == mPower + 1));
                    break;
                case STATE.KICK:
                    hitEnemy();
                    mTimeKickAnimation = 0.6f;
                    offSetAnimation(3, (mWeapon.getType() == mPower + 1));
                    break;
                case STATE.SHOT:
                    shot();
                    mTimeShotAnimation = 0.4f;
                    offSetAnimation(4, false);
                    break;
                case STATE.DAMAGE:
                    offSetAnimation(4, (mWeapon.getType() == mPower + 1));
                    break;
            }

            // Ligero ajuste de velocidad para 3 estados concretos
            if ((newState == STATE.KICK) || (newState == STATE.FIST) || (newState == STATE.SHOT)) mSpeed = new Vector2f(0.0f, 0.0f);
        }

        /**
         * Método para calcular la velocidad de movimiento segun el estado
         * */
        private void speedUpdate()
        {
            if (mState != (short)STATE.DAMAGE)
            {
                if ((mState == (short)STATE.RUN) || (mState == (short)STATE.IDLE)) mSpeed = new Vector2f(0.0f, 0.0f);

                if (mLeft) mSpeed = new Vector2f(-1.0f * mBaseSpeed, mSpeed.Y);
                if (mRight) mSpeed = new Vector2f(1.0f * mBaseSpeed, mSpeed.Y);

                if ((!mRight) && (!mLeft)) mSpeed = new Vector2f(mSpeed.X / 1.03f, mSpeed.Y);

                // Gravity
                if ((mState == (short)STATE.FALL) || (mState == (short)STATE.JUMP)) mSpeed += new Vector2f(0.0f, 9.8f);
                if ((mState == (short)STATE.FALL)) mSpeed += new Vector2f(0.0f, 9.8f / 2.0f);
            }
        }

        /**
         * Método para controlar la maquina de estados del objeto
         */
        private void updateState()
        {
            switch (mState)
            {
                case (short)STATE.IDLE:
                    if ((mRight) || (mLeft)) changeState(STATE.RUN);
                    break;
                case (short)STATE.RUN:
                    if ((!mLeft) && (!mRight)) changeState(STATE.IDLE);
                    break;
                case (short)STATE.JUMP:
                    if (mSpeed.Y > 0.0f) changeState(STATE.FALL);
                    break;
                case (short)STATE.FALL:
                    if (mGrounding) changeState(STATE.IDLE);
                    break;
                case (short)STATE.FIST:
                    if (mTimeFistAnimation <= 0.0f) changeState(STATE.IDLE);
                    break;
                case (short)STATE.KICK:
                    if (mTimeKickAnimation <= 0.0f) changeState(STATE.IDLE);
                    break;
                case (short)STATE.SHOT:
                    if (mTimeShotAnimation <= 0.0f) changeState(STATE.IDLE);
                    break;
                case (short)STATE.DAMAGE:
                    if (mTimeToGetDaamageAgain <= 0.0f) respawn();
                    break;
                default: break;
            }
        }

        /**
         * Método para comprobar las colisiones con todos los objetos 
         */
        private void checkCollision()
        {
            updateCollision();
            bool collision = false;
            Background bg = MyGame.Instance.getScene().GetFirst<Background>();
            for (int i = 0; i < bg.getCollisionAmount(); i++)
            {
                if (getCollisionBox().Intersects(bg.getCollisionBox(i)))
                {
                    collision = true;
                    switch (bg.getCollisionType(i))
                    {
                        case Background.COLL_TYPE.GROUND:
                            if (mState == (short)STATE.FALL) ground(bg.getCollisionBox(i).Top);
                            break;
                        case Background.COLL_TYPE.LEFT_WALL:
                            if (mLeft) mSpeed = new Vector2f(0.0f, mSpeed.Y);
                            break;
                        case Background.COLL_TYPE.RIGHT_WALL:
                            if (mRight) mSpeed = new Vector2f(0.0f, mSpeed.Y);
                            break;
                        case Background.COLL_TYPE.LOWER_PLAT:
                            if (mState == (short)STATE.JUMP) fall();
                            break;
                        default: break;
                    }
                }
            }
            if ((!collision) && (mState != (short)STATE.FALL) && (mSpeed.Y >= 0.0f)) fall();
        }

        /**
         * Método para saltar
         */
        private void jump()
        {
            changeState(STATE.JUMP);
            mSpeed += new Vector2f(0.0f, -mBaseSpeed * 1.80f);
            mGrounding = false;
        }

        /**
         * Método para parar un salto y caer en picado
         */
        private void fall()
        {
            changeState(STATE.FALL);
            mGrounding = false;
            mSpeed = new Vector2f(mSpeed.X, 0.0f);
        }

        /**
         * método para indicar que he tocado suelo
         * 
         * @param float offset   -> Distancia en la que estamos métidos dentro del objeto
         */
        private void ground(float offset)
        {
            changeState(STATE.IDLE);
            mGrounding = true;
            Position = new Vector2f(Position.X, offset - GetGlobalBounds().Height / 2.0f);
        }

        /** Override del método getCollisionBox */
        public override FloatRect getCollisionBox(int pos = 0)
        {
            return base.getCollisionBox(pos);
        }

        /**
         * Método para recoger un arma del suelo
         * 
         * @param Weapon wp -> Arma que recogemos
         */
        public bool pickUpWeapon(Weapon wp)
        {
            bool picked = false;

            if ((wp.getType() - 1) == mPower)
            {
                if (mWeapon != null) mWeapon.Destroy();
                mWeapon = wp;
                picked = true;
                MyGame.Instance.getHUD().updateHUD(mME, mHP, mPower);
            }

            return picked;
        }

        /**
         * Método para disparar la bala
         */
        private void shot()
        {
            if ((mWeapon.getType() != (short)Weapon.WEAPON_TYPE.FIST) && (mWeapon.getType() != (short)Weapon.WEAPON_TYPE.SWORD))
            {
                if (mWeapon != null)
                {
                    if (mWeapon.getType() == mPower + 1)
                    {
                        Bullet bllt = MyGame.Instance.getScene().Create<Bullet>();
                        bllt.init(Position, mWeapon.getType(), (Scale.X < 0.0f ? -1.0f : 1.0f), mWeapon.getDamage());

                        if (mWeapon.getType() == (short)Weapon.WEAPON_TYPE.CROSSBOW)
                        {
                            Sound crossbow = Resources.Sound("Audio/ballesta");
                            crossbow.Volume = 75;
                            crossbow.Play();
                        }
                        if (mWeapon.getType() == (short)Weapon.WEAPON_TYPE.REVOLVER)
                        {
                            Sound gunshot = Resources.Sound("Audio/gunshot");
                            gunshot.Volume = 50;
                            gunshot.Play();
                        }

                    }
                }
            }
            else
            {
                hitEnemy();
            }
        }

        /**
         * Método para intentar pegar al enemy
         */
        private void hitEnemy()
        {
            if (mEnemy != null)
            {
                if (mEnemy.isReady())
                {
                    if (MathF.Abs(mEnemy.Position.X - Position.X) <= getCollisionBox().Width / 1.5f)
                    {
                        if (mState == (short)STATE.FIST)
                        {
                            Sound fist = Resources.Sound("Audio/fist");
                            fist.Volume = 125;
                            fist.Play();
                        }

                        if (mState == (short)STATE.KICK)
                        {
                            Sound kick = Resources.Sound("Audio/kick");
                            kick.Volume = 75;
                            kick.Play();
                        }

                        mEnemy.damageMe(mWeapon.getDamage()); upgrade();
                    }
                }
            }
        }

        /**
         * Método para comprobar suqien soy
         * 
         * @return string   -> mME
         */
        public string getMe()
        {
            return mME;
        }

        /**
         * Método setter del enemigo
         * 
         * @param PlayerEntity enemy    -> player 2
         */
        public void setEnemy(PlayerEntity enemy)
        {
            mEnemy = enemy;
        }

        /**
         * Método para hacerme daño
         * 
         * @param float damage  | -> daño ar ecibr
         */
        public void damageMe(float damage)
        {
            mHP -= damage;
            if (mHP <= 0.0f)
            {
                mHP = 0.0f;
                mAlive = false;
            }
            mTimeToGetDaamageAgain = 1.0f;
            changeState(STATE.DAMAGE);
            MyGame.Instance.getHUD().updateHUD(mME, mHP, mPower);
        }

        public void upgrade()
        {
            mPower = (int)MathF.Min(mPower + 1, 2);
            MyGame.Instance.getHUD().updateHUD(mME, mHP, mPower);
        }

        /**
         * Método para respawnear
         */
        public void respawn()
        {
            mTimeToGetDaamageAgain = 1.0f;
            changeState(STATE.IDLE);
            mPower = (int)MathF.Max(0, mPower - 1);
            MyGame.Instance.getHUD().updateHUD(mME, mHP, mPower);
            mSpeed = new Vector2f(0.0f, 0.0f);
            if (mME == "P1") Position = new Vector2f(325.0f, 500.0f);
            else Position = new Vector2f(950.0f, 500.0f);
        }

        /**
         * Método que ocmprueba si dpoemos recibir daño de neuvo
         * 
         * @return bool     -> true si podemos recibir daño
         */
        public bool isReady()
        {
            if (mState != (short)STATE.DAMAGE) return true;
            return false;
        }

        public bool isAlive()
        {
            return mAlive;
        }

    }
}