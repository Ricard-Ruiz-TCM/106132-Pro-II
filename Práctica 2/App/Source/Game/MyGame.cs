using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using SFML.Audio;

namespace TcGame
{
    public class MyGame : Game
    {
        public Scene Scene { private set; get; }

        public DebugManager Debug { private set; get; }

        public Vector2f MousePos { get { return new Vector2f(Mouse.GetPosition(Window).X, Mouse.GetPosition(Window).Y); } }

        public RenderWindow Window { private set; get; }

        public HUD mHUD { private set; get; }

        public Plane mPlayer { private set; get; }

        /// <summary>
        /// Speed in pixels/second of the world vertical scroll
        /// </summary>
        public float WorldSpeed = 30.0f;

        /// <summary>
        /// Unique instance of MyGame
        /// </summary>
        private static MyGame instance;

        /// <summary>
        /// Accesor for MyGame, implements the Singleton pattern
        /// </summary>
        public static MyGame Instance
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

        public void Init()
        {
            Resources.LoadResources();

            VideoMode videoMode = new VideoMode(1024, 768);
            Window = new RenderWindow(videoMode, "MyGame");
            Window.SetVerticalSyncEnabled(true);

            Debug = new DebugManager();
            Debug.Init();

            Scene = new Scene();

            Background background;
            background = Scene.Create<Background>();
            background.Speed = WorldSpeed;

            createHUD();

            createPlayer();

            // Spawners that will be the responsibles for creating new actors in scene
            CreatePersonSpawner();
            CreateOvniSpawner();
            CreateTankSpawner();

            Music();
        }

        private void CreatePersonSpawner()
        {
            ActorSpawner<Person> spawner;
            spawner = Scene.Create<ActorSpawner<Person>>();
            spawner.MinPosition = new Vector2f(0.0f, -200.0f);
            spawner.MaxPosition = new Vector2f(1000.0f, -200.0f);
            spawner.MinTime = 2.0f;
            spawner.MinTime = 4.0f;
            spawner.Reset();
        }

        private void CreateOvniSpawner()
        {
            ActorSpawner<Ovni> spawner;
            spawner = Scene.Create<ActorSpawner<Ovni>>();
            spawner.MinPosition = new Vector2f(0.0f, +1000.0f);
            spawner.MaxPosition = new Vector2f(1000.0f, +1000.0f);
            spawner.MinTime = 8.0f;
            spawner.MinTime = 15.0f;
            spawner.Reset();
        }

        private void CreateTankSpawner()
        {
            ActorSpawner<Tank> spawner;
            spawner = Scene.Create<ActorSpawner<Tank>>();
            spawner.MinPosition = new Vector2f(0.0f, -400.0f);
            spawner.MaxPosition = new Vector2f(1000.0f, 0.0f);
            spawner.MinTime = 8.0f;
            spawner.MinTime = 10.0f;
            spawner.Reset();
        }

        /**
         * Método para instanciar el jugador principal en la escena
         */
        private void createPlayer()
        {
            mPlayer = Scene.Create<Plane>(new Vector2f(Window.Size.X * 0.4f, Window.Size.Y * 0.7f));
        }

        /**
         * Método para instanciar el HUD del juego
         */
        private void createHUD()
        {
            mHUD = Scene.Create<HUD>();
        }

        public void DeInit()
        {
            Debug.DeInit();
            Window.Dispose();
        }

        public void Update(float dt)
        {
            Window.DispatchEvents();

            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                Window.Close();
            }

            Debug.Update(dt);
            Scene.Update(dt);

        }

        public void Draw()
        {
            Window.Clear(new Color(100, 100, 100));

            Window.Draw(Scene);
            Window.Draw(Debug);

            Window.Display();
        }

        public bool IsAlive()
        {
            return Window.IsOpen;
        }

        public void Music()
        {
            Sound canción = Resources.Sound("Audio/musica_juego");
            canción.Volume = 40;
            canción.Loop = true;
            canción.Play();
        }
    }
}