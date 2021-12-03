using SFML.Window;
using SFML.System;

namespace TcGame
{
  public class Shield : AnimatedActor
  {
    private float d = 0.0f;
    private float totalTime = 0.2f;

    public bool Enabled { private set; get; }

    public Shield()
    {
      AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/Shield"), 3, 2);
      AnimatedSprite.FrameTime = 0.03f;
      Center();
    }

    public override void Update(float dt)
    {
      base.Update(dt);

      Scale = MathUtil.Lerp(new Vector2f(0.0f, 0.0f), new Vector2f(1.0f, 1.0f), d);
      d += dt / totalTime * (Enabled ? 1.0f : -1.0f);
      d = MathUtil.Clamp(d, 0.0f, 1.0f);
    }

    public void Enable(float seconds)
    {
      if (!Enabled)
      {
        Enabled = true;
        Engine.Get.Timer.SetTimer(seconds, Disable);
      }
    }

    private void Disable()
    {
      Enabled = false;
    }
  }
}

