namespace CSharpTopics.Section6
{
    public class Disposable : IDisposable
    {
        //We want a factory method
        private Disposable(Action start, Action end)
        {
            this.End = end;
            start();
        }

        public Action End { get; }

        public void Dispose()
        {
            End();
        }

        //Factory method
        public static Disposable Create(Action start, Action end)
        {
            return new Disposable(start, end);
        }
    }
}