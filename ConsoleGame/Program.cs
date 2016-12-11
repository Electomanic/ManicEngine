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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Nantuko.ManicEngine;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            MapCordinate renderMin = new MapCordinate(-5, -5);
            MapCordinate renderMax = new MapCordinate(5, 5);

            World world = new World(3);

            world.CreateAllTiles();

            var tilesToRender = world.GetTiles(renderMin, renderMax);

            for (;;)
            {
                RenderTiles(tilesToRender);
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Quick console renderer for testing the engine
        /// </summary>
        /// <param name="tiles">The part of the world to render</param>
        private static void RenderTiles(Tile[,] tiles)
        {
            char mapWith = (char) (tiles.GetLength(1));
            char mapHeight = (char) (tiles.GetLength(1));

            Console.Clear();

            for (int y = mapHeight - 1; y > -1; y--)
            {
                Console.Write(y + "\t");

                for (int x = 0; x < mapWith; x++)
                {
                    if (tiles[x, y] != null)
                    {
                        //Console.ForegroundColor = C
                        float temperature = tiles[x, y].GetStat(TileProperty.GetType("Temperature"));
                        Console.Write("[" + temperature.ToString("F1") + "]");
                    }
                    else
                    {
                        Console.Write("     ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("\t  ");

            for (int i = 0; i < mapWith; i++)
            {
                Console.Write(i + "    ");
            }
        }
    }
}
