using UnityEngine;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;

public static class Logger
{
    static StringBuilder builder = new(150);
    static StackTrace stackTrace = new StackTrace();
    static StackFrame stackFrame = new StackFrame();
    static System.Type extractedType;

    public static void LogWithTag(LogCategory category = LogCategory.NonSpecified, string log = "", Object obj = null, bool autoAddTag = true)
    {
        builder.Clear();
        builder.Append($"{category}");
        builder.Append(" : ");
        builder.Append($"{log}");

#if !RELEASE_MODE
        if (obj != null)
            UnityEngine.Debug.Log(builder, obj);
        else
            UnityEngine.Debug.Log(builder);
#endif
    }

    public static void LogWarningWithTag(LogCategory category = LogCategory.NonSpecified, string log = "", Object obj = null, bool autoAddTag = true)
    {
        builder.Clear();
        builder.Append($"{category}");
        builder.Append(" : ");
        builder.Append($"{log}");

#if !RELEASE_MODE
        if (obj != null)
            UnityEngine.Debug.LogWarning(builder, obj);
        else
            UnityEngine.Debug.LogWarning(builder);
#endif
    }

    public static void LogErrorWithTag(LogCategory category = LogCategory.NonSpecified, string log = "", Object obj = null, bool autoAddTag = true)
    {
        builder.Clear();

        LogCategory foundCategory = category;

        if (category == LogCategory.NonSpecified && autoAddTag)
        {
            stackFrame = new StackFrame(1);
            extractedType = stackFrame.GetMethod().DeclaringType;
            foundCategory = GetLogCategory(extractedType);
        }

        builder.AppendFormat("{0} : {1}", foundCategory, log);

        if (obj != null)
            UnityEngine.Debug.LogError(builder, obj);
        else
            UnityEngine.Debug.LogError(builder);
    }

    static Dictionary<System.Type, LogCategory> _typeToLogCategory = new();
    static Dictionary<string, LogCategory> _stringComparisonToCategory = new()
    {

    };

    [RuntimeInitializeOnLoadMethod]
    static void ClearDictionaries()
    {
        _typeToLogCategory.Clear();
    }

    static LogCategory GetLogCategory(System.Type type)
    {
        if (_typeToLogCategory.TryGetValue(type, out LogCategory logCategory))
            return logCategory;

        string typeString = type.ToString();

        foreach (var stringToCategory in _stringComparisonToCategory)
        {
            if (!typeString.Contains(stringToCategory.Key, System.StringComparison.OrdinalIgnoreCase))
                continue;

            _typeToLogCategory.Add(type, stringToCategory.Value);

            return stringToCategory.Value;
        }

        _typeToLogCategory.Add(type, LogCategory.NonSpecified);

        return LogCategory.NonSpecified;
    }
}