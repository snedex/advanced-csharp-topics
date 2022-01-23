using System.Diagnostics;
using System.Runtime.Serialization;

namespace CSharpTopics.Section4
{
    public static class Part2Extensions
    {
        //Storing data associated with an object
        //Not storing references as they wont be destroyed in a timely manner
        //Weak reference will not prevent GC and no increase use/call count
        private static Dictionary<WeakReference, object> data 
            = new Dictionary<WeakReference, object>();

        public static object GetTag(this object o)
        {
            //Check to see if the object reference is still alive
            var key = data.Keys.FirstOrDefault(k => k.IsAlive && k.Target == o);

            return key != null ? data[key] : null;
        }

        //And the storing of data
        public static void SetTag(this object o, object tag)
        {
            var key = data.Keys.FirstOrDefault(k => k.IsAlive && k.Target == o);
            if(key != null)
            {
                data[key] = tag;
            } else
            {
                data.Add(new WeakReference(o), tag);
            }
        }
    }
}