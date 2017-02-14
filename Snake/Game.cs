using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Game
    {
        public const int BorderWidth = 100;
        public const int BorderHeight = 25;
        public const int BorderStart = 2;

        public bool isPaused;
        public bool isGameOver;

        private bool isPlaying;

        private Snake snake;
        private AISnake aiSnake;

        private GameOverMenu gameOverMenu;
        private PauseMenu pauseMenu;
        private FruitManager fruitManager;

        public void Init()
        {
            Console.CursorVisible = false;
            //snake = new Snake(5, this);
            aiSnake = new AISnake(5, this);
            pauseMenu = new PauseMenu(this);
            gameOverMenu = new GameOverMenu(this);
            fruitManager = new FruitManager(10);
            isPlaying = true;
            isPaused = false;
            isGameOver = false;
            DrawBorder();
        }

        public void GameOver()
        {
            Console.SetCursorPosition(0, 0);
            isGameOver = true;
            //snake.Pause();
            fruitManager.Pause();
            System.Threading.Thread.Sleep(200);
            //gameOverMenu.DrawMenu(snake.getScore());
            gameOverMenu.DrawMenu(10);

        }

        public void Pause()
        {
            Console.SetCursorPosition(0, 0);
            isPaused = true;
            //snake.Pause();
            fruitManager.Pause();
            System.Threading.Thread.Sleep(200);
            pauseMenu.DrawMenu();
        }

        public void Restart(bool reset)
        {
            Console.Clear();
            isGameOver = false;
            isPaused = false;

            if (reset) snake.Reset();

            DrawBorder();
            fruitManager.UnPause();
            snake.UnPause(); 
        }

        public void EndGame()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ResetColor();
            isPlaying = false;
        }

        private void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            //Draws the left and right borders
            for (int i = BorderStart; i < BorderHeight; i++)
            {
                Console.SetCursorPosition(BorderStart, i);
                Console.WriteLine("|");
                Console.SetCursorPosition(BorderWidth, i);
                Console.WriteLine("|");
            }

            //Draws the top and bottom borders
            for (int i = BorderStart; i < BorderWidth; i++)
            {
                Console.SetCursorPosition(i, BorderStart);
                Console.WriteLine("-");
                Console.SetCursorPosition(i, BorderHeight);
                Console.WriteLine("-");
            }

            Console.SetCursorPosition(BorderStart, BorderHeight + 2);
            Console.Write("Movment : Arrow keys                                                                    Pause : Esc");
        }

        public void Start()
        {
            do
            {
                if (!HighScore.SelectingName)
                {
                    while (Console.KeyAvailable)
                    {
                        if (isPaused)
                        {
                            pauseMenu.ManageKeyPresses(Console.ReadKey().Key);
                        }
                        else if (isGameOver)
                        {
                            gameOverMenu.ManageKeyPresses(Console.ReadKey().Key);
                        }
                        else
                        {
                            //snake.SetDirection(Console.ReadKey().Key);
                        }
                    }
                }
            } while (isPlaying);
        }
    }
}