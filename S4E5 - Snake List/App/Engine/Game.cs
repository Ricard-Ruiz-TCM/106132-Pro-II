namespace Snake
{
	public interface Game
	{
		void Init();

		void DeInit();

		void Update(float dt);

		void Draw();

		bool IsAlive();
	}
}
