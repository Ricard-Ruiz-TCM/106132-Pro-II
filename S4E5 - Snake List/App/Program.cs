using System;

namespace Snake
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Engine e = new Engine();
			e.Run( new snakeGame());
		}
	}
}
