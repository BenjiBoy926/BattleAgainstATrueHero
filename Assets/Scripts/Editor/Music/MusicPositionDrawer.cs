using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MusicPosition))]
public class MusicPositionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = LayoutUtilities.standardControlHeight;

        // Give the label on the top line
        EditorGUI.LabelField(position, label);

        // Move the position down
        position.y += position.height;

        // Increase indent for the other fields
        EditorGUI.indentLevel++;

        // Compile the layout
        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(LayoutChild.Width(LayoutSize.Exact(50f)))
            .PushChild(LayoutChild.Width(LayoutSize.RatioOfRemainder(0.33f), LayoutMargin.Right(10f)))
            .PushChild(LayoutChild.Width(LayoutSize.Exact(60f)))
            .PushChild(LayoutChild.Width(LayoutSize.RatioOfRemainder(0.33f), LayoutMargin.Right(10f)))
            .PushChild(LayoutChild.Width(LayoutSize.Exact(35f)))
            .PushChild(LayoutChild.Width(LayoutSize.RatioOfRemainder(0.33f)));
        Layout layout = builder.Compile(EditorGUI.IndentedRect(position));

        // Decrease indent back to what it was
        EditorGUI.indentLevel--;

        // Save the old indent
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Edit the phrase field
        EditorGUI.LabelField(layout.Next(), "Phrase:");
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_phrase"), GUIContent.none);

        // Edit the measure field
        EditorGUI.LabelField(layout.Next(), "Measure:");
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_measure"), GUIContent.none);

        // Edit the beat field
        EditorGUI.LabelField(layout.Next(), "Beat:");
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_beat"), GUIContent.none);

        EditorGUI.indentLevel = oldIndent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LayoutUtilities.standardControlHeight * 2f;
    }
}
