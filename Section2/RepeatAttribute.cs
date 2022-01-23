namespace CSharpTopics.Section2;

public class RepeatAttribute : Attribute
{
    public int Iterations { get; }
    
    public RepeatAttribute(int iterations)
    {
        Iterations = iterations;
    }
}

public class AttributeDemo 
{
    [Repeat(3)]
    public void SomeMethod()
    {

    }
}
