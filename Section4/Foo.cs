namespace CSharpTopics.Section4
{
    public class Foo
    {
        public virtual string Name => "Foo";
    }

    //Extension methods are not carried over to derived types
    public class FooDerived : Foo
    {
        public override string Name => "FooBar";

    }
}