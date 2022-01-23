using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

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
             //See: Part4Extensions for the extension code
            Console.WriteLine("\n===Maybe Monad===\n\n");
            //Internalised with the ?. operator already

            //However if you have application logic in null checks the ?. isn't as suitable
            MyMethod(new PersonPart30() 
            {
                Address = new Address() {
                    PostCode = "SY3 1AA"
                }
            });
        }

        public void MyMethod(PersonPart30 p)
        {
            string postcode = "UNKNOWN";

            //This is the old way, which is now made redundant with ?. notation
            if(p != null && p.Address != null && p.Address.PostCode != null)
                postcode = p.Address.PostCode;
            Console.WriteLine($"Old way postcode:\n {postcode}");

            //Now we can just write
            postcode = "UNKNOWN";
            postcode = p?.Address?.PostCode;
            Console.WriteLine($"Maybe Monad:\n {postcode}");

            //However there might be additional code between null checks
            //So the maybe monad isn't useful here..
            postcode = "UNKNOWN";
            if(p != null)
            {
                if(HasMedicalRecord(p) && p.Address != null)
                {
                    CheckAddress(p.Address);
                    if(p.Address.PostCode != null)
                        postcode = p.Address.PostCode;
                    else
                        postcode = "UNKNOwN";
                }
            }
            Console.WriteLine($"If Example:\n {postcode}");

            //So we would write extensions to handle this logic instead for drilling down
            postcode = "UNKNOWN";
            postcode = p.With(x => x.Address).With(x => x.PostCode);
            Console.WriteLine($"With Example:\n {postcode}");

            //Now we can do a maybe monad with logic without large if blocks
            postcode = "UNKNOWN";
            postcode = p.If(HasMedicalRecord)
                        .With(x => x.Address)
                        .Do(CheckAddress)
                        .Return(x => x.PostCode, "UNKOWN");
            Console.WriteLine($"Fluent Extension Example:\n {postcode}");

            //You can do this with value types as well
            int value = 42.WithValue(ValidateMeaningOfLife);
            Console.WriteLine($"Value Type Example:\n {value}");
        }

        private int ValidateMeaningOfLife(int arg)
        {
            if(arg == 42)
                return arg;

            return -1;
        }

        private void CheckAddress(Address address)
        {
            //Do something!
        }

        private bool HasMedicalRecord(PersonPart30 p)
        {
            return true;
        }

        public void ExtensionMethodPatterns()
        {
            //See: Part3Extensions for the extension code
            Console.WriteLine("\n===Extension Method Patterns===\n\n");
            //Name shortening wrappers
            //Hurts readability but can help making things simpler
            //AppendLine().AppendLine()
            var sb = new StringBuilder();
            sb.al("123").al("456");
            Console.WriteLine($"Shortened Calls:\n {sb.ToString()}");

            //Multple Calls in one call
            sb.Clear();
            sb.AppendFormatLine("{0} {1}","Stood far back", 
                "when the Gravitas was handed out");
            Console.WriteLine($"M..m..m..m.. Multicall:\n {sb.ToString()}");

            //Composite actions
            List<ulong> thingsToXor = new List<ulong>() { 6455601654ul, 345645673ul, 4706707ul, 570867ul, 7456450540ul, 8450434ul, 954043245ul, 63452ul };
            ulong result = thingsToXor.Xor();
            Console.WriteLine($"Xor of thingsToXor:\n {result}");

            //params extensions
            //The normal way of doing things, adding or adding by a range
            var list = new List<int>();
            list.AddRange(new[] {1,2,3});

            //Now with a params extension
            list.AddRange(1, 2, 3);
            sb.Clear();

            sb.Append("List after param AddRange(params T[]):\n { ");
            foreach(var item in list)
            {
                sb.Append($"{item}, ");
            }
            sb.Append(" }");
            Console.WriteLine(sb.ToString());

            //Antistatic methods.... 
            //wrapping static methods to instances, the old way
            string.Format("foo {0}", 42);

            //More convienient, prior to interpolation
            Console.WriteLine("Anti Static:\n {0}".f("My name is Bond, James Bond"));
            
            //Factory Extension Method
            //Can be more readable, this example isn't great
            var notToday = 10.June(2020);
            Console.WriteLine($"Factory notToday: {notToday.ToString("dd MMM yyyy")}");
            
        }

        public void ExtensionMethodsAndPersistience()
        {
            //See: Part2Extensions for the extension code
            Console.WriteLine("\n===Storing Data with Extension Methods===\n\n");
            //Storing some data using the extensions ion Part2 Extensions via weka references
            string s = "Meaning of Life";
            s.SetTag(42);

            Console.WriteLine($"String: {s} data: {s.GetTag()}");

            s = "Gravitas";
            s.SetTag("What Gravitas?");

            Console.WriteLine($"String: {s} data: {s.GetTag()}");
        }

        public void ExtensionMethods()
        {
            //See: Part1Extensions for the extension code
            Console.WriteLine("\n===Extension Methods===\n\n");

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