using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TimeSignatureLocation))]
public class TimeSignatureLocationDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Display the prefix label
        Rect content = EditorGUI.PrefixLabel(position, label);

        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Build an area for the time signature and the "at" measure int editor at the end
        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.7f)))
            .PushChild(new LayoutChild(LayoutSize.Exact(85f)))
            .PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.3f)));
        Layout layout = builder.Compile(content);

        // Display the signature property
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_signature"), GUIContent.none);

        // Display the "at measure" label with the designated style
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.richText = true;
        EditorGUI.LabelField(layout.Next(), new GUIContent("<color=white>at measure</color>"), style);

        // Display the measure property
        EditorGUI.BeginChangeCheck();
        SerializedProperty measure = property.FindPropertyRelative("_measure");
        EditorGUI.PropertyField(layout.Next(), measure, GUIContent.none);

        // If the value was changed, make sure it is not lower than 1
        if(EditorGUI.EndChangeCheck())
        {
            measure.intValue = Mathf.Max(measure.intValue, 1);
        }

        EditorGUI.indentLevel = oldIndent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LayoutUtilities.standardControlHeight;
    }
}
