using System;
using System.Collections.Generic;
using System.Text;

namespace HexBaronCS
{
    class Player
    {
        protected int piecesInSupply, fuel, VPs, lumber;
        protected string name;

        public Player(string n, int v, int f, int l, int t)
        {
            name = n;
            VPs = v;
            fuel = f;
            lumber = l;
            piecesInSupply = t;
        }

        public virtual string GetStateString()
        {
            return "VPs: " + VPs.ToString() + "   Pieces in supply: " + piecesInSupply.ToString() + "   Lumber: " + lumber.ToString() + "   Fuel: " + fuel.ToString();
        }

        public virtual int GetVPs()
        {
            return VPs;
        }

        public virtual int GetFuel()
        {
            return fuel;
        }

        public virtual int GetLumber()
        {
            return lumber;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void AddToVPs(int n)
        {
            VPs += n;
        }

        public virtual void UpdateFuel(int n)
        {
            fuel += n;
        }

        public virtual void UpdateLumber(int n)
        {
            lumber += n;
        }

        public int GetPiecesInSupply()
        {
            return piecesInSupply;
        }

        public virtual void RemoveTileFromSupply()
        {
            piecesInSupply -= 1;
        }
    }
}
