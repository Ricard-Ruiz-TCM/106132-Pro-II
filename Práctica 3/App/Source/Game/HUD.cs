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

        public bool InfoVisible = true;
        public bool PointsVisible = true;

        private List<Sprite> lifes = new List<Sprite>();
        private int NumLifes;

        public HUD()
        {
            Sprite = new Sprite(Resources.Texture("Textures/HUD"));
            pointsText = new Text("1000", Resources.Font("Fonts/neuro"));
            pointsText.CharacterSize = 50;
            pointsText.Position = new Vector2f(100.0f, 50.0f);

            infoText = new Text("Info", Resources.Font("Fonts/neuro"));
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
            lifes.Clear();

            for (int i = 0; i < 3; ++i)
            {
                var life = new Sprite(Resources.Texture("Textures/Life"));
                life.Position = new Vector2f(190.0f + i * 20.0f, 15.0f);
                lifes.Add(life);
            }

            NumLifes = 3;
            Points = 0;
        }

        public void LostLife()
        {
            NumLifes = (NumLifes > 0) ? (NumLifes - 1) : 0;
        }

        public bool IsAlive()
        {
            return NumLifes > 0;
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
            if (PointsVisible)
            {
                base.Draw(rt, rs);

                pointsText.DisplayedString = Points.ToString("00000");
                rt.Draw(pointsText);

                for (int i = 0; i < NumLifes; ++i)
                {
                    rt.Draw(lifes[i], rs);
                }
            }

            if (InfoVisible)
            {
                rt.Draw(infoText, rs);
            }
        }
    }
}

