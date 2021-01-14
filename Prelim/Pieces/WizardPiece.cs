using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class for a wizard
    /// </summary>
    class WizardPiece : Piece
    {
        /// <summary>
        /// Initialises wizard with correct default parameters
        /// </summary>
        /// <param name="player1">Whether owned by player1</param>
        public WizardPiece(bool player1)
            : base(player1)
        {
            pieceType = "W";
            VPValue = 3;
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
            //distance of 1, 1 fuel
            if (distanceBetweenTiles == 1)
                return fuelCostOfMove;

            if (distanceBetweenTiles <= 3 && !specialMovedThisTurn)
            {
                specialMovedThisTurn = true;
                return fuelCostOfMove * 5;
            }

            return -1;
        }
    }
}
