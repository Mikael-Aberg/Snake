using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Snake
{
    class HighScore
    {
        public static bool SelectingName = false;

        private List<Tuple<string, int>> highScoreList = new List<Tuple<string, int>>();
        private string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "_" };
        private int maxHighScorers = 10;

        public HighScore()
        {
            if (File.Exists(@"highScore.dat"))
            {
                LoadList();
            }
            else {
                SaveList();
            }
        }

        private void LoadList()
        {
            FileStream inStr = new FileStream(@"highScore.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            highScoreList = bf.Deserialize(inStr) as List<Tuple<string, int>>;
            inStr.Close();
        }

        private void SaveList()
        {
            FileStream stream = new FileStream(@"highScore.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, highScoreList);
            stream.Close();
        }

        public void AddToHighScore(int score)
        {
            if (CheckIfHighScore(score))
            {
                SelectingName = true;
                string name = GetScorerName();
                SelectingName = false;
                int counter = 0;
                foreach (Tuple<string, int> scorer in highScoreList)
                {
                    if (scorer.Item2 < score) break;
                    else { counter++; }
                }

                if (highScoreList.Count < maxHighScorers) highScoreList.Add(new Tuple<string, int>(name, score));
                else
                {
                    highScoreList.Insert(counter, new Tuple<string, int>(name, score));
                    highScoreList.RemoveAt(highScoreList.Count - 1);
                }

                highScoreList.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                SaveList();
            }
        }

        private string GetScorerName()
        {
            bool looping = true;
            int selected = 0;
            int row = 0;
            int maxSelected = letters.Length - 1;
            string name = "";
            DrawNameSelectedName(selected, row, name);
            do
            {
                while (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case (ConsoleKey.UpArrow):
                            selected++;
                            if (selected > maxSelected) selected = 0;
                            break;
                        case (ConsoleKey.DownArrow):
                            selected--;
                            if (selected < 0) selected = maxSelected;
                            break;
                        case (ConsoleKey.Enter):
                            if (row > 1) {  return name += letters[selected]; };
                            name += letters[selected];
                            row++;
                            break; 
                    }
                    DrawNameSelectedName(selected, row, name);
                }
            } while (looping);

            return null;
        }

        private void DrawNameSelectedName(int selected, int row, string name)
        {
            Console.SetCursorPosition((Game.BorderWidth - Game.BorderStart) / 2, Game.BorderStart + 5);

            int counter = 0;
            foreach(char letter in name)
            {
                Console.Write(letter + " ");
                counter++;
            }

            while(counter < 3)
            {
                if(counter == row)
                {
                    Console.Write(letters[selected] + " ");
                }
                else
                {
                    Console.Write(" ");
                }
                counter++;
            }

            counter = 0;
            Console.SetCursorPosition((Game.BorderWidth - Game.BorderStart) / 2, Game.BorderStart + 6);
            while (counter < 3)
            {
                Console.Write("_" + " ");
                counter++;
            }
        }

        public void DrawHighScoreList()
        {
            int counter = 0;
            foreach(Tuple<string,int> scorer in highScoreList)
            {
                Console.SetCursorPosition((Game.BorderWidth - 10), (Game.BorderStart + 1) + counter);
                Console.Write($"{counter + 1}. {scorer.Item1} {scorer.Item2}");
                counter++;
            }
        }

        public bool CheckIfHighScore(int score)
        {
            if (highScoreList.Count < maxHighScorers) return true;

            foreach(Tuple<string, int> scorer in highScoreList)
            {
                if (scorer.Item2 > score) return true;
            }
            return false;
        }
    }
}
