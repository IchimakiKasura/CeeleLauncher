namespace HoyoLauncher.Core.Attributes;

/// <summary>
/// Sets all the Events Automatically
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
                foreach(MethodInfo methods in types.GetMethods(Flags))
                    methods.Invoke(types, null);
            }
    }
}