using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    public static class Utility
    {
        public static int CharacterIndexPopup(Rect position, int index, bool readOnly)
        {
            // If character name cannot be edited then use a label
            if (readOnly)
            {
                position = EditorGUI.PrefixLabel(position, new GUIContent("Character Name"));

                // Put index to zero for proper layout
                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                // If the index is still below zero, we know that no character name has been selected
                if (index < 0)
                {
                    EditorGUI.LabelField(position, "Nothing selected");
                } 
                else // If the index is valid then display the character's name as a label
                {
                    EditorGUI.LabelField(position, Settings.CharacterNames[index]);
                }

                EditorGUI.indentLevel = oldIndent;
            }
            else // If character name can be edited then use a popup
            {
                index = Popup(position, index, Settings.CharacterNames, new GUIContent("Character Name"));
            }

            return index;
        }

        public static int Popup(Rect position, int index, string[] values, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            // Wipe indent so that rects are property placed
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if(values.Length > 0)
            {
                // Make sure index is not an invalid value
                index = Mathf.Clamp(index, 0, values.Length);
                index = EditorGUI.Popup(position, index, values);
            }
            else
            {
                EditorGUI.LabelField(position, "Nothing to select");
                index = -1;
            }

            // Restore the old indent
            EditorGUI.indentLevel = oldIndent;

            return index;
        }

        public static void CharacterExpressionPropertyField(Rect position, SerializedProperty property, bool characterNameReadOnly)
        {
            SerializedProperty characterIndex = property.FindPropertyRelative("characterIndex");
            SerializedProperty fontIndex = property.FindPropertyRelative("fontIndex");
            SerializedProperty voiceClipIndex = property.FindPropertyRelative("voiceClipIndex");
            SerializedProperty visualType = property.FindPropertyRelative("visualType");
            SerializedProperty visualElementIndex = property.FindPropertyRelative("visualElementIndex");

            // Set the height to the height for only one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            characterIndex.intValue = CharacterIndexPopup(position, characterIndex.intValue, characterNameReadOnly);
            // Layout a foldout
            characterIndex.isExpanded = EditorGUI.Foldout(position, characterIndex.isExpanded, GUIContent.none);
            position.y += position.height;

            // Get the data for the character at this index
            CharacterSettings characterSettings = Settings.GetCharacter(characterIndex.intValue);

            if (characterIndex.isExpanded && characterSettings != null)
            {
                EditorGUI.indentLevel++;

                // Layout the font
                fontIndex.intValue = Popup(position, fontIndex.intValue, characterSettings.FontNames, new GUIContent("Font"));
                position.y += position.height;
                // Layout the voice clip
                voiceClipIndex.intValue = Popup(position, voiceClipIndex.intValue, characterSettings.VoiceClipNames, new GUIContent("Voice Clip"));
                position.y += position.height;

                // Switch the editing based off of 
                switch(visualType.enumValueIndex)
                {
                    case (int)CharacterExpression.VisualExpressionType.None: break;
                    case (int)CharacterExpression.VisualExpressionType.Animation:
                        visualElementIndex.intValue = Popup(position, visualElementIndex.intValue, characterSettings.AnimationTriggers, new GUIContent("Animation Trigger"));
                        break;
                    case (int)CharacterExpression.VisualExpressionType.Face:
                        visualElementIndex.intValue = Popup(position, visualElementIndex.intValue, characterSettings.FaceNames, new GUIContent("Face"));
                        break;
                    case (int)CharacterExpression.VisualExpressionType.Pose:
                        visualElementIndex.intValue = Popup(position, visualElementIndex.intValue, characterSettings.PoseNames, new GUIContent("Pose"));
                        break;
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}

