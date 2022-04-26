using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    //[CustomPropertyDrawer(typeof(Paragraph))]
    public class ParagraphDrawer : PropertyDrawer
    {
        private const int maxCharacters = 20;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty expression = property.FindPropertyRelative(nameof(expression));
            SerializedProperty reader = property.FindPropertyRelative(nameof(reader));
            SerializedProperty delayTime = property.FindPropertyRelative(nameof(delayTime));
            SerializedProperty paragraph = property.FindPropertyRelative(nameof(paragraph));

            // Set the height for just one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;



            base.OnGUI(position, property, label);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
