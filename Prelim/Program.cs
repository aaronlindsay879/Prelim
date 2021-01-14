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
    /// <summary>
    /// The main program - handles calling all other classes and methods to make the game work
    /// </summary>
    class Program
    {
        /// <summary>
        /// Initialises the program with required operations to have it working
        /// </summary>
        /// <param name="args">Not used</param>
        static void Main(string[] args)
        {
            bool fileLoaded = true;

            //initialises players and grid
            Player player1, player2;
            HexGrid grid;

            //finds choice from user input, quits on Q
            //input of "1" uses default game
            //input of "2" loads from file
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

        /// <summary>
        /// Loads a game from a file
        /// </summary>
        /// <param name="player1">First player</param>
        /// <param name="player2">Second player</param>
        /// <param name="grid">Grid</param>
        /// <returns>Returns bool indicating success</returns>
        public static bool LoadGame(out Player player1, out Player player2, out HexGrid grid)
        {
            //gets file to load
            Console.Write("Enter the name of the file to load: ");
            string fileName = Console.ReadLine();

            //initialises data structures for loading file
            List<string> items;
            string lineFromFile;

            //try and read from the given file
            try
            {
                using (StreamReader myStream = new StreamReader(fileName))
                {
                    //read the first line, get items and generate a player from it
                    lineFromFile = myStream.ReadLine();
                    items = lineFromFile.Split(',').ToList();
                    player1 = new Player(items[0], Convert.ToInt32(items[1]), Convert.ToInt32(items[2]), Convert.ToInt32(items[3]), Convert.ToInt32(items[4]));

                    //do the same for the second line to generate the second player
                    lineFromFile = myStream.ReadLine();
                    items = lineFromFile.Split(',').ToList();
                    player2 = new Player(items[0], Convert.ToInt32(items[1]), Convert.ToInt32(items[2]), Convert.ToInt32(items[3]), Convert.ToInt32(items[4]));

                    //read the third line, which gives info on the size of the grid
                    int gridSize = Convert.ToInt32(myStream.ReadLine());
                    grid = new HexGrid(gridSize);

                    //read the fourth line which gives info on terrain in grid
                    List<string> t = new List<string>(myStream.ReadLine().Split(','));
                    grid.SetUpGridTerrain(t);

                    //finally read all remaining lines to setup grid with info from each line
                    lineFromFile = myStream.ReadLine();
                    while (lineFromFile != null)
                    {
                        //get all the items from the line
                        items = lineFromFile.Split(',').ToList();

                        //if the first item is a "1", add a true piece to the grid at given coordinates
                        if (items[0] == "1")
                        {
                            grid.AddPiece(true, items[1], Convert.ToInt32(items[2]));
                        }
                        //otherwise add a false piece to the grid at given coordinates
                        else
                        {
                            grid.AddPiece(false, items[1], Convert.ToInt32(items[2]));
                        }

                        //repeat until file finished
                        lineFromFile = myStream.ReadLine();
                    }
                }
            }
            catch
            {
                //if file does not load, setup dummy variables and return false (indicating failure)
                Console.WriteLine("File not loaded");
                player1 = new Player("", 0, 0, 0, 0);
                player2 = new Player("", 0, 0, 0, 0);
                grid = new HexGrid(0);

                return false;
            }

            //return true (indicating success)
            return true;
        }

        /// <summary>
        /// A method to setup a default game
        /// </summary>
        /// <param name="player1">Player one</param>
        /// <param name="player2">Player two</param>
        /// <param name="grid">Grid</param>
        public static void SetUpDefaultGame(out Player player1, out Player player2, out HexGrid grid)
        {
            //sets up needed variables, including defautl terrain (t), gridSize, the grid and the players
            List<string> t = new List<string>() {" ", "#", "#", " ", "~", "~", " ", " ", " ", "~", " ", "#", "#", " ", " ", " "
                                                 , " ", " ", "#", "#", "#", "#", "~", "~", "~", "~", "~", " ", "#", " ", "#", " "};
            int gridSize = 8;
            grid = new HexGrid(gridSize);
            player1 = new Player("Player One", 0, 20, 10, 5);
            player2 = new Player("Player Two", 1, 10, 10, 5);

            //setup the grid with the information above
            grid.SetUpGridTerrain(t);
            grid.AddPiece(true, "Baron", 0);
            grid.AddPiece(true, "Serf", 8);
            grid.AddPiece(false, "Baron", 31);
            grid.AddPiece(false, "Serf", 23);
        }

        /// <summary>
        /// Checks if the move command format is valid
        /// </summary>
        /// <param name="items">The move command items</param>
        /// <returns>Bool indicating validity</returns>
        public static bool CheckMoveCommandFormat(List<string> items)
        {
            //checks if there are 3 items, then checks if second and third items are valid integers
            //if all conditions true, return true

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

        /// <summary>
        /// Checks if a standard command is valid
        /// </summary>
        /// <param name="items">Items in command</param>
        /// <returns>Bool indicating validity</returns>
        public static bool CheckStandardCommandFormat(List<string> items)
        {
            //checks if there are 2 items, then checks if second item is valid integer
            //if all conditions true, return true

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

        /// <summary>
        /// Checks if an upgrade command is valid
        /// </summary>
        /// <param name="items">Items in command</param>
        /// <returns>Bool indicating validity</returns>
        public static bool CheckUpgradeCommandFormat(List<string> items)
        {
            //first, checks if there are 3 items
            //then checks if the second item is "LESS" or "PBDS" - if neither, returns false
            //then checks the third item is a valid integer
            //if all conditions true, return true

            int result;
            if (items.Count == 3)
            {
                if (items[1].ToUpper() != "LESS" && items[1].ToUpper() != "PBDS" && items[1].ToUpper() != "WIZ")
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

        /// <summary>
        /// Checks if a generic command is valid
        /// </summary>
        /// <param name="items">Items in the command</param>
        /// <returns>Bool indicating validity</returns>
        public static bool CheckCommandIsValid(List<string> items)
        {
            //first checks if there are any items in the command
            //then uses a switch to check for valid commands
            //if command matches one of the cases, run the validity check for that command
            //if command matches none of the cases, return false

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

        /// <summary>
        /// A method to playe the game
        /// </summary>
        /// <param name="player1">Player one</param>
        /// <param name="player2">Player two</param>
        /// <param name="grid">Grid</param>
        public static void PlayGame(Player player1, Player player2, HexGrid grid)
        {
            //sets up variables needed for game to run
            bool gameOver = false;
            bool player1Turn = true;
            bool validCommand;
            List<string> commands = new List<string>();

            //writes the current state of the players
            Console.WriteLine("Player One current state - " + player1.GetStateString());
            Console.WriteLine("Player Two current state - " + player2.GetStateString());

            do
            {
                //writes the current grid to screen, with a variable to indicate which player is playing
                Console.WriteLine(grid.GetGridAsString(player1Turn));

                //if it's the first players turn, ask the first player what commands to enter
                //otherwise ask the second player
                if (player1Turn)
                    Console.WriteLine(player1.GetName() + " state your three commands, pressing enter after each one.");
                else
                    Console.WriteLine(player2.GetName() + " state your three commands, pressing enter after each one.");

                //take three command inputs from cli, and add to the commands list
                for (int count = 1; count <= 3; count++)
                {
                    Console.Write("Enter command: ");
                    commands.Add(Console.ReadLine().ToLower());
                }

                //for each entered command
                foreach (var c in commands)
                {
                    //split into componenets and check if command is valid
                    List<string> items = new List<string>(c.Split(' '));
                    validCommand = CheckCommandIsValid(items);

                    //if command is invalid, tell user and skip command
                    //if command is valid, run code for command
                    if (!validCommand)
                        Console.WriteLine("Invalid command");
                    else
                    {
                        //setup variables to store changes
                        int fuelChange = 0;
                        int lumberChange = 0;
                        int supplyChange = 0;
                        string summaryOfResult;

                        //if player 1's turn, calculate changes for that player
                        if (player1Turn)
                        {
                            //execute the command to generate a summary, and updated change variables
                            summaryOfResult = grid.ExecuteCommand(items, ref fuelChange, ref lumberChange, ref supplyChange,
                                player1.GetFuel(), player1.GetLumber(), player1.GetPiecesInSupply());

                            //updates lumber and fuel from above command
                            player1.UpdateLumber(lumberChange);
                            player1.UpdateFuel(fuelChange);

                            //if supply change is 1, remove the tile from command
                            if (supplyChange == 1)
                                player1.RemoveTileFromSupply();
                        }
                        else
                        {
                            //same as above but for player 2

                            summaryOfResult = grid.ExecuteCommand(items, ref fuelChange, ref lumberChange, ref supplyChange,
                                player2.GetFuel(), player2.GetLumber(), player2.GetPiecesInSupply());
                            player2.UpdateLumber(lumberChange);
                            player2.UpdateFuel(fuelChange);
                            if (supplyChange == 1)
                            {
                                player2.RemoveTileFromSupply();
                            }
                        }

                        //writes the summary to screen
                        Console.WriteLine(summaryOfResult);
                    }
                }

                //clear the commands, flip the player who's playing and setup new variables
                commands.Clear();
                player1Turn = !player1Turn;
                int player1VPsGained = 0;
                int player2VPsGained = 0;

                //if gameOver == true, destroy pieces and count VPs - adds VPs to both players
                //if gameOver == false, update gameOver to check if baron has been destroyed
                if (gameOver)
                {
                    grid.DestroyPiecesAndCountVPs(ref player1VPsGained, ref player2VPsGained);
                }
                else
                    gameOver = grid.DestroyPiecesAndCountVPs(ref player1VPsGained, ref player2VPsGained);

                //resets specials
                grid.ResetWizards();

                //add the VPs gained to each player
                player1.AddToVPs(player1VPsGained);
                player2.AddToVPs(player2VPsGained);

                //writes the current state to screen, asks to continue
                Console.WriteLine("Player One current state - " + player1.GetStateString());
                Console.WriteLine("Player Two current state - " + player2.GetStateString());
                Console.Write("Press Enter to continue...");
                Console.ReadLine();
            } while (!gameOver || !player1Turn); //repeats until done

            //once game is finished, display grid and write end messages
            Console.WriteLine(grid.GetGridAsString(player1Turn));
            DisplayEndMessages(player1, player2);
        }

        /// <summary>
        /// Method to display end messages
        /// </summary>
        /// <param name="player1">Player one</param>
        /// <param name="player2">Player two</param>
        public static void DisplayEndMessages(Player player1, Player player2)
        {
            //wrties the final state of each player
            Console.WriteLine();
            Console.WriteLine(player1.GetName() + " final state: " + player1.GetStateString());
            Console.WriteLine();
            Console.WriteLine(player2.GetName() + " final state: " + player2.GetStateString());
            Console.WriteLine();

            //display winner depending on who has more VPs
            if (player1.GetVPs() > player2.GetVPs())
            {
                Console.WriteLine(player1.GetName() + " is the winner!");
            }
            else
            {
                Console.WriteLine(player2.GetName() + " is the winner!");
            }
        }

        /// <summary>
        /// Method to display the main menu
        /// </summary>
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