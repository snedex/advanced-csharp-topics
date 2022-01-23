using System.Runtime.InteropServices;

namespace CSharpTopics.Section5
{
    public class Section5 : SectionBase
    {
        public override void RunSection()
        {
            #region Memory Management

            InParameters();

            RefReadonlyParameters();

            RefStructAndSpanT();

            SpanTDemo();

            #endregion
        }

        private void SpanTDemo()
        {
            Console.WriteLine("\n===Span<T> Demo===\n\n");

            unsafe 
            {
                //Span<T> can refer to parts or the whole memory
                byte* ptr = stackalloc byte[100];
                //This is managed memory
                Span<byte> memory = new Span<byte>(ptr, 100);

                //Allocate 123 bytes on memory to then access with Span<T>
                IntPtr unamangedPtr = Marshal.AllocHGlobal(123);
                Span<byte> unmgMem = new Span<byte>(unamangedPtr.ToPointer(), 123);
                Marshal.FreeHGlobal(unamangedPtr);
            }

            //You can also use them on arrays
            char[] stuff = "hello".ToCharArray();
            Span<char> arrayMem = stuff;
            
            //Strings are immutable, so we need a readonly span
            ReadOnlySpan<char> roSpan = "Hello, is it me you're looking for?".AsSpan();

            Console.WriteLine($"Our span has {roSpan.Length} elements");

            //We can fill the entire span to modify an array
            arrayMem.Fill('x');
            Console.WriteLine($"Our arrayMem span is now {arrayMem}");
            arrayMem.Clear();
            Console.WriteLine($"Our arrayMem span is now {arrayMem}");
        }

        private void RefStructAndSpanT()
        {
            Console.WriteLine("\n===Span<T> and ref struct===\n\n");
            Console.WriteLine("This section has no content, was a slideshow to explain");
            /*
             What is Span<T> and why?
             Managed memory 
                - Small obj < 85k managed heap
                - Large obj > 85k large object managed heap slower GC
             
             Unmanaged memory
                - Allocated on unmanaged heap, Marshal.AllocHGlobal
                - Released Manually with Marshal.FreeHGlobal
             
             Stack Memory 
                - stackalloc keyword
                - Very fast alloc/dealloc
                - Very small < 1M, overflow kills the process
                - Method scoped
                - Nobody uses it?

             Referring to a range of values and memory is generalised as Span<T>
            */

            /*
                ref struct must be stackalloc
                Can never be created on the heap
                supports Span<T>
                can't box, assign it to an object
                can't be declared on classes or structs 
                can't be in iterators
                can't be in async task
                can't be in a lamba or local functions
                prevents promotion to the heap
            */
        }

        private void RefReadonlyParameters()
        {
            Console.WriteLine("\n===ref readonly Parameters===\n\n");

            //Referencing a value type defined elsewhere without copying it on every access
            Point p1 = new Point(5,7);
            var distFromOrigin = MeasureDistance(p1, Origin);

            //We can do a by value copy if we need it
            Point originCopy = Origin;

            //But we can't bypass the readonly to mess with it by assigning it to a writeable ref
            //ref var messWithOrigin = ref Origin;
            //messWithOrigin.X++;

            //But we can assign to a read only ref
            ref readonly var readableOrigin = ref Origin;
            //And you still cant touch it
            //readableOrigin.X++;
        }

        //Tempted to do this to have a reference everywhere
        //However this is copied per access of this variable
        //public static Point Origin { get; set; }
        private static Point origin = new Point();
        //Now we defined the access via a reference proprty
        //ref readonly returns a read only reference, essentially a Ptr
        public static ref readonly Point Origin => ref origin;

        private void InParameters()
        {
            Console.WriteLine("\n==='in' Parameters===\n\n");

            //Reference semantics to value types
            //Struct Point takes 16 bytes fo memory, when passed, this makes a full copy
            var p1 = new Point(1, 1);
            var p2 = new Point(4, 5);

            var distance = MeasureDistance(p1, p2);

            Console.WriteLine($"the distance between {p1} and {p2} is {distance}");

            //Can call on a new object
            //Added explicit In for overloading
            var origin = MeasureDistance(in p1, in p2);
            Console.WriteLine($"Distance from the 'in' origin is: {origin}");
            
            //overloading
            origin = MeasureDistance(p1, new Point(), false);
            Console.WriteLine($"Distance from overloaded origin is: {origin}");
            
            origin = MeasureDistance(in p1, p2, false);
            Console.WriteLine($"Distance from in overloaded origin is: {origin}");

            //You cannot pass.. I am servant of the secret fire..
            //you cannot pass in an expression or a new T() into an in parameter, it needs to be allocated first
            //origin = MeasureDistance(in p1, in new Point(), false);
        }

        private void ChangeMe(ref Point p)
        {
            p = new Point();
        }

        //Calling this effectively is passing in 4 doubles, an example from the start of the examples
        private double MeasureDistanceHighMem(Point p1, Point p2)
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        //Similar to out keyword, in keyword is a by reference than the default by value
        //Basically a memory address passed in, 32/34 bits instead of the 4 doubles 256 bits
        private double MeasureDistance(in Point p1, in Point p2)
        {
            //In keyword makes a parameter read only / immutable.
            //p1 = new Point();
            //Cant change it by ref either
            //ChangeMe(ref p1);

            //Mutating method example, this creates the byval copy and modifies it, p1 is not changed
            //this still compiles unlike prior examples
            p1.Reset();

            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        //can do overloads though, this will compile
        //You will have to specify the in keyword to select the method to call
        //the runtime will select the byval method first
        private double MeasureDistance(Point p1, Point p2)
        {
            return 0.0;
        }

        private double MeasureDistance(in Point p1, in Point p2, bool yes)
        {
            return yes ? 0.0 : 1.0;
        }
    }
}