using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class containing information on an individual tile
    /// </summary>
    class Tile
    {
        protected string terrain;
        protected int x, y, z;
        protected Piece pieceInTile;
        protected List<Tile> neighbours = new List<Tile>();

        /// <summary>
        /// Initialises a tile given x, y and z coordinates
        /// </summary>
        /// <param name="xcoord">X coord</param>
        /// <param name="ycoord">Y coord</param>
        /// <param name="zcoord">Z coord</param>
        public Tile(int xcoord, int ycoord, int zcoord)
        {
            //sets up variables with default values for terrain and whether piece in tile
            x = xcoord;
            y = ycoord;
            z = zcoord;
            terrain = " ";
            pieceInTile = null;
        }

        /// <summary>
        /// Get the distance from this tile to another tile <paramref name="t"/>
        /// </summary>
        /// <param name="t">Other tile</param>
        /// <returns>Integer distance from this tile to another tile</returns>
        public int GetDistanceToTileT(Tile t)
        {
            //calculates the max of the x and y distances, and then max of the previous distance and the z distance
            return Math.Max(Math.Max(Math.Abs(Getx() - t.Getx()),
                                     Math.Abs(Gety() - t.Gety())),
                            Math.Abs(Getz() - t.Getz()));
        }

        /// <summary>
        /// Adds a given tile <paramref name="n"/> to neighbours
        /// </summary>
        /// <param name="n">Tile to add</param>
        public void AddToNeighbours(Tile n)
        {
            neighbours.Add(n);
        }

        /// <summary>
        /// Gets a list of neighbours
        /// </summary>
        /// <returns>List of neighbours</returns>
        public List<Tile> GetNeighbours()
        {
            return neighbours;
        }

        /// <summary>
        /// Sets the piece in the tile
        /// </summary>
        /// <param name="thePiece">Piece to set</param>
        public void SetPiece(Piece thePiece)
        {
            pieceInTile = thePiece;
        }

        /// <summary>
        /// Sets the terrain in the tile
        /// </summary>
        /// <param name="t">Terrain to set</param>
        public void SetTerrain(string t)
        {
            terrain = t;
        }

        /// <summary>
        /// Gets the x-coordinate of tile
        /// </summary>
        /// <returns>x-coord</returns>
        public int Getx()
        {
            return x;
        }

        /// <summary>
        /// Gets the y-coordinate of the tile
        /// </summary>
        /// <returns>y-coord</returns>
        public int Gety()
        {
            return y;
        }

        /// <summary>
        /// Gets the z-coordinate of the tile
        /// </summary>
        /// <returns>z-coord</returns>
        public int Getz()
        {
            return z;
        }

        /// <summary>
        /// Gets the terrain of the tile
        /// </summary>
        /// <returns>terrain</returns>
        public string GetTerrain()
        {
            return terrain;
        }

        /// <summary>
        /// Gets the piece in the tile
        /// </summary>
        /// <returns>Piece</returns>
        public Piece GetPieceInTile()
        {
            return pieceInTile;
        }
    }
}
