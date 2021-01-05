using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class representing a LESS piece
    /// </summary>
    class LESSPiece : Piece
    {
        /// <summary>
        /// Initialises LESS with correct default parameters
        /// </summary>
        /// <param name="player1">Whether owned by player1</param>
        public LESSPiece(bool player1)
            : base(player1)
        {
            pieceType = "L";
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
            //only allow move if distance 1 and start terrain is not #
            if (distanceBetweenTiles == 1 && startTerrain != "#")
            {
                if (startTerrain == "~" || endTerrain == "~")
                {
                    //if moving from ~ or to ~, double fuel cost
                    return fuelCostOfMove * 2;
                }
                else
                {
                    return fuelCostOfMove;
                }

            }
            return -1;
        }

        /// <summary>
        /// Calculates lumber gained from saw operation
        /// </summary>
        /// <param name="terrain">Terrain</param>
        /// <returns>Lumber gained</returns>
        public int Saw(string terrain)
        {
            if (terrain != "#")
            {
                return 0;
            }
            return 1;
        }

    }
}
