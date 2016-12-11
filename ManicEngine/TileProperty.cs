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
    public struct TileProperty
    {
        private static readonly Dictionary<string,TilePropertyType> TilePropertyTypeDictionary;

        public TilePropertyType Type { get; private set; }
        public float Value { get; set; }

        public TileProperty(TilePropertyType type, float value)
        {
            Type = type;
            Value = value;
        }

        static TileProperty()
        {
            // TODO Read types from file
            TilePropertyTypeDictionary = new Dictionary<string, TilePropertyType>
            {
                {"Invalid", new TilePropertyType("Invalid", 0, 0, 0, 0, 0)},
                {"Temperature", new TilePropertyType("Temperature", -40, 60, 15, 30, 0.1f)}
            };
        }

        public static TilePropertyType GetType(string typeName)
        {
            return TilePropertyTypeDictionary.ContainsKey(typeName) ? TilePropertyTypeDictionary[typeName] : TilePropertyTypeDictionary["Invalid"];
        }

        public static List<string> GetTypeNames()
        {
            var list = new List<string>(TilePropertyTypeDictionary.Keys);
            if (list.Contains("Invalid")) list.Remove("Invalid");

            return list;
        }
    }

    public struct TilePropertyType
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
