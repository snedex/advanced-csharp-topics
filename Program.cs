using System.Reflection;
// See https://aka.ms/new-console-template for more information


Console.WriteLine("==Advanced CSharp Topics Follow along==");

//Lets use some lessons to invoke all of these as we move through sections of the course
var ts = Assembly.GetExecutingAssembly().GetTypes();

foreach(var t in ts.OrderBy(t => t.Name))
{
    if(t.BaseType == typeof(SectionBase))
    {
        var instance = Activator.CreateInstance(t);
        var runMethod = t.GetMethod("RunSection", Array.Empty<Type>());
        runMethod.Invoke(instance, Array.Empty<object>());
    }
}
