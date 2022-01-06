using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

[CustomPropertyDrawer(typeof(Palette))]
public class PaletteDrawer : PropertyDrawer
{
    private const string RelativePropertyPath = "colors";
    private const float lineHeight = 20;
    private int height;
    private int width;
    public float time;
    int offset = 0;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var prop = property.FindPropertyRelative(RelativePropertyPath);
        this.height = property.FindPropertyRelative("height").intValue;

        if (prop == null || !prop.isArray)
            return lineHeight;

        var lines = Mathf.CeilToInt(width / 8) + 2;
        offset = this.height > 1 ? 1 : 0;
        lines += offset;
        var height = lineHeight * lines;
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, lineHeight), "Palette");
        // get the colors
        this.width = property.FindPropertyRelative("width").intValue;
        this.height = property.FindPropertyRelative("height").intValue;
        var p = property.FindPropertyRelative(RelativePropertyPath);
        if (p.isArray)
        {
            if (this.height > 1)
                time = EditorGUI.Slider(new Rect(position.x, position.y + lineHeight, position.width, lineHeight), "Time", time, 0, height - 1);
            else
                time = 0;
            DrawColors(position, p.Copy(), time);
            // Sync the object
            property.serializedObject.ApplyModifiedProperties();
        }
        property.FindPropertyRelative("t").floatValue = time;
        property.serializedObject.ApplyModifiedProperties();
    }

    private void DrawColors(Rect position, SerializedProperty property, float t)
    {

        var arr = property.Copy();
        property.Next(true); // skip generic field
        property.Next(true); // advance to array size field
        int len = this.width;
        int h = Mathf.RoundToInt(t) * this.width;
        property.Next(true); // advance to first element
        int height = len / 8;
        int width = 8;
        // iterate over all the full rows
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var index = x + y * width;
                DrawField(y, x, index);
            }
        }
        // iterate over last row
        if (len % 8 != 0)
        {
            for (int x = 0; x < len % 8; x++)
            {
                var index = x + height * width;
                DrawField(height, x, index);
            }
        }

        void DrawField(int y, int x, int index)
        {
            Rect r = new Rect(position.x + x * position.width / 8, position.y + (y + 1 + offset) * lineHeight, position.width / 8, lineHeight);
            SerializedProperty element = arr.GetArrayElementAtIndex(h + index);
            var c = EditorGUI.ColorField(r, element.colorValue);
            c.a = 1;
            element.colorValue = c;
        }
    }
}