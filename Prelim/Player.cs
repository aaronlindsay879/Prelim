using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    /// <summary>
    /// A class representing player information
    /// </summary>
    class Player
    {
        protected int piecesInSupply, fuel, VPs, lumber;
        protected string name;

        /// <summary>
        /// Initialises player information
        /// </summary>
        /// <param name="n">Name of player</param>
        /// <param name="v">VPs for player</param>
        /// <param name="f">Fuel for player</param>
        /// <param name="l">Lumber for player</param>
        /// <param name="t">Pieces in supply for player</param>
        public Player(string n, int v, int f, int l, int t)
        {
            name = n;
            VPs = v;
            fuel = f;
            lumber = l;
            piecesInSupply = t;
        }

        /// <summary>
        /// Gets the state of the player and formats it as a string
        /// </summary>
        /// <returns>State as string</returns>
        public virtual string GetStateString()
        {
            return "VPs: " + VPs.ToString() +
               "   Pieces in supply: " + piecesInSupply.ToString() +
               "   Lumber: " + lumber.ToString() +
               "   Fuel: " + fuel.ToString();
        }

        /// <summary>
        /// Get the VPs for the player
        /// </summary>
        /// <returns>VPs</returns>
        public virtual int GetVPs()
        {
            return VPs;
        }

        /// <summary>
        /// Gets the fuel for the player
        /// </summary>
        /// <returns>Fuel</returns>
        public virtual int GetFuel()
        {
            return fuel;
        }
        
        /// <summary>
        /// Gets the lumber for the player
        /// </summary>
        /// <returns>Lumber</returns>
        public virtual int GetLumber()
        {
            return lumber;
        }

        /// <summary>
        /// Gets the name of the player
        /// </summary>
        /// <returns>Name</returns>
        public virtual string GetName()
        {
            return name;
        }

        /// <summary>
        /// Adds VPs to the player's VPs
        /// </summary>
        /// <param name="n">VPs to add</param>
        public virtual void AddToVPs(int n)
        {
            VPs += n;
        }

        /// <summary>
        /// Updates the player's fuel
        /// </summary>
        /// <param name="n">Fuel to add</param>
        public virtual void UpdateFuel(int n)
        {
            fuel += n;
        }

        /// <summary>
        /// Updates the player's lumber
        /// </summary>
        /// <param name="n">Lumber to add</param>
        public virtual void UpdateLumber(int n)
        {
            lumber += n;
        }

        /// <summary>
        /// Gets the number of pieces in supply
        /// </summary>
        /// <returns>Pieces in supply</returns>
        public int GetPiecesInSupply()
        {
            return piecesInSupply;
        }

        /// <summary>
        /// Removes a tile from supply
        /// </summary>
        public virtual void RemoveTileFromSupply()
        {
            piecesInSupply -= 1;
        }
    }
}
