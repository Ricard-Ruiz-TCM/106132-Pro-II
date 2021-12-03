
namespace TcGame
{
    public class Heart : Item
    {
        public override void init()
        {
            loadTexture("Data/Textures/Heart.png"); 
            setType(ITEMS.HEART);
        }
    }
}