using SFML.Graphics;

namespace TcGame
{
  public class Life : StaticActor
  {
    public Life()
    {
      Sprite = new Sprite(Resources.Texture("Textures/Life"));
      Center();
    }
  }
}

