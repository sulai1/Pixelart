using Algo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ImageStats : EditorWindow
{
    private const int MaxWidth = 256;
    [SerializeField]
    Palette palette;
    private int ditherColors;
    private int clusteredColors;
    Texture2D tex;
    private Vector2 scrollPosition;
    private Texture2D newTex;
    private Texture2D ditherTex;
    private int nCluster;
    private Material mat_pixelate;
    private Material mat_palette;
    private int nIter;
    private bool useDither;
    private int dither;
    private int colors;
    private bool showPalette;
    private int height;

    [MenuItem("Window/ImageStats")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ImageStats));
    }

    public static float Hue(Color c)
    {
        Color.RGBToHSV(c, out float h, out float _, out float _);
        return h;
    }
    int tab;
    private void OnGUI()
    {

        EditorGUI.BeginChangeCheck();
        tab = GUILayout.Toolbar(tab, new string[] { "Convert", "Palette" });
        DrawTextures();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        switch (tab)
        {
            case 0:
                EditorGUI.BeginChangeCheck();
                DrawControls();
                if (EditorGUI.EndChangeCheck())
                    Recalculate();
                break;
            case 1:
                EditorGUI.BeginChangeCheck();
                DrawPalette();
                if (EditorGUI.EndChangeCheck())
                    UpdateMaterial();
                break;
            default:
                break;
        }


        if (GUILayout.Button("Create"))
        {
            var go = ScriptableObject.CreateInstance<PaletteTexture>();
            go.palette = palette;
            go.Texture = newTex;
            AssetDatabase.CreateAsset(go, "Assets/" + tex.name + ".asset");
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawControls()
    {
        tex = EditorGUILayout.ObjectField(tex, typeof(Texture2D), true) as Texture2D;
        nCluster = EditorGUILayout.IntSlider("Colors per Channel", nCluster, 8, 32);
        nIter = EditorGUILayout.IntSlider("Number of Iterations", nIter, 1, 32);

        // show dither controlls
        useDither = EditorGUILayout.Foldout(useDither, "Dither");
        if (!useDither)
            return;

        dither = EditorGUILayout.IntSlider("Quantisation Factor", dither, 2, 32);
        // optionally show the dither texture
        if (ditherTex != null)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"Dithered ({ditherColors} Colors)");
            EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(ditherTex.width, Math.Max(MaxWidth, ditherTex.height)), ditherTex, mat_pixelate, ScaleMode.ScaleToFit);
            EditorGUILayout.EndVertical();
        }
    }
    private void OnEnable()
    {
        UpdateMaterial();
    }

    private void DrawPalette()
    {
        showPalette = EditorGUILayout.Foldout(showPalette, "Palette");
        if (!showPalette)
            return;
        EditorGUI.BeginChangeCheck();
        SerializedObject serializedObject = new SerializedObject(this);
        height = EditorGUILayout.IntSlider("Animations", height, 1, MaxWidth);
        if (palette != null)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("palette"));
        palette.Height = height;
    }

    private void UpdateMaterial()
    {

        if (mat_pixelate == null || mat_palette == null)
        {
            mat_pixelate = Resources.Load<Material>("Material/Pixelate");
            mat_palette = Resources.Load<Material>("Material/Palette");
        }
        mat_palette.SetFloat("Time", palette.t);
        mat_palette.SetInt("Height", height - 1);
        mat_palette.SetTexture("_MaskTex", palette.Texture);
    }

    private void DrawTextures()
    {
        EditorGUILayout.BeginHorizontal();
        if (tex == null)
            return;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField($"Original ({colors} Colors)");
        EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(tex.width, Math.Max(MaxWidth, tex.height)), tex, mat_pixelate, ScaleMode.ScaleToFit);
        EditorGUILayout.EndVertical();

        if (newTex == null)
            return;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField($"Clustered ({clusteredColors} Colors)");
        EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(newTex.width, Math.Max(MaxWidth, newTex.height)), newTex, mat_palette, ScaleMode.ScaleToFit);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void Recalculate()
    {

        colors = tex.GetPixels().Distinct().Count();
        var kmeans = new KMeans(Math.Min(nCluster, colors), nIter);
        if (useDither)
        {
            ditherTex = TextureTools.Dither(tex, dither);
            ditherColors = ditherTex.GetPixels().Distinct().Count();
            newTex = kmeans.Cluster(ditherTex);
        }
        else
        {
            ditherTex = null;
            newTex = kmeans.Cluster(tex);
        }
        palette = new Palette(kmeans.Centers.Length) { Colors = kmeans.Centers };

        clusteredColors = palette.NumColors;
        UpdateMaterial();
    }

}
