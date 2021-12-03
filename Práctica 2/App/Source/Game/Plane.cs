using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using SFML.Audio;

namespace TcGame
{
    public class Plane : AnimatedActor
    {

        private float mSpeed;

        private bool n, s, e, w;

        private AnimatedSprite mGas;

        private float mReload;

        public Plane()
        {
            Layer = ELayer.Front;

            mSpeed = 5.0f;
            n = s = e = w = false;

            mReload = -0.05f;

            AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/Player/Plane"), 4, 1);

            mGas = new AnimatedSprite(Resources.Texture("Textures/FX/PlaneCloudGas"), 4, 1);

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            updateCollisionBox();

            n = s = e = w = false;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) w = true;
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) n = true;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) e = true;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) s = true;

            if (s) Position += new SFML.System.Vector2f(0.0f, mSpeed);
            if (n) Position -= new SFML.System.Vector2f(0.0f, mSpeed);
            if (w) Position -= new SFML.System.Vector2f(mSpeed, 0.0f);
            if (e) Position += new SFML.System.Vector2f(mSpeed, 0.0f);

            if (mReload <= 0.0f)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) shoot();
            }
            else mReload -= dt;

            CheckCollision();

            mGas.Update(dt);
            mGas.Position = new SFML.System.Vector2f(Position.X + GetLocalBounds().Width / 6.0F, Position.Y + GetLocalBounds().Height / 2.0F);

        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            mGas.Draw(target, states);
        }

        /**
         * Método para actualizar la caja de colisión teniendo en cuenta GetGlobalBounds();
         */
        private void updateCollisionBox()
        {
            mCollisionBox = GetGlobalBounds();
            mCollisionBox.Top += mCollisionBox.Height / 5.8f; mCollisionBox.Left += mCollisionBox.Width / 4.5f; mCollisionBox.Width /= 2.0f; mCollisionBox.Height /= 3.0f;
        }

        /**
         * Método para comprobar colisiones con los objetos Person
         */
        private void CheckCollision()
        {
            foreach(Person actor in MyGame.Instance.Scene.GetAll<Person>())
            {
                if (actor.getCollisionBox().Intersects(getCollisionBox()))
                {
                    actor.rescued();
                }
            }
        }
        
        /**
         * Método para disparar 2 proyetiles
         */
        private void shoot()
        {
            Bullet bullet; mReload = 0.15f;

            Sound shoot = Resources.Sound("Audio/disparo");
            shoot.Volume = 40;
            shoot.Play();

            bullet = MyGame.Instance.Scene.Create<Bullet>();
            bullet.load(Bullet.BULLET_TYPE.BULLET_PLANE, new Vector2f(Position.X + GetLocalBounds().Width / 3.0f, Position.Y));

            bullet = MyGame.Instance.Scene.Create<Bullet>();
            bullet.load(Bullet.BULLET_TYPE.BULLET_PLANE, new Vector2f(Position.X + GetLocalBounds().Width / 1.5f, Position.Y));
        }
        
    }
}