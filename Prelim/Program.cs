//Skeleton Program code for the AQA A Level Paper 1 Summer 2021 examination
//this code should be used in conjunction with the Preliminary Material
//written by the AQA Programmer Team
//developed in the Visual Studio Community Edition programming environment

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HexBaronCS
{
    class Program
    {
        static void Main(string[] args)
        {
            bool fileLoaded = true;
            Player player1, player2;
            HexGrid grid;
            string choice = "";
            while (choice != "Q")
            {
                DisplayMainMenu();
                choice = Console.ReadLine();
                if (choice == "1")
                {
                    SetUpDefaultGame(out player1, out player2, out grid);
                    PlayGame(player1, player2, grid);
                }
                else if (choice == "2")
                {
                    fileLoaded = LoadGame(out player1, out player2, out grid);
                    if (fileLoaded)
                        PlayGame(player1, player2, grid);
                }
            }
        }

        public static bool LoadGame(out Player player1, out Player player2, out HexGrid grid)
        {
            Console.Write("Enter the name of the file to load: ");
            string fileName = Console.ReadLine();
            List<string> items;
            string lineFromFile;
            try
            {
                using (StreamReader myStream = new StreamReader(fileName))
                {
                    lineFromFile = myStream.ReadLine();
                    items = lineFromFile.Split(',').ToList();
                    player1 = new Player(items[0], Convert.ToInt32(items[1]), Convert.ToInt32(items[2]), Convert.ToInt32(items[3]), Convert.ToInt32(items[4]));
                    lineFromFile = myStream.ReadLine();
                    items = lineFromFile.Split(',').ToList();
                    player2 = new Player(items[0], Convert.ToInt32(items[1]), Convert.ToInt32(items[2]), Convert.ToInt32(items[3]), Convert.ToInt32(items[4]));
                    int gridSize = Convert.ToInt32(myStream.ReadLine());
                    grid = new HexGrid(gridSize);
                    List<string> t = new List<string>(myStream.ReadLine().Split(','));
                    grid.SetUpGridTerrain(t);
                    lineFromFile = myStream.ReadLine();
                    while (lineFromFile != null)
                    {
                        items = lineFromFile.Split(',').ToList();
                        if (items[0] == "1")
                        {
                            grid.AddPiece(true, items[1], Convert.ToInt32(items[2]));
                        }
                        else
                        {
                            grid.AddPiece(false, items[1], Convert.ToInt32(items[2]));
                        }
                        lineFromFile = myStream.ReadLine();
                    }
                }
            }
            catch
            {
                Console.WriteLine("File not loaded");
                player1 = new Player("", 0, 0, 0, 0);
                player2 = new Player("", 0, 0, 0, 0);
                grid = new HexGrid(0);
                return false;
            }
            return true;
        }

        public static void SetUpDefaultGame(out Player player1, out Player player2, out HexGrid grid)
        {
            List<string> t = new List<string>() {" ", "#", "#", " ", "~", "~", " ", " ", " ", "~", " ", "#", "#", " ", " ", " "
                                                 , " ", " ", "#", "#", "#", "#", "~", "~", "~", "~", "~", " ", "#", " ", "#", " "};
            int gridSize = 8;
            grid = new HexGrid(gridSize);
            player1 = new Player("Player One", 0, 10, 10, 5);
            player2 = new Player("Player Two", 1, 10, 10, 5);
            grid.SetUpGridTerrain(t);
            grid.AddPiece(true, "Baron", 0);
            grid.AddPiece(true, "Serf", 8);
            grid.AddPiece(false, "Baron", 31);
            grid.AddPiece(false, "Serf", 23);
        }

        public static bool CheckMoveCommandFormat(List<string> items)
        {
            int result;
            if (items.Count == 3)
            {
                for (var count = 1; count <= 2; count++)
                {
                    try
                    {
                        result = Convert.ToInt32(items[count]);
                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool CheckStandardCommandFormat(List<string> items)
        {
            int result;
            if (items.Count == 2)
            {
                try
                {
                    result = Convert.ToInt32(items[1]);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool CheckUpgradeCommandFormat(List<string> items)
        {
            int result;
            if (items.Count == 3)
            {
                if (items[1].ToUpper() != "LESS" && items[1].ToUpper() != "PBDS")
                    return false;
                try
                {
                    result = Convert.ToInt32(items[2]);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool CheckCommandIsValid(List<string> items)
        {
            if (items.Count > 0)
            {
                switch (items[0])
                {
                    case "move":
                        {
                            return CheckMoveCommandFormat(items);
                        }

                    case "dig":
                    case "saw":
                    case "spawn":
                        {
                            return CheckStandardCommandFormat(items);
                        }

                    case "upgrade":
                        {
                            return CheckUpgradeCommandFormat(items);
                        }
                }
            }
            return false;
        }

        public static void PlayGame(Player player1, Player player2, HexGrid grid)
        {
            bool gameOver = false;
            bool player1Turn = true;
            bool validCommand;
            List<string> commands = new List<string>();
            Console.WriteLine("Player One current state - " + player1.GetStateString());
            Console.WriteLine("Player Two current state - " + player2.GetStateString());
            do
            {
                Console.WriteLine(grid.GetGridAsString(player1Turn));
                if (player1Turn)
                    Console.WriteLine(player1.GetName() + " state your three commands, pressing enter after each one.");
                else
                    Console.WriteLine(player2.GetName() + " state your three commands, pressing enter after each one.");
                for (int count = 1; count <= 3; count++)
                {
                    Console.Write("Enter command: ");
                    commands.Add(Console.ReadLine().ToLower());
                }
                foreach (var c in commands)
                {
                    List<string> items = new List<string>(c.Split(' '));
                    validCommand = CheckCommandIsValid(items);
                    if (!validCommand)
                        Console.WriteLine("Invalid command");
                    else
                    {
                        int fuelChange = 0;
                        int lumberChange = 0;
                        int supplyChange = 0;
                        string summaryOfResult;
                        if (player1Turn)
                        {
                            summaryOfResult = grid.ExecuteCommand(items, ref fuelChange, ref lumberChange, ref supplyChange,
                                player1.GetFuel(), player1.GetLumber(), player1.GetPiecesInSupply());
                            player1.UpdateLumber(lumberChange);
                            player1.UpdateFuel(fuelChange);
                            if (supplyChange == 1)
                                player1.RemoveTileFromSupply();
                        }
                        else
                        {
                            summaryOfResult = grid.ExecuteCommand(items, ref fuelChange, ref lumberChange, ref supplyChange,
                                player2.GetFuel(), player2.GetLumber(), player2.GetPiecesInSupply());
                            player2.UpdateLumber(lumberChange);
                            player2.UpdateFuel(fuelChange);
                            if (supplyChange == 1)
                            {
                                player2.RemoveTileFromSupply();
                            }
                        }
                        Console.WriteLine(summaryOfResult);
                    }
                }

                commands.Clear();
                player1Turn = !player1Turn;
                int player1VPsGained = 0;
                int player2VPsGained = 0;
                if (gameOver)
                {
                    grid.DestroyPiecesAndCountVPs(ref player1VPsGained, ref player2VPsGained);
                }
                else
                    gameOver = grid.DestroyPiecesAndCountVPs(ref player1VPsGained, ref player2VPsGained);
                player1.AddToVPs(player1VPsGained);
                player2.AddToVPs(player2VPsGained);
                Console.WriteLine("Player One current state - " + player1.GetStateString());
                Console.WriteLine("Player Two current state - " + player2.GetStateString());
                Console.Write("Press Enter to continue...");
                Console.ReadLine();
            }
            while (!gameOver || !player1Turn);
            Console.WriteLine(grid.GetGridAsString(player1Turn));
            DisplayEndMessages(player1, player2);
        }

        public static void DisplayEndMessages(Player player1, Player player2)
        {
            Console.WriteLine();
            Console.WriteLine(player1.GetName() + " final state: " + player1.GetStateString());
            Console.WriteLine();
            Console.WriteLine(player2.GetName() + " final state: " + player2.GetStateString());
            Console.WriteLine();
            if (player1.GetVPs() > player2.GetVPs())
            {
                Console.WriteLine(player1.GetName() + " is the winner!");
            }
            else
            {
                Console.WriteLine(player2.GetName() + " is the winner!");
            }
        }

        public static void DisplayMainMenu()
        {
            Console.WriteLine("1. Default game");
            Console.WriteLine("2. Load game");
            Console.WriteLine("Q. Quit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");
        }
    }
}