using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UndertaleStyleText.Editor
{
    [CustomPropertyDrawer(typeof(CharacterExpression))]
    public class CharacterExpressionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Utility.CharacterExpressionPropertyField(position, property, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty characterIndex = property.FindPropertyRelative("characterIndex");
            SerializedProperty visualType = property.FindPropertyRelative("visualType");
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Get the data for the character with this name
            CharacterSettings characterSettings = Settings.GetCharacter(characterIndex.intValue);

            // Check if character is expanded and has some character settings
            if (characterIndex.isExpanded && characterSettings != null)
            {
                // If the visual type is none, only four total controls exist
                if (visualType.enumValueIndex == 0) height *= 3;
                // If the visual type is not none then 5 controls exist
                else height *= 4;
            }

            return height;
        }
    }
}
