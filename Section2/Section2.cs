namespace CSharpTopics.Section2
{
    public class Section2 : SectionBase
    {
        public override void RunSection()
        {
            #region Reflection
            Reflection();

            Inspection();

            Construction();

            Invocation();

            DelegatesAndEvents();

            Attributes();

            #endregion
        }

        private void Attributes()
        {
            Console.WriteLine("\n===Attributes===\n\n");

            var sm = typeof(AttributeDemo).GetMethod("SomeMethod");

            foreach (var attr in sm.GetCustomAttributes(false))
            {
                Console.WriteLine($"Found Attribute: \n{attr.GetType()}\n");

                //TIL: some nice casting syntax sugar, an out var of type
                if (attr is RepeatAttribute ra)
                {
                    Console.WriteLine($"Need to repeat {ra.Iterations} times\n");
                }
            }
        }

        private void DelegatesAndEvents()
        {
            Console.WriteLine("\n===Delegates And Events===\n\n");
            var demo = new Demo();

            //Lets subscribe via reflection
            var eventInfo = typeof(Demo).GetEvent("MyEvent");
            var handlerMethod = demo.GetType().GetMethod("Handler");

            var handler = Delegate.CreateDelegate(
                eventInfo.EventHandlerType,
                null,
                handlerMethod
            );

            eventInfo.AddEventHandler(demo, handler);

            //Workaround as I am not invoking from delcared type
            handler.Method.Invoke(demo, new object[] { null, 321 });
        }

        private void Invocation()
        {
            //The same as ctor invocation
            Console.WriteLine("\n===Invocation===\n\n");

            var s = "justkeepswimming  ";
            var t = typeof(string);
            Console.WriteLine($"string type:\n {t.FullName}\n");

            var trimMethod = t.GetMethod("Trim", Array.Empty<Type>());
            var result = trimMethod.Invoke(s, Array.Empty<object>());
            Console.WriteLine($"Invoke trim result:\n {s} {s.Length}\n");

            //Complex example, static with an out
            var numStr = "647";
            //.MakeByRefType() represents the out keyword in the method call
            var parseMethod = typeof(int).GetMethod("TryParse",
                new[] { typeof(string), typeof(int).MakeByRefType() });
            Console.WriteLine($"Parse Method:\n {parseMethod}\n");

            //create the parameters
            //The null is overwritten by the method call due to by ref
            object[] args = { numStr, null };

            //For a static method, no reference is needed
            var isOk = parseMethod.Invoke(null, args);

            Console.WriteLine($"numStr parsed ok?:\n {isOk}\n");
            Console.WriteLine($"numStr byref value:\n {args[1]}\n");

            //Generic<T> invocation
            var at = typeof(Activator);
            var method = at.GetMethod("CreateInstance", Array.Empty<Type>());
            Console.WriteLine($"CreateInstance Method:\n {method}\n");

            var ciGeneric = method.MakeGenericMethod(typeof(Guid));
            Console.WriteLine($"CreateInstance<Guid> Method:\n {ciGeneric}\n");

            //The Guid method requires no parameters, so we can get away with null
            var guid = ciGeneric.Invoke(null, null);
            Console.WriteLine($"CreateInstance<Guid> guid:\n {guid}\n");

        }

        private void Construction()
        {
            Console.WriteLine("\n===Construction===\n\n");

            var t = typeof(bool);
            var b = Activator.CreateInstance(t);

            Console.WriteLine($"bool CreateInstance() b:\n {b}\n");

            b = Activator.CreateInstance<bool>();
            Console.WriteLine($"bool CreateInstance<T>() b:\n {b}\n");

            var wc = Activator.CreateInstance("System", "System.Timers.Timer");
            Console.WriteLine($"Timer wc:\n {wc}\n");
            Console.WriteLine($"wc.UnWrap():\n {wc.Unwrap()}\n");

            var alType = Type.GetType("System.Collections.ArrayList");
            Console.WriteLine($"ArrayList Type alType:\n {alType}\n");

            //Get the default constructor, empty array of parameter types
            var alCtor = alType.GetConstructor(Array.Empty<Type>());
            Console.WriteLine($"ArrayList default ctor:\n {alCtor}\n");

            //Invoke it create an empty array list
            var al = alCtor.Invoke(Array.Empty<object>());
            Console.WriteLine($"ArrayList instance:\n {al}\n");

            //Get parameterised ctor of string and init
            var st = typeof(string);
            var sctor = st.GetConstructor(new[] { typeof(char[]) });
            var str = sctor.Invoke(new object[] {
                new[] { 't', 'e', 's', 't' }
            });
            Console.WriteLine($"String instance:\n {str}\n");

            //Init a generic list
            var listType = Type.GetType("System.Collections.Generic.List`1");
            var listOfInt = listType.MakeGenericType(typeof(int));
            var listIntCtor = listOfInt.GetConstructor(Array.Empty<Type>());
            var listInstance = listIntCtor.Invoke(Array.Empty<object>());
            Console.WriteLine($"List<int> instance:\n {listInstance}\n");

            //Create Array Types
            var charType = typeof(char);
            var charArr = Array.CreateInstance(charType, 10);
            Console.WriteLine($"charArr:\n {charArr.GetType().FullName}\n");
            Console.WriteLine($"char[] instance:\n {((char[])charArr).Length}\n");

            //Get the constructor this time
            var charArrEmpty = charType.MakeArrayType();
            Console.WriteLine($"char[] type:\n {charArrEmpty.FullName}\n");
            var charArrCtor = charArrEmpty.GetConstructor(new[] { typeof(int) });
            char[] arr = (char[])charArrCtor.Invoke(new object[] { 20 });
            Console.WriteLine($"char[] arr instance:\n {arr.Length}\n");

            for (int i = 0; i < arr.Length; i++)
                arr[i] = (char)('A' + i);

            Console.WriteLine($"Char string:\n {new string(arr)}\n");
        }

        public void Inspection()
        {
            Console.WriteLine("\n===Inspection===\n\n");
            var t = typeof(Guid);
            Console.Write($"t Names:\n {t.FullName} {t.Name}\n");

            //Get the constructors
            foreach (var ctor in t.GetConstructors())
            {
                Console.Write($" - Guid(");
                var pars = ctor.GetParameters();

                for (var i = 0; i < pars.Length; i++)
                {
                    Console.Write($"{pars[i].ParameterType.Name} {pars[i].Name}");
                    if (i + 1 != pars.Length) Console.Write($", ");
                }
                Console.WriteLine($")");
            }

            //Get the methods
            foreach (var method in t.GetMethods())
            {
                Console.Write($" - {method.Name}(");
                var pars = method.GetParameters();

                for (var i = 0; i < pars.Length; i++)
                {
                    Console.Write($"{pars[i].ParameterType.Name} {pars[i].Name}");
                    if (i + 1 != pars.Length) Console.Write($", ");
                }
                Console.WriteLine($")");
            }
        }

        public void Reflection()
        {
            Console.WriteLine("\n===Reflection===\n\n");
            //System.Type getting information
            var t = typeof(int);
            var t2 = "hello".GetType();
            Console.Write($"t Methods:\n {t.GetMethods()}\n");
            Console.Write($"t2 FullName:\n {t2.FullName}\n");
            Console.Write($"t2 Fields:\n {t2.GetFields()}\n");
            Console.Write($"t2 Methods:\n {t2.GetMethods()}\n");

            var a = typeof(string).Assembly;
            Console.Write($"Assembly Info:\n {a.ToString()}\n");

            var types = a.GetTypes();
            Console.Write($"Assembly Types:\n {types.ToString()}\n");

            Console.Write($"Assembly Type at 10 Name:\n {types[10].FullName}\n");
            Console.Write($"Assembly Type at 10 Methods:\n {types[10].GetMethods()}\n");

            //Getting type info by name
            var t3 = Type.GetType("System.Int64");
            Console.Write($"Type Int64 by name:\n {t3?.FullName}\n");
            Console.Write($"Type Int64 by methods:\n {t3?.GetMethods()?.Count()}\n");

            //Generics
            var t4 = Type.GetType("Systems.Collections.Generics.List`1");
            Console.Write($"Type Generics List by name:\n {t4?.FullName}\n");
            Console.Write($"Type Generics List by name:\n {t4?.GetMethods()?.Count()}\n");
        }
    }
}