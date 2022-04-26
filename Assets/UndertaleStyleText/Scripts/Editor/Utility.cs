using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    public static class Utility
    {
        public static void SetCharacterExpressionValues(SerializedProperty src, SerializedProperty dest)
        {
            string characterIndex = nameof(characterIndex);
            string fontIndex = nameof(fontIndex);
            string voiceClipIndex = nameof(voiceClipIndex);
            string visualType = nameof(visualType);
            string visualElementIndex = nameof(visualElementIndex);

            // Copy the values from source into destination
            dest.FindPropertyRelative(characterIndex).intValue = src.FindPropertyRelative(characterIndex).intValue;
            dest.FindPropertyRelative(fontIndex).intValue = src.FindPropertyRelative(fontIndex).intValue;
            dest.FindPropertyRelative(voiceClipIndex).intValue = src.FindPropertyRelative(voiceClipIndex).intValue;
            dest.FindPropertyRelative(visualType).enumValueIndex = src.FindPropertyRelative(visualType).enumValueIndex;
            dest.FindPropertyRelative(visualElementIndex).intValue = src.FindPropertyRelative(visualElementIndex).intValue;
        }

        public static void SetReaderSettingsValues(SerializedProperty src, SerializedProperty dest)
        {
            string useCurrentTimescale = nameof(useCurrentTimescale);
            string autoAdvance = nameof(autoAdvance);

            // Copy the values from source into destination
            dest.FindPropertyRelative("delay.enumValueIndex").intValue = src.FindPropertyRelative("delay.enumValueIndex").intValue;
            dest.FindPropertyRelative("delay.customDelay").floatValue = src.FindPropertyRelative("delay.customDelay").floatValue;
            dest.FindPropertyRelative(useCurrentTimescale).boolValue = src.FindPropertyRelative(useCurrentTimescale).boolValue;
            dest.FindPropertyRelative(autoAdvance).boolValue = src.FindPropertyRelative(autoAdvance).boolValue;
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

        public static void CharacterExpressionPropertyField(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty characterIndex = property.FindPropertyRelative("characterIndex");
            SerializedProperty fontIndex = property.FindPropertyRelative("fontIndex");
            SerializedProperty voiceClipIndex = property.FindPropertyRelative("voiceClipIndex");
            SerializedProperty visualType = property.FindPropertyRelative("visualType");
            SerializedProperty visualElementIndex = property.FindPropertyRelative("visualElementIndex");

            // Set the height to the height for only one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            characterIndex.intValue = Popup(position, characterIndex.intValue, Settings.CharacterNames, label);
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
                // Layout the visual type property
                EditorGUI.PropertyField(position, visualType);
                position.y += position.height;

                // Increase indent for fields dependent on previous
                EditorGUI.indentLevel++;

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

                // Restore original indent
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }
    }
}

