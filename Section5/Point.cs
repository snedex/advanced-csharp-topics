namespace CSharpTopics.Section5
{
    public struct Point
    {
        //16 bytes of memory
        public double X, Y;
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        //Mutating Method example for in param
        public void Reset()
        {
            X = Y = 0;
        }
    }
}