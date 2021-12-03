using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class MyGame : Game
    {
        public HUD GameHUD
        {
            private set; get;
        }

        private static MyGame ms_Instance;

        public static MyGame Get
        {
            get
            {
                if (ms_Instance == null)
                {
                    ms_Instance = new MyGame();
                }

                return ms_Instance;
            }
        }

        private MyGame()
        {

        }

        public void Init()
        {
            Resources.LoadResources();

            Engine.Get.Window.SetVerticalSyncEnabled(true);
            Engine.Get.Window.KeyPressed += HandleKeyPressed;

            CreateBackground();
            CreateTank();
            CreateHUD();
            CreateEnemySpawner();



        }
        public void DeInit()
        {

        }
        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {

        }

        public void Update(float dt)
        {

        }

        private void CreateBackground() 
        {
            Engine.Get.Scene.Create<Background>();
        }
        private void CreateTank() 
        {
            Engine.Get.Scene.Create<Tank>();
        }
        private void CreateHUD()
        {
            GameHUD = Engine.Get.Scene.Create<HUD>();
        }

        private void CreateEnemySpawner() 
        {
            ActorSpawner<Enemy> spawner;
            spawner = Engine.Get.Scene.Create<ActorSpawner<Enemy>>();
            const float spawnLimitOffset = 50.0f;
            spawner.MinPosition = new Vector2f(spawnLimitOffset, spawnLimitOffset);
            spawner.MaxPosition = new Vector2f(Engine.Get.ViewportSize.X- spawnLimitOffset, Engine.Get.ViewportSize.Y- spawnLimitOffset);
            spawner.MinTime = 1.0f;
            spawner.MaxTime = 4.0f;
            spawner.Reset();
        }


    }
}

