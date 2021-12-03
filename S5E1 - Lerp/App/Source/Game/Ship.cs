using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
  public class Ship : StaticActor
  {
    private static Vector2f Up = new Vector2f(0.0f, -1.0f);
    private Vector2f Forward = Up;
    private float Speed = 200.0f;
    private float RotationSpeed = 100.0f;
    private Shield shield;
    private float ShootFreq = 0.02f;

    public Ship()
    {
      Sprite = new Sprite(Resources.Texture("Textures/Ship"));
      Center();
      OnDestroy += OnShipDestroy;

      Engine.Get.Window.KeyPressed += HandleKeyPressed;

      var flame = Engine.Get.Scene.Create<Flame>(this);
      flame.Position = Origin + new Vector2f(20.0f, 62.0f);

      var flame2 = Engine.Get.Scene.Create<Flame>(this);
      flame2.Position = Origin + new Vector2f(-20.0f, 62.0f);

      shield = Engine.Get.Scene.Create<Shield>(this);
      shield.Position = Origin + new Vector2f(0.0f, 20.0f);
    }

    private void HandleKeyPressed(object sender, KeyEventArgs e)
    {
      if (e.Code == Keyboard.Key.C)
      {
        Shoot<Rocket>();
      }

      if (e.Code == Keyboard.Key.G)
      {
        shield.Enable(5.0f);
      }
    }

    public bool HasShield()
    {
      return shield.Enabled;
    }

    public override void Update(float dt)
    {
      if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
      {
        Speed = 800.0f;
      }
      else
      {
        Speed = 300.0f;

        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
          Rotation -= RotationSpeed * dt;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
          Rotation += RotationSpeed * dt;
        }
      }

      ShootFreq -= dt;
      if (ShootFreq < 0.0f)
      {
        ShootFreq = 0.15f;
        TryToShoot();
      }

      Forward = Up.Rotate(Rotation);
      Position += Forward * Speed * dt;

      MyGame.ResolveLimits(this);
      CheckCollision();
    }

    private void CheckCollision()
    {
      if (!HasShield())
      {
        var asteroids = Engine.Get.Scene.GetAll<Asteroid>();
        foreach (var a in asteroids)
        {
          Vector2f toAsteroid = a.WorldPosition - WorldPosition;
          if (toAsteroid.Size() < 50.0f)
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

    private void TryToShoot()
    {
      if (Mouse.IsButtonPressed(Mouse.Button.Left))
      {
        Vector2f toMouse = Engine.Get.MousePos - Position;
        Shoot<Bullet>(toMouse.Normal());
      }
    }

    private void Shoot<T>() where T : Bullet
    {
      Shoot<T>(Forward);
    }

    private void Shoot<T>(Vector2f Direction) where T : Bullet
    {
      var rocket = Engine.Get.Scene.Create<T>();
      rocket.WorldPosition = WorldPosition;
      rocket.Forward = Direction;
    }
  }
}

