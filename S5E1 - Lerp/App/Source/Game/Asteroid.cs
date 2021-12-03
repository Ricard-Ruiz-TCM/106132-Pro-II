using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
  public class Asteroid : StaticActor
  {
    public float RotationSpeed = 20.0f;
    public float Speed = 200.0f;
    public Vector2f Forward = new Vector2f(1.0f, 0.0f);

    private Sprite[] sprites;
    private int damage;

    public Asteroid()
    {
      sprites = new Sprite[]
      {
        new Sprite(Resources.Texture("Textures/Asteroid00")),
        new Sprite(Resources.Texture("Textures/Asteroid01")),
        new Sprite(Resources.Texture("Textures/Asteroid02"))
      };

      Sprite = sprites[0];

      damage = 0;
      Center();
      OnDestroy += OnAsteroidDestroyed;
    }

    public override void Update(float dt)
    {
      Position += Forward * Speed * dt;
      Rotation += RotationSpeed * dt;
      MyGame.ResolveLimits(this);
    }

    public void Impact()
    {
      ++damage;
      if (damage < sprites.Length)
      {
        Sprite = sprites[damage];

        var explosion = Engine.Get.Scene.Create<Explosion>();
        explosion.WorldPosition = WorldPosition;
      }
      else
      {
        Destroy();
      }
    }

    private void OnAsteroidDestroyed(Actor obj)
    {
      var hud = Engine.Get.Scene.GetFirst<HUD>();
      if (hud != null)
      {
        hud.Points += 100;
      }

      var explosion = Engine.Get.Scene.Create<Explosion>();
      explosion.WorldPosition = WorldPosition;
    }
  }
}
