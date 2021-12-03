
namespace TcGame
{
    public class Axe : Weapon
    {
        public override void init()
        {
            loadTexture("Data/Textures/Axe.png");
            setType(ITEMS.AXE);
        }
    }
}