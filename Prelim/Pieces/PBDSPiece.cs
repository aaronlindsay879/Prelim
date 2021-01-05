using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class representing a PBDS piece
    /// </summary>
    class PBDSPiece : Piece
    {
        static Random rNoGen = new Random();

        /// <summary>
        /// Initialises PBDS with correct default parameters
        /// </summary>
        /// <param name="player1">Whether owned by player1</param>
        public PBDSPiece(bool player1)
            : base(player1)
        {
            pieceType = "P";
            VPValue = 2;
            fuelCostOfMove = 2;
        }

        /// <summary>
        /// Checks if a given move is valid
        /// </summary>
        /// <param name="distanceBetweenTiles">Distance between tiles</param>
        /// <param name="startTerrain">Start terrain</param>
        /// <param name="endTerrain">End terrain</param>
        /// <returns>Fuel cost, -1 if null</returns>
        public override int CheckMoveIsValid(int distanceBetweenTiles, string startTerrain, string endTerrain)
        {
            //invalid move if distance is not 1, or start terrain is ~
            if (distanceBetweenTiles != 1 || startTerrain == "~")
            {
                return -1;
            }
            return fuelCostOfMove;
        }

        /// <summary>
        /// Calculates fuel gained from dig operation
        /// </summary>
        /// <param name="terrain">Terrain</param>
        /// <returns>Fuel gained</returns>
        public int Dig(string terrain)
        {
            //if terrain is not ~, always 0
            if (terrain != "~")
            {
                return 0;
            }

            //90% chance of 1, 10% of 5
            if (rNoGen.NextDouble() < 0.9)
            {
                return 1;
            }
            else
            {
                return 5;
            }
        }
    }
}
