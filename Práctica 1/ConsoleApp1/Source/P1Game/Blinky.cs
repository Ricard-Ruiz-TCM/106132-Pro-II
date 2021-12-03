
namespace TcGame
{
    public class Blinky : Item
    {
        public override void init()
        {
            loadTexture("Data/Textures/Blinky.png"); 
            setType(ITEMS.BLINKY);
        }
    }
}