using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ManicEngine - Log";

            using (RenderWindow window = new RenderWindow())
            {
                window.Title = "ManicEngine - View";
                window.Run(200, 200);
            }
        }
    }
}
