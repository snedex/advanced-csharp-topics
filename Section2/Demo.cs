namespace CSharpTopics.Section2;
public class Demo
{
    public event EventHandler<int> MyEvent;

    public void Handler(object sender, int arg)
    {
        Console.WriteLine($"Event:\n I just got {arg}\n");
    }
}
