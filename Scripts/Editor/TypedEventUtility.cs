using System;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    internal static class TypedEventUtility
    {
        public static object CreateTypedEventInstance(Type eventArgumentType)
        {
            if (eventArgumentType == null)
            {
                return null;
            }
            Type openGenericType = typeof(TypedEvent<>);
            Type closedGenericType = openGenericType.MakeGenericType(eventArgumentType);
            return Activator.CreateInstance(closedGenericType);
        }

        public static Type GetEventTypeOfTypedEvent(object typedEvent)
        {
            if (typedEvent is not ITypedEvent)
            {
                return null;
            }
            Type typedEventType = typedEvent.GetType();
            Type genericTypedEventTypeDefinition = typedEventType.GetGenericTypeDefinition();
            Type targetGenericTypeDefinition = typeof(TypedEvent<>);
            if (genericTypedEventTypeDefinition != targetGenericTypeDefinition)
            {
                return null;
            }
            Type[] genericArguments = typedEventType.GetGenericArguments();
            if ((genericArguments == null) || (genericArguments.Length == 0))
            {
                return null;
            }
            return genericArguments[0];
        }

        public static string GetTypedEventMenuName(Type type)
        {
            if (type == null)
            {
                return "<None>";
            }
            object[] attributes = type.GetCustomAttributes(typeof(TypedEventAttribute), false);
            if ((attributes == null) || (attributes.Length == 0))
            {
                return type.Name;
            }
            object attributeUncasted = attributes[0];
            if (attributeUncasted is not TypedEventAttribute attribute)
            {
                return type.Name;
            }
            if (string.IsNullOrWhiteSpace(attribute.MenuName))
            {
                return type.Name;
            }
            return attribute.MenuName;
        }

        public static int Compare(Type x, Type y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            string xMenuName = GetTypedEventMenuName(x);
            string yMenuName = GetTypedEventMenuName(y);
            return string.Compare(xMenuName, yMenuName);
        }
    }
}
