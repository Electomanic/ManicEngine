/*  
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
using System.Threading.Tasks;
using Nantuko.ManicEngine;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            MapCordinate c1 = new MapCordinate(5, 5);
            MapCordinate c2 = new MapCordinate(0, 0);
            MapCordinate c3 = new MapCordinate(-1, 1);
            MapCordinate c4 = new MapCordinate(2, 5);

            World world = new World(1);

            //world.CreateTile(c4);
            //var tile1 = world.GetTile(c4); //OK
            //var c5 = world.GetTileCordinate(tile1);

            world.CreateAllTiles();

            //var tile2 = world.GetTile(c3); //OK
            //var tiles = world.GetTiles(c1, c2); //OK

        }
    }
}
