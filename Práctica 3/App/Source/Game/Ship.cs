using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TcGame
{
    public class Ship : StaticActor
    {
        private static Vector2f Up = new Vector2f(0.0f, -1.0f);
        private Vector2f Forward = Up;
        private float Speed = 200.0f;
        private float RotationSpeed = 100.0f;

        /** Controla si la cadencia de disparo me permite disprar */
        private bool mReadyToShot;
        /** Controla el tiempo entre disparos */
        private float mTimeBTWShots;
        /** Controla si estoy apretnado el gatillo para disparar */
        private bool mWeaponTrigger;

        /** Objecto del escudo */
        private Shield mShield;

        public Ship()
        {
            Sprite = new Sprite(Resources.Texture("Textures/Ship"));
            Center();
            OnDestroy += OnShipDestroy;

            mReadyToShot = true;
            mWeaponTrigger = false;

            Engine.Get.Window.KeyPressed += HandleKeyPressed;
            Engine.Get.Window.KeyReleased += HandleKeyReleased;
            Engine.Get.Window.MouseButtonPressed += HandleMouseButtonPressed;
            Engine.Get.Window.MouseButtonReleased += HandleMouseButtonReleased;

            mShield = Engine.Get.Scene.Create<Shield>(this);
            mShield.Position = Origin + new Vector2f(0.0f, 20.0f);

            var flame = Engine.Get.Scene.Create<Flame>(this);
            flame.Position = Origin + new Vector2f(20.0f, 62.0f);

            var flame2 = Engine.Get.Scene.Create<Flame>(this);
            flame2.Position = Origin + new Vector2f(-20.0f, 62.0f);
        }

        private void HandleMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {

            if (e.Button == Mouse.Button.Left)
            {
                mWeaponTrigger = true;
            }

        }

        private void HandleMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {

            if (e.Button == Mouse.Button.Left)
            {
                mWeaponTrigger = false;
            }

        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.C)
            {
                Shoot<Rocket>();
            }

            if (e.Code == Keyboard.Key.LShift)
            {
                Speed = 800.0f;
                RotationSpeed = 0.0f;
            }

            if(e.Code == Keyboard.Key.G)
            {
                if (!mShield.isActive()) mShield.active();
            }

            // ==> EJERCICIO 5
            // By pressing G a wild Shield will appear! There are 151 Shields, ¡¿can you catch 'em all?!
            // It is quite likely that Shield needs to be a new class, and it would be useful that it has different states,
            // that represent if it is being activated, already activated or being deactivated
            // Take into account that the addition of the Shield changes a little bit the behaviour of this Ship!
        }

        private void HandleKeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.LShift)
            {
                Speed = 200.0f;
                RotationSpeed = 100.0f;
            }
        }

        public override void Update(float dt)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                Rotation -= RotationSpeed * dt;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                Rotation += RotationSpeed * dt;
            }

            Forward = Up.Rotate(Rotation);
            Position += Forward * Speed * dt;

            if (mWeaponTrigger)
            {
                if (mReadyToShot)
                {
                    Shoot<Bullet>();
                    mReadyToShot = false;
                    mTimeBTWShots = 0.2f;
                }

                mTimeBTWShots -= dt;
                if (mTimeBTWShots <= 0.0f) mReadyToShot = true;
            }

            MyGame.ResolveLimits(this);
            CheckCollision();
        }

        private void CheckCollision()
        {
            var asteroids = Engine.Get.Scene.GetAll<Asteroid>();
            foreach (var a in asteroids)
            {
                Vector2f toAsteroid = a.WorldPosition - WorldPosition;
                if (toAsteroid.Size() < 50.0f)
                {
                    if (!mShield.isActive())
                    {
                        Destroy();
                        a.Destroy();
                    }
                }
            }
        }

        void OnShipDestroy(Actor obj)
        {
            Engine.Get.Window.KeyPressed -= HandleKeyPressed;
        }

        private void Shoot<T>() where T : Bullet
        {
            var rocket = Engine.Get.Scene.Create<T>();
            rocket.WorldPosition = WorldPosition;
            rocket.Forward = Forward;
        }
    }
}

