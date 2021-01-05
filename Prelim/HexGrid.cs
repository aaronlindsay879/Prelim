﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class HexGrid
    {
        protected List<Tile> tiles = new List<Tile>();
        protected List<Piece> pieces = new List<Piece>();
        int size;
        bool player1Turn;

        public HexGrid(int n)
        {
            size = n;
            SetUpTiles();
            SetUpNeighbours();
            player1Turn = true;
        }

        public void SetUpGridTerrain(List<string> listOfTerrain)
        {
            for (int count = 0; count < listOfTerrain.Count; count++)
            {
                tiles[count].SetTerrain(listOfTerrain[count]);
            }
        }

        public void AddPiece(bool belongsToPlayer1, string typeOfPiece, int location)
        {
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
            pieces.Add(newPiece);
            tiles[location].SetPiece(newPiece);
        }

        public string ExecuteCommand(List<string> items, ref int fuelChange, ref int lumberChange,
                                     ref int supplyChange, int fuelAvailable, int lumberAvailable,
                                     int piecesInSupply)
        {
            switch (items[0])
            {
                case "move":
                    {
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

        private bool CheckTileIndexIsValid(int tileToCheck)
        {
            return tileToCheck >= 0 && tileToCheck < tiles.Count;
        }

        private bool CheckPieceAndTileAreValid(int tileToUse)
        {
            if (CheckTileIndexIsValid(tileToUse))
            {
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

        private bool ExecuteCommandInTile(List<string> items, ref int fuel, ref int lumber)
        {
            int tileToUse = Convert.ToInt32(items[1]);
            if (CheckPieceAndTileAreValid(tileToUse) == false)
            {
                return false;
            }
            Piece thePiece = tiles[tileToUse].GetPieceInTile();
            items[0] = items[0][0].ToString().ToUpper() + items[0].Substring(1);
            if (thePiece.HasMethod(items[0]))
            {
                string methodToCall = items[0];
                Type t = thePiece.GetType();
                System.Reflection.MethodInfo method = t.GetMethod(methodToCall);
                object[] parameters = { tiles[tileToUse].GetTerrain() };
                if (items[0] == "Saw")
                {
                    lumber += Convert.ToInt32(method.Invoke(thePiece, parameters));
                }
                else if (items[0] == "Dig")
                {
                    fuel += Convert.ToInt32(method.Invoke(thePiece, parameters));
                    if (Math.Abs(fuel) > 2)
                    {
                        tiles[tileToUse].SetTerrain(" ");
                    }
                }
                return true;
            }
            return false;
        }

        private int ExecuteMoveCommand(List<string> items, int fuelAvailable)
        {
            int startID = Convert.ToInt32(items[1]);
            int endID = Convert.ToInt32(items[2]);
            if (!CheckPieceAndTileAreValid(startID) || !CheckTileIndexIsValid(endID))
            {
                return -1;
            }
            Piece thePiece = tiles[startID].GetPieceInTile();
            if (tiles[endID].GetPieceInTile() != null)
            {
                return -1;
            }
            int distance = tiles[startID].GetDistanceToTileT(tiles[endID]);
            int fuelCost = thePiece.CheckMoveIsValid(distance, tiles[startID].GetTerrain(), tiles[endID].GetTerrain());
            if (fuelCost == -1 || fuelAvailable < fuelCost)
            {
                return -1;
            }
            MovePiece(endID, startID);
            return fuelCost;
        }

        private int ExecuteSpawnCommand(List<string> items, int lumberAvailable, int piecesInSupply)
        {
            int tileToUse = Convert.ToInt32(items[1]);
            if (piecesInSupply < 1 || lumberAvailable < 3 || !CheckTileIndexIsValid(tileToUse))
            {
                return -1;
            }
            Piece ThePiece = tiles[tileToUse].GetPieceInTile();
            if (ThePiece != null)
            {
                return -1;
            }
            bool ownBaronIsNeighbour = false;
            List<Tile> listOfNeighbours = new List<Tile>(tiles[tileToUse].GetNeighbours());
            foreach (var n in listOfNeighbours)
            {
                ThePiece = n.GetPieceInTile();
                if (ThePiece != null)
                {
                    if (player1Turn && ThePiece.GetPieceType() == "B" || !player1Turn && ThePiece.GetPieceType() == "b")
                    {
                        ownBaronIsNeighbour = true;
                        break;
                    }
                }
            }
            if (!ownBaronIsNeighbour)
            {
                return -1;
            }
            Piece newPiece = new Piece(player1Turn);
            pieces.Add(newPiece);
            tiles[tileToUse].SetPiece(newPiece);
            return 3;
        }

        private int ExecuteUpgradeCommand(List<string> items, int lumberAvailable)
        {
            int tileToUse = Convert.ToInt32(items[2]);
            if (!CheckPieceAndTileAreValid(tileToUse) || lumberAvailable < 5 || !(items[1] == "pbds" || items[1] == "less"))
            {
                return -1;
            }
            else
            {
                Piece thePiece = tiles[tileToUse].GetPieceInTile();
                if (thePiece.GetPieceType().ToUpper() != "S")
                {
                    return -1;
                }
                thePiece.DestroyPiece();
                if (items[1] == "pbds")
                {
                    thePiece = new PBDSPiece(player1Turn);
                }
                else
                {
                    thePiece = new LESSPiece(player1Turn);
                }
                pieces.Add(thePiece);
                tiles[tileToUse].SetPiece(thePiece);
                return 5;
            }
        }

        private void SetUpTiles()
        {
            int evenStartY = 0;
            int evenStartZ = 0;
            int oddStartZ = 0;
            int oddStartY = -1;
            int x, y, z;
            for (int count = 1; count <= size / 2; count++)
            {
                y = evenStartY;
                z = evenStartZ;
                for (x = 0; x <= size - 2; x += 2)
                {
                    Tile tempTile = new Tile(x, y, z);
                    tiles.Add(tempTile);
                    y -= 1;
                    z -= 1;
                }
                evenStartZ += 1;
                evenStartY -= 1;
                y = oddStartY;
                z = oddStartZ;
                for (x = 1; x <= size - 1; x += 2)
                {
                    Tile tempTile = new Tile(x, y, z);
                    tiles.Add(tempTile);
                    y -= 1;
                    z -= 1;
                }
                oddStartZ += 1;
                oddStartY -= 1;
            }
        }

        private void SetUpNeighbours()
        {
            foreach (var fromTile in tiles)
            {
                foreach (var toTile in tiles)
                {
                    if (fromTile.GetDistanceToTileT(toTile) == 1)
                    {
                        fromTile.AddToNeighbours(toTile);
                    }
                }
            }
        }

        public bool DestroyPiecesAndCountVPs(ref int player1VPs, ref int player2VPs)
        {
            bool baronDestroyed = false;
            List<Tile> listOfTilesContainingDestroyedPieces = new List<Tile>();
            foreach (var t in tiles)
            {
                if (t.GetPieceInTile() != null)
                {
                    List<Tile> listOfNeighbours = new List<Tile>(t.GetNeighbours());
                    int noOfConnections = 0;
                    foreach (var n in listOfNeighbours)
                    {
                        if (n.GetPieceInTile() != null)
                        {
                            noOfConnections += 1;
                        }
                    }
                    Piece thePiece = t.GetPieceInTile();
                    if (noOfConnections >= thePiece.GetConnectionsNeededToDestroy())
                    {
                        thePiece.DestroyPiece();
                        if (thePiece.GetPieceType().ToUpper() == "B")
                            baronDestroyed = true;
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
            foreach (var t in listOfTilesContainingDestroyedPieces)
            {
                t.SetPiece(null);
            }
            return baronDestroyed;
        }

        public string GetGridAsString(bool p1Turn)
        {
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

        private void MovePiece(int newIndex, int oldIndex)
        {
            tiles[newIndex].SetPiece(tiles[oldIndex].GetPieceInTile());
            tiles[oldIndex].SetPiece(null);
        }

        public string GetPieceTypeInTile(int ID)
        {
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

        private string CreateBottomLine()
        {
            string line = "   ";
            for (var count = 1; count <= size / 2; count++)
            {
                line += @" \__/ ";
            }
            return line + Environment.NewLine;
        }

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