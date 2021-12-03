using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using SFML.System;

namespace TcGame
{
  public class Bullet : StaticActor
  {
    public static Vector2f Up = new Vector2f(0.0f, -1.0f);

    public Vector2f Forward = new Vector2f(0.0f, -1.0f);
    public float Speed = 500.0f;
    public bool KillShip;

    public Bullet()
    {
      Sprite = new Sprite(Resources.Texture("Textures/Bullet"));
      Center();
    }

    public override void Update(float dt)
    {
      //Forward = Forward.Rotate(RotationSpeed * dt);
      Rotation = MathUtil.AngleWithSign(Forward, Up);
      Position += Forward * Speed * dt;

      CheckScreenLimits();
      CheckCollisions();
    }

    private void CheckCollisions()
    {
      CheckAsteroidCollisions();
      CheckSpaceShipsCollision();
    }

    private void CheckScreenLimits()
    {
      var Bounds = GetGlobalBounds();
      var ScreenSize = Engine.Get.Window.Size;

      if ((Bounds.Left > ScreenSize.X) ||
          (Bounds.Left + Bounds.Width < 0.0f) ||
          (Bounds.Top + Bounds.Width < 0.0f) ||
          (Bounds.Top > ScreenSize.Y))
      {
        Destroy();
      }
    }

    private void CheckSpaceShipsCollision()
    {
      if (KillShip)
      {
        var ships = Engine.Get.Scene.GetAll<Ship>();
        foreach (var ship in ships)
        {
          var toShip = ship.WorldPosition - WorldPosition;
          if (toShip.Size() < 50.0f)
          {
            if (!ship.HasShield())
            {
              ship.Destroy();
            }

            Destroy();
          }
        }
      }
    }

    private void CheckAsteroidCollisions()
    {
      var asteroids = Engine.Get.Scene.GetAll<Asteroid>();
      foreach (var a in asteroids)
      {
        var toAsteroid = a.WorldPosition - WorldPosition;
        if (toAsteroid.Size() < 50.0f)
        {
          a.Impact();
          Destroy();
        }
      }
    }
  }
}

