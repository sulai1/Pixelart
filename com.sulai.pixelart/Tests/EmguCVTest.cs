using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algo;
using Struct;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Emgu.CV.Util;
using Emgu.CV.Structure;

namespace Tests
{
    public class EmguCVTest
    {
        [Test]
        public void ImageConvertTest()
        {
            Texture2D tex = new Texture2D(3, 3);
            tex.hideFlags = tex.hideFlags | HideFlags.DontSaveInEditor;
            tex.SetPixel(0, 0, Color.red);
            tex.SetPixel(1, 0, Color.green);
            tex.SetPixel(2, 0, Color.green);
            tex.SetPixel(0, 1, Color.blue);
            tex.SetPixel(1, 1, Color.yellow);
            tex.SetPixel(2, 1, Color.green);
            tex.SetPixel(0, 2, Color.blue);
            tex.SetPixel(1, 2, Color.yellow);
            tex.SetPixel(2, 2, Color.green);

            var img = CvTools.Tex2Img(tex);
            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                {
                    Color expected = tex.GetPixel(x, y);
                    var actual = img[y, x];
                    Assert.That(expected.r, Is.EqualTo(actual.Red / 255f).Within(.0001f), $"At {x},{y}");
                    Assert.That(expected.g, Is.EqualTo(actual.Green / 255f).Within(.0001f), $"At {x},{y}");
                    Assert.That(expected.b, Is.EqualTo(actual.Blue / 255f).Within(.0001f), $"At {x},{y}");
                    Assert.That(expected.a, Is.EqualTo(actual.Alpha / 255f).Within(.0001f), $"At {x},{y}");

                }

            var newTex = CvTools.Img2Tex(img);
            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Width; y++)
                {
                    Assert.AreEqual(tex.GetPixel(x, y), newTex.GetPixel(x, y), $"At {x},{y}");
                }
        }
        //[Test]
        //public void Vector2ImageTest()
        //{
        //    var v = new int[] { 1, 2, 3, 4 };
        //    var img = CvTools.FromList(v,2,2,4);
        //    for(int i=0;i<v.Length;i++)
        //    {
        //        int expected = v[i];
        //        int y = i / 2;
        //        int x = i % 2;
        //        int actual = Mathf.RoundToInt(img.GetPixel(x, y).a);
        //        Assert.AreEqual(expected,actual,$"At{x},{y}");
        //    }
        //}
        // A Test behaves as an ordinary method
        [Test]
        public void ClusterTest()
        {
            var clustering = new KMeans(2);
            var data = new Texture2D(3, 3);
            data.hideFlags = data.hideFlags | HideFlags.DontSaveInEditor;
            data.SetPixels32(new Color32[] {
                Color.red, Color.blue, Color.red,
                Color.green, Color.red, Color.green,
                Color.green, Color.red, Color.green
            });
            var expected = new int[] { 0, 0, 1, 1 };
            clustering.Cluster(data);
        }
    }
}
