using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class WireGroup
    {
        public List<Wire> wires;
        public List<Coords> coords;
        public int groupChannelSize;

        public WireGroup(Wire wire)
        {
            coords = new List<Coords>();
            wires = new List<Wire>();
            Add(wire);
        }

        public void Add(Wire wire)
        {
            if (!wires.Exists(x => x.Point1 == wire.Point1 && x.Point2 == wire.Point2))
            {
                wires.Add(wire);
            }
        }

        public bool Contains(Wire wire)
        {
            if (wires.Exists(x => x.Point1 == wire.Point1 && x.Point2 == wire.Point2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContainsConnectingWire(Wire wire)
        {
            if (wires.Exists(x => x.Point1.x == wire.Point1.x && x.Point1.y == wire.Point1.y || x.Point1.x == wire.Point2.x && x.Point1.y == wire.Point2.y ||
             x.Point2.x == wire.Point1.x && x.Point2.y == wire.Point1.y || x.Point2.x == wire.Point2.x && x.Point2.y == wire.Point2.y))
            {
                return true;
            }
            return false;
        }

        public bool ContainsConnectingCoord(Coords coord)
        {
            if (coords.Exists(x => x == coord)) return true;
            return false;
        }

        public bool Intersection(WireGroup group)
        {
            for (int i = 0; i < group.wires.Count; i++)
            {
                if (ContainsConnectingWire(group.wires[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static WireGroup Merge(WireGroup a, WireGroup b)
        {
            List<Wire> newWireList = new List<Wire>();
            newWireList.AddRange(a.wires);
            foreach (var wire in b.wires)
            {
                if (!newWireList.Exists(x => x.Point1 == wire.Point1 && x.Point2 == wire.Point2))
                {
                    newWireList.Add(wire);
                }
            }
            a.wires = newWireList;
            return a;
        }

        public void Process()
        {
            coords = new List<Coords>();
            foreach (var wire in wires)
            {
                if (!coords.Exists(x => x == wire.Point1))
                {
                    coords.Add(wire.Point1);
                }
                if (!coords.Exists(x => x == wire.Point2))
                {
                    coords.Add(wire.Point2);
                }
            }
        }
    }
}
