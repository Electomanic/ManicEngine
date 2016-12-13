/*  
    ManicEngine - TileProperty
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
using System.Linq;
using System.Text;

namespace Nantuko.ManicEngine
{
    public class TileProperty
    {
        public int TilePropertyTypeIndex { get; private set; }
        public float Value { get; set; }

        public TileProperty(int typeIndex, float value)
        {
            TilePropertyTypeIndex = typeIndex;
            Value = value;
        }

        /* Static below */

        private static readonly TilePropertyType[] TileProperties;

        static TileProperty()
        {
            // TODO Read types from file
            TileProperties = new TilePropertyType[]
            {
                new TilePropertyType("Temperature", -40, 60, 15, 30, 0.1f)
            };
        }

        public static TilePropertyType[] GetTypes()
        {
            return TileProperties;
        }

        public static TilePropertyType GetType(int index)
        {
            TilePropertyType type = null;

            if(index <= TileProperties.Length) type = TileProperties[index];
            return type;
        }

        public static string[] GetTypeNames()
        {
            string[] strings = new string[TileProperties.Length];

            for (int i = 0; i < TileProperties.Length; i++)
            {
                strings[i] = TileProperties[i].Name;
            }

            return strings;
        }
    }

    public class TilePropertyType
    {
        public string Name { get; }
        public float MinValue { get; }
        public float MaxValue { get; }
        internal float MinInitialValue { get; }
        internal float MaxInitialValue { get; }
        internal float Spread { get; }

        internal TilePropertyType(string name, float minValue, float maxValue, float minInitialValue, float maxInitialValue, float spread)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            MinInitialValue = minInitialValue;
            MaxInitialValue = maxInitialValue;
            Spread = spread;
        }
    }
}
