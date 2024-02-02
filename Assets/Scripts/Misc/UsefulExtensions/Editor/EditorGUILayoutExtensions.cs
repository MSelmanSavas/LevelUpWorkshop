
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;

namespace UsefulExtensions.EditorGUILayout
{
    public static class EditorGUILayoutExtensions
    {
        public static void HorizontalSeperator()
        {
            UnityEditor.EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public static void HorizontalSeperator(Rect pos)
        {
            UnityEditor.EditorGUI.LabelField(pos, "", GUI.skin.horizontalSlider);
        }
    }
}

#endif