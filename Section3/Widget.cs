using System.Dynamic;

namespace CSharpTopics.Section3
{
    public class Widget : DynamicObject
    {
        //This is the method that is called when attempting to access a member of the class
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = binder.Name;
            return true;
        }

        //This is called when accesses are done with a key or index
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
        {
            //In this case, we only want one index and build a string of the length of the index
            if(indexes.Length == 1)
            {
                result = new string('*', (int)indexes[0]);
                return true;
            }

            result = null;
            return false;
        }

        public void WhatIsThis()
        {
            //Cannot use 'this' in a dynamic object as it needs static type checks
            //Console.WriteLine(this.World);

            //We can work around this by wrapping this in a Dynamic This.
            Console.WriteLine(This.World);
        }

        public dynamic This => this;
    }
}