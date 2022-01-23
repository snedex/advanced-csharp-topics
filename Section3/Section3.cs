using System.Dynamic;
using System.Text;
using System.Xml.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace CSharpTopics.Section3;

public class Section3 : SectionBase
{
    public override void RunSection()
    {
        #region Dynamic Programming
        
        DynamicObjects();

        TheDynamicObject();

        DynamicXML();

        TheExpandoObject();

        VisitorPatternWithDynamicDispatch();

        #endregion
    }

    private void VisitorPatternWithDynamicDispatch()
    {
        //DLR has some performance costs
        Console.WriteLine("\n===Visitor Pattern With Dynamic Dispatch===\n\n");

        //1+2+3
        var e = new Addition(
            new Addition(
                new Literal(1),
                new Literal(2)
            ), new Literal(3)
        );

        //Now print out the expression
        var sb = new StringBuilder();
        //You want to do this, but the expression isn't a literal, so you need to implement this call
        //ExpressionPrinter.Print(e, sb);

        //We can cast to a dynamic parameter to dispatch 
        ExpressionPrinter.Print((dynamic)e, sb);
        Console.WriteLine($"final output: {sb}");

        //This can cover overloads nicely without lots of if statements
        //Comes at a cost of using the DLR.
    }

    private void TheExpandoObject() 
    {
        Console.WriteLine("\n===The ExpandoObject===\n\n");

        dynamic person = new ExpandoObject();

        person.FirstName = "John";
        person.Age = 22;
        
        Console.WriteLine($"Person called {person.FirstName} and is {person.Age} years old");

        //You can expand expando objects too!
        person.Address = new ExpandoObject();
        person.Address.City = "London";
        person.Address.Country = "UK";
        Console.WriteLine($"{person.FirstName} lives in {person.Address.City}, {person.Address.Country}\n");

        //We can also create methods...
        person.SayHello = new Action(() => { Console.WriteLine("Hello!"); });

        //And we can also do events! First init the variable
        person.FallsIll = null;

        //Declare an anonymous handler inline
        person.FallsIll += new EventHandler<dynamic>((sender, args) => {
            Console.WriteLine($"A doctor is required for {args}");
        });

        //Now to invoke it, dynamic type allows us to specify the first name
        EventHandler<dynamic> e = person.FallsIll;
        e?.Invoke(person, person.FirstName);

        //Castable to the dictionary as this is essentially how it acts
        var dict = (IDictionary<string, object>)person;
        Console.WriteLine($"Has FirstName: {dict.ContainsKey("FirstName")}");
        Console.WriteLine($"Has LastName: {dict.ContainsKey("LastName")}");

        //and we can manipulate it
        dict["LastName"] = "Smith";
        Console.WriteLine($"Has LastName: {dict.ContainsKey("LastName")}");
        Console.WriteLine($"{person.FirstName}'s Last name is now: {dict["LastName"]}");
    }

    private void DynamicXML()
    {
         Console.WriteLine("\n===XML Traversal===\n\n");

        var xml = "<people><person name=\"Will\" /></people>";

        //Get the name using XML Linq
        var node = XElement.Parse(xml);
        var name = node.Element("person").Attribute("name");
        Console.WriteLine($"Person's name is:\n {name?.Value}\n");

        //Using Dynamic now
        //Goal is x.Person.Name
        dynamic dynXml = new DynamicXmlNode(node);
        Console.WriteLine($"dynamic Person's name is:\n {dynXml.person.name}\n");
    }

    private void TheDynamicObject()
    {
        Console.WriteLine("\n===The Dynamic Object===\n\n");

        //Create a custom dynamic object
        //Don't call it by type as it wont behave a weak type
        Widget staticWidget = new Widget();

        //Declare as a dynamic object
        dynamic w = new Widget();
        var widget = new Widget() as dynamic;

        //Now try to get the member hello and it's value
        Console.WriteLine($"Widget.Hello:\n {w.Hello} is it me you're looking for?\n");

        //Now lets get the same nubmer of stars specificed in an index
        Console.WriteLine($"Widget[10]]:\n {w[10]}\n");

        //Using a wrapped this
        Console.WriteLine($"Widget.WhatIsThis():");
        w.WhatIsThis();
    }

    private void DynamicObjects()
    {
        Console.WriteLine("\n===Dynamic Objects===\n\n");

        //Facilitates late binding at runtime.
        //Compiler wont throw exceptions, but at execution it will
        dynamic d = "hello";
        try
        {
            int n = d.Area;
        }
        catch 
        {
            Console.WriteLine("dynamic var doesn't have an Area, it's a string\n");
        }

        Console.WriteLine($"d type:\n {d.GetType()}\n");
        Console.WriteLine($"d length:\n {d.Length}\n");

        d += "world";
        Console.WriteLine($"d value:\n {d}\n");

        //Handling it
        try
        {
            int n = d.Area;
        }
        catch (RuntimeBinderException ex)
        {
            Console.WriteLine($"Binder Error:\n {ex.Message}\n");
        }

        //But we can change the type
        d = 321;
        Console.WriteLine($"d type:\n {d.GetType()}\n");
        d--;
        Console.WriteLine($"d value:\n {d}\n");

    }
}
