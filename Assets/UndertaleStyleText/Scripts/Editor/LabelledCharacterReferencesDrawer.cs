using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomPropertyDrawer(typeof(LabelledCharacterReferences))]
    public class LabelledCharacterReferencesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty characterName = property.FindPropertyRelative(nameof(characterName));
            SerializedProperty references = property.FindPropertyRelative(nameof(references));

            // Layout the character name as a prefix and the references as the position
            position = EditorGUI.PrefixLabel(position, new GUIContent(characterName.stringValue));
            EditorGUI.PropertyField(position, references, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
