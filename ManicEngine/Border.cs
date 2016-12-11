/*  
    ManicEngine - Border
    Copyright(C) 2016 Johannes Hall

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/>.
*/

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
