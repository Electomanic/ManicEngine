using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nantuko.ManicEngine;

namespace OpenGLGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ManicEngine - Log";

            RenderWindow3D window = new RenderWindow3D();
            window.Run();

        }
    }
}
