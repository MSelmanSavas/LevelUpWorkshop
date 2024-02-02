using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UsefulDataTypes.Attributes;
using UsefulDataTypes.Utils;

namespace UsefulDataTypes.Editor
{
    [CustomPropertyDrawer(typeof(TypeReference), true)]
    [CustomPropertyDrawer(typeof(TypeReferenceOptionsAttribute))]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        string[] _foundTypeNames = new string[0];
        System.Type[] _foundTypes = new Type[0];

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int index = 0;
            int selectedIndex = 0;

            TypeReference typeReference = GeneralUtilities.GetTargetObjectOfProperty(property) as TypeReference;
            var typeOptionsAttribute = attribute as TypeReferenceOptionsAttribute ?? null;

            if (_foundTypes.Length <= 0)
            {
                Assembly assembly = GeneralUtilities.GetAssemblyByName("Assembly-CSharp");

                if (GeneralUtilities.GetTargetObjectOfProperty(property).GetType().IsGenericType)
                {
                    object foundObj = GeneralUtilities.GetTargetObjectOfProperty(property);
                    System.Type type = foundObj.GetType().GetGenericTypeDefinition();

                    if (type == typeof(TypeReferenceInheritedFrom<>).GetGenericTypeDefinition())
                    {
                        assembly = Assembly.GetAssembly(foundObj.GetType().GetGenericArguments()[0]);
                    }
                    else if (type == typeof(TypeReferenceIncludesFrom<>).GetGenericTypeDefinition())
                    {
                        assembly = Assembly.GetAssembly(foundObj.GetType().GetGenericArguments()[0]);
                    }
                }

                List<Type> assemblyTypes = assembly.GetTypes().ToList();

                if (typeOptionsAttribute != null)
                {
                    if (typeOptionsAttribute.InheritedTypes.Count > 0)
                        assemblyTypes.RemoveAll(x => !IsInheritedFrom(typeOptionsAttribute.InheritedTypes, x));

                    if (typeOptionsAttribute.IncludeTypes.Count > 0)
                        assemblyTypes.RemoveAll(x => !IsContainedInIncluded(typeOptionsAttribute.IncludeTypes, x));

                    if (typeOptionsAttribute.RestrictTypes.Count > 0)
                        assemblyTypes.RemoveAll(x => !typeOptionsAttribute.RestrictTypes.Contains(x));
                }

                if (GeneralUtilities.GetTargetObjectOfProperty(property).GetType().IsGenericType)
                {
                    object foundObj = GeneralUtilities.GetTargetObjectOfProperty(property);
                    System.Type type = foundObj.GetType().GetGenericTypeDefinition();

                    if (type == typeof(TypeReferenceInheritedFrom<>).GetGenericTypeDefinition())
                    {
                        assemblyTypes.RemoveAll(x => !IsInheritedFrom(foundObj.GetType().GetGenericArguments().ToList(), x));
                    }
                    else if (type == typeof(TypeReferenceIncludesFrom<>).GetGenericTypeDefinition())
                    {
                        assemblyTypes.RemoveAll(x => !IsContainedInIncluded(foundObj.GetType().GetGenericArguments().ToList(), x));
                    }
                }

                _foundTypes = assemblyTypes.ToArray();
            }


            if (_foundTypeNames.Length <= 0)
            {
                if ((typeOptionsAttribute?.ShouldSort).GetValueOrDefault())
                    Array.Sort(_foundTypes, (x, y) => String.Compare(x.Name, y.Name));

                _foundTypeNames = _foundTypes.Select(x => new String(x.Name)).ToArray();
            }

            index = Array.IndexOf(_foundTypeNames, typeReference.Type?.Name);

            //EditorGUI.BeginProperty(position, label, property);
            selectedIndex = EditorGUI.Popup(position, label.text, index, _foundTypeNames);
            //EditorGUI.EndProperty();

            if (selectedIndex >= 0 && index != selectedIndex)
            {
                typeReference.SetType(_foundTypes[selectedIndex]);
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.UpdateIfRequiredOrScript();

                EditorUtility.SetDirty(property.serializedObject.targetObject);

                if (property.serializedObject.targetObject is ScriptableObject scriptableObject)
                {
                    AssetDatabase.SaveAssetIfDirty(scriptableObject);
                }
            }
        }

        public bool IsInheritedFrom(List<Type> inheritedTypes, Type check)
        {
            if (inheritedTypes.Count <= 0)
                return true;

            if (check.IsAbstract)
                return false;

            for (int i = 0; i < inheritedTypes.Count; i++)
            {
                if (check.IsSubclassOf(inheritedTypes[i]))
                    return true;
            }
            return false;
        }
        public bool IsContainedInIncluded(List<Type> includes, Type check)
        {
            if (check.IsAbstract)
                return false;

            for (int i = 0; i < includes.Count; i++)
            {
                if (includes[i].IsAssignableFrom(check))
                    return true;
            }

            return false;
        }

    }

}
