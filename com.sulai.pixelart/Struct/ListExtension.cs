using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Struct
{

    public static class NArray
    {
        public static Vector2[] Binary(this Vector2[] self, Vector2[] other, Func<Vector2, Vector2, Vector2> f)
        {
            return self.Zip(other, f).ToArray();
        }
        public static Vector2[] Zeros(int n)
        {
            return new Vector2[n];
        }
    }

    public static class ListExtension
    {
        public static (int, T) ArgMax<T>(this List<T> l, Func<T, T, float> c)
        {
            (int x, T val) max = (0, l[0]);
            foreach ((int i, T val) e in l.Enumerate())
            {
                if (c(max.val, e.val) < 0)
                    max = e;
            }
            return max;
        }
        public static (int, T) ArgMix<T>(this List<T> l, Func<T, T, float> c)
        {
            (int x, T val) max = (0, l[0]);
            foreach ((int i, T val) e in l.Enumerate())
            {
                if (c(max.val, e.val) > 0)
                    max = e;
            }
            return max;
        }
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> data)
        {
            int index = 0;
            foreach (var el in data)
                yield return (index++, el);
        }
    } 
}