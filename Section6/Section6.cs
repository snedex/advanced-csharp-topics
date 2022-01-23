using System;
using System.Diagnostics;
using System.Threading;

namespace CSharpTopics.Section6
{
    public class Section6 : SectionBase
    {
        public override void RunSection()
        {
            #region Assorted Topics

            ExploitingDisposable();

            ContinuationPassingStyle();

            LocalIoC();

            Mnemonics();

            #endregion
        }

        private void Mnemonics()
        {
            Console.WriteLine("\n===Mnemonics===\n\n");

            //Emmet, HTML Zen
            //Essentially is snippets in a chain
            //http://github.com/nesteruk/Mnemonics
            //Only for resharper...

             Console.WriteLine($"This was a demo of an Resharper extension: http://github.com/nesteruk/Mnemonics");
        }

        private void LocalIoC()
        {
            //See Part3Extensions for extension code
            Console.WriteLine("\n===Local Inversion of Control===\n\n");

            //Normal calls, can we do this inverting control?
            //The idea is making it read more like english, not sure if i agree here
            var list = new List<int>();
            list.Add(24);
            Console.WriteLine($"List {nameof(list)} has {list.Count} elements\n");

            //add to the list
            25.AddTo(list);
            Console.WriteLine($"Inverted Add:");
            Console.WriteLine($"List {nameof(list)} has {list.Count} elements\n");

            //Adding to multiple lists
            var list2 = new List<int>();
            96.AddToLists(list, list2);
            Console.WriteLine($"Adding as params:");
            Console.WriteLine($"List {nameof(list)} has {list.Count} elements");
            Console.WriteLine($"List {nameof(list2)} has {list2.Count} elements");

            //This is not IoC dependency injection, we're inverting the Add operation.

            //Checking an op code
            ProcessCommand("XOR");

            //Checking for people with no names
            Process(new Person());
        }

        public void Process(Person person)
        {
            //This is aparently hard to read
            // if (person.Names.Count == 0) 
            // {

            // }

            //If hard to read as the ! is present 
            // if(!person.Names.Any())
            // {

            // }

            //The more readable extension
            if(person.HasNo(p => p.Names))
            {

            }

            //We can check if the have some
            if(person.HasSome(p => p.Names))
            {

            }

            //and now we can chain these if we need.
            //First implicit casting
            if(person.HasSomeImplicit(p => p.Names))
            {

            }

            //Now we have a readable and fluent interface to chain ops
            if(person.HasSomeImplicit(p=> p.Names).And.HasNoChain(p => p.Children))
            {
                //Names are different here just so you can see the evolution of the extenion method
            }
            //Not sure if I like this.
        }

        public void ProcessCommand(string opcode)
        {
            //This is aparently hard to read, you ideally want to say is opcode in
            // if (opcode == "AND" || opcode == "OR" || opcode == "XOR")
            // {

            // }

            //Alternative would be a .Contains, aparently ugly
            // if (new[] { "AND", "OR", "XOR"}.Contains(opcode))
            // {

            // }

            //Or a split .Contains, this is ugly,
            // if ("AND OR XOR".Split(' ').Contains(opcode))
            // {

            // }

            //Cleaner implementation would be
            if(opcode.IsOneOf("AND", "OR", "XOR"))
            {

            }
        }


        private void ContinuationPassingStyle()
        {
            Console.WriteLine("\n===Continuation Passing Style===\n\n");
            //Used in javascript, a chain of calls for pass results

            //Specifying a workflow, allows splitting up of algorithms
            var solver = new QuadraticEquationSolver();
            var solutions = solver.Start(1, 10, 16);
            Console.WriteLine($"Solver first run: {solutions}");

            //Giving a state back to use in follow on logic
            var result = solver.StartWithResult(1, 10, 5, out solutions);
            if(result == WorkflowResult.Solved)
            {
                //Do something :)
                Console.WriteLine($"Quadratic Root: {solutions}");
            }
        }

        private void ExploitingDisposable()
        {
            //IDisposable, neat syntax tricks
            Console.WriteLine("\n===Exploiting Disposable===\n\n");

            //The normal way of using an IDisposable
            using(var mc = new MyClass())
            {

            }

            using(new MyClass())
            {
                //Cant use the MyClass instance in this scope
            }

            //Now we're doing things on the dispose
            using (new SimpleTimer())
            {
                Thread.Sleep(1000);
            }

            //Now we can invoke what we like with a generic disposable object
            var st = new Stopwatch();
            using(Disposable.Create(
                () => st.Start(),
                () => st.Stop()
            ))
            {
                Console.WriteLine("We are about to wait with our Disposable");
                Thread.Sleep(1002);
            }
            Console.WriteLine($"We waited for you, a whole {st.ElapsedMilliseconds}msec");
        }
    }
}