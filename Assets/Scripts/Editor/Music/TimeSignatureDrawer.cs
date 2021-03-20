using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TimeSignature))]
public class TimeSignatureDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect content = EditorGUI.PrefixLabel(position, label);

        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Build a layout where the center slash has an exact size of 10f
        // and the two properties fill up the remaining space
        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)))
            .PushChild(new LayoutChild(LayoutSize.Exact(10f)))
            .PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)));
        Layout layout = builder.Compile(content);

        // Check for changes in the property values
        EditorGUI.BeginChangeCheck();

        // Display beats per measure and beats per quarter note on one line with a slash between
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_beatsPerMeasure"), GUIContent.none);

        // Display middle slash with bold middle-center style
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.richText = true;
        EditorGUI.LabelField(layout.Next(), new GUIContent("<color=white>/</color>"), style);

        // Display beats per quarter note editor
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_beatsPerWholeNote"), GUIContent.none);

        EditorGUI.indentLevel = oldIndent;

        // If properties were changed, validate the new values
        if(EditorGUI.EndChangeCheck())
        {
            ValidateSubProperties(property);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LayoutUtilities.standardControlHeight;
    }

    private void ValidateSubProperties(SerializedProperty property)
    {
        SerializedProperty subProperty = property.FindPropertyRelative("_beatsPerMeasure");
        subProperty.intValue = Mathf.Max(subProperty.intValue, 1);

        subProperty = property.FindPropertyRelative("_beatsPerWholeNote");
        subProperty.intValue = Mathf.Max(subProperty.intValue, 1);
    }
}
