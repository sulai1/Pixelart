using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Algo
{
    public class CvTools
    {
        public static Image<Rgba, Byte> Tex2Img(Texture2D texture)
        {
            Texture2D tex; 
            switch (texture.format)
            {
                case TextureFormat.RGBA32:
                    tex = texture;
                    break;
                default:
                    tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
                    tex.hideFlags = tex.hideFlags | HideFlags.DontSaveInEditor;
                    Graphics.ConvertTexture(texture, tex);
                    break;
            }
            Image<Rgba, Byte> img = new Image<Rgba, Byte>(tex.width, tex.height);
            byte[] bytes = tex.GetRawTextureData();
            img.Bytes = bytes;
            return img;
        }

        public static Texture2D Img2Tex(Image<Rgba, Byte> img)
        {
            Texture2D tex = new Texture2D(img.Width, img.Height, TextureFormat.RGBA32, false);
            tex.hideFlags = tex.hideFlags | HideFlags.DontSaveInEditor;
            tex.SetPixelData(img.Bytes, 0);
            return tex;
        }

        public static Texture2D FromList(int[] vector, int width, int height, int maxValue = 256, Texture2D mask = null)
        {
            if (width * height != vector.Length)
                throw new IndexOutOfRangeException($"Input vector size should be width times height");
            var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            tex.hideFlags = tex.hideFlags | HideFlags.DontSaveInEditor;
            for (int i = 0; i < vector.Length; i++)
            {
                float gray = vector[i] / (float)maxValue;
                int x = i % width;
                int y = i / width;
                tex.SetPixel(x, y, new Color(gray, gray, gray, mask == null ? 1 : mask.GetPixel(x, y).a));
            }
            tex.Apply();
            return tex;
        }
    }
}