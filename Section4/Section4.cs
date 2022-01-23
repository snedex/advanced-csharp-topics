using System.Diagnostics;
using System.Runtime.Serialization;

namespace CSharpTopics.Section4
{
    public class Section4 : SectionBase
    {
        public override void RunSection()
        {
            #region Extension methods

            ExtensionMethods();

            ExtensionMethodsAndPersistience();

            ExtensionMethodPatterns();

            ExtensionMethodMaybeMonad();


            #endregion
        }

        public void ExtensionMethodMaybeMonad()
        {

        }

        public void ExtensionMethodPatterns()
        {

        }

        public void ExtensionMethodsAndPersistience()
        {
            //Storing some data
        }

        public void ExtensionMethods()
        {
            Console.WriteLine("\n===Exnteion Methods===\n\n");

            //foo extension
            var foo = new Foo();
            Console.WriteLine($"Foo's name is {foo.Name} and it's {foo.Measure()} characters.");

            //A system type extension
            var number = 42;
            Console.WriteLine($"the number {number} in binary is {number.ToBinary()}.");

            //Extension methods are tied to the instance type and dont care about polymorphism
            var derived = new FooDerived();
            Foo parent = derived;
            Console.WriteLine($"Derived Length is: {derived.Measure()}.");
            Console.WriteLine($"Parent Length is: {parent.Measure()}.");

            //You cannot override method implementations using extension methods
            Console.WriteLine($"Foo.ToString(): {foo.ToString()}.");

            //You can do tuples too
            var me = ("Will", 500).ToPerson();
            Console.WriteLine($"My name is {me.Name} and I'm {me.Age} years old.");

            //Generics anyone?
            var len = Tuple.Create(1.0, false).Measure();
            Console.WriteLine($"Generic Tuple.Measure(): {len}");

            //Extend a delegate to measure execution time
            Func<int> calc = delegate
            {
                Thread.Sleep(1000);
                return 42;
            };
            var st = calc.Measure();
            Console.WriteLine($"We took {st.ElapsedMilliseconds}msec to execute this.");

            //You can extend anything! Extend all the things!
            var test = new List<string>();
            test.DeepCopy();
        }
    }

    

    
}