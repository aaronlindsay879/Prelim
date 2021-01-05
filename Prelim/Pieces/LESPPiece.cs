using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class LESSPiece : Piece
    {
        public LESSPiece(bool player1)
            : base(player1)
        {
            pieceType = "L";
            VPValue = 3;
        }

        public override int CheckMoveIsValid(int distanceBetweenTiles, string startTerrain, string endTerrain)
        {
            if (distanceBetweenTiles == 1 && startTerrain != "#")
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
