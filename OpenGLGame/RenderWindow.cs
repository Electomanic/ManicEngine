using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nantuko.ManicEngine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenGLGame
{
    class RenderWindow : GameWindow
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private Tile[,] _tilesToRender;
        private readonly World _world;

        public RenderWindow()
        {
            Console.Title = "ManicEngine - Log";

            MapCordinate renderMin = new MapCordinate(-25, -25);
            MapCordinate renderMax = new MapCordinate(25, 25);

            Random random = new Random();

            const ushort size = 25;
            long seed = random.Next(int.MinValue, int.MaxValue); //4768642378678;

            _world = new World(size, seed);

            Console.WriteLine("Creating World of size " + size + " with seed " + seed);

            Stopwatch.Start();
            _world.CreateAllTiles();
            Stopwatch.Stop();

            Console.WriteLine("World creation time: " + Stopwatch.ElapsedMilliseconds + " ms");

            _tilesToRender = _world.GetTiles(renderMin, renderMax);

            RenderFrame += RenderFrameEventHandler;
            Resize += ResizeEventHandler;
        }

        private void ResizeEventHandler(object sender, EventArgs e)
        {
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        private DateTime _last = DateTime.Now;

        private void RenderFrameEventHandler(object sender, FrameEventArgs e)
        {
            TimeSpan renderTime = new TimeSpan(DateTime.Now.Ticks - _last.Ticks);
            _last = DateTime.Now;

            Title = "ManicEngine - OpenGL - " + (1000.0/renderTime.Milliseconds).ToString("F0") + " fps";

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            RenderTiles();
            SwapBuffers();
        }

        private void RenderTiles()
        {
            char mapWith = (char)_tilesToRender.GetLength(1);
            char mapHeight = (char)_tilesToRender.GetLength(1);

            for (int y = mapHeight - 1; y > -1; y--)
            {
                for (int x = 0; x < mapWith; x++)
                {
                    if (_tilesToRender[x, y] != null)
                    {
                        var cordinate = _world.GetTileCordinate(_tilesToRender[x, y]);
                        float temperature = _tilesToRender[x, y].GetStat(TileProperty.GetType("Temperature"));
                        Color4 color = new Color4(temperature/100, temperature/100, temperature/100,1f);

                        DrawRectangle(cordinate, 1f, 1f, color);
                    }
                }
            }
        }

        private void DrawRectangle(MapCordinate cordinate, float with, float height, Color4 color)
        {
            int mapWith = _tilesToRender.GetLength(1);
            int mapHeight = _tilesToRender.GetLength(1);

            float x = (cordinate.X - 0.5f)/100f;
            float y = (cordinate.Y - 0.5f)/100f;

            x = (cordinate.X - 0.5f) / mapWith*3;
            y = (cordinate.Y - 0.5f) / mapHeight*3;

            height = height/mapHeight*3;
            with = with/mapWith*3;


            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(color);
            GL.Vertex3(x, y, 4);
            GL.Vertex3(x + with, y, 4);
            GL.Vertex3(x + with, y + height, 4);
            GL.Vertex3(x, y + height, 4);
            GL.End();
        }
    }
}
