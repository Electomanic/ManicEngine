using System;
using System.Collections.Generic;

namespace Nantuko.ManicEngine
{
    public class Border
    {
        /// <summary>
        /// Calculates the unique borderId for a pair two pairs of x-y cordinates
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns>The generated border if sucessfull or 0 if not</returns>
        public static long CalculateBorderId(short x1, short y1, short x2, short y2)
        {
            long borderId;

            long cordinate1 = (ushort)x1 << 16 | (ushort)y1;
            long cordinate2 = (ushort)x2 << 16 | (ushort)y2;

            if (cordinate1 == cordinate2) borderId = 0;

            else if (cordinate1 < cordinate2) borderId = cordinate1 << 32 | cordinate2;
            else borderId =                              cordinate2 << 32 | cordinate1;

            return borderId;
        }

        public static long CalculateBorderId(MapCordinate cordinate1, MapCordinate cordinate2)
        {
            return CalculateBorderId(cordinate1.X, cordinate1.Y, cordinate2.X, cordinate2.Y);
        }
    }
}
