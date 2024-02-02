using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SingletonMonoBehaviour;

public class RefBook : SingletonMonoBehaviour<RefBook>
{
    protected override bool dontDestroyOnLoad => true;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
#endif
    Dictionary<System.Type, List<object>> References = new Dictionary<System.Type, List<object>>();

    protected override void AwakeInternal() { }
    protected override void OnValidateInternal() { }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector] 
#endif
    System.Type lastAddedType;
    
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector] 
#endif
    System.Type lastAccessedType;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector] 
#endif
    System.Type lastRemovedType;

    public static bool Add(object obj) => Instance.AddInternal(obj);
    protected virtual bool AddInternal(object obj)
    {
        lastAddedType = obj.GetType();

        if (!References.ContainsKey(lastAddedType))
            References.Add(lastAddedType, new List<object>());

        if (References[lastAddedType].Contains(obj))
        {
            Debug.LogError($"References already contains obj of type : {lastAddedType},{obj}! Can't add...");
            return false;
        }

        References[lastAddedType].Add(obj);
        return true;
    }

    public static bool TryGetIfNull<T>(ref T obj, int accessIndex = 0) where T : class
    {
        if (obj != null)
            return true;

        return Instance.TryGetInternal(out obj, accessIndex);
    }

    public static bool TryGet<T>(out T obj, int accessIndex = 0) where T : class => Instance.TryGetInternal(out obj, accessIndex);

    /// <summary>
    /// Tries to returns a registered Reference of type. Return first of registered type if accessIndex is not specified.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected virtual bool TryGetInternal<T>(out T obj, int accessIndex = 0) where T : class
    {
        lastAccessedType = typeof(T);

        try
        {
            if (References.ContainsKey(lastAccessedType))
                if (References[lastAccessedType].Count > accessIndex)
                {
                    obj = References[lastAccessedType][accessIndex] as T;
                    return true;
                }

            obj = default;
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while trying to get a Reference of type : {lastAccessedType}! Error : {e}");
            obj = default;
            return false;
        }
    }

    public static bool TryGet<T>(out T obj, System.Type type, int accessIndex = 0) where T : class => Instance.TryGetInternal(out obj, type, accessIndex);
    protected virtual bool TryGetInternal<T>(out T obj, System.Type type, int accessIndex = 0) where T : class
    {
        lastAccessedType = type;

        try
        {
            if (References.ContainsKey(type))
                if (References[type].Count > accessIndex)
                {
                    obj = References[type][accessIndex] as T;
                    return true;
                }

            obj = default;
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while trying to get a Reference of type : {lastAccessedType}! Error : {e}");
            obj = default;
            return false;
        }
    }

    public static bool TryGet(out object obj, System.Type type, int accessIndex = 0) => Instance.TryGetInternal(out obj, type, accessIndex);
    protected virtual bool TryGetInternal(out object obj, System.Type type, int accessIndex = 0)
    {
        lastAccessedType = type;

        try
        {
            if (References.ContainsKey(type))
                if (References[type].Count > accessIndex)
                {
                    obj = References[type][accessIndex];
                    return true;
                }

            obj = default;
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while trying to get a Reference of type : {lastAccessedType}! Error : {e}");
            obj = default;
            return false;
        }
    }

    public static bool TryGetAll<T>(List<T> objs) => Instance.TryGetAllInternal(objs);
    public static List<T> TryGetAll<T>()
    {
        List<T> allReferences = new List<T>();
        if (Instance.TryGetAllInternal(allReferences))
            return allReferences;

        return Enumerable.Empty<T>() as List<T>;
    }

    protected virtual bool TryGetAllInternal<T>(List<T> objs)
    {
        lastAccessedType = typeof(T);

        try
        {
            objs.AddRange(References[lastAccessedType].Cast<T>());
            return true;
        }
        catch (System.Exception e)
        {
            //Debug.LogError($"Error while trying to get a Reference of type : {lastAccessedType}! Error : {e}");
            //objs = default;
            return false;
        }
    }

    public static bool TryGetAll(System.Type type, ref List<object> objs) => Instance.TryGetAllInternal(type, ref objs);
    public static List<object> TryGetAll(System.Type type)
    {
        List<object> allReferences = new List<object>();

        if (Instance.TryGetAllInternal(type, ref allReferences))
            return allReferences;

        return Enumerable.Empty<object>() as List<object>;
    }

    protected virtual bool TryGetAllInternal(System.Type type, ref List<object> objs)
    {
        lastAccessedType = type;

        try
        {
            objs = References[lastAccessedType].ToList();
            return true;
        }
        catch (System.Exception e)
        {
            //Debug.LogError($"Error while trying to get a Reference of type : {lastAccessedType}! Error : {e}");
            //objs = default;
            return false;
        }
    }

    public static bool Remove(object obj) => Instance.RemoveInternal(obj);

    protected bool RemoveInternal(object obj)
    {
        lastRemovedType = obj.GetType();

        if (!References.ContainsKey(lastRemovedType))
        {
            Debug.LogWarning($"There is no reference of type : {lastRemovedType}");
            return false;
        }

        if (!References[lastRemovedType].Contains(obj))
        {
            Debug.LogWarning($"There is no obj found in references of type : {lastRemovedType},{obj}");
            return false;
        }

        References[obj.GetType()].Remove(obj);
        return true;
    }

    public static bool RemoveAtIndex(System.Type type, int Index = 0) => Instance.RemoveAtIndexInternal(type, Index);
    protected bool RemoveAtIndexInternal(System.Type type, int Index = 0)
    {
        lastRemovedType = type;

        if (!References.ContainsKey(lastRemovedType))
        {
            Debug.LogError($"There is no reference of type : {lastRemovedType}");
            return false;
        }

        if (References[lastRemovedType].Count <= Index)
        {
            Debug.LogError($"There is no Index : {Index} in References of type :{lastRemovedType}");
            return false;
        }

        References[lastRemovedType].RemoveAt(Index);
        return true;
    }
}
