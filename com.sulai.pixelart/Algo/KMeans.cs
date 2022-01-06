using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Struct;

namespace Algo
{
    public class KMeans
    {
        private readonly int k;
        private readonly int attempts;
        public Color[] Centers { get; private set; }

        public KMeans(int k, int attempts = 10)
        {
            this.k = k;
            this.attempts = attempts;
        }

        public Texture2D Cluster(Texture2D tex)
        {
            var img = CvTools.Tex2Img(tex);
            var clustering = new VectorOfInt(tex.width * tex.height);
            var centers = new VectorOfPoint3D32F(k);

            // only use rgb
            var m = img.Convert<Rgb, float>().Mat;
            m = m.Reshape(1, tex.width * tex.height);

            CvInvoke.Kmeans(m, k, clustering, new MCvTermCriteria(), attempts, KMeansInitType.PPCenters, centers);

            return CvTools.FromList(RemapCenters(), tex.width, tex.height, k - 1, tex);
            int[] RemapCenters()
            {
                // Convert to colors
                Centers = new Color[k];
                for (int i = 0; i < k; i++)
                {
                    var c = centers[i];
                    Centers[i] = new Color(c.X / 255f, c.Y / 255f, c.Z / 255f);
                }

                // sort by hsv
                var sorted = Centers.Enumerate().ToArray();
                Array.Sort(sorted, (a, b) =>
                {
                    Color.RGBToHSV(a.Item2, out float ha, out float sa, out float va);
                    Color.RGBToHSV(b.Item2, out float hb, out float sb, out float vb);
                    if (ha > hb)
                        return 1;
                    else if (ha < hb)
                        return -1;
                    else if (va > vb)
                        return 1;
                    else if (va < vb)
                        return -1;
                    else if (sa > sb)
                        return 1;
                    else if (sa < sb)
                        return -1;
                    return 0;
                });
                Centers = sorted.Select(el => el.Item2).ToArray();

                // create map
                Dictionary<int, int> map = new Dictionary<int, int>();
                int index = 0;
                foreach ((int i, Color c) in sorted)
                    map[i] = index++;

                // apply map
                var array = clustering.ToArray();
                for (int i = 0; i < array.Length; i++)
                    array[i] = map[array[i]];
                return array;
            }
        }
    }
}