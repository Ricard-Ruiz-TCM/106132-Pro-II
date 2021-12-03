using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using SFML.System;

namespace TcGame
{
    public class Bullet : StaticActor
    {
        public float m_LifeTime = 3.0f;
        public Vector2f m_Forward = new Vector2f(0.0f, -1.0f);
        public float m_Speed = 300.0f;

        public Bullet()
        {
            Layer = ELayer.Back;
            SetTexture(Resources.Texture("Textures/Bullets/TankBullet"));
        }

        public void SetTexture(Texture texture)
        {
            Sprite = new Sprite(texture);
            Center();
        }

        public override void Update(float dt)
        {
            Position += m_Forward * m_Speed * dt;
            m_LifeTime -= dt;

            if (m_LifeTime < 0.0f)
            {
                Destroy();
            }

            var enemies = Engine.Get.Scene.GetAll<Enemy>();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetGlobalBounds().Intersects(GetGlobalBounds()))
                {
                    enemy.Destroy();
                    Explosion explosion = Engine.Get.Scene.Create<Explosion>();
                    explosion.WorldPosition = WorldPosition;
                    Destroy();

                    MyGame.Get.GameHUD.OnEnemyKilled();
                }
            }
        }
    }
}

