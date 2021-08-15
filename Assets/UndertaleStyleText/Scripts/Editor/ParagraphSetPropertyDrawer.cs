using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomPropertyDrawer(typeof(ParagraphSet))]
    public class ParagraphSetPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty characterIndex = property.FindPropertyRelative(nameof(characterIndex));
            SerializedProperty visualType = property.FindPropertyRelative(nameof(visualType));
            SerializedProperty paragraphs = property.FindPropertyRelative(nameof(paragraphs));

            Debug.Log("Hello?");

            // Set the height for just one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            // Layout for the character index
            EditorGUI.BeginChangeCheck();
            characterIndex.intValue = Utility.CharacterIndexPopup(position, characterIndex.intValue, false);
            position.y += position.height;

            // If we changed this character index,
            // then we need to change the index on all of the paragraphs
            if(EditorGUI.EndChangeCheck())
            {
                for(int i = 0; i < paragraphs.arraySize; i++)
                {
                    SerializedProperty element = paragraphs.GetArrayElementAtIndex(i);
                    SerializedProperty childCharacterIndex = element.FindPropertyRelative("expression.characterIndex");
                    childCharacterIndex.intValue = characterIndex.intValue;
                }
            }

            // Layout for the visual expression type
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, visualType);
            position.y += position.height;

            // If we changed the visual type,
            // then we need to change the visual type on all of the paragraphs
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < paragraphs.arraySize; i++)
                {
                    SerializedProperty element = paragraphs.GetArrayElementAtIndex(i);
                    SerializedProperty childVisualType = element.FindPropertyRelative("expression.visualType");
                    childVisualType.enumValueIndex = visualType.enumValueIndex;
                }
            }

            // Layout for all paragraphs
            EditorGUI.PropertyField(position, paragraphs);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty paragraphs = property.FindPropertyRelative(nameof(paragraphs));
            float height = 2f * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            height += EditorGUI.GetPropertyHeight(paragraphs);
            return height;
        }
    }
}