using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class BaronPiece : Piece
    {
        public BaronPiece(bool player1)
            : base(player1)
        {
            pieceType = "B";
            VPValue = 10;
        }

        public override int CheckMoveIsValid(int distanceBetweenTiles, string startTerrain, string endTerrain)
        {
            if (distanceBetweenTiles == 1)
            {
                return fuelCostOfMove;
            }
            return -1;
        }
    }
}
