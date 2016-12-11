/*  
    ManicEngine - World
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
    /// <summary>
    /// A World contains and manages a collection of Tiles
    /// </summary>
    public class World
    {
        private readonly short _lowAddress;
        private readonly short _highAddress;

        private readonly Dictionary<long, Border> _borderDictionary;
        private readonly Dictionary<Tile, MapCordinate> _tileDictionary;
        private readonly Tile[,] _tileMap;

        internal delegate List<Tile> GetBordeingTilesDelegate(Tile tile);

        public World(ushort maxAdress)
        {
            if (maxAdress*2 + 1 > short.MaxValue) throw new ArgumentOutOfRangeException("Max world size is: " + (maxAdress*2 + 1) + " Size requested was: " + maxAdress);

            var size = maxAdress*2 + 1;

            _lowAddress = (short) -maxAdress;
            _highAddress = (short) maxAdress;

            _tileMap = new Tile[size, size];
            _tileDictionary = new Dictionary<Tile, MapCordinate>();
            _borderDictionary = new Dictionary<long, Border>();
        }

        /// <summary>
        /// Only for debugging
        /// </summary>
        public void CreateAllTiles()
        {
            for (short x = _lowAddress; x <= _highAddress; x++)
            {
                for (short y = _lowAddress; y <= _highAddress; y++)
                {
                    CreateTile(new MapCordinate(x, y));
                }
            }
        }

        // TODO not working
        internal List<Tile> GetBorderingTiles(Tile tile)
        {
            MapCordinate tileCordinate = GetTileCordinate(tile);
            List<Tile> tileList = new List<Tile>();

            if (tileCordinate != null)
            {
                short xOrg = tileCordinate.X;
                short yOrg = tileCordinate.Y;

                short xMin = (short) (xOrg-1);
                short yMin = (short) (yOrg-1);
                short xMax = (short) (xOrg+1);
                short yMax = (short) (xOrg+1);

                if (xOrg >= _lowAddress)  xMin = _lowAddress;
                if (xOrg <= _highAddress) xMax = _highAddress;
                if (yOrg >= _lowAddress)  yMin = _lowAddress;
                if (yOrg <= _highAddress) yMax = _highAddress;

                MapCordinate lowerBound = new MapCordinate((short)(xMin - 1), (short)(yMin - 1));
                MapCordinate upperBound = new MapCordinate((short)(xMax + 1), (short)(yMax + 1)); ;

                Tile[,] tiles = GetTiles(lowerBound, upperBound);

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (x != 1 && y != 1)
                        {
                            if(tiles[x, y] != null) tileList.Add(tiles[x, y]);
                        }
                    }
                }
            }

            return tileList;
        }

        private bool IsTileCordinateValid(MapCordinate cordinate)
        {
            return cordinate.X <= _highAddress && cordinate.X >= _lowAddress && cordinate.Y <= _highAddress && cordinate.Y >= _lowAddress;
        }

        private MapCordinate ArrayToWorld(ArrayCordinate cordinate)
        {
            return new MapCordinate((short) (cordinate.X - _highAddress), (short) (cordinate.Y - _highAddress));
        }

        private ArrayCordinate WorldToArray(MapCordinate cordinate)
        {
            return new ArrayCordinate((short)(cordinate.X + _highAddress), (short)(cordinate.Y + _highAddress));
        }

        public Tile GetTile(MapCordinate cordinate)
        {
            Tile tile = null;

            if (IsTileCordinateValid(cordinate))
            {
                ArrayCordinate arrayCordinate = WorldToArray(cordinate);

                tile = _tileMap[arrayCordinate.X, arrayCordinate.Y];
            }

            return tile; 
        }

        public Tile[,] GetTiles(MapCordinate lowerBound, MapCordinate upperBound)
        {
            short xLower = Math.Min(lowerBound.X, upperBound.X);
            short xUpper = Math.Max(lowerBound.X, upperBound.X);
            short yLower = Math.Min(lowerBound.Y, upperBound.Y);
            short yUpper = Math.Max(lowerBound.Y, upperBound.Y);

            lowerBound = new MapCordinate(xLower, yLower);
            upperBound = new MapCordinate(xUpper, yUpper);

            ArrayCordinate lowerWorldArrayBound = WorldToArray(lowerBound);
            ArrayCordinate upperWorldArrayBound = WorldToArray(upperBound);

            short xMax = (short) (upperWorldArrayBound.X - lowerWorldArrayBound.X + 1);
            short yMax = (short) (upperWorldArrayBound.Y - lowerWorldArrayBound.Y + 1);

            var tiles = new Tile[xMax, yMax];

            for (short x = 0; x < xMax; x++)
            {
                for (short y = 0; y < yMax; y++)
                {
                    short xMap = (short) (x + lowerBound.X);
                    short yMap = (short) (y + lowerBound.Y);

                    tiles[x, y] = GetTile(new MapCordinate(xMap, yMap));
                }
            }

            return tiles;
        }

        public MapCordinate GetTileCordinate(Tile tile)
        {
            if (tile == null) return null;

            return _tileDictionary.ContainsKey(tile) ? _tileDictionary[tile] : null;
        }

        public bool CreateTile(MapCordinate cordinate)
        {
            bool creationSucessfull = false;

            if (IsTileCordinateValid(cordinate))
            {
                ArrayCordinate arrayCordinate = WorldToArray(cordinate);

                if (_tileMap[arrayCordinate.X, arrayCordinate.Y] == null)
                {
                    Tile tile = new Tile(cordinate.X, cordinate.Y);

                    _tileMap[arrayCordinate.X, arrayCordinate.Y] = tile;
                    _tileDictionary.Add(tile, cordinate);

                    var neighbours = GetBorderingTiles(tile);
                    tile.Neighbours = GetBorderingTiles(tile);

                    if (tile.Neighbours != null) foreach (var neighbour in neighbours)
                    {
                        if(neighbour != null) neighbour.Neighbours = GetBorderingTiles(neighbour);
                    }

                    creationSucessfull = true;
                }
            }

            return creationSucessfull;
        }

        private class ArrayCordinate
        {
            public short X { get; }
            public short Y { get; }

            public ArrayCordinate(short x, short y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
