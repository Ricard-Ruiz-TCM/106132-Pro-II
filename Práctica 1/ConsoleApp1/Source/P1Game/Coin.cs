
namespace TcGame
{
    public class Coin : Item
    {
        public override void init()
        {
            loadTexture("Data/Textures/Coin.png"); 
            setType(ITEMS.COIN);
        }
    }
}