using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace TcGame
{
  public class Rocket : Bullet
  {
    private float RotationSpeed = 90.0f;

    public Rocket()
    {
      Speed = 300.0f;

      Sprite = new Sprite(Resources.Texture("Textures/Rocket"));
      Center();

      var flame = Engine.Get.Scene.Create<Flame>(this);
      flame.Position = Origin + new Vector2f(0.0f, 40.0f);
    }

    public override void Update(float dt)
    {
      base.Update(dt);
      FollowAsteroid(dt);
    }

    private void FollowAsteroid(float dt)
    {
      Asteroid nearest = GetNearestAsteroid();
      if (nearest != null)
      {
        var ToNearest = nearest.WorldPosition - WorldPosition;
        Forward = Forward.Rotate(RotationSpeed * dt * MathUtil.Sign(ToNearest.Normal(), Forward));
      }
    }

    private Asteroid GetNearestAsteroid()
    {
      Asteroid NearestAsteroid = null;
      float MinDistance = 10000.0f;
      var asteroids = Engine.Get.Scene.GetAll<Asteroid>();
      foreach (var a in asteroids)
      {
        var toAsteroid = a.WorldPosition - WorldPosition;
        if (toAsteroid.Size() < MinDistance)
        {
          MinDistance = toAsteroid.Size();
          NearestAsteroid = a;
        }
      }

      return NearestAsteroid;
    }
  }
}

