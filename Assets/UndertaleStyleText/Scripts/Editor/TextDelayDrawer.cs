using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomPropertyDrawer(typeof(TextDelay))]
    public class TextDelayDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty enumValueIndex = property.FindPropertyRelative(nameof(enumValueIndex));
            SerializedProperty customDelay = property.FindPropertyRelative(nameof(customDelay));

            // Set the height for just one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Get the enum names
            string[] enumNames = System.Enum.GetNames(typeof(TextDelayLevel));
            // Setup the list of options
            string[] options = new string[enumNames.Length + 1];

            // Copy the enums into the list of options
            for(int i = 0; i < enumNames.Length; i++)
            {
                options[i] = enumNames[i];
            }
            // Add the special "Custom" option at the end
            options[enumNames.Length] = "Custom";

            // Edit the enum value index as a popup
            enumValueIndex.intValue = Utility.Popup(position, enumValueIndex.intValue, options, label);
            position.y += position.height;

            if(enumValueIndex.intValue == enumNames.Length)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(position, customDelay);
                EditorGUI.indentLevel--;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty enumValueIndex = property.FindPropertyRelative(nameof(enumValueIndex));
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            // Get the enum names
            string[] enumNames = System.Enum.GetNames(typeof(TextDelayLevel));

            // If the last option is selected we need room for the custom delay
            if (enumValueIndex.intValue == enumNames.Length)
            {
                height *= 2;
            }

            return height;
        }
    }
}
