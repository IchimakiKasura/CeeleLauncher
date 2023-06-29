namespace HoyoLauncher.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class StaticWindowAttribute : Attribute
{
    /// <summary>
    /// Attribute for Static property of the Window
    /// </summary>
    public StaticWindowAttribute() { }
}