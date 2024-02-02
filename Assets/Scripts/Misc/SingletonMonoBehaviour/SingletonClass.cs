using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonClass<T> where T : class, new()
{
    public static T Instance
    {
        get
        {
            //Debug.LogError("get _instance : " + _instance);
            if (_instance == null)
            {
                _instance = new T();
            }

            if (Application.isEditor && !Application.isPlaying)
            {
                if (_editorInstance == null)
                    _editorInstance = _instance;
                //Debug.LogError("get _editorInstance : " + _editorInstance);
                return _editorInstance;
            }

            return _instance;
        }
    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
    [Sirenix.OdinInspector.ReadOnly]
#endif
    protected static T _instance;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
    [Sirenix.OdinInspector.ReadOnly]
#endif
    protected static T _editorInstance;

}

public abstract class SingletonEditorClass<T> where T : class, new()
{
    public static T Instance
    {
        get
        {
            //Debug.LogError("get _instance : " + _instance);
            if (_editorInstance == null)
            {
                _editorInstance = new T();
            }

            return _editorInstance;
        }
    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
    [Sirenix.OdinInspector.ReadOnly]
#endif
    protected static T _editorInstance;

}
