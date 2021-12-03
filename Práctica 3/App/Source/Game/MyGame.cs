using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class MyGame : Game
    {
        private enum State
        {
            None,
            Start,
            Playing,
            Restarting,
            GameOver
        }

        private State currentState;

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static MyGame instance;

        /// <summary>
        /// Returns the Singleton Instance
        /// </summary>
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

        /// <summary>
        /// Private Constructor (Singleton pattern purposes)
        /// </summary>
        private MyGame()
        {

        }

        private HUD myHUD;

        /// <summary>
        /// Initializes the game
        /// </summary>
        public void Init()
        {
            Engine.Get.Window.KeyPressed += HandleKeyPressed;

            Engine.Get.Scene.Create<Background>();
            myHUD = Engine.Get.Scene.Create<HUD>();

            ChangeState(State.Start);
        }

        /// <summary>
        /// DeInitializes the game
        /// </summary>
        public void DeInit()
        {

        }

        /// <summary>
        /// Updates the Finite State Machine (FSM) of the game
        /// </summary>
        public void Update(float dt)
        {

        }

        private void OnShipDestroy(Actor obj)
        {
            if (currentState == State.Playing)
            {
                myHUD.LostLife();

                if (myHUD.IsAlive())
                {
                    ChangeState(State.Restarting);
                }
                else
                {
                    ChangeState(State.GameOver);
                }
            }
        }

        private void ChangeState(State newState)
        {
            // Exit states
            if (currentState == State.Start)
            {
                myHUD.InfoVisible = false;
            }
            else if (currentState == State.Playing)
            {
                myHUD.PointsVisible = false;

                DestroyAll<AsteroidSpawner>();
                DestroyAll<Asteroid>();
            }
            else if (currentState == State.Restarting)
            {
                myHUD.InfoVisible = false;
            }

            // Enter states
            if (newState == State.Start)
            {
                myHUD.InfoVisible = true;
                myHUD.PointsVisible = false;
                myHUD.ResetAll();

                myHUD.ShowInfo("Press Start");
            }
            else if (newState == State.Playing)
            {
                myHUD.PointsVisible = true;

                var ship = Engine.Get.Scene.Create<Ship>();
                ship.Position = GetPlayerInitialPosition();
                ship.OnDestroy += OnShipDestroy;

                Engine.Get.Scene.Create<AsteroidSpawner>();
            }
            else if (newState == State.Restarting)
            {
                myHUD.InfoVisible = true;
                myHUD.ShowInfo("Restarting");
            }
            else if (newState == State.GameOver)
            {
                myHUD.InfoVisible = true;
                myHUD.ShowInfo("GameOver");
                Engine.Get.Timer.SetTimer(5.0f, RestartGame);
            }

            currentState = newState;
        }

        public static void ResolveLimits(Actor actor)
        {
            var ScrenSize = Engine.Get.Window.Size;
            var MyBounds = actor.GetGlobalBounds();

            // Top Bounds
            if (MyBounds.Top + MyBounds.Height < 0.0f)
            {
                actor.Position = new Vector2f(actor.Position.X, ScrenSize.Y + MyBounds.Height / 2.0f);
            }

            // Bottom Bounds
            if (MyBounds.Top > ScrenSize.Y)
            {
                actor.Position = new Vector2f(actor.Position.X, -MyBounds.Height / 2.0f);
            }

            // Left Bounds
            if (MyBounds.Left + MyBounds.Width < 0.0f)
            {
                actor.Position = new Vector2f(ScrenSize.X + MyBounds.Width / 2.0f, actor.Position.Y);
            }

            // Right Bounds
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

        private Vector2f GetPlayerInitialPosition()
        {
            var ScreenSize = Engine.Get.Window.Size;
            return new Vector2f(ScreenSize.X / 2.0f, ScreenSize.Y - 200.0f);
        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Enter)
            {
                if ((currentState == State.Start) || (currentState == State.Restarting))
                {
                    ChangeState(State.Playing);
                }
            }
        }

        private void RestartGame()
        {
            ChangeState(State.Start);
        }
    }
}
