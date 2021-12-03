using SFML.Graphics;
using SFML.System;

namespace TcGame
{
  /// <summary>
  /// Actor that contains an Sprite
  /// </summary>
  public class StaticActor : Actor
  {
    public Sprite Sprite { get; set; }

    public override void Draw(RenderTarget target, RenderStates states)
    {
      base.Draw(target, states);

      if (Sprite != null)
      {
        states.Transform *= Transform;
        target.Draw(Sprite, states);
      }
    }

    public override FloatRect GetLocalBounds()
    {
      return (Sprite != null) ? Sprite.GetLocalBounds() : new FloatRect();
    }
  }
}
