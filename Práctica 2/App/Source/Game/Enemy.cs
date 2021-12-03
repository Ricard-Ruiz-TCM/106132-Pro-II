using SFML.Graphics;
using SFML.Audio;

namespace TcGame
{
    public abstract class Enemy : StaticActor
    {
        protected Enemy()
        {
            OnDestroy += Explode;
        }

        private void Explode(Actor actor)
        {
            Sound explode = Resources.Sound("Audio/destrucion");
            explode.Volume = 60;
            explode.Play();

            MyGame.Instance.Scene.Create<Explosion>(Position);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            CheckCollision();
        }

        /**
         * Método para Comprobar collisiones con los objetos Bullet
         */
        private void CheckCollision()
        {
            foreach (Bullet bullet in MyGame.Instance.Scene.GetAll<Bullet>())
            {
                if (bullet.getType() == Bullet.BULLET_TYPE.BULLET_PLANE)
                {
                    if (bullet.getCollisionBox().Intersects(getCollisionBox()))
                    {
                        bullet.Destroy(); Destroy();
                    }
                }
            }
        }

    }
}

