using System;
using UnityEngine;

namespace Algo
{
    public interface INorm<T>
    {
        float Distance(T a, T b);
    }
    public class Norm<T> : INorm<T>
    {
        private readonly Func<T, T, float> f;

        public Norm(Func<T, T, float> f)
        {
            this.f = f;
        }

        public float Distance(T a, T b) => f(a, b);

        public static Norm<float> NormF => new Norm<float>((a, b) => Mathf.Abs(a - b));
        public static Norm<Vector2> NormV2 => new Norm<Vector2>((a, b) => Vector2.Distance(a, b));
        public static Norm<Vector3> NormV3 => new Norm<Vector3>((a, b) => Vector3.Distance(a, b));
        public static Norm<Color> NormHSV => new Norm<Color>((a, b) =>
        {
            Color.RGBToHSV(a, out float ha, out float sa, out float va);
            Color.RGBToHSV(b, out float hb, out float sb, out float vb);
            return Vector3.Distance(new Vector3(ha, sa, va), new Vector3(ha, sa, va));
        });
    }

}