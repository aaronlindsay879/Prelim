using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class Piece
    {
        protected bool destroyed, belongsToPlayer1;
        protected int fuelCostOfMove, VPValue, connectionsToDestroy;
        protected string pieceType;

        public Piece(bool player1)
        {
            fuelCostOfMove = 1;
            belongsToPlayer1 = player1;
            destroyed = false;
            pieceType = "S";
            VPValue = 1;
            connectionsToDestroy = 2;
        }

        public virtual int GetVPs()
        {
            return VPValue;
        }

        public virtual bool GetBelongsToPlayer1()
        {
            return belongsToPlayer1;
        }

        public virtual int CheckMoveIsValid(int distanceBetweenTiles, string startTerrain, string endTerrain)
        {
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

        public virtual bool HasMethod(string methodName)
        {
            return this.GetType().GetMethod(methodName) != null;
        }

        public virtual int GetConnectionsNeededToDestroy()
        {
            return connectionsToDestroy;
        }

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

        public virtual void DestroyPiece()
        {
            destroyed = true;
        }
    }
}
