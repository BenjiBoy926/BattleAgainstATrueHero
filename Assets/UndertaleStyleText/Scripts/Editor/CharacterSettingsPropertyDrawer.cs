using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomPropertyDrawer(typeof(CharacterSettings))]
    public class CharacterSettingsPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty characterIndex = property.FindPropertyRelative("characterName");
            SerializedProperty fonts = property.FindPropertyRelative("fonts");
            SerializedProperty voiceClips = property.FindPropertyRelative("voiceClips");
            SerializedProperty animationTriggers = property.FindPropertyRelative("animationTriggers");
            SerializedProperty faces = property.FindPropertyRelative("faces");
            SerializedProperty poses = property.FindPropertyRelative("poses");

            // Set the height of the position for just one control
            position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Edit character name
            EditorGUI.PropertyField(position, characterIndex);
            // Put a foldout in the same position
            characterIndex.isExpanded = EditorGUI.Foldout(position, characterIndex.isExpanded, GUIContent.none);
            position.y += position.height;

            if (characterIndex.isExpanded)
            {
                EditorGUI.indentLevel++;

                // Edit voice clips
                EditorGUI.PropertyField(position, fonts, true);
                position.y += EditorGUI.GetPropertyHeight(fonts);
                // Edit voice clips
                EditorGUI.PropertyField(position, voiceClips, true);
                position.y += EditorGUI.GetPropertyHeight(voiceClips);
                // Edit animation triggers
                EditorGUI.PropertyField(position, animationTriggers, true);
                position.y += EditorGUI.GetPropertyHeight(animationTriggers);
                // Edit faces
                EditorGUI.PropertyField(position, faces, true);
                position.y += EditorGUI.GetPropertyHeight(faces);
                // Edit poses
                EditorGUI.PropertyField(position, poses, true);
                position.y += EditorGUI.GetPropertyHeight(poses);

                EditorGUI.indentLevel--;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty characterName = property.FindPropertyRelative("characterName");
            SerializedProperty fonts = property.FindPropertyRelative("fonts");
            SerializedProperty voiceClips = property.FindPropertyRelative("voiceClips");
            SerializedProperty animationTriggers = property.FindPropertyRelative("animationTriggers");
            SerializedProperty faces = property.FindPropertyRelative("faces");
            SerializedProperty poses = property.FindPropertyRelative("poses");

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (characterName.isExpanded)
            {
                height += EditorGUI.GetPropertyHeight(fonts);
                height += EditorGUI.GetPropertyHeight(voiceClips);
                height += EditorGUI.GetPropertyHeight(animationTriggers);
                height += EditorGUI.GetPropertyHeight(faces);
                height += EditorGUI.GetPropertyHeight(poses);
            }

            return height;
        }
    }
}
