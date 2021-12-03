using SFML.Graphics;

namespace TcGame
{
    public class HUD : Actor
    {

        private Font mFont_LuckiestGuy_ttf;

        private Text mTxTRescuedPPL;
        private Text mTxTAmountRescued;
        private int mAmountRescued;


        private Text mTxTCapturedPPL;
        private Text mTxTAmountCaptured;
        private int mAmountCaptured;

        private float mTxTOffset;

        /** Constructor */
        public HUD()
        {
            Layer = ELayer.HUD;

            mAmountRescued = mAmountCaptured = 0;

            mTxTOffset = 10.0f;
            mFont_LuckiestGuy_ttf = new Font(Resources.Font("Fonts/LuckiestGuy"));

            mTxTRescuedPPL = new Text("Rescued People: ", mFont_LuckiestGuy_ttf);
            mTxTRescuedPPL.FillColor = new Color(255, 128, 0);
            mTxTRescuedPPL.Position = new SFML.System.Vector2f(mTxTOffset, mTxTOffset);
            mTxTAmountRescued = new Text("0", mFont_LuckiestGuy_ttf);
            mTxTAmountRescued.Position = new SFML.System.Vector2f(mTxTRescuedPPL.GetGlobalBounds().Width + mTxTOffset, mTxTOffset);

            mTxTCapturedPPL = new Text("Captured People: ", mFont_LuckiestGuy_ttf);
            mTxTCapturedPPL.FillColor = new Color(255, 128, 0);
            mTxTCapturedPPL.Position = new SFML.System.Vector2f(10.0f, mTxTRescuedPPL.GetLocalBounds().Height + (mTxTOffset * 2.0f));
            mTxTAmountCaptured = new Text("0", mFont_LuckiestGuy_ttf);
            mTxTAmountCaptured.Position = new SFML.System.Vector2f(mTxTCapturedPPL.GetGlobalBounds().Width + mTxTOffset, mTxTCapturedPPL.Position.Y);
        }
        
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            mTxTRescuedPPL.Draw(target, states);
            mTxTAmountRescued.Draw(target, states);

            mTxTCapturedPPL.Draw(target, states);
            mTxTAmountCaptured.Draw(target, states);

        }

        /**
         * Método para incrementar la cantidad de gente capturada
         */
        public void captured(int amount = 1)
        {
            mAmountCaptured += amount;
            mTxTAmountCaptured.DisplayedString = mAmountCaptured.ToString();
        }

        /**
         * Método para incrementar la cantidad de gente salvada
         */
        public void rescued()
        {
            mAmountRescued++;
            mTxTAmountRescued.DisplayedString = mAmountRescued.ToString();
        }

    }
}