using System;

namespace TeteraDeRusell
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Engine engine = new Engine();
            engine.Run(new TeaGame());
        }
    }
}
