namespace TcGame
{
    public class Flame : AnimatedActor
    {
        public Flame()
        {
            AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/Flame"), 7, 1);
            AnimatedSprite.FrameTime = 0.02f;

            Center();
        }
    }
}
