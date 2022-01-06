using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PaletteTexture))]
public class PaletteTexureeDITOR : Editor
{
    private const int HEIGHT = 256;
    private static Material material;
    public override void OnInspectorGUI()
    {
        var tex = target as PaletteTexture;
        if (tex == null)
            return;

        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            UpdateMaterial(tex);
        }
        EditorGUI.DrawPreviewTexture(EditorGUILayout.GetControlRect(false, HEIGHT), tex.Texture, material);
    }

    private static void UpdateMaterial(PaletteTexture tex)
    {

        if (material == null)
            material = Resources.Load<Material>("Material/Palette");
        material.SetTexture("_MaskTex", tex.palette.Texture);
        material.SetFloat("Time", tex.palette.t / (tex.palette.Height - 1));
        material.SetInt("Height", tex.palette.Height - 1);
    }

    private void OnEnable()
    {
        UpdateMaterial(target as PaletteTexture);
    }
}
