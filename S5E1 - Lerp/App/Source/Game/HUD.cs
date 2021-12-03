using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;

namespace TcGame
{
  public class HUD : StaticActor
  {
    public int Points;
    private Text pointsText;
    private Text infoText;

    private List<Sprite> lifes = new List<Sprite>();

    public HUD()
    {
      Font f = new Font("Data/Fonts/neuro.ttf");
      pointsText = new Text("1000", f);
      pointsText.CharacterSize = 50;
      pointsText.Position = new Vector2f(100.0f, 50.0f);

      infoText = new Text("Info", f);
      infoText.Position = new Vector2f(Engine.Get.Window.Size.X / 3.0f*2.0f, 50.0f);
      infoText.CharacterSize = 100;
    }

    public void ShowInfo(string info)
    {
      var ScreenSize = Engine.Get.Window.Size;

      infoText.DisplayedString = info;
      infoText.Origin = new Vector2f(infoText.GetLocalBounds().Width / 2.0f, infoText.GetLocalBounds().Height);
      infoText.Position = new Vector2f(ScreenSize.X, ScreenSize.Y) / 2.0f;
    }

    public void ResetAll()
    {
       Points = 0;
    }

  

    public override void Update(float dt)
    {
      base.Update(dt);

      float s = (float)Math.Sin(Engine.Get.Time * 2.0f) * 5.0f + 5.0f;
      byte alpha = (byte)(MathUtil.Lerp(0.2f, 1.0f, s) * 255.0f);
      infoText.FillColor = new Color(alpha, alpha, alpha, alpha);
    }

    public override void Draw(RenderTarget rt, RenderStates rs)
    {
        base.Draw(rt, rs);

        pointsText.DisplayedString = Points.ToString("00000");
        rt.Draw(pointsText);
        rt.Draw(infoText, rs);
    }
  }
}

