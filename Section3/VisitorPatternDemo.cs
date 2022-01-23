using System.Text;

namespace CSharpTopics.Section3
{
    public abstract class Expression
    {
        //This would be a way of implementing it, we're assumign we can't change the object hierarchy here
        //public abstract void Print(StringBuilder sb);
    }

    public class Literal : Expression 
    {
        public Literal(double value) => Value = value;

        public Double Value { get;  }
    }

    public class Addition : Expression 
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public Addition(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }

    public class ExpressionPrinter 
    {
        //If you comment this out with the dynamic parameter hinting, this will cause a binder exception at runtime
        public static void Print(Literal literal, StringBuilder sb)
        {
            sb.Append(literal.Value);
        }

        public static void Print(Addition addition, StringBuilder sb)
        {
            //This now starts to get ugly as you need lots of overloads
            //Use the dynamic hint to find the overload
            sb.Append("(");
            Print((dynamic)addition.Left, sb);
            sb.Append("+");
            Print((dynamic)addition.Right, sb);
            sb.Append(")");
        }

        // public static void Print(Expression e, StringBuilder sb)
        // {
        //     //and a general purpose method needs a lot of if statements
        //     if(e.GetType() == typeof(Literal))
        //     {
                
        //     }
        // }
    }
}