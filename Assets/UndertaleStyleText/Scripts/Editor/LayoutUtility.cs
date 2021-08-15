using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    public static class LayoutUtility
    {
        public static void ArrayOnEnumField<EnumType>(SerializedProperty array)
        {
            ArrayOnEnumField<EnumType>(array, new GUIContent(ObjectNames.NicifyVariableName(array.name)));
        }

        public static void ArrayOnEnumField<EnumType>(SerializedProperty array, GUIContent label)
        {
            // Put in a foldout for the items
            array.isExpanded = EditorGUILayout.Foldout(array.isExpanded, label);

            // Increase indent for array elements
            EditorGUI.indentLevel++;

            // If array is expanded then edit the elements
            if (array.isExpanded)
            {
                // Get the names of the enums
                string[] enumNames = System.Enum.GetNames(typeof(EnumType));
                array.arraySize = enumNames.Length;

                // Edit each element
                for (int i = 0; i < enumNames.Length; i++)
                {
                    SerializedProperty element = array.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(element, new GUIContent(enumNames[i]), true);
                }
            }

            // Restore the old indent
            EditorGUI.indentLevel--;
        }
    }
}

