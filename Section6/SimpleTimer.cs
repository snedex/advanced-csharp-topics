using System.Diagnostics;

namespace CSharpTopics.Section6
{
    public class SimpleTimer : IDisposable
    {
        private readonly Stopwatch st;
        public SimpleTimer()
        {
            st = new Stopwatch();
            st.Start();
        }

        public void Dispose()
        {
            st.Stop();
            Console.WriteLine($"Dispose: Elapsed time: {st.ElapsedMilliseconds}msecs");
        }
    }
}