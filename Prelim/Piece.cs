using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class representing an individual piece on the board
    /// </summary>
    class Piece
    {
        protected bool destroyed, belongsToPlayer1;
        protected int fuelCostOfMove, VPValue, connectionsToDestroy;
        protected string pieceType;

        /// <summary>
        /// Initialises the player with default values and ownership given by <paramref name="player1"/>
        /// </summary>
        /// <param name="player1">Whether owned by player1</param>
        public Piece(bool player1)
        {
            fuelCostOfMove = 1;
            belongsToPlayer1 = player1;
            destroyed = false;
            pieceType = "S";
            VPValue = 1;
            connectionsToDestroy = 2;
        }

        /// <summary>
        /// Gets the VPs awarded when destroyed
        /// </summary>
        /// <returns>VPs</returns>
        public virtual int GetVPs()
        {
            return VPValue;
        }

        /// <summary>
        /// Gets whether the piece belongs to player1
        /// </summary>
        /// <returns>Ownership boolean</returns>
        public virtual bool GetBelongsToPlayer1()
        {
            return belongsToPlayer1;
        }

        /// <summary>
        /// Checks whether the move is valid, given <paramref name="distanceBetweenTiles"/>, <paramref name="startTerrain"/> and <paramref name="endTerrain"/>
        /// </summary>
        /// <param name="distanceBetweenTiles">Distance between the two tiles</param>
        /// <param name="startTerrain">Start terrain</param>
        /// <param name="endTerrain">End terrain</param>
        /// <returns>An int representing fuel usage, -1 for invalid move</returns>
        public virtual int CheckMoveIsValid(int distanceBetweenTiles, string startTerrain, string endTerrain)
        {
            //if the distance is 1 and either tile is "~", return double fuel usage
            //if the distance is 1 and neither tile is "~", return normal fuel usage
            //otherwise return -1

            if (distanceBetweenTiles == 1)
            {
                if (startTerrain == "~" || endTerrain == "~")
                {
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
        /// Method to check if this class has a given method <paramref name="methodName"/>
        /// </summary>
        /// <param name="methodName">Method to check for</param>
        /// <returns>Bool indicating if class contains method</returns>
        public virtual bool HasMethod(string methodName)
        {
            return this.GetType().GetMethod(methodName) != null;
        }

        /// <summary>
        /// Gets the connections needed to destroy piece
        /// </summary>
        /// <returns>connections needed to destroy</returns>
        public virtual int GetConnectionsNeededToDestroy()
        {
            return connectionsToDestroy;
        }

        /// <summary>
        /// Gets the piece type, capitalised for player1 and lowercase for player2
        /// </summary>
        /// <returns>Piece type</returns>
        public virtual string GetPieceType()
        {
            if (belongsToPlayer1)
            {
                return pieceType;
            }
            else
            {
                return pieceType.ToLower();
            }
        }

        /// <summary>
        /// Destroys the piece
        /// </summary>
        public virtual void DestroyPiece()
        {
            destroyed = true;
        }
    }
}
