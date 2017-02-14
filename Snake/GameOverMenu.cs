using System;

namespace Snake
{
    class GameOverMenu
    {
        private string[] gameOverMenu = new string[] { "Play again", "HighScore", "Exit" };

        private int selected = 0;
        private int maxSelection;

        private int middleX;
        private int middleY;

        private Game game;
        private HighScore highScore;

        private bool draw = true;

        public GameOverMenu(Game game)
        {
            middleX = (Game.BorderWidth - Game.BorderStart) / 2;
            middleY = (Game.BorderHeight - Game.BorderStart) / 2;
            maxSelection = gameOverMenu.Length;
            highScore = new HighScore();
            this.game = game;
        }

        internal void DrawMenu(int score)
        {
            if(score != -1)
            {
                if (highScore.CheckIfHighScore(score))
                {
                    highScore.AddToHighScore(score);
                    highScore.DrawHighScoreList();
                }
            }
            draw = true;
            Console.SetCursorPosition(middleX, middleY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("GAME OVER");

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
                Console.WriteLine(gameOverMenu[i]);
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
                            game.Restart(true);
                            draw = false;
                            break;
                        case (1):
                            highScore.DrawHighScoreList();
                            break;
                        case (2):
                            game.EndGame();
                            draw = false;
                            break;
                    }
                    break;
            }
            if(draw) DrawMenu(-1);
        }
    }
}