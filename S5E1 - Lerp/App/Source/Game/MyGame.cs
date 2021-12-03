using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class MyGame : Game
    {

        private static MyGame instance;

        public static MyGame Get
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyGame();
                }

                return instance;
            }
        }

        private MyGame()
        {

        }

        private HUD myHUD;
        private Fish f;

        public void Init()
        {
            Engine.Get.Window.KeyPressed += HandleKeyPressed;
            System.Threading.Thread.Sleep(3);

            Engine.Get.Scene.Create<Background>();
            myHUD = Engine.Get.Scene.Create<HUD>();
            f = Engine.Get.Scene.Create<Fish>();
        }

        public void DeInit()
        {

        }

        public void Update(float dt)
        {

        }

        public static void ResolveLimits(Actor actor)
        {
            var ScrenSize = Engine.Get.Window.Size;
            var MyBounds = actor.GetGlobalBounds();

            if (MyBounds.Top + MyBounds.Height < 0.0f)
            {
                actor.Position = new Vector2f(actor.Position.X, ScrenSize.Y + MyBounds.Height / 2.0f);
            }

            if (MyBounds.Top > ScrenSize.Y)
            {
                actor.Position = new Vector2f(actor.Position.X, -MyBounds.Height / 2.0f);
            }

            if (MyBounds.Left + MyBounds.Width < 0.0f)
            {
                actor.Position = new Vector2f(ScrenSize.X + MyBounds.Width / 2.0f, actor.Position.Y);
            }

            if (MyBounds.Left > ScrenSize.X)
            {
                actor.Position = new Vector2f(-MyBounds.Width / 2.0f, actor.Position.Y);
            }
        }

        private void DestroyAll<T>() where T : Actor
        {
            var actors = Engine.Get.Scene.GetAll<T>();
            actors.ForEach(x => x.Destroy());
        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {

        }


    }
}
