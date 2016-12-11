namespace Nantuko.ManicEngine
{
    /// <summary>
    /// Holds a world cordinate consisting of the x and y value
    /// </summary>
    public class MapCordinate
    {
        public short X { get; }
        public short Y { get; }

        public MapCordinate(short x, short y)
        {
            X = x;
            Y = y;
        }
    }
}
