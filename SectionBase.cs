using System.Text;

public abstract class SectionBase
{
    public abstract void RunSection();

    internal string ByteArrayToString(byte[] array)
    {
        var sb = new StringBuilder();

        sb.Append("{ ");
        Array.ForEach<byte>(array, b => sb.Append($"{b.ToString()}, "));
        sb.Remove(sb.Length - 2, 2);
        sb.Append(" }");

        return sb.ToString();
    }
}
