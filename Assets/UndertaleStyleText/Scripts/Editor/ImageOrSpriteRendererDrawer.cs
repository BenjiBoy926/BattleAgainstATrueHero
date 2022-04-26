using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomPropertyDrawer(typeof(ImageOrSpriteRenderer))]
    public class ImageOrSpriteRendererDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty type = property.FindPropertyRelative(nameof(type));
            SerializedProperty image = property.FindPropertyRelative(nameof(image));
            SerializedProperty spriteRenderer = property.FindPropertyRelative(nameof(spriteRenderer));

            // Set the height for just one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            // Edit the property type
            EditorGUI.PropertyField(position, type, label);
            position.y += position.height;

            // Increase indent level for next properties
            EditorGUI.indentLevel++;

            if (type.enumValueIndex == 0)
            {
                EditorGUI.PropertyField(position, image);
            }
            else EditorGUI.PropertyField(position, spriteRenderer);

            // Restore old indent level
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2f * (EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight);
        }
    }
}
