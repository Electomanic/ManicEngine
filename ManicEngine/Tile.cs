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
    public enum TileStatType
    {
        Food,
        Temperature,
        Humidity,
    }

    public class Tile
    {
        private readonly Dictionary<TileStatType, float> _tileStatsDictionary;
        private List<Tile> _neighbours;

        // TODO remove temporary debugging cordinates
        private short _x;
        private short _y;

        public List<Tile> Neighbours
        {
            get { return _neighbours; }
            internal set
            {
                _neighbours = value;
            }
        }

        internal Tile(short x, short y)
        {
            _x = x;
            _y = y;
            _tileStatsDictionary=new Dictionary<TileStatType, float>();
        }

        public float GetStat(TileStatType stat)
        {
            return _tileStatsDictionary.ContainsKey(stat) ? _tileStatsDictionary[stat] : 0f;
        }

        public float IncrementTileStatBy(TileStatType stat, float value)
        {
            float newValue = GetStat(stat) + value;
            SetTileStat(stat, newValue);
            return newValue;
        }

        public float DecrementTileStatBy(TileStatType stat, float value)
        {
            float newValue = GetStat(stat) - value;
            SetTileStat(stat, newValue);
            return newValue;
        }

        public void SetTileStat(TileStatType stat, float value)
        {
            if (_tileStatsDictionary.ContainsKey(stat)) _tileStatsDictionary[stat] = value;
            else _tileStatsDictionary.Add(stat,value);
        }
    }
}
