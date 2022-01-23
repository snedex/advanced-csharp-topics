using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace CSharpTopics.Section4
{
    public static class Part4Extensions
    {
        //Maybe extension to provide maybe monda functionality with application logic
        public static TResult With<TInput, TResult>(this TInput o, 
            Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if(o == null) return null; //Do nothing
            else return evaluator(o); //Continue the evaluation chain
        }

        //Extension to determine flow control for checking null
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class 
        {
            if(o == null) return null;
            return evaluator(o) ? o : null;
        }

        //Extenion method for invoking an action, but not changing the context
        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if(o == null) return null;
            action(o);
            return o;
        }

        //Extension to evaluate the final result and return state
        public static TResult Return<TInput, TResult>(this TInput o, 
        Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
            {
                if(o == null) return failureValue;
                return evaluator(o);
            }

        //Extension method for value types can be applied also
        public static TResult WithValue<TInput, TResult>(this TInput o,
            Func<TInput, TResult> evaluator)
            where TInput : struct
            {
                return evaluator(o);
            }
    }
}