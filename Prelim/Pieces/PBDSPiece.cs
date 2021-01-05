using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class PBDSPiece : Piece
    {
        static Random rNoGen = new Random();

        public PBDSPiece(bool player1)
            : base(player1)
        {
            pieceType = "P";
            VPValue = 2;
            fuelCostOfMove = 2;
        }

        public override int CheckMoveIsValid(int distanceBetweenTiles, string startTerrain, string endTerrain)
        {
            if (distanceBetweenTiles != 1 || startTerrain == "~")
            {
                return -1;
            }
            return fuelCostOfMove;
        }

        public int Dig(string terrain)
        {
            if (terrain != "~")
            {
                return 0;
            }
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
