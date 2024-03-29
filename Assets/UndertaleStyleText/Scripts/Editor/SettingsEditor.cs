using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Get all the properties to edit
            SerializedProperty characterRoster = serializedObject.FindProperty(nameof(characterRoster));
            SerializedProperty textDelays = serializedObject.FindProperty(nameof(textDelays));
            SerializedProperty readTime = serializedObject.FindProperty(nameof(readTime));
            SerializedProperty advanceButtonName = serializedObject.FindProperty(nameof(advanceButtonName));

            // Edit all of the properties
            EditorGUILayout.PropertyField(characterRoster);
            LayoutUtility.ArrayOnEnumField<TextDelayLevel>(textDelays);
            EditorGUILayout.PropertyField(readTime);
            EditorGUILayout.PropertyField(advanceButtonName);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
