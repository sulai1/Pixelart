using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Struct;

[Serializable]
public class Palette
{
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    public float t;
    public IEnumerable<Color> Colors
    {
        get { foreach (var c in colors) yield return c; }
        set
        {
            var count = value.Count();
            height = Mathf.CeilToInt(count / (float)width);
            colors = value.ToArray();
        }
    }

    public Palette(int width, int height = 1)
    {
        this.width = width;
        this.height = height;
    }
    public Texture2D Texture
    {
        get
        {
            var paletteTexture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                hideFlags = HideFlags.DontSaveInEditor
            };
            paletteTexture.SetPixels(colors, 0);
            paletteTexture.Apply();
            return paletteTexture;
        }
    }


    public int Height
    {
        get => height;
        set
        {
            if (value == height)
                return;
            var colors = new Color[width*value];
            for(int i=0;i<colors.Length;i++)
                colors[i] = this.colors[i%this.colors.Length];
            this.colors = colors;
            height = value;
        }
    }
    public int Width { get => width; }
    public int NumColors { get => colors.Length; }
}