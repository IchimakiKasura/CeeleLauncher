namespace HoyoLauncher.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class StaticWindowAttribute : Attribute
{
    public StaticWindowAttribute() { }
}