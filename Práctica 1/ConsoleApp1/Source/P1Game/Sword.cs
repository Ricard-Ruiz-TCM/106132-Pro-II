
namespace TcGame
{
    public class Sword : Weapon
    {
        public override void init()
        {
            loadTexture("Data/Textures/Sword.png"); 
            setType(ITEMS.SWORD);
        }
    }
}