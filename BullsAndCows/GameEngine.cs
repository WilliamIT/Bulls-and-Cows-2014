﻿namespace BullsAndCows
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class GameEngine
    {
        private readonly Scoreboard scoreboard = new Scoreboard();
        private readonly Random randomNumber = new Random();

        public string Ch { get; set; }

        public int Number { get; set; }

        public bool NotCheated { get; set; }

        public int Attempts { get; set; }

        public void StartNewGame()
        {
            Help.GameInstructions();
            this.Number = 1111;////randomNumber.Next(1000, 10000);
            this.Attempts = 1;
            this.NotCheated = true;
            this.Ch = "XXXX";
        }

        public bool ReadAction()
        {
            Console.WriteLine("Enter your guess or command: ");

            string line = Console.ReadLine().Trim();
            Regex patt = new Regex("[1-9][0-9][0-9][0-9]");

            switch (line)
            {
                case "top":
                    this.scoreboard.ShowScoreboard();
                    break;
                case "restart":
                    this.StartNewGame();
                    break;
                case "help":
                    Help.Cheat(this.Number, this.Ch, this.randomNumber);
                    break;
                case "exit":
                    ////return false;
                    Environment.Exit(0);
                    break;
                default:

                    if (patt.IsMatch(line))
                    {
                        int guess = int.Parse(line);
                        this.ProcessGuess(guess);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a 4-digit number or");
                        Console.WriteLine("one of the commands: 'top', 'restart', 'help' or 'exit'.");
                    }

                    break;
            }

            return true;
        }

        public void ProcessWin()
        {
            Console.WriteLine("Congratulations! You guessed the secret number in {0} attempts.", this.Attempts);

            if (this.NotCheated)
            {
                this.scoreboard.AddToScoreboard(this.Attempts);
            }

            this.StartNewGame();
        }

        public void ProcessGuess(int guess)
        {
            if (guess == this.Number)
            {
                this.ProcessWin();
            }
            else
            {
                string snum = this.Number.ToString(), sguess = guess.ToString();
                bool[] isBull = new bool[4];
                int bulls = 0, cows = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (isBull[i] = snum[i] == sguess[i])
                    {
                        bulls++;
                    }
                }

                int[] digs = new int[10];

                for (int d = 0; d < 10; d++)
                {
                    digs[d] = 0;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (!isBull[i])
                    {
                        digs[snum[i] - '0']++;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (!isBull[i])
                    {
                        if (digs[sguess[i] - '0'] > 0)
                        {
                            cows++;
                            digs[sguess[i] - '0']--;
                        }
                    }
                }

                Console.WriteLine("\nWrong number! Bulls: {0}, Cows: {1}", bulls, cows);
                this.Attempts++;
            }
        }
    }
}