

namespace CSharpTopics.Section6
{
    public static class Part3Extensions
    {
        //More expensive call to add to a list
        //Makes it slightly more readable in code, call on the object to add to a collection
        public static T AddTo<T>(this T self, ICollection<T> col)
        {
            col.Add(self);
            return self;
        }

        //This allows adding to multple collections at once
        public static T AddToLists<T>(this T self, params ICollection<T>[] cols)
        {
            foreach(var col in cols)
                col.Add(self);
            return self;
        }

        //This wraps the contains call of a collection to make it cleaner
        public static bool IsOneOf<T>(this T self, params T[] values)
        {
            return values.Contains(self);
        }

        //Wraps checking a collection has no values
        public static bool HasNo<TSubject, T>(this TSubject self, 
            Func<TSubject, IEnumerable<T>> props)
        {
            return !props(self).Any();
        }

        //Wraps checking a collection has values
        public static bool HasSome<TSubject, T>(this TSubject self, 
            Func<TSubject, IEnumerable<T>> props)
        {
            return props(self).Any();
        }

        public static BoolMarker<TSubject> HasNoImplicit<TSubject, T>(this TSubject self, 
            Func<TSubject, IEnumerable<T>> props)
        {
            return new BoolMarker<TSubject>(!props(self).Any(), self);
        }

         public static BoolMarker<TSubject> HasSomeImplicit<TSubject, T>(this TSubject self, 
            Func<TSubject, IEnumerable<T>> props)
        {
            return new BoolMarker<TSubject>(props(self).Any(), self);
        }

        //We can now setup extensions actiing on the boolmarkers for chaining
        public static BoolMarker<T> HasNoChain<T, U>(this BoolMarker<T> marker, 
            Func<T, IEnumerable<U>> props)
        {
            //If it's an and and already failed, do nothing, already failed
            if(marker.PendingOp == BoolMarker<T>.Operation.And && !marker.Result)
                return marker;
            return new BoolMarker<T>(!props(marker.Self).Any(), marker.Self);
        }

         public static BoolMarker<T> HasSomeChain<T, U>(this BoolMarker<T> marker, 
            Func<T, IEnumerable<U>> props)
        {
            //If it's an and and already failed, do nothing, already failed
            if(marker.PendingOp == BoolMarker<T>.Operation.And && !marker.Result)
                return marker;
            return new BoolMarker<T>(props(marker.Self).Any(), marker.Self);
        }
    }
}