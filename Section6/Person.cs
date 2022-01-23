namespace CSharpTopics.Section6
{
    public class Person
    {
        public List<string> Names = new List<string>();

        public List<Person> Children = new List<Person>();

    }

    //Temp type that carries evalulation result and eval object
    public struct BoolMarker<T> 
    {
        public bool Result;
        public T Self;

        //Define operations
        public enum Operation {
            None, 
            And,
            Or
        }

        internal Operation PendingOp;

        //Add an and boolmarker
        public BoolMarker<T> And => new BoolMarker<T>(Result, Self, Operation.And);

        internal BoolMarker(bool result, T self, Operation pendingOp)
        {
            Result = result;
            Self = self;
            PendingOp = pendingOp;
        }

        public BoolMarker(bool result, T self) 
            : this(result, self, Operation.None)
        {

        }

        //Implicit cast operator to get a bool from teh result of the eval
        public static implicit operator bool(BoolMarker<T> marker)
        {
            return marker.Result;
        }
    }
}