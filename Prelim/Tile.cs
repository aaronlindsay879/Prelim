using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class Tile
    {
        protected string terrain;
        protected int x, y, z;
        protected Piece pieceInTile;
        protected List<Tile> neighbours = new List<Tile>();

        public Tile(int xcoord, int ycoord, int zcoord)
        {
            x = xcoord;
            y = ycoord;
            z = zcoord;
            terrain = " ";
            pieceInTile = null;
        }

        public int GetDistanceToTileT(Tile t)
        {
            return Math.Max(Math.Max(Math.Abs(Getx() - t.Getx()), Math.Abs(Gety() - t.Gety())), Math.Abs(Getz() - t.Getz()));
        }

        public void AddToNeighbours(Tile n)
        {
            neighbours.Add(n);
        }

        public List<Tile> GetNeighbours()
        {
            return neighbours;
        }

        public void SetPiece(Piece thePiece)
        {
            pieceInTile = thePiece;
        }

        public void SetTerrain(string t)
        {
            terrain = t;
        }

        public int Getx()
        {
            return x;
        }

        public int Gety()
        {
            return y;
        }

        public int Getz()
        {
            return z;
        }

        public string GetTerrain()
        {
            return terrain;
        }

        public Piece GetPieceInTile()
        {
            return pieceInTile;
        }
    }

}
