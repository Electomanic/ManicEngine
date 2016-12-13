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
using System.Runtime.InteropServices;
using OpenTK;

namespace Nantuko.ManicEngine
{
    public class Tile
    {
        //private readonly Dictionary<int, float> _tileStatsDictionary;

        private readonly float[] _stats;
        private Tile[] _neighbours;

        private readonly Vector3 _cordinates;

        public float X { get { return _cordinates.X; } }
        public float Y { get { return _cordinates.Y; } }

        public Tile[] Neighbours
        {
            private get { return _neighbours; }
            set { _neighbours = value; }
        }

        internal Tile(float x, float y)
        {
            _cordinates = new Vector3(x,y,0f);
            //_tileStatsDictionary=new Dictionary<int, float>();
            _stats = new float[TileProperty.GetTilePropertyCount()];
        }

        public void AddNeighbour(Tile neighbour)
        {
            foreach (var tile in Neighbours)
            {
                if (Math.Abs(tile.X - neighbour.X) < 0.5 && Math.Abs(tile.Y - neighbour.Y) < 0.5) break;
 
                Array.Resize(ref _neighbours, Neighbours.Length+1);
                _neighbours[_neighbours.Length + 1] = neighbour;
            }
        }

        public float GetStat(uint typeindex)
        {
            return typeindex <= TileProperty.GetTilePropertyCount() ? _stats[typeindex] : float.NaN;
        }

        public float IncrementTileStatBy(uint typeindex, float value)
        {
            float newValue = GetStat(typeindex) + value;
            SetTileStat(typeindex, newValue);
            return newValue;
        }

        public float DecrementTileStatBy(uint typeindex, float value)
        {
            float newValue = GetStat(typeindex) - value;
            SetTileStat(typeindex, newValue);
            return newValue;
        }

        public void SetTileStat(uint typeindex, float value)
        {
            if (typeindex <= TileProperty.GetTilePropertyCount()) _stats[typeindex] = value;
        }
    }
}
