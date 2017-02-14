using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake
{
    class FruitManager
    {
        public static List<Fruit> fruits { get; set; }

        private int maxFruits;
        private Timer fruitTimer;

        public FruitManager(int maxFruits)
        {
            this.maxFruits = maxFruits;
            Init();
        }

        private void Init()
        {
            fruits = new List<Fruit>();

            //Timer to create fruits
            fruitTimer = new System.Timers.Timer();
            fruitTimer.Elapsed += new ElapsedEventHandler(CreateFruitAndDraw);
            fruitTimer.Interval = 5000;
            fruitTimer.Enabled = true;
        }

        private void CreateFruitAndDraw(object sender, ElapsedEventArgs e)
        {
            RemoveFruits();

            if (fruits.Count < maxFruits)
            {
                Random r = new Random();

                int xPos = r.Next(Game.BorderStart + 1, Game.BorderWidth - 1);
                int yPos = r.Next(Game.BorderStart + 1, Game.BorderHeight - 1);

                fruits.Add(new Fruit() { x = xPos, y = yPos, color = ConsoleColor.Green });
            }
            DrawFruits();
        }

        private void DrawFruits()
        {
            int numberOfFruits = fruits.Count;

            for (int i = 0; i < numberOfFruits; i++)
            {
                Console.ForegroundColor = fruits[i].color;
                Console.SetCursorPosition(fruits[i].x, fruits[i].y);
                Console.Write("x");
            }
            RemoveFruits();
        }

        internal void Pause()
        {
            fruitTimer.Elapsed -= new ElapsedEventHandler(CreateFruitAndDraw);
        }

        internal void UnPause()
        {
            fruitTimer.Elapsed += new ElapsedEventHandler(CreateFruitAndDraw);
            DrawFruits();
        }

        public static void RemoveFruits()
        {
            for(int i = 0; i < fruits.Count; i++)
            {
                if (fruits[i].remove) fruits.Remove(fruits[i]);
            }
        }
    }
}
