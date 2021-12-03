namespace TcGame
{
    public class Explosion : AnimatedActor
    {
        public Explosion()
        {
            AnimatedSprite = new AnimatedSprite(Resources.Texture("Textures/Explosion"), 9, 1);
            AnimatedSprite.Loop = false;
            AnimatedSprite.FrameTime = 0.1f;
            Engine.Get.Timer.SetTimer(AnimatedSprite.FrameTime * 9.0f, Destroy);

            Center();
        }
    }
}

