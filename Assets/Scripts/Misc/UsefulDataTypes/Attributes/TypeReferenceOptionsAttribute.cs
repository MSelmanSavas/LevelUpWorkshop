using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UsefulDataTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class TypeReferenceOptionsAttribute : PropertyAttribute
    {
        public List<Type> InheritedTypes = new List<Type>();

        public List<Type> IncludeTypes = new List<Type>();
        public HashSet<Type> RestrictTypes = new HashSet<Type>();
        public bool ShouldSort = false;

        public TypeReferenceOptionsAttribute(Type[] inheritedTypes = null, Type[] includeTypes = null, Type[] restrictTypes = null, bool shouldSort = false)
        {
            if (inheritedTypes != null)
                InheritedTypes = inheritedTypes.ToList();

            if (includeTypes != null)
                IncludeTypes = includeTypes.ToList();

            if (restrictTypes != null)
                RestrictTypes = restrictTypes.ToHashSet();

            ShouldSort = shouldSort;
        }

        public TypeReferenceOptionsAttribute(Type inheritedFrom)
        {
            InheritedTypes.Add(inheritedFrom);
        }

        public TypeReferenceOptionsAttribute(params Type[] includeTypes)
        {
            IncludeTypes = includeTypes.ToList();
        }

        public TypeReferenceOptionsAttribute(Type inheritedFrom, params Type[] includeTypes)
        {
            InheritedTypes.Add(inheritedFrom);
            IncludeTypes = includeTypes.ToList();
        }

        public TypeReferenceOptionsAttribute(System.Type[] inheritedTypes, System.Type[] includeTypes)
        {
            InheritedTypes.AddRange(inheritedTypes.ToList());
            IncludeTypes = includeTypes.ToList();
        }
    }

}
