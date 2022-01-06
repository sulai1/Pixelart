using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plot
{
    Texture2D texture;
    const int colorbarHeight = 10;
    public Plot(int width, int height)
    {
        // add additional space x+1 for plotting double sized dots and y+h for drawing the colorbar
        texture = new Texture2D(width + 1, height + colorbarHeight, TextureFormat.RGBA32, false);
        texture.hideFlags = texture.hideFlags | HideFlags.DontSaveInEditor;
    }

    public int Width => texture.width;
    public int Height => texture.height;
    public Texture2D Texture => texture;

    public void Scatter(IEnumerable<Color> points)
    {
        Func<Color,(Vector2,Color)> l = (c) => 
        {
            Color.RGBToHSV(c, out float h, out float s, out float v);
            return (new Vector2(h, s), c);
        };
        Scatter<Color>(points, l);
    }
    public void Scatter<T>(IEnumerable<T> points, Func<T,(Vector2,Color)> select)
    {
        Clear();
        ColorBar();

        // draw the points
        foreach (var p in points)
        {
            (Vector2 v, Color c) = select(p);
            Color.RGBToHSV(c, out float x, out float y, out float _);
            var uv = Transform(v);
            texture.SetPixel(uv.x, uv.y, c);
            texture.SetPixel(uv.x + 1, uv.y, c);
            texture.SetPixel(uv.x, uv.y + 1, c);
            texture.SetPixel(uv.x + 1, uv.y + 1, c);
        }
        texture.Apply();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="points"></param>
    /// <param name="selector">return a value between 0 and one</param>
    /// <param name="bins"></param>
    public void Histogram<T>(IEnumerable<T> points, Func<T, float> selector, int bins)
    {
        Clear();
        var binArray = new int[bins];
        int count = 0;
        foreach (var p in points)
        {
            int bin = Mathf.FloorToInt(selector(p) * bins);
            binArray[bin]++;
            count++;
        }

        int maxh = binArray.Max();
        for (int i = 0; i < binArray.Length; i++)
        {

            int x = i * texture.width / bins;
            int end = (i + 1) * texture.width / bins;
            int w = end - x;
            Box(new RectInt(x, 0, w, texture.height * binArray[i] / maxh));
        }
        texture.Apply();
    }
    public void HeatMap(float[,] values)
    {
        //for (int x = 0; x < r.x + values.he; x++)
        //    for (int y = r.y; y < r.y + r.height; y++)
        //    {
        //        texture.SetPixel(x, y, Color.HSVToRGB(x / (float)texture.width, .5f, .5f));
        //    }
    }
    public void Box(RectInt r)
    {
        for (int x = r.x; x < r.x + r.width; x++)
            for (int y = r.y; y < r.y + r.height; y++)
            {
                texture.SetPixel(x, y, Color.HSVToRGB(x / (float)texture.width, .5f, .5f));
            }
    }

    public void Clear()
    {
        for (int x = 0; x < texture.width; x++)
            for (int y = 0; y < texture.height; y++)
                texture.SetPixel(x, y, Color.black);
    }

    public void ColorBar()
    {
        // draw colorbar
        for (int x = 0; x < texture.width; x++)
            for (int y = texture.height - 1; y > texture.height - colorbarHeight; y--)
                texture.SetPixel(x, y, Color.HSVToRGB(x / (float)texture.width, 1, 1));
    }

    public Vector2Int Transform(Vector2 point)
    {
        return new Vector2Int((int)(point.x * Width), (int)(point.y * Height));
    }
}