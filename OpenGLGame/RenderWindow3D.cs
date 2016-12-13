using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGLGame
{
    class RenderWindow3D
    {
        private readonly RenderWindow Window;

        public RenderWindow3D()
        {
            Window = new RenderWindow();
            Window.Load += OnLoad;
            Window.UpdateFrame += OnUpdateFrame;
            Window.RenderFrame += OnRenderFrame;
            Window.Resize += OnResize;
        }

        private void OnLoad(object sender, EventArgs e)
        {

        }

        private void OnResize(object sender, EventArgs e)
        {

        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {

        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {

        }
    }
}
