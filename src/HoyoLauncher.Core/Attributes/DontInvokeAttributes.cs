namespace HoyoLauncher.Core.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DontInvokeAttribute : Attribute
{
    /// <summary>
    /// Attribute for EventsAttribute methods to not invoke
    /// </summary>
    public DontInvokeAttribute() { }
}