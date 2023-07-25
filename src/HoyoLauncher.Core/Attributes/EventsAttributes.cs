namespace HoyoLauncher.Core.Attributes;

/// <summary>
/// Sets all the Events Automatically<br/>
/// Only Publicly Static voids are invoked not private voids
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class EventsAttribute : Attribute
{
    private static readonly BindingFlags Flags = BindingFlags.Public | BindingFlags.Static;

    public EventsAttribute() { }

    public static void SetEvents()
    {
        foreach(Type types in Assembly.GetExecutingAssembly().GetTypes())
            if(types.GetCustomAttributes(typeof(EventsAttribute), true).Length > 0)
            {
                EventDebugger(types.Name);

                foreach(MethodInfo methods in types.GetMethods(Flags))
                    if(methods.GetCustomAttributes(typeof(DontInvokeAttribute), true).Length is 0)
                        methods.Invoke(types, null);
            }
    }

    
    [Conditional("DEBUG")]
    static void EventDebugger(string name) =>
        Debug.WriteLine($"Class [ {name} ] has invoked its methods.");
}