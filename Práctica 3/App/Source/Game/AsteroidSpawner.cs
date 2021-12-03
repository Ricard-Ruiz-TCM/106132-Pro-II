using System;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    /// <summary>
    /// Responsible for create asteroids
    /// </summary>
    public class AsteroidSpawner : Actor
    {
        private static float[] spawnSeconds = { 2.0f, 3.0f, 5.0f };
        private static float[] speeds = { 50.0f, 20.0f, 80.0f };
        private static float[] rotationSpeeds = { 20.0f, -50.0f, 30.0f };
        private static float[] scales = { 0.5f, 0.7f, 0.8f };

        public AsteroidSpawner()
        {
            Engine.Get.Timer.SetTimer(1.0f, SpawnAsteroid);
            OnDestroy += OnAsteroidSpawnerDestroyed;
        }

        /// <summary>
        /// Creates a new asteroid
        /// </summary>
        private void SpawnAsteroid()
        {
            var random = Engine.Get.random;
            var ScreenSize = Engine.Get.Window.Size;
            var Right = new Vector2f(1.0f, 0.0f);
            var Down = new Vector2f(0.0f, 1.0f);

            var asteroid = Engine.Get.Scene.Create<Asteroid>();
            asteroid.Speed = speeds[random.Next(speeds.Length)];
            asteroid.RotationSpeed = rotationSpeeds[random.Next(rotationSpeeds.Length)];
            float scale = scales[random.Next(scales.Length)];
            asteroid.Scale = new Vector2f(scale, scale);

            switch (random.Next(2))
            {
                case 0:
                    asteroid.Position = new Vector2f(-200.0f, ScreenSize.Y / 2.0f);
                    asteroid.Forward = Right.Rotate(random.Next(-20, +20));
                    break;

                case 1:
                    asteroid.Position = new Vector2f(ScreenSize.X / 2.0f, -200.0f);
                    asteroid.Forward = Down.Rotate(random.Next(-20, +20));
                    break;
            }

            Engine.Get.Timer.SetTimer(spawnSeconds[random.Next(spawnSeconds.Length)], SpawnAsteroid);
        }

        private void OnAsteroidSpawnerDestroyed(Actor obj)
        {
            Engine.Get.Timer.ClearTimer(SpawnAsteroid);
        }
    }
}

