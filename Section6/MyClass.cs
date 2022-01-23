using System;
using System.Diagnostics;
using System.Threading;

namespace CSharpTopics
{
    public class MyClass : IDisposable
    {

        public MyClass()
        {
            Console.WriteLine("Ctor: Oh Hi");    
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose: Bye Felicia"); 
        }
    }

}