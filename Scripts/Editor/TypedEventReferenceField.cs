using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    [UxmlElement]
    internal partial class TypedEventReferenceField : PopupField<Type>
    {
        public TypedEventReferenceField() : base(
            InitChoices(),
            0,
            TypedEventUtility.GetTypedEventMenuName,
            TypedEventUtility.GetTypedEventMenuName) { }

        private static List<Type> InitChoices()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesWithAttribute<TypedEventAttribute>();
            int capacity = types.Count + 1;
            List<Type> choices = new(capacity)
            {
                null
            };
            choices.AddRange(types);
            choices.Sort(TypedEventUtility.Compare);
            return choices;
        }
    }
}
