using System;

namespace Snake
{
    class PauseMenu
    {
        private string[] pauseMenu = new string[2] { "Resume", "Exit" };

        private int selected = 0;
        private int maxSelection;

        private int middleX;
        private int middleY;

        private Game game;

        private bool draw = true;

        public PauseMenu(Game game)
        {
            middleX = (Game.BorderWidth - Game.BorderStart) / 2;
            middleY = (Game.BorderHeight - Game.BorderStart) / 2;
            maxSelection = pauseMenu.Length;
            this.game = game;
        }

        internal void DrawMenu()
        {
            draw = true;
            Console.SetCursorPosition(middleX, middleY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("PAUSE");

            for (int i = 0; i < maxSelection; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.SetCursorPosition(middleX, (middleY + i + 1));
                Console.WriteLine(pauseMenu[i]);
            }
        }

        internal void ManageKeyPresses(ConsoleKey key)
        {
            switch (key)
            {
                case (ConsoleKey.UpArrow):
                    selected--;
                    if (selected < 0) selected = 0;
                    break;
                case (ConsoleKey.DownArrow):
                    selected++;
                    if (selected > maxSelection - 1) selected = maxSelection - 1;
                    break;
                case (ConsoleKey.Enter):
                    switch (selected)
                    {
                        case (0):
                            game.Restart(false);
                            draw = false;
                            break;
                        case (1):
                            game.EndGame();
                            draw = false;
                            break;
                    }
                    break;
            }
            if (draw) DrawMenu();
        }
    }
}
