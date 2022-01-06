using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Algo
{
    public class TextureTools
    {
        private const float factor = 255;

        public static Texture2D Quantize(Texture2D tex, int factor, bool apply = true)
        {
            var newTex = new Texture2D(tex.width, tex.height);
            newTex.hideFlags = newTex.hideFlags | HideFlags.DontSaveInEditor;
            for (int x = 1; x < tex.width - 1; x++)
                for (int y = 0; y < tex.height - 1; y++)
                {
                    var oldPixel = tex.GetPixel(x, y);
                    var newPixel = new Color(
                        Mathf.RoundToInt(oldPixel.r * factor) / (float)factor,
                        Mathf.RoundToInt(oldPixel.g * factor) / (float)factor,
                        Mathf.RoundToInt(oldPixel.b * factor) / (float)factor,
                        Mathf.RoundToInt(oldPixel.a * factor) / (float)factor);
                    newTex.SetPixel(x, y, newPixel);
                }
            if (apply) newTex.Apply();
            return newTex;
        }
        public static Texture2D Dither(Texture2D tex, int factor, bool apply = true)
        {
            var newTex = new Texture2D(tex.width, tex.height, tex.format, tex.mipmapCount > 0);
            newTex.hideFlags = newTex.hideFlags | HideFlags.DontSaveInEditor;
            newTex.SetPixels32(tex.GetPixels32());
            for (int x = 1; x < tex.width; x++)
                for (int y = 0; y < tex.height - 1; y++)
                {
                    var oldPixel = newTex.GetPixel(x, y);
                    var newPixel = new Color(
                        Mathf.RoundToInt(oldPixel.r * factor) / (float)factor,
                        Mathf.RoundToInt(oldPixel.g * factor) / (float)factor,
                        Mathf.RoundToInt(oldPixel.b * factor) / (float)factor,
                        Mathf.RoundToInt(oldPixel.a * factor) / (float)factor);
                    newTex.SetPixel(x, y, newPixel);
                    float errR = (oldPixel.r - newPixel.r);
                    float errG = (oldPixel.g - newPixel.g);
                    float errB = (oldPixel.b - newPixel.b);
                    float errA = (oldPixel.a - newPixel.a);

                    Color pixel = newTex.GetPixel(x + 1, y);
                    float r = pixel.r + errR * 7 / 16;
                    float g = pixel.g + errG * 7 / 16;
                    float b = pixel.b + errB * 7 / 16;
                    float a = pixel.a + errA * 7 / 16;
                    newTex.SetPixel(x + 1, y, new Color(r, g, b, a));

                    pixel = newTex.GetPixel(x - 1, y + 1);
                    r = pixel.r + errR * 3 / 16;
                    g = pixel.g + errG * 3 / 16;
                    b = pixel.b + errB * 3 / 16;
                    a = pixel.a + errA * 3 / 16;
                    newTex.SetPixel(x - 1, y + 1, new Color(r, g, b, a));

                    pixel = newTex.GetPixel(x, y + 1);
                    r = pixel.r + errR * 5 / 16;
                    g = pixel.g + errG * 5 / 16;
                    b = pixel.b + errB * 5 / 16;
                    a = pixel.a + errA * 5 / 16;
                    newTex.SetPixel(x, y + 1, new Color(r, g, b, a));

                    pixel = newTex.GetPixel(x + 1, y + 1);
                    r = pixel.r + errR * 1 / 16;
                    g = pixel.g + errG * 1 / 16;
                    b = pixel.b + errB * 1 / 16;
                    a = pixel.a + errA * 1 / 16;
                    newTex.SetPixel(x + 1, y + 1, new Color(r, g, b, a));
                }

            QuantizeRow(tex, factor, newTex);
            if (apply) newTex.Apply();
            return newTex;
        }

        private static void QuantizeRow(Texture2D tex, int factor, Texture2D newTex)
        {
            for (int y = 0; y < tex.height; y++)
            {
                var oldPixel = newTex.GetPixel(0, y);
                var newPixel = new Color(
                    Mathf.RoundToInt(oldPixel.r * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.g * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.b * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.a * factor) / (float)factor);
                newTex.SetPixel(0, y, newPixel);

                oldPixel = newTex.GetPixel(tex.width - 1, y);
                newPixel = new Color(
                    Mathf.RoundToInt(oldPixel.r * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.g * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.b * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.a * factor) / (float)factor);
                newTex.SetPixel(tex.width - 1, y, newPixel);
            }
            for (int x = 0; x < tex.height; x++)
            {
                var oldPixel = newTex.GetPixel(x, 0);
                var newPixel = new Color(
                    Mathf.RoundToInt(oldPixel.r * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.g * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.b * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.a * factor) / (float)factor);
                newTex.SetPixel(x, 0, newPixel);

                oldPixel = newTex.GetPixel(x, tex.height - 1);
                newPixel = new Color(
                    Mathf.RoundToInt(oldPixel.r * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.g * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.b * factor) / (float)factor,
                    Mathf.RoundToInt(oldPixel.a * factor) / (float)factor);
                newTex.SetPixel(x, tex.height - 1, newPixel);
            }
        }
    }
}
