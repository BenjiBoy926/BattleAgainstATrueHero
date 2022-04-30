using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UndertaleStyleText.Editor
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueEditor : UnityEditor.Editor
    {
        #region Editor Overrides
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;

            SerializedProperty sayOnAwake = serializedObject.FindProperty(nameof(sayOnAwake));
            SerializedProperty endEvent = serializedObject.FindProperty(nameof(endEvent));
            endEvent = endEvent.GetEndProperty();
            IEnumerable<SerializedProperty> properties = EditorGUIAuto.ToEnd(sayOnAwake, endEvent, false, false);

            foreach (SerializedProperty property in properties)
            {
                if (property.name == "characterReferences")
                    LabelledCharacterReferencesField(property);
                else if (property.name == "paragraphEvents")
                    ParagraphEventsField(property);
                else
                    EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private Methods
        private void LabelledCharacterReferencesField(SerializedProperty property)
        {
            Dialogue dialogue = target as Dialogue;
            DialogueData dialogueData = dialogue.DialogueData;

            // Do not layout the property if there is no dialogue data
            if (dialogueData == null) return;

            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName);
            if (!property.isExpanded) return;

            // Get a list of all the characters that say paragraphs in the data
            EditorGUI.indentLevel++;
            List<string> characterNames = new List<string>();
            foreach (Paragraph paragraph in dialogueData.Paragraphs)
            {
                string characterName = paragraph.Expression.CharacterSettings.CharacterName;
                if (!characterNames.Contains(characterName))
                    characterNames.Add(characterName);
            }
            // Force the array to be the same size as number of characters
            property.arraySize = characterNames.Count;
            for (int i = 0; i < characterNames.Count; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                SerializedProperty characterName = element.FindPropertyRelative(nameof(characterName));
                SerializedProperty references = element.FindPropertyRelative(nameof(references));
                GUIContent label = new GUIContent($"{characterNames[i]} References");

                characterName.stringValue = characterNames[i];
                EditorGUILayout.PropertyField(references, label, true);
            }
            EditorGUI.indentLevel--;
        }
        private void ParagraphEventsField(SerializedProperty property)
        {
            Dialogue dialogue = target as Dialogue;
            DialogueData dialogueData = dialogue.DialogueData;
            if (dialogueData == null) return;

            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName);
            if (!property.isExpanded) return;

            EditorGUI.indentLevel++;
            property.arraySize = dialogueData.Paragraphs.Count;
            for (int i = 0; i < property.arraySize; i++)
            {
                // Setup the text labels
                Paragraph paragraph = dialogueData.Paragraphs[i];
                string character = paragraph.Expression.CharacterSettings.CharacterName;
                string text = paragraph.Text;
                string startText = text.RedactedSubstring(0, 20);
                
                // Get the current element
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, $"{character}: '{startText}' Events");
                if (!element.isExpanded) continue;

                // Layout the start property and end property
                EditorGUI.indentLevel++;
                startText = $"[START] '{startText}'";
                string endText = text.RedactedSubstring(text.Length - 20, 20);
                endText = $"'{endText}' [END]";
                element.Next(true);
                EditorGUILayout.PropertyField(element, new GUIContent(startText), true);
                element.Next(false);
                EditorGUILayout.PropertyField(element, new GUIContent(endText), true);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        #endregion
    }
}
