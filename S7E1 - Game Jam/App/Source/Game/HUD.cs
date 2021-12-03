using SFML.Graphics;
using System.Collections.Generic;
using SFML.System;
using System;


namespace TcGame
{
    /** Clase para controlar la interfaz de usuario HUD */
    public class HUD : Actor
    {
        private Font mFont_tarrgetacadital_ttf;

        private Text mPlayer1;
        private Text mPlayer2;

        private Sprite mHPBack;

        private Sprite mPlayer1HP;
        private Sprite mPlayer2HP;

        private int mP1Power;
        private int mP2Power;

        private Sprite mStars;

        /** Constructor */
        public HUD()
        {

            mLayer = ELayer.HUD;
            mFont_tarrgetacadital_ttf = new Font(Resources.Font("Fonts/tarrgetacadital"));

            mPlayer1 = new Text("Player 1", mFont_tarrgetacadital_ttf);
            mPlayer1.FillColor = new Color(0, 0, 0);
            mPlayer1.Position = new Vector2f(MyGame.Instance.getWinowSize().X / 2.0f - mPlayer1.GetLocalBounds().Width - 55.0f, 20);

            mPlayer2 = new Text("Player 2", mFont_tarrgetacadital_ttf);
            mPlayer2.FillColor = new Color(255, 255, 255);
            mPlayer2.Position = new Vector2f(MyGame.Instance.getWinowSize().X / 2.0f + 50.0f, 20);

            mHPBack = new Sprite(Resources.Texture("Elements/HUD/hp_back"));
            mHPBack.Position = new Vector2f(MyGame.Instance.getWinowSize().X / 2.0f - mHPBack.GetLocalBounds().Width - 50.0f, 70.0f);

            mPlayer1HP = new Sprite(Resources.Texture("Elements/HUD/hp"));
            mPlayer2HP = new Sprite(Resources.Texture("Elements/HUD/hp"));

            mPlayer1HP.Position = mHPBack.Position + new Vector2f(1.0f, 1.0f + mPlayer1HP.GetLocalBounds().Height / 2.0f);
            mPlayer2HP.Position = new Vector2f(MyGame.Instance.getWinowSize().X / 2.0f + 49.0f + mHPBack.GetLocalBounds().Width, 71.0f + mPlayer2HP.GetLocalBounds().Height / 2.0f);

            mPlayer1HP.Origin = new Vector2f(0.0f, mPlayer1HP.GetLocalBounds().Height / 2.0f);
            mPlayer2HP.Origin = new Vector2f(0.0f, mPlayer2HP.GetLocalBounds().Height / 2.0f);

            mPlayer2HP.Scale = new Vector2f(-1.0f, 1.0f);

            mStars = new Sprite(Resources.Texture("Elements/HUD/STAR"));

            mP1Power = mP2Power = 0;

        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        /** Override del método Draw de Actor */
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            mPlayer1.Draw(target, states);
            mPlayer2.Draw(target, states);

            mHPBack.Draw(target, states);

            Vector2f hppos = mHPBack.Position;
            mHPBack.Position = new Vector2f(MyGame.Instance.getWinowSize().X / 2.0f + 50.0f, 70.0f);
            mHPBack.Draw(target, states);
            mHPBack.Position = hppos;

            mPlayer1HP.Draw(target, states);
            mPlayer2HP.Draw(target, states);

            for (int i = 0; i < mP1Power; i++)
            {
                mStars.Position = new Vector2f(mHPBack.Position.X + 15.0f + (mStars.GetLocalBounds().Width + 10.0f) * i, mHPBack.Position.Y + 15.0f);
                mStars.Draw(target, states);
            }
            for (int i = 0; i < mP2Power; i++)
            {
                mStars.Position = new Vector2f(MyGame.Instance.getWinowSize().X / 2.0f + 50.0f + mHPBack.GetLocalBounds().Width - 30.0f - (mStars.GetLocalBounds().Width + 10.0f) * i, mHPBack.Position.Y + 15.0f);
                mStars.Draw(target, states);
            }
        }

        public void updateHUD(string player, float hp, int power)
        {
            HealthBar(player, hp);
            PoweBar(player, power);
        }

        public void HealthBar(string player, float hp)
        {
            if (player == "P1")
            {
                mPlayer1HP.Scale = new Vector2f(hp * 1.88f, 1.0f);
            }
            if (player == "P2")
            {
                mPlayer2HP.Scale = new Vector2f(-(hp * 1.88f), 1.0f);
            }

        }

        public void PoweBar(string player, int power)
        {
            if (player == "P1")
            {
                mP1Power = power;
            }
            if (player == "P2")
            {
                mP2Power = power;
            }
        }
    }
}
