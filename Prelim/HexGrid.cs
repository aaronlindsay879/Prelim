using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class representing the grid
    /// </summary>
    class HexGrid
    {
        protected List<Tile> tiles = new List<Tile>();
        protected List<Piece> pieces = new List<Piece>();
        int size;
        bool player1Turn;

        /// <summary>
        /// Initialises the grid with size <paramref name="n"/>, and default values for other
        /// </summary>
        /// <param name="n">Size of grid</param>
        public HexGrid(int n)
        {
            size = n;
            SetUpTiles();
            SetUpNeighbours();
            player1Turn = true;
        }

        /// <summary>
        /// Sets up the grid terrain given a list <paramref name="listOfTerrain"/>
        /// </summary>
        /// <param name="listOfTerrain">List of terrain</param>
        public void SetUpGridTerrain(List<string> listOfTerrain)
        {
            //for each string in terrain, set the corresponding tiles terrain to it
            for (int count = 0; count < listOfTerrain.Count; count++)
            {
                tiles[count].SetTerrain(listOfTerrain[count]);
            }
        }

        /// <summary>
        /// Adds a piece to the grid, with ownership given by <paramref name="belongsToPlayer1"/>, type <paramref name="typeOfPiece"/> and location <paramref name="location"/>
        /// </summary>
        /// <param name="belongsToPlayer1">Ownership bool, true for player1</param>
        /// <param name="typeOfPiece">Type of piece</param>
        /// <param name="location">Location of piece</param>
        public void AddPiece(bool belongsToPlayer1, string typeOfPiece, int location)
        {
            ///sets the piece to the correct type given by typeOfPiece
            Piece newPiece;
            if (typeOfPiece == "Baron")
            {
                newPiece = new BaronPiece(belongsToPlayer1);
            }
            else if (typeOfPiece == "LESS")
            {
                newPiece = new LESSPiece(belongsToPlayer1);
            }
            else if (typeOfPiece == "PBDS")
            {
                newPiece = new PBDSPiece(belongsToPlayer1);
            }
            else
            {
                newPiece = new Piece(belongsToPlayer1);
            }

            //adds to pieces and set tile
            pieces.Add(newPiece);
            tiles[location].SetPiece(newPiece);
        }

        /// <summary>
        /// Executes a given command
        /// </summary>
        /// <param name="items">Items in command</param>
        /// <param name="fuelChange">Fuel change variable</param>
        /// <param name="lumberChange">Lumber change variable</param>
        /// <param name="supplyChange">Supply change variable</param>
        /// <param name="fuelAvailable">Fuel available</param>
        /// <param name="lumberAvailable">Lumber available</param>
        /// <param name="piecesInSupply">Pieces in supply</param>
        /// <returns>String indicating success</returns>
        public string ExecuteCommand(List<string> items, ref int fuelChange, ref int lumberChange,
                                     ref int supplyChange, int fuelAvailable, int lumberAvailable,
                                     int piecesInSupply)
        {
            //switch based on the first item in command
            switch (items[0])
            {
                case "move":
                    {
                        //if move, execute move command
                        int fuelCost = ExecuteMoveCommand(items, fuelAvailable);
                        if (fuelCost < 0)
                        {
                            return "That move can't be done";
                        }
                        fuelChange = -fuelCost;
                        break;
                    }
                case "saw":
                case "dig":
                    {
                        if (!ExecuteCommandInTile(items, ref fuelChange, ref lumberChange))
                        {
                            return "Couldn't do that";
                        }
                        break;
                    }
                case "spawn":
                    {
                        int lumberCost = ExecuteSpawnCommand(items, lumberAvailable, piecesInSupply);
                        if (lumberCost < 0)
                            return "Spawning did not occur";
                        lumberChange = -lumberCost;
                        supplyChange = 1;
                        break;
                    }
                case "upgrade":
                    {
                        int lumberCost = ExecuteUpgradeCommand(items, lumberAvailable);
                        if (lumberCost < 0)
                            return "Upgrade not possible";
                        lumberChange = -lumberCost;
                        break;
                    }
            }
            return "Command executed";
        }

        /// <summary>
        /// Checks whether an index lies in the grid
        /// </summary>
        /// <param name="tileToCheck">Index of tile</param>
        /// <returns>Boolean indicating validity</returns>
        private bool CheckTileIndexIsValid(int tileToCheck)
        {
            return tileToCheck >= 0 && tileToCheck < tiles.Count;
        }

        /// <summary>
        /// Checks whether piece AND tile index is valid
        /// </summary>
        /// <param name="tileToUse">Tile to check</param>
        /// <returns>Bool indicating validity</returns>
        private bool CheckPieceAndTileAreValid(int tileToUse)
        {
            //check if index is valid
            if (CheckTileIndexIsValid(tileToUse))
            {
                //gets piece and check the data makes sense
                Piece thePiece = tiles[tileToUse].GetPieceInTile();
                if (thePiece != null)
                {
                    if (thePiece.GetBelongsToPlayer1() == player1Turn)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Executes a given command <paramref name="items"/> 
        /// </summary>
        /// <param name="items">Command items</param>
        /// <param name="fuel">Fuel variable</param>
        /// <param name="lumber">Lumber variable</param>
        /// <returns>Bool indicating success</returns>
        private bool ExecuteCommandInTile(List<string> items, ref int fuel, ref int lumber)
        {
            //gets tile from second command item, returns false if invalid position
            int tileToUse = Convert.ToInt32(items[1]);
            if (CheckPieceAndTileAreValid(tileToUse) == false)
            {
                return false;
            }

            //gets the piece in the tile
            Piece thePiece = tiles[tileToUse].GetPieceInTile();

            //capitalises first character of first command item
            items[0] = items[0][0].ToString().ToUpper() + items[0].Substring(1);

            //checks if the piece class has a method given by the first command item
            if (thePiece.HasMethod(items[0]))
            {
                //gets the methods name and type of piece
                string methodToCall = items[0];
                Type t = thePiece.GetType();

                //find the actual method of that name in that type
                System.Reflection.MethodInfo method = t.GetMethod(methodToCall);
                //gets parametesr from the tile
                object[] parameters = { tiles[tileToUse].GetTerrain() };

                //if saw command, invoke the saw command via reflection and add to lumber
                if (items[0] == "Saw")
                {
                    lumber += Convert.ToInt32(method.Invoke(thePiece, parameters));
                }
                //if dig command, invoke dig command via reflection and add to fuel
                else if (items[0] == "Dig")
                {
                    fuel += Convert.ToInt32(method.Invoke(thePiece, parameters));

                    //if fuel is greater than 2 or less than -2, clear the terrain in the tile
                    if (Math.Abs(fuel) > 2)
                    {
                        tiles[tileToUse].SetTerrain(" ");
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Executes a given move command given by <paramref name="items"/>
        /// </summary>
        /// <param name="items">Command</param>
        /// <param name="fuelAvailable">Fuel available</param>
        /// <returns>Int indicating fuel usage, -1 if invalid</returns>
        private int ExecuteMoveCommand(List<string> items, int fuelAvailable)
        {
            //find the start and end tiles, check both tiles are valid
            int startID = Convert.ToInt32(items[1]);
            int endID = Convert.ToInt32(items[2]);
            if (!CheckPieceAndTileAreValid(startID) || !CheckTileIndexIsValid(endID))
            {
                return -1;
            }

            //finds the piece at start, check it's valid piece
            Piece thePiece = tiles[startID].GetPieceInTile();
            if (tiles[endID].GetPieceInTile() != null)
            {
                return -1;
            }

            //find the distance between the two tiles, and calculates fuel usage
            int distance = tiles[startID].GetDistanceToTileT(tiles[endID]);
            int fuelCost = thePiece.CheckMoveIsValid(distance, tiles[startID].GetTerrain(), tiles[endID].GetTerrain());
            //if not enough fuel available (or invalid), return -1
            if (fuelCost == -1 || fuelAvailable < fuelCost)
            {
                return -1;
            }

            //move the piece and return fuelCost
            MovePiece(endID, startID);
            return fuelCost;
        }

        /// <summary>
        /// Executes a spawn command given by <paramref name="items"/>
        /// </summary>
        /// <param name="items">Command</param>
        /// <param name="lumberAvailable">Lumber available</param>
        /// <param name="piecesInSupply">Pieces in supply</param>
        /// <returns>Int representing lumber usage, -1 for invalid</returns>
        private int ExecuteSpawnCommand(List<string> items, int lumberAvailable, int piecesInSupply)
        {
            //find the index of the tile from command
            int tileToUse = Convert.ToInt32(items[1]);
            //checks if piecesInSupply is greater than 1, more than 3 lumber available and tile is valid
            if (piecesInSupply < 1 || lumberAvailable < 3 || !CheckTileIndexIsValid(tileToUse))
            {
                return -1;
            }
            
            //gets the piece and checks if valid
            Piece ThePiece = tiles[tileToUse].GetPieceInTile();
            if (ThePiece != null)
            {
                return -1;
            }

            //makes a bool indicating whether own baron is neighbour and gets a list of neighbours
            bool ownBaronIsNeighbour = false;
            List<Tile> listOfNeighbours = new List<Tile>(tiles[tileToUse].GetNeighbours());
            //iterates through each neighbour
            foreach (var n in listOfNeighbours)
            {
                //gets the piece, if piece isn't null
                ThePiece = n.GetPieceInTile();
                if (ThePiece != null)
                {
                    //checks if neighbour is baron - and if so, set ownBaronIsNeighbour to true and break
                    if (player1Turn && ThePiece.GetPieceType() == "B" || !player1Turn && ThePiece.GetPieceType() == "b")
                    {
                        ownBaronIsNeighbour = true;
                        break;
                    }
                }
            }

            //if baron is not neighbour, invalid move
            if (!ownBaronIsNeighbour)
            {
                return -1;
            }

            //create a new piece and add to the correct tile
            Piece newPiece = new Piece(player1Turn);
            pieces.Add(newPiece);
            tiles[tileToUse].SetPiece(newPiece);

            //return 3 - for lumber cost
            return 3;
        }

        /// <summary>
        /// Executes an upgrade command given by <paramref name="items"/>
        /// </summary>
        /// <param name="items">Command</param>
        /// <param name="lumberAvailable">Lumber available</param>
        /// <returns>Int representing lumber cost, -1 for invalid</returns>
        private int ExecuteUpgradeCommand(List<string> items, int lumberAvailable)
        {
            //gets the tile, checks if it's valid, more than 5 lumber available and piece is either pbds or less
            int tileToUse = Convert.ToInt32(items[2]);
            if (!CheckPieceAndTileAreValid(tileToUse) || lumberAvailable < 5 || !(items[1] == "pbds" || items[1] == "less"))
            {
                return -1;
            }
            else
            {
                //fetches piece from grid
                Piece thePiece = tiles[tileToUse].GetPieceInTile();

                //if piece isn't "S", invalid
                if (thePiece.GetPieceType().ToUpper() != "S")
                {
                    return -1;
                }

                //destroy the piece and create a new one given by second command item
                thePiece.DestroyPiece();
                if (items[1] == "pbds")
                {
                    thePiece = new PBDSPiece(player1Turn);
                }
                else
                {
                    thePiece = new LESSPiece(player1Turn);
                }

                //add the new piece, and set the tile to it
                pieces.Add(thePiece);
                tiles[tileToUse].SetPiece(thePiece);

                //return 5 - for lumber cost
                return 5;
            }
        }

        /// <summary>
        /// Sets up the tiles
        /// </summary>
        private void SetUpTiles()
        {
            /* This is a long method which simply creates a grid, using the follow coordinate system: 
             *   - x coordinate is the 0-indexed column starting from the left
             *   - y coordinate is a 0-indexed decreasing diagonal coordinate, going up diagonally from the left. 
             *   - z coordinate is another 0-indexed diagonal coordinate, going down diagonally from the left.
             * See Images/hexgrid.png for an example
             */

            //sets up starting variables
            int evenStartY = 0;
            int evenStartZ = 0;
            int oddStartZ = 0;
            int oddStartY = -1;
            int x, y, z;

            /* for 1..(size/2)
             * this is it needs to make both the even and odd rows, so if it goes up to half of size
             * it can create even and odd to create the correct size.
             */
            for (int count = 1; count <= size / 2; count++)
            {
                //prepares variables to start on left
                y = evenStartY;
                z = evenStartZ;

                /* for x..(size - 2) in increments of 2
                 * this creates a row of tiles starting straight from the left, such as the first, third, fifth, etc. row
                 */
                for (x = 0; x <= size - 2; x += 2)
                {
                    //create a new tile at the x, y and z coords
                    Tile tempTile = new Tile(x, y, z);

                    //add the tile to tiles, and decrement y and z by 1
                    tiles.Add(tempTile);
                    y -= 1;
                    z -= 1;
                }

                //changes the variables to prepare for rows shifted by one
                evenStartZ += 1;
                evenStartY -= 1;
                y = oddStartY;
                z = oddStartZ;

                /* for x..(size - 1) starting from 1 in increments of 2
                 * this creates a row of tiles starting straight one away from the left, such as the second, fourth, etc. row
                 */
                for (x = 1; x <= size - 1; x += 2)
                {
                    Tile tempTile = new Tile(x, y, z);
                    tiles.Add(tempTile);
                    y -= 1;
                    z -= 1;
                }

                //prepares variables to prepare for next row
                oddStartZ += 1;
                oddStartY -= 1;
            }
        }

        /// <summary>
        /// Sets up the neighbours
        /// </summary>
        private void SetUpNeighbours()
        {
            //for each tile
            foreach (var fromTile in tiles)
            {
                //for all other tiles
                foreach (var toTile in tiles)
                {
                    //if distance is 1, add to list of neighbours
                    if (fromTile.GetDistanceToTileT(toTile) == 1)
                    {
                        fromTile.AddToNeighbours(toTile);
                    }
                }
            }
        }

        /// <summary>
        /// Destroys pieces where required and counts VPs
        /// </summary>
        /// <param name="player1VPs">Player 1 VP variable</param>
        /// <param name="player2VPs">Player 2 VP variable</param>
        /// <returns>Bool indicating if baron has been destroyed</returns>
        public bool DestroyPiecesAndCountVPs(ref int player1VPs, ref int player2VPs)
        {
            //creates a bool indicating if baron has been destroyed, and a list of tiles where pieces have been destroyed
            bool baronDestroyed = false;
            List<Tile> listOfTilesContainingDestroyedPieces = new List<Tile>();

            //for every tile
            foreach (var t in tiles)
            {
                //if the piece isn't null
                if (t.GetPieceInTile() != null)
                {
                    //get a list of neighbours, and increment number of connections for each neighbour with a piece
                    List<Tile> listOfNeighbours = new List<Tile>(t.GetNeighbours());
                    int noOfConnections = 0;
                    foreach (var n in listOfNeighbours)
                    {
                        if (n.GetPieceInTile() != null)
                        {
                            noOfConnections += 1;
                        }
                    }

                    //get the piece in the current tile
                    Piece thePiece = t.GetPieceInTile();

                    //if the number of connections is greater than the number needed to destroy
                    if (noOfConnections >= thePiece.GetConnectionsNeededToDestroy())
                    {
                        //destroy the tile, and if baron destroyed indicate that with variable
                        thePiece.DestroyPiece();
                        if (thePiece.GetPieceType().ToUpper() == "B")
                            baronDestroyed = true;

                        //add destroyed tile to list, and award VPs to correct player
                        listOfTilesContainingDestroyedPieces.Add(t);
                        if (thePiece.GetBelongsToPlayer1())
                        {
                            player2VPs += thePiece.GetVPs();
                        }
                        else
                        {
                            player1VPs += thePiece.GetVPs();
                        }
                    }
                }
            }

            //for each tile that was destroyed, set piece to null
            foreach (var t in listOfTilesContainingDestroyedPieces)
            {
                t.SetPiece(null);
            }

            //return whether baron destroyed
            return baronDestroyed;
        }

        /// <summary>
        /// Formats the grid as a string
        /// </summary>
        /// <param name="p1Turn">Whether it's player 1s turn</param>
        /// <returns>Formatted string</returns>
        public string GetGridAsString(bool p1Turn)
        {
            //TODO: remaining comments

            int listPositionOfTile = 0;
            player1Turn = p1Turn;
            string gridAsString = CreateTopLine() + CreateEvenLine(true, ref listPositionOfTile);
            listPositionOfTile += 1;
            gridAsString += CreateOddLine(ref listPositionOfTile);
            for (var count = 1; count <= size - 2; count += 2)
            {
                listPositionOfTile += 1;
                gridAsString += CreateEvenLine(false, ref listPositionOfTile);
                listPositionOfTile += 1;
                gridAsString += CreateOddLine(ref listPositionOfTile);
            }
            return gridAsString + CreateBottomLine();
        }

        /// <summary>
        /// Moves a piece from a startIndex <paramref name="oldIndex"/> to an endIndex <paramref name="newIndex"/>
        /// </summary>
        /// <param name="newIndex">New index to move to</param>
        /// <param name="oldIndex">Old index to move from</param>
        private void MovePiece(int newIndex, int oldIndex)
        {
            //moves the piece
            tiles[newIndex].SetPiece(tiles[oldIndex].GetPieceInTile());
            tiles[oldIndex].SetPiece(null);
        }

        /// <summary>
        /// Gets the type of piece in a given tile ID
        /// </summary>
        /// <param name="ID">ID for tile</param>
        /// <returns>String representing type of piece</returns>
        public string GetPieceTypeInTile(int ID)
        {
            //get the piece, if not null return the type of it
            Piece thePiece = tiles[ID].GetPieceInTile();
            if (thePiece == null)
            {
                return " ";
            }
            else
            {
                return thePiece.GetPieceType();
            }
        }

        /// <summary>
        /// Creates a string with the needed number of bottom hex's
        /// </summary>
        /// <returns>Partial formatted string</returns>
        private string CreateBottomLine()
        {
            string line = "   ";
            for (var count = 1; count <= size / 2; count++)
            {
                line += @" \__/ ";
            }
            return line + Environment.NewLine;
        }

        /// <summary>
        /// Creates a string with the needed number of top hex's
        /// </summary>
        /// <returns>Partial formatted string</returns>
        private string CreateTopLine()
        {
            string line = Environment.NewLine + "  ";
            for (var count = 1; count <= size / 2; count++)
            {
                line += "__    ";
            }
            return line + Environment.NewLine;
        }

        private string CreateOddLine(ref int listPositionOfTile)
        {
            string line = "";
            for (var count = 1; count <= size / 2; count++)
            {
                if (count > 1 & count < size / 2)
                {
                    line += GetPieceTypeInTile(listPositionOfTile) + @"\__/";
                    listPositionOfTile += 1;
                    line += tiles[listPositionOfTile].GetTerrain();
                }
                else if (count == 1)
                {
                    line += @" \__/" + tiles[listPositionOfTile].GetTerrain();
                }
            }
            line += GetPieceTypeInTile(listPositionOfTile) + @"\__/";
            listPositionOfTile += 1;
            if (listPositionOfTile < tiles.Count)
            {
                line += tiles[listPositionOfTile].GetTerrain() + GetPieceTypeInTile(listPositionOfTile) + @"\" + Environment.NewLine;
            }
            else
            {
                line += @"\" + Environment.NewLine;
            }
            return line;
        }

        private string CreateEvenLine(bool firstEvenLine, ref int listPositionOfTile)
        {
            string line = " /" + tiles[listPositionOfTile].GetTerrain();
            for (var count = 1; count <= size / 2 - 1; count++)
            {
                line += GetPieceTypeInTile(listPositionOfTile);
                listPositionOfTile += 1;
                line += @"\__/" + tiles[listPositionOfTile].GetTerrain();
            }
            if (firstEvenLine)
            {
                line += GetPieceTypeInTile(listPositionOfTile) + @"\__" + Environment.NewLine;
            }
            else
            {
                line += GetPieceTypeInTile(listPositionOfTile) + @"\__/" + Environment.NewLine;
            }
            return line;
        }
    }
}
