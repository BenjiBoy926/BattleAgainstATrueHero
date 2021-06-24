using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MonologueAdvanceSettings))]
public class MonologueAdvanceSettingsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = LayoutUtilities.standardControlHeight;
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

        // Display other properties if property is expanded
        if(property.isExpanded)
        {
            SerializedProperty autoAdvance = property.FindPropertyRelative("_autoAdvance");
 
            // Indent out once
            EditorGUI.indentLevel++;

            // Display advance if paused setting
            position.y += position.height;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_advanceIfPaused"));

            // Display auto advance
            position.y += position.height;
            EditorGUI.PropertyField(position, autoAdvance);

            // If we are automatically advancing, edit the read time
            if (autoAdvance.boolValue)
            {
                position.y += position.height;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("_readTime"));
            }
            // If we are not automatically advancing, then edit the advance button and advance indicator property
            else
            {
                position.y += position.height;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("_advanceButton"));

                position.y += position.height;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("_advanceIndicator"));
            }

            // Move indent level back
            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Start with room for foldout only
        float height = LayoutUtilities.standardControlHeight;
        SerializedProperty autoAdvance = property.FindPropertyRelative("_autoAdvance");

        if(property.isExpanded)
        {
            // Add space for unscaled checkbox and auto advance checkbox
            height += LayoutUtilities.standardControlHeight * 2f;

            // If auto advance is true, give height for read time
            if (autoAdvance.boolValue)
            {
                height += LayoutUtilities.standardControlHeight;
            }
            // If audo advance is false, give height for advance indicator and advance button
            else
            {
                height += LayoutUtilities.standardControlHeight * 2f;
            }
        }

        return height;
    }
}
