using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace CSharpTopics.Section4
{
    public static class Part3Extensions
    {
        //Shortening wrapper
        public static StringBuilder al(this StringBuilder sb, string text)
        {
            return sb.AppendLine(text);
        }

        //Multiple calls
        //AppendLine()
        //AppendFormat()
        public static StringBuilder AppendFormatLine(
            this StringBuilder sb, string format, params object[] args)
        {
            return sb.AppendFormat(format, args).AppendLine();
        }
        
        //Composite 
        // x ^ y is ok but what about multiple ops?
        public static ulong Xor(this IList<ulong> values)
        {
            ulong first = values[0];
            foreach(var x in values.Skip(1))
                first ^= x;
            return first;
        }

        //Params Extension
        public static void AddRange<T>(this IList<T> list,
            params T[] objects)
        {
            foreach(var o in objects)
                list.Add(o);

            //Causes a stack overflow
            //list.AddRange(objects);
        }

        //Antistatic wrapper
        public static string f(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        //Factory Extension
        public static DateTime June(this int day, int year)
        {
            return new DateTime(year, 6, day);
        }
    }
}