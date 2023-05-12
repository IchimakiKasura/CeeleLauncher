namespace HoyoLauncher.Core.Attributes;

/// <summary>
/// Sets all the Events Automatically
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class EventsAttribute : Attribute
{
    public EventsAttribute() { }

    public static void SetEvents()
    {
        foreach(Type types in Assembly.GetExecutingAssembly().GetTypes())
            if(types.GetCustomAttributes(typeof(EventsAttribute), true).Length > 0)
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
                foreach(MethodInfo methods in types.GetMethods(flags))
                    methods.Invoke(types, null);
            }
    }
}