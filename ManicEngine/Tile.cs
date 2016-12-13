/*  
    ManicEngine - Tile
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


    public class Tile
    {
        private readonly Dictionary<int, float> _tileStatsDictionary;

        public short X { get; }
        public short Y { get; }

        public List<Tile> Neighbours { get; internal set; }
        public List<Border> Borders { get; internal set; }

        internal Tile(short x, short y)
        {
            X = x;
            Y = y;
            _tileStatsDictionary=new Dictionary<int, float>();
        }

        public float GetStat(int typeindex)
        {
            return _tileStatsDictionary.ContainsKey(typeindex) ? _tileStatsDictionary[typeindex] : 0f;
        }

        public float IncrementTileStatBy(int stat, float value)
        {
            float newValue = GetStat(stat) + value;
            SetTileStat(stat, newValue);
            return newValue;
        }

        public float DecrementTileStatBy(int stat, float value)
        {
            float newValue = GetStat(stat) - value;
            SetTileStat(stat, newValue);
            return newValue;
        }

        public void SetTileStat(int stat, float value)
        {
            if (_tileStatsDictionary.ContainsKey(stat)) _tileStatsDictionary[stat] = value;
            else _tileStatsDictionary.Add(stat,value);
        }
    }
}
