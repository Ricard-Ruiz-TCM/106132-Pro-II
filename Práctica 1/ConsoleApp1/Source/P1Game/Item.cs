using SFML.Graphics;

namespace TcGame
{
    public abstract class Item : Sprite
    {

        public enum ITEMS
        {
            HEART, SWORD, AXE, BOMB, COIN, CLYDE, BLINKY,
            TOTAL_ITEMS
        }

        private ITEMS mType;

        public virtual void init()
        {
        }

        public void deInit()
        {
            Texture.Dispose();
        }

        public virtual bool isWeapon()
        {
            return false;
        }

        public void loadTexture(string path)
        {
            Texture = new SFML.Graphics.Texture(path);
            Origin = new SFML.System.Vector2f(Texture.Size.X / 2.0f, Texture.Size.Y / 2.0f);
        }

        public ITEMS getType()
        {
            return mType;
        }

        public void setType(ITEMS type)
        {
            mType = type;
        }

    }

}
