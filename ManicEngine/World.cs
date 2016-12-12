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

        private readonly OpenSimplexNoise _simplexNoise ;

        internal delegate List<Tile> GetBordeingTilesDelegate(Tile tile);

        public World(ushort maxAdress, long seed)
        {
            if (maxAdress*2 + 1 > short.MaxValue) throw new ArgumentOutOfRangeException("Max world size is: " + (maxAdress*2 + 1) + " Size requested was: " + maxAdress);

            _simplexNoise = new OpenSimplexNoise(seed);

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
                    CreateTile(new MapCordinate(x, y),false);
                }
            }

            foreach (var tile in _tileMap)
            {
                var neighbours = GetBorderingTiles(tile);

                if (neighbours != null)
                {
                    tile.Neighbours = neighbours;
                }
            }
        }

        internal List<Tile> GetBorderingTiles(Tile tile)
        {
            //return null;

            List<Tile> tileList = new List<Tile>();

            short xOrg = tile.X;
            short yOrg = tile.Y;

            short xMin = (short) (xOrg - 1);
            short yMin = (short) (yOrg - 1);
            short xMax = (short) (xOrg + 1);
            short yMax = (short) (xOrg + 1);

            if (xOrg <= _lowAddress) xMin = _lowAddress;
            if (xOrg >= _highAddress) xMax = _highAddress;
            if (yOrg <= _lowAddress) yMin = _lowAddress;
            if (yOrg >= _highAddress) yMax = _highAddress;

            MapCordinate lowerBound = new MapCordinate(xMin, yMin);
            MapCordinate upperBound = new MapCordinate(xMax, yMax);

            Tile[,] tiles = GetTiles(lowerBound, upperBound);

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tile != tiles[x, y] && tiles[x, y] != null) tileList.Add(tiles[x, y]);
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
            return CreateTile(cordinate, true);
        }

        private bool CreateTile(MapCordinate cordinate, bool calculateNeighbours)
        {
            bool creationSucessfull = false;

            if (IsTileCordinateValid(cordinate))
            {
                ArrayCordinate arrayCordinate = WorldToArray(cordinate);

                if (_tileMap[arrayCordinate.X, arrayCordinate.Y] == null)
                {
                    Tile tile = new Tile(cordinate.X, cordinate.Y);

                    foreach (var name in TileProperty.GetTypeNames())
                    {
                        var property = TileProperty.GetType(name);
                        double divider = 100.0;

                        float value = (float)_simplexNoise.Evaluate(cordinate.X / divider, cordinate.Y / divider,6,0.7);

                        value = (value + 1)/2;

                        value = value * (property.MaxInitialValue - property.MinInitialValue) + property.MinInitialValue;

                        tile.SetTileStat(property, value);
                    }

                    _tileMap[arrayCordinate.X, arrayCordinate.Y] = tile;
                    _tileDictionary.Add(tile, cordinate);

                    if (calculateNeighbours)
                    {
                        var neighbours = GetBorderingTiles(tile);

                        if (neighbours != null)
                        {
                            tile.Neighbours = neighbours;

                            foreach (var neighbour in neighbours)
                            {
                                neighbour?.Neighbours.Add(tile);
                            }
                        }
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
