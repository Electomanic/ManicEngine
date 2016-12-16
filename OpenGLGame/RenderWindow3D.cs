using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nantuko.ManicEngine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using BufferTarget = OpenTK.Graphics.OpenGL4.BufferTarget;
using BufferUsageHint = OpenTK.Graphics.OpenGL4.BufferUsageHint;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using CullFaceMode = OpenTK.Graphics.OpenGL4.CullFaceMode;
using EnableCap = OpenTK.Graphics.OpenGL4.EnableCap;
using FrontFaceDirection = OpenTK.Graphics.OpenGL4.FrontFaceDirection;
using GL = OpenTK.Graphics.OpenGL4.GL;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
using ShaderType = OpenTK.Graphics.OpenGL4.ShaderType;
using VertexAttribPointerType = OpenTK.Graphics.OpenGL4.VertexAttribPointerType;

namespace OpenGLGame
{
    class RenderWindow3D
    {
        private readonly GameWindow _window;

        private int
            _vertexShaderId,
            _fragmentShaderId,
            _programId,
            _vaoId,
            _vboId,
            _colorBufferId;

        private readonly string VertexShader =

                "#version 400" + "\n" +

                "layout(location=0) in vec4 in_Position;" + "\n" +
                "layout(location=1) in vec4 in_Color;" + "\n" +
                "out vec4 ex_Color;" + "\n" +

                "void main(void)" + "\n" +
                "{" + "\n" +
                "  gl_Position = in_Position;" + "\n" +
                "  ex_Color = in_Color;" + "\n" +
                "}"
            ;

        private readonly string FragmentShader =

                "#version 400" + "\n" +
                "in vec4 ex_Color;" + "\n" +
                "out vec4 out_Color;" + "\n" +

                "void main(void)" + "\n" +
                "{" + "\n" +
                "  out_Color = ex_Color;" + "\n" +
                "}"
            ;


        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private const short WorldSize = 50;

        private Tile[,] _tilesToRender;
        private readonly World _world;
        private readonly Random _random = new Random();
        private readonly Vector3 _renderMin = new Vector3(-WorldSize, -WorldSize, 0);
        private readonly Vector3 _renderMax = new Vector3(WorldSize, WorldSize, 0);
        private readonly long _seed;

        private Vector4[] _vertices;
        private Vector4[] _colors;

        public RenderWindow3D()
        {
            Console.Title = "ManicEngine - Log";

            _window = new GameWindow(1280, 720, GraphicsMode.Default, "OpenTK Test", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible);

            _seed = _random.Next(int.MinValue, int.MaxValue); // Old seed: 4768642378678;
            _world = new World((ushort)Math.Abs(WorldSize), _seed);

            _window.Resize += OnResize;
            _window.RenderFrame += OnRenderFrame;
            _window.Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Console.WriteLine("Creating World of size " + WorldSize + " with seed " + _seed);
            Stopwatch.Start();
            _world.CreateAllTiles();
            Stopwatch.Stop();
            Console.WriteLine("World creation time: " + Stopwatch.ElapsedMilliseconds + " ms");

            _tilesToRender = _world.GetTiles(_renderMin, _renderMax);

            CreateShaders();
            CreateVBO();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Cw);

            _modelMatrix = Matrix4.Identity;
            _viewMatrix = Matrix4.Identity;
            _projectionMatrix = Matrix4.Identity;



            _projectionMatrix = Matrix4.CreateTranslation(new Vector3(0, 0, 0)) *
                    Matrix4.CreateScale(new Vector3(0, 0, 0)) *
                    Matrix4.CreateRotationX(0) *
                    Matrix4.CreateRotationY(0) *
                    Matrix4.CreateRotationZ(0);


            //_modelMatrixUniformLocation = GL.GetUniformLocation(ShaderIds[0], "ModelMatrix");
            //_viewMatrixUniformLocation = GL.GetUniformLocation(ShaderIds[0], "ViewMatrix");
            //_projectionMatrixUniformLocation = GL.GetUniformLocation(ShaderIds[0], "ProjectionMatrix");


        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            _window.Title = "ManicEngine - OpenGL - " + (1000.0 / e.Time).ToString("F0") + " fps";

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int passes = _tilesToRender.GetLength(1) - 1;
            int stripLenght = _tilesToRender.GetLength(0) * 2;
            int offset = 0;

            for (int i = 0; i < passes; i++)
            {
                GL.DrawArrays(PrimitiveType.TriangleStrip, offset, stripLenght);
                offset += stripLenght;
            }

            _window.SwapBuffers();
        }

        private void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _window.Width, _window.Height);
        }

        private void CreateShaders()
        {
            _vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertexShaderId, VertexShader);
            GL.CompileShader(_vertexShaderId);

            _fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragmentShaderId, FragmentShader);
            GL.CompileShader(_fragmentShaderId);

            _programId = GL.CreateProgram();
            GL.AttachShader(_programId, _vertexShaderId);
            GL.AttachShader(_programId, _fragmentShaderId);
            GL.LinkProgram(_programId);
            GL.UseProgram(_programId);
        }

        private void CreateVBO()
        {
            int arrayLenght = _tilesToRender.GetLength(0) * 2 *_tilesToRender.GetLength(1) - _tilesToRender.GetLength(1);

            _vertices = new Vector4[arrayLenght];
            _colors = new Vector4[arrayLenght];

            int x = 0;
            int y = _tilesToRender.GetLength(0) - 1;
            int yMod = 1;

            int xSize = _tilesToRender.GetLength(0);
            int ySize = _tilesToRender.GetLength(1);

            int arrayIndex = 0;

            for (; y >= 0 + 1; y--)
            {
                for (; x < xSize; x++)
                {
                    for (; yMod >= 0; yMod--)
                    {
                        Tile tile = _tilesToRender[x, y - yMod];

                        float z = tile.GetStat(0);
                        float c = 0.5f;

                        Vector4 vector = new Vector4(tile.X/100f, tile.Y/100f, z, 1f);
                        _vertices[arrayIndex] = vector;

                        Vector4 color = new Vector4(z, z , z , 1f);
                       
                        _colors[arrayIndex] = color;

                        arrayIndex++;
                    }

                    yMod = 1;
                }
                x = 0;
            }

            //GL.GenVertexArrays(1, out _vaoId);
            //GL.BindVertexArray(_vaoId);

            _vboId = GL.GenVertexArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector4.SizeInBytes * _vertices.Length, _vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            _colorBufferId = GL.GenVertexArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector4.SizeInBytes * _colors.Length, _colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //GL.GenBuffer();
        }
       
        private Matrix4 _modelMatrix;
        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private int _projectionMatrixUniformLocation;
        private int _viewMatrixUniformLocation;
        private int _modelMatrixUniformLocation;

        public void ApplyTransform()
        {
           // GL.matrix

        }

        public void Run()
        {
            _window.Run();
        }
    }
}
