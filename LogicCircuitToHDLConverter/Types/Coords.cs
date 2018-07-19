using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Coords
    {
        public int x;
        public int y;

        public Coords()
        {
            x = -1000;
            y = -1000;
        }

        public Coords(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public Coords(Coords coord)
        {
            x = coord.x;
            y = coord.y;
        }

        public static bool operator ==(Coords a, Coords b)
        {
            if (a.x == b.x && a.y == b.y) return true;
            return false;
        }

        public static bool operator !=(Coords a, Coords b)
        {
            if (a.x == b.x && a.y == b.y) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Coords))
            {
                Coords temp = (Coords)obj;
                if (x == temp.x && y == temp.y) return true;
                return false;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
