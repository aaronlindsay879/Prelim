using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class for a baron
    /// </summary>
    class BaronPiece : Piece
    {
        /// <summary>
        /// Initialises baron with correct default parameters
        /// </summary>
        /// <param name="player1">Whether owned by player1</param>
        public BaronPiece(bool player1)
            : base(player1)
        {
            pieceType = "B";
            VPValue = 10;
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
            //only allow moves with distance of 1
            if (distanceBetweenTiles == 1)
            {
                return fuelCostOfMove;
            }
            return -1;
        }
    }
}
