using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake
{
    class Snake
    {
        private int x;
        private int y;

        List<SnakeBodyPart> snake = new List<SnakeBodyPart>();

        Directions direction;

        private int snakeSize { get; set; }
        private int originalSnakeSize;
        private Timer drawTimer;

        private Game game;

        public Snake(int snakeSize, Game game)
        {
            originalSnakeSize = snakeSize;
            this.snakeSize = originalSnakeSize;
            this.game = game;
            direction = Directions.right;
            CreateSnake();
            StartTimer();
        }

        private void StartTimer()
        {
            drawTimer = new System.Timers.Timer();
            drawTimer.Elapsed += new ElapsedEventHandler(MoveSnake);
            drawTimer.Interval = 200;
            drawTimer.Enabled = true;
        }

        public void Pause()
        {
            drawTimer.Elapsed -= new ElapsedEventHandler(MoveSnake);
        }

        public void UnPause()
        {
            drawTimer.Elapsed += new ElapsedEventHandler(MoveSnake);
        }

        private void CreateSnake()
        {
            //Creates the snake at top left corner of the border
            x = Game.BorderStart + 1;
            y = Game.BorderStart + 1;

            //Creates all parts of the snake, starting with the head
            for (int i = originalSnakeSize; i > 0; i--)
            {
                SnakeBodyPart snakeBody = new SnakeBodyPart();
                snakeBody.x = i;
                snakeBody.y = 0;
                snake.Add(snakeBody);
            }
        }

        internal int getScore()
        {
            return snakeSize - originalSnakeSize;
        }

        public void SetDirection(ConsoleKey key)
        {
            switch (key)
            {
                case (ConsoleKey.UpArrow):
                    if (direction != Directions.down) direction = Directions.up;
                    break;
                case (ConsoleKey.DownArrow):
                    if (direction != Directions.up) direction = Directions.down;
                    break;
                case (ConsoleKey.LeftArrow):
                    if (direction != Directions.right) direction = Directions.left;
                    break;
                case (ConsoleKey.RightArrow):
                    if (direction != Directions.left) direction = Directions.right;
                    break;
                case (ConsoleKey.Escape):
                    game.Pause();
                    break;
            }
        }

        private void MoveSnake(object source, ElapsedEventArgs e)
        {
            switch (direction)
            {
                //Moves the snake depending on the users input
                //If the snake goes outside the borders it will appear on the opposite side
                case Directions.up:
                    y--;
                    if (y <= Game.BorderStart)
                    {
                        y = Game.BorderHeight - 1;
                    }
                    break;
                case Directions.down:
                    y++;
                    if (y >= Game.BorderHeight)
                    {
                        y = Game.BorderStart + 1;
                    }
                    break;
                case Directions.left:
                    x--;
                    if (x <= Game.BorderStart)
                    {
                        x = Game.BorderWidth - 1;
                    }
                    break;
                case Directions.right:
                    x++;
                    if (x >= Game.BorderWidth)
                    {
                        x = Game.BorderStart + 1;
                    }
                    break;
            }
            if (CheckForSelfCollision()) game.GameOver();
            DrawSnake(CheckForFruitCollision());
        }

        private bool CheckForSelfCollision()
        {
            foreach(SnakeBodyPart bodyPart in snake)
            {
                if((bodyPart.x == snake[0].x && bodyPart.y == snake[0].y) && (bodyPart != snake[0]))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckForFruitCollision()
        {
            int numberOfFruits = FruitManager.fruits.Count;
            for (int i = 0; i < numberOfFruits; i++)
            {
                if (i < FruitManager.fruits.Count)
                {
                    if (FruitManager.fruits[i].x == x && FruitManager.fruits[i].y == y)
                    {
                        FruitManager.fruits[i].remove = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public SnakeBodyPart getSnakeHead()
        {
            return snake[0];
        }

        public void DrawSnake(bool isGrowing)
        {
            //If the snake is not growing the last bodypart in the list will be removed
            if (!isGrowing)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(snake[snakeSize - 1].x, snake[snakeSize - 1].y);
                //Removes the bodypart from the screen without having to redraw the hole screen
                Console.WriteLine(" ");
                snake.RemoveAt(snakeSize - 1);
            }
            else
            {
                snakeSize++;
            }

            isGrowing = false;
            snake[0].color = ConsoleColor.White;
            snake.Insert(0, new SnakeBodyPart() { x = x, y = y, color = ConsoleColor.Red });

            for (int i = 0; i < snakeSize; i++)
            {
                Console.ForegroundColor = snake[i].color;
                Console.SetCursorPosition(snake[i].x, snake[i].y);
                Console.WriteLine("o");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(3, 1);
            Console.Write($"Score: {snakeSize - originalSnakeSize}   X: {x} Y: {y} ");
        }

        public void Reset()
        {
            snake.RemoveRange(0, snakeSize);
            snakeSize = originalSnakeSize;
            CreateSnake();
            direction = Directions.right;
        }
    }
}
