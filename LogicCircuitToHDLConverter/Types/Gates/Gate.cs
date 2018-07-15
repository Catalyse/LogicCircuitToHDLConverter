using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    /// <summary>
    /// This struct is used to indentify the first pin starting at the top left or top right.
    /// </summary>
    public struct GatePinOffset
    {
        public GatePinOffset(int x, int y, bool isTwo)
        {
            OffsetX = x;
            OffsetY = y;
            IsTwoPins = isTwo;
        }
        public bool IsTwoPins;//This is relevant because at two pins there is extra space between the pins which changes the map.
        public int OffsetX;
        public int OffsetY;
    }

    public class Gate
    {
        public CircuitSymbol symbol;
        public readonly Dictionary<int, GatePinOffset> LeftPinOffset = new Dictionary<int, GatePinOffset>
        {
            { 2, new GatePinOffset(0, 1, true) },
            { 3, new GatePinOffset(0, 1, false) },
            { 4, new GatePinOffset(0, 1, false) },
            { 5, new GatePinOffset(0, 1, false) },
            { 6, new GatePinOffset(0, 1, false) },
            { 7, new GatePinOffset(0, 1, false) },
            { 8, new GatePinOffset(0, 1, false) },
            { 9, new GatePinOffset(0, 1, false) },
            { 10, new GatePinOffset(0, 1, false) },
            { 11, new GatePinOffset(0, 1, false) },
            { 12, new GatePinOffset(0, 1, false) },
            { 13, new GatePinOffset(0, 1, false) },
            { 14, new GatePinOffset(0, 1, false) },
            { 15, new GatePinOffset(0, 1, false) },
            { 16, new GatePinOffset(0, 1, false) },
            { 17, new GatePinOffset(0, 1, false) },
            { 18, new GatePinOffset(0, 1, false) }
        };
        public readonly Dictionary<int, GatePinOffset> RightPinOffset = new Dictionary<int, GatePinOffset>
        {
            { 2, new GatePinOffset(3, 2, false) },
            { 3, new GatePinOffset(3, 2, false) },
            { 4, new GatePinOffset(3, 2, false) },
            { 5, new GatePinOffset(3, 3, false) },
            { 6, new GatePinOffset(3, 3, false) },
            { 7, new GatePinOffset(3, 4, false) },
            { 8, new GatePinOffset(3, 4, false) },
            { 9, new GatePinOffset(3, 5, false) },
            { 10, new GatePinOffset(3, 5, false) },
            { 11, new GatePinOffset(3, 6, false) },
            { 12, new GatePinOffset(3, 6, false) },
            { 13, new GatePinOffset(3, 7, false) },
            { 14, new GatePinOffset(3, 7, false) },
            { 15, new GatePinOffset(3, 8, false) },
            { 16, new GatePinOffset(3, 8, false) },
            { 17, new GatePinOffset(3, 9, false) },
            { 18, new GatePinOffset(3, 9, false) }
        };

        public Gate(CircuitSymbol _symbol)
        {
            symbol = _symbol;
        }
    }
}
