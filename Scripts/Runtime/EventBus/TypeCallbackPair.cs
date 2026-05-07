#nullable enable

using System;

namespace PostEnot.Toolkits.EventManagement
{
    internal readonly struct TypeCallbackPair : IEquatable<TypeCallbackPair>
    {
        public TypeCallbackPair(Type type, Delegate callback)
        {
            Type = type;
            Callback = callback;
        }

        public readonly Type Type { get; }
        public readonly Delegate Callback { get; }

        public bool Equals(TypeCallbackPair other)
            => (Type == other.Type) && (Callback == other.Callback);

        public override int GetHashCode() => HashCode.Combine(Type, Callback);

        public override bool Equals(object obj)
        {
            if (obj is TypeCallbackPair other)
            {
                return Equals(other);
            }
            return false;
        }

        public override string ToString()
            => $"{nameof(TypeCallbackPair)} {{ {nameof(Type)} = {Type}, {nameof(Callback)} = {Callback} }}";

        public static bool operator ==(TypeCallbackPair a, TypeCallbackPair b) => a.Equals(b);

        public static bool operator !=(TypeCallbackPair a, TypeCallbackPair b) => !a.Equals(b);
    }
}
