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

            // Display bool value
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
        float height = LayoutUtilities.standardControlHeight;
        SerializedProperty autoAdvance = property.FindPropertyRelative("_autoAdvance");

        if(property.isExpanded)
        {
            // If auto advance is true, give only height for one checkbox
            if (autoAdvance.boolValue)
            {
                height += LayoutUtilities.standardControlHeight * 2f;
            }
            // If audo advance is false, give height for advance button, indicator, and read time
            else
            {
                height += LayoutUtilities.standardControlHeight * 3f;
            }
        }

        return height;
    }
}
