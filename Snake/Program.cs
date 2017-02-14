using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Init();
            game.Start();
        }
    }
}
