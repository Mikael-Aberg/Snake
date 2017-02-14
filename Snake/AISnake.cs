using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake
{
    class AISnake
    {
        private int x;
        private int y;

        private Fruit targetFruit;

        private bool isLookingForFruit = true;

        List<SnakeBodyPart> snake = new List<SnakeBodyPart>();

        Directions direction;

        private int snakeSize { get; set; }
        private int originalSnakeSize;
        private Timer drawTimer;

        private Game game;

        public AISnake(int snakeSize, Game game)
        {
            originalSnakeSize = snakeSize;
            this.snakeSize = originalSnakeSize;
            this.game = game;
            direction = Directions.right;
            targetFruit = new Fruit();
            targetFruit.x = 500;
            targetFruit.y = 500;
            CreateSnake();
            StartTimer();
        }

        private void StartTimer()
        {
            drawTimer = new System.Timers.Timer();
            drawTimer.Elapsed += new ElapsedEventHandler(ChooseDirection);
            drawTimer.Interval = 200;
            drawTimer.Enabled = true;
        }

        private void CreateSnake()
        {
            //Creates the snake at top left corner of the border
            x = Game.BorderStart + 1;
            y = Game.BorderHeight - 1;

            //Creates all parts of the snake, starting with the head
            for (int i = originalSnakeSize; i > 0; i--)
            {
                SnakeBodyPart snakeBody = new SnakeBodyPart();
                snakeBody.x = i;
                snakeBody.y = 0;
                snake.Add(snakeBody);
            }
        }

        private void ChooseDirection(object source, ElapsedEventArgs e)
        {
            if(isLookingForFruit) GetClosestFruit();

            // Right
            if (targetFruit.x > x && direction != Directions.left) { direction = Directions.right; }
            // Left
            else if (targetFruit.x < x && direction != Directions.right) { direction = Directions.left; }
            // Up
            else if (targetFruit.y < y && direction != Directions.down) { direction = Directions.up; }
            // Down
            else if (targetFruit.y > y && direction != Directions.up) { direction = Directions.down; }

            MoveSnake();
        }

        private void GetClosestFruit()
        {
            int closestDistance = 500;


            foreach (Fruit fruit in FruitManager.fruits)
            {
                int dX = fruit.x - x;
                int dY = fruit.y - y;
                int distance = dX * dX + dY * dY;

                if (distance < closestDistance)
                {
                    targetFruit = fruit;
                    closestDistance = distance;
                }

                    //if (fruit.x - x < closestFruit.x && fruit.y - y < closestFruit.y && fruit != targetFruit)
                    //{
                    //    targetFruit = fruit;
                    //}
            }

            isLookingForFruit = false;
        }

        private void MoveSnake()
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
                        FruitManager.RemoveFruits();
                        isLookingForFruit = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckForSelfCollision()
        {
            foreach (SnakeBodyPart bodyPart in snake)
            {
                if ((bodyPart.x == snake[0].x && bodyPart.y == snake[0].y) && (bodyPart != snake[0]))
                {
                    return true;
                }
            }
            return false;
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
            snake.Insert(0, new SnakeBodyPart() { x = x, y = y, color = ConsoleColor.Cyan });

            for (int i = 0; i < snakeSize; i++)
            {
                Console.ForegroundColor = snake[i].color;
                Console.SetCursorPosition(snake[i].x, snake[i].y);
                Console.WriteLine("o");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(3, 1);
            Console.Write($"Score: {snakeSize - originalSnakeSize}   X: {x} Y: {y}    |    TargetX: {targetFruit.x} TargetY: {targetFruit.y}     |  Number of fruits: {FruitManager.fruits.Count}   ");
        }
    }
}
