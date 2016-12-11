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
        /// Calculates the unique borderId for a pair of MapCordinates
        /// </summary>
        /// <param name="mapCordinate1"></param>
        /// <param name="mapCordinate2"></param>
        /// <returns>The generated border if sucessfull or 0 if not</returns>
        public static long CalculateBorderId(MapCordinate mapCordinate1, MapCordinate mapCordinate2)
        {
            long borderId;

            if (AreNeighbours(mapCordinate1, mapCordinate2))
            {
                long cordinate1 = (ushort)mapCordinate1.X << 16 | (ushort)mapCordinate1.Y;
                long cordinate2 = (ushort)mapCordinate2.X << 16 | (ushort)mapCordinate2.Y;

                if (cordinate1 < cordinate2) borderId = cordinate1 << 32 | cordinate2;
                else borderId = cordinate2 << 32 | cordinate1;
            }
            else
            {
                borderId = 0;
            }

            return borderId;
        }

        private static bool AreNeighbours(MapCordinate mapCordinate1, MapCordinate mapCordinate2)
        {
            bool areNeighbours;

            var xMin = mapCordinate1.X < mapCordinate2.X ? mapCordinate1.X : mapCordinate2.X;
            var yMin = mapCordinate1.Y < mapCordinate2.Y ? mapCordinate1.Y : mapCordinate2.Y;
            var xMax = mapCordinate1.X > mapCordinate2.X ? mapCordinate1.X : mapCordinate2.X;
            var yMax = mapCordinate1.Y > mapCordinate2.Y ? mapCordinate1.Y : mapCordinate2.Y;

            if      (xMin == xMax && yMin == yMax)       areNeighbours = false;
            else if (xMin + 1 < xMax && yMin + 1 < yMax) areNeighbours = false;
            else                                         areNeighbours = true;

            return areNeighbours;
        }
    }
}
