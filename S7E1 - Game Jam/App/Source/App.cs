namespace TcGame
{
    public class App
    {
        public static void Main()
        {
            Engine engine = new Engine();
            engine.Run(MyGame.Instance);
        }
    }
}