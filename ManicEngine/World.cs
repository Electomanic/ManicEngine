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
using OpenTK;

namespace Nantuko.ManicEngine
{
    /// <summary>
    /// A World contains and manages a collection of Tiles
    /// </summary>
    public class World
    {
        private readonly short _lowAddress;
        private readonly short _highAddress;
        private readonly Tile[,] _tileMap;
        private readonly OpenSimplexNoise _simplexNoise ;

        public World(ushort maxAdress, long seed)
        {
            if (maxAdress*2 + 1 > short.MaxValue) throw new ArgumentOutOfRangeException("Max world size is: " + (maxAdress*2 + 1) + " Size requested was: " + maxAdress);

            _simplexNoise = new OpenSimplexNoise(seed);

            var size = maxAdress*2 + 1;

            _lowAddress = (short) -maxAdress;
            _highAddress = (short) maxAdress;

            _tileMap = new Tile[size, size];
        }

        public int TileCount
        {
            get { return _tileMap.GetLength(0)*_tileMap.GetLength(1); }
        }

        public void CreateAllTiles()
        {
            for (short x = _lowAddress; x <= _highAddress; x++)
            {
                for (short y = _lowAddress; y <= _highAddress; y++)
                {
                    CreateTile(new Vector3(x, y,0),false);
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

        internal Tile[] GetBorderingTiles(Tile tile)
        {
            var tileList = new List<Tile>();

            short xOrg = (short)Math.Round(tile.X);
            short yOrg = (short)Math.Round(tile.Y);

            short xMin = (short) (xOrg - 1);
            short yMin = (short) (yOrg - 1);
            short xMax = (short) (xOrg + 1);
            short yMax = (short) (yOrg + 1);

            if (xOrg <= _lowAddress) xMin = _lowAddress;
            if (xOrg >= _highAddress) xMax = _highAddress;
            if (yOrg <= _lowAddress) yMin = _lowAddress;
            if (yOrg >= _highAddress) yMax = _highAddress;

            Vector3 lowerBound = new Vector3(xMin, yMin,0);
            Vector3 upperBound = new Vector3(xMax, yMax,0);

            var tiles = GetTiles(lowerBound, upperBound);

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tile != tiles[x, y] && tiles[x, y] != null) tileList.Add(tiles[x, y]);
                }
            }

            return tileList.ToArray();
        }

        private bool IsTileCordinateValid(Vector3 cordinate)
        {
            return cordinate.X <= _highAddress && cordinate.X >= _lowAddress && cordinate.Y <= _highAddress && cordinate.Y >= _lowAddress;
        }

        private Vector3 ArrayToWorld(Vector3 cordinate)
        {
            return new Vector3(cordinate.X - _highAddress, cordinate.Y - _highAddress, cordinate.Z);
        }

        private Vector3 WorldToArray(Vector3 cordinate)
        {
            return new Vector3((cordinate.X + _highAddress), (cordinate.Y + _highAddress),cordinate.Z);
        }

        public Tile GetTile(Vector3 cordinate)
        {
            Tile tile = null;

            if (IsTileCordinateValid(cordinate))
            {
                Vector3 arrayCordinate = WorldToArray(cordinate);

                tile = _tileMap[(int)Math.Round(arrayCordinate.X), (int)Math.Round(arrayCordinate.Y)];
            }

            return tile; 
        }

        public Tile[,] GetTiles(Vector3 lowerBound, Vector3 upperBound)
        {
            short xLowerIn = (short)Math.Floor(lowerBound.X);
            short yLowerIn = (short)Math.Floor(lowerBound.Y);
            short xUpperIn = (short)Math.Floor(upperBound.X);
            short yUpperIn = (short)Math.Floor(upperBound.Y);

            short xLower = Math.Min(xLowerIn, xUpperIn);
            short xUpper = Math.Max(xLowerIn, xUpperIn);
            short yLower = Math.Min(yLowerIn, yUpperIn);
            short yUpper = Math.Max(yLowerIn, yUpperIn);

            lowerBound = new Vector3(xLower, yLower, 0);
            upperBound = new Vector3(xUpper, yUpper, 0);

            Vector3 lowerWorldArrayBound = WorldToArray(lowerBound);
            Vector3 upperWorldArrayBound = WorldToArray(upperBound);

            short xMax = (short) Math.Round(upperWorldArrayBound.X - lowerWorldArrayBound.X + 1);
            short yMax = (short) Math.Round(upperWorldArrayBound.Y - lowerWorldArrayBound.Y + 1);

            var tiles = new Tile[xMax, yMax];

            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    float xMap =  x + lowerBound.X;
                    float yMap =  y + lowerBound.Y;

                    tiles[x, y] = GetTile(new Vector3(xMap, yMap, 0));
                }
            }

            return tiles;
        }

        public Vector3 GetTileCordinate(Tile tile)
        {
            if (tile == null) return new Vector3(float.NaN,float.NaN, float.NaN);

            return new Vector3(tile.X,tile.Y,0);
        }

        public bool CreateTile(Vector3 cordinate)
        {
            return CreateTile(cordinate, true);
        }

        private bool CreateTile(Vector3 cordinate, bool calculateNeighbours)
        {
            bool creationSucessfull = false;

            cordinate = new Vector3((float)Math.Round(cordinate.X), (float)Math.Round(cordinate.Y), cordinate.Y);

            if (IsTileCordinateValid(cordinate))
            {
                var arrayCordinate = WorldToArray(cordinate);
                var x = (char) Math.Floor(arrayCordinate.X);
                var y = (char) Math.Floor(arrayCordinate.Y);

                if (_tileMap[x, y] == null)
                {
                    var tile = new Tile(cordinate.X, cordinate.Y);

                    uint i = 0;
                    foreach (var type in TileProperty.GetTypes())
                    {
                        double divider = 100;

                        float value = (float)_simplexNoise.Evaluate(cordinate.X / divider, cordinate.Y / divider,6,0.7);
                        value = (value + 1)/2;
                        value = value * (type.MaxInitialValue - type.MinInitialValue) + type.MinInitialValue;
                        tile.SetTileStat(i, value);

                        i++;
                    }

                    _tileMap[x, y] = tile;

                    if (calculateNeighbours)
                    {
                        var neighbours = GetBorderingTiles(tile);

                        if (neighbours != null)
                        {
                            tile.Neighbours = neighbours;

                            foreach (var neighbour in neighbours)
                            {
                                neighbour?.AddNeighbour(tile);
                            }
                        }
                    }
                    creationSucessfull = true;
                }
            }
            return creationSucessfull;
        }
    }
}
