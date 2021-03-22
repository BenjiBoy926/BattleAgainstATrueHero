using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MusicSignature))]
public class MusicSignatureDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect foldoutPos = new Rect(position.position, new Vector2(position.width, LayoutUtilities.standardControlHeight));
        property.isExpanded = EditorGUI.Foldout(foldoutPos, property.isExpanded, label);
        position.y += foldoutPos.height;

        // If property is expanded, then edit the sub properties
        if(property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty startingSignature = property.FindPropertyRelative("_startingSignature");
            SerializedProperty subsequentSignatures = property.FindPropertyRelative("subsequentSignatures");

            // Build a vertical layout where each height is the height of the subproperties
            Layout.Builder builder = new Layout.Builder();
            builder.Orientation(LayoutOrientation.Vertical)
                .PushChild(LayoutChild.Height(LayoutSize.Exact(EditorGUI.GetPropertyHeight(startingSignature)), LayoutMargin.Bottom()))
                .PushChild(LayoutChild.Height(LayoutSize.Exact(EditorGUI.GetPropertyHeight(subsequentSignatures)), LayoutMargin.Bottom()));
            Layout layout = builder.Compile(position);

            // Edit the starting signature
            EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("_startingSignature"), true);
            // Edit the subsequent signatures
            EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("subsequentSignatures"), true);

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = LayoutUtilities.standardControlHeight;

        if(property.isExpanded)
        {
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_startingSignature"), true);
            height += EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("subsequentSignatures"), true);
            height += EditorGUIUtility.standardVerticalSpacing;
        }

        return height;
    }
}
