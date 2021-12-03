
namespace TcGame
{
    public class Bomb : Item
    {
        public override void init()
        {
            loadTexture("Data/Textures/Bomb.png");
            setType(ITEMS.BOMB);
        }
    }
}