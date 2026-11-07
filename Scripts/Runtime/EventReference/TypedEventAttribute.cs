#nullable enable

using System;

namespace PostEnot.Toolkits.EventManagement
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class TypedEventAttribute : Attribute
    {
        public string? MenuName { get; set; }
    }
}
