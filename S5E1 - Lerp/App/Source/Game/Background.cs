using SFML.Graphics;
using SFML.Window;

namespace TcGame
{
  public class Background : StaticActor
  {
//    private Shader shader;
    private Texture t;

    public Background()
    {
      var screenSize = Engine.Get.Window.Size;

      t = new Texture("Data/Textures/Back.png");
      Sprite = new Sprite(t);
      Sprite.TextureRect = new IntRect(0, 0, (int)screenSize.X, (int)screenSize.Y);

   
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
      //states.Shader = shader;
      states.Texture = t;
      base.Draw(target, states);
    }
  }
}

