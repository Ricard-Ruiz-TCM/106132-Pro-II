using SFML.Audio;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System;

namespace TcGame
{
    /** Clase Singleton de juego, es el núcleo del juego */
    public class MyGame : Game
    {

        /** Instancia de MyGame */
        private static MyGame mInstance;

        /** 
         * Método Singleton para recuperar la instancia
         * 
         * @return MyGame   -> mInstance
         */
        public static MyGame Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new MyGame();
                }

                return mInstance;
            }
        }

        /** Scena principal y única dle juego */
        private Scene mScene;

        /** Herramientas para debug y desarrollo */
        private DebugManager mDebugManager;

        /** Ventana principal gestionada por SFML */
        private RenderWindow mWindow;

        /** Objecto HUD del juego */
        private HUD mHUD;

        /** Bools que controlan si existen estos personajes */
        private bool mPlayer1, mPlayer2;

        private PlayerEntity mPlayerEntity1, mPlayerEntity2;

        /** Constructor */
        private MyGame() { }

        public void Init()
        {
            // Init del vídeo
            VideoMode videoMode = new VideoMode(1280, 720);
            mWindow = new RenderWindow(videoMode, "GameJam");
            // Set del vídeo
            mWindow.SetVerticalSyncEnabled(true);

            // Init de los Resources
            Resources.LoadResources();

            // Init del DebugManager
            mDebugManager = new DebugManager();
            mDebugManager.Init();

            // Creación de la escena
            mScene = new Scene();

            // Creación del escenario
            mScene.Create<Background>();

            // Creación del HUD
            mHUD = mScene.Create<HUD>();

            // No establecemos ningun jugador inicial
            mPlayer1 = mPlayer2 = false;

            //Reproducción de la música
            Music();

        }

        public void DeInit()
        {
            mDebugManager.DeInit();
            mWindow.Dispose();
        }

        /**
         * Método virtual para hacer el "update" del objeto
         * 
         * @param dt -> delta time 
         */
        public Vector2f MousePos { get { return new Vector2f(Mouse.GetPosition(mWindow).X, Mouse.GetPosition(mWindow).Y); } }

        public void Update(float dt)
        {
            mWindow.DispatchEvents();

            pollEvents();

            if (mPlayer1) mPlayer1 = mPlayerEntity1.isAlive();
            if (mPlayer2) mPlayer2 = mPlayerEntity2.isAlive();

            mDebugManager.Update(dt);
            mScene.Update(dt);

        }

        /**
         * Método virtual para hacer el "render" del objeto
         * 
         * @param target -> ?
         * @param states -> ?
         */
        public void Draw()
        {
            mWindow.Clear(new Color(128, 128, 128));

            mWindow.Draw(mScene);
            mWindow.Draw(mDebugManager);

            mWindow.Display();
        }

        /**
        * Método para gestionar todo el input del teclado/mando del objeto
        */
        private void pollEvents()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                mWindow.Close();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.F1))
            {
                if (!mPlayer1) createPlayer1();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.F2))
            {
                if (!mPlayer2) createPlayer2();
            }
        }

        /**
         * Método para crear el jugador 1
         */
        private void createPlayer1()
        {
            
            mPlayer1 = true;
            if (mPlayerEntity1 == null) mPlayerEntity1 = mScene.Create<PlayerEntity>(new Vector2f(325.0f, 500.0f));
            mPlayerEntity1.init("P1", new Vector2f(325.0f, 500.0f));
            if (mPlayerEntity2 != null)
            {
                mPlayerEntity1.setEnemy(mPlayerEntity2);
                mPlayerEntity2.setEnemy(mPlayerEntity1);
            }
        }

        /**
         * Método para crear el jugador 2
         */
        private void createPlayer2()
        {
            mPlayer2 = true;
            if (mPlayerEntity2 == null) mPlayerEntity2 = mScene.Create<PlayerEntity>(new Vector2f(950.0f, 500.0f));
            mPlayerEntity2.Scale = new Vector2f(mPlayerEntity2.Scale.X * -1, mPlayerEntity2.Scale.Y);
            mPlayerEntity2.init("P2", new Vector2f(950.0f, 500.0f));
            if (mPlayerEntity1 != null)
            {
                mPlayerEntity2.setEnemy(mPlayerEntity1);
                mPlayerEntity1.setEnemy(mPlayerEntity2);
            }
        }

        /**
         * Método para reproducir la música
         */
        private void Music()
        {
            Sound canción = Resources.Sound("Audio/musica");
            canción.Volume = 25;
            canción.Loop = true;
            canción.Play();
        }

        /**
        * Método que comprueba si la ventana sigue abierta
        * 
        * @return bool     -> mWindow.IsOpen
        */
        public bool IsAlive()
        {
            return mWindow.IsOpen;
        }

        /**
         * Método get de Scene
         * 
         * @return Scene -> mScene
         */
        public Scene getScene()
        {
            return mScene;
        }

        /**
         * Método get de DebugManager
         * 
         * @return DebugManager -> mDebugManager
         */
        public DebugManager getDM()
        {
            return mDebugManager;
        }

        /**
         * Método get del tamaño de la pantaña
         * 
         * @return Vector2u -> mWindow.Size
         */
        public Vector2u getWinowSize()
        {
            return mWindow.Size;
        }

        /**
         * Método get del HUD
         * 
         * @return HUD  -> mHUD
         */
        public HUD getHUD()
        {
            return mHUD;
        }

    }
}