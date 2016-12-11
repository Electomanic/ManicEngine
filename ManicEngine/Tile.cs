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
