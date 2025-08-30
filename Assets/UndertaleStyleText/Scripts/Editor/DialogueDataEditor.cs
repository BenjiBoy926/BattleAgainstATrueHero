using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomEditor(typeof(DialogueData))]
    public class DialogueDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty defaultExpression = serializedObject.FindProperty(nameof(defaultExpression));
            SerializedProperty defaultReader = serializedObject.FindProperty(nameof(defaultReader));
            SerializedProperty paragraphs = serializedObject.FindProperty(nameof(paragraphs));

            // Layout the default expression and reader
            EditorGUILayout.PropertyField(defaultExpression);
            EditorGUILayout.PropertyField(defaultReader);

            // Layout the property field for the paragraphs list and check if new elements were added to the end
            int oldArraySize = paragraphs.arraySize;
            EditorGUILayout.PropertyField(paragraphs);
            if(oldArraySize < paragraphs.arraySize)
            {
                // Loop through all new elements and set values to defaults
                for (int i = oldArraySize; i < paragraphs.arraySize; i++)
                {
                    SerializedProperty p = paragraphs.GetArrayElementAtIndex(i);
                    SerializedProperty expression = p.FindPropertyRelative(nameof(expression));
                    SerializedProperty reader = p.FindPropertyRelative(nameof(reader));

                    // Set the expression and reader on the new paragraph to the default values
                    Utility.SetCharacterExpressionValues(defaultExpression, expression);
                    Utility.SetReaderSettingsValues(defaultReader, reader);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
