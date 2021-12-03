using SFML.Window;
using SFML.System;

namespace TcGame
{
  public class Explosion : AnimatedActor
  {
    private float LifeTime;

    public Explosion()
    {
      AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/FX/Explosion"), 4, 1);
      AnimatedSprite.Loop = false;
      AnimatedSprite.FrameTime = 0.2f;
      LifeTime = AnimatedSprite.FrameTime * 3.0f;
      Center();
    }

    public override void Update(float dt)
    {
      base.Update(dt);
      Position += new Vector2f(0.0f, 30.0f * dt);

      LifeTime -= dt;
      if (LifeTime < 0.0f)
      {
        Destroy();
      }
    }
  }
}

