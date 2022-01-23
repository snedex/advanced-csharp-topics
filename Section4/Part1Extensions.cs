using System.Diagnostics;
using System.Runtime.Serialization;

namespace CSharpTopics.Section4
{
    public static class Part1Extensions
    {
        //Define a Foo Extension
        public static int Measure(this Foo foo)
        {
            return foo.Name.Length;
        }

        public static int Measure(this FooDerived foo)
        {
            return foo.Name.Length;
        }

        //Extending an int to binary
        public static string ToBinary(this int n)
        {
            return Convert.ToString(n, 2);
        }

        //You can extend an interface too
        public static void Save(this ISerializable serializable)
        {
            
        }

        public static string ToString(this Foo foo)
        {
            return "test";
        }

        //Tuple conversion extension
        public static Person ToPerson(this (string name, int age) data)
        {
            return new Person { Name = data.name, Age = data.age };
        }

        //Generic Tuple Extension
        public static int Measure<T, U>(this Tuple<T, U> t)
        {
            return t.Item2.ToString().Length;
        }

        //Extending a delegate to measure execution time
        public static Stopwatch Measure(this Func<int> f)
        {
            var st = new Stopwatch();
            st.Start();
            f();
            st.Stop();
            return st;
        }

        //Extend all the things! with constraints too
        public static void DeepCopy<T>(this T o)
            where T : class
        {

        }
    }
}