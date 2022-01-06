using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Struct
{

    public interface IMatrix<T>
    {
        T Get(int x, int y);

        int Width { get; }
        int Height { get; }
    }

    public static class MatrixExtension
    {

        public static IEnumerable<T> Iterate<T>(this IMatrix<T> m)
        {
            for (int x = 0; x < m.Width; x++)
            {
                for (int y = 0; y < m.Height; y++)
                {
                    yield return m.Get(x, y);
                }
            }
        }
        public static IEnumerable<(int, int, T)> Enumerate<T>(this IMatrix<T> m)
        {
            for (int x = 0; x < m.Width; x++)
            {
                for (int y = 0; y < m.Height; y++)
                {
                    yield return (x, y, m.Get(x, y));
                }
            }
        }
        public static (int, int, T) ArgMax<T>(this IMatrix<T> m, Func<T, T, float> c)
        {
            (int x, int y, T val) max = (0, 0, m.Get(0, 0));
            foreach ((int x, int y, T val) e in m.Enumerate())
            {
                if (c(max.val, e.val) < 0)
                    max = e;
            }
            return max;
        }
        public static (int, int, T) ArgMin<T>(this IMatrix<T> m, Func<T, T, float> c)
        {
            (int x, int y, T val) max = (0, 0, m.Get(0, 0));
            foreach ((int x, int y, T val) e in m.Enumerate())
            {
                if (c(max.val, e.val) > 0)
                    max = e;
            }
            return max;
        }
        public static string Str<T>(this IMatrix<T> self)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < self.Width; y++)
            {
                for (int x = 0; x < self.Width; x++)
                {
                    sb.Append(self.Get(x, y));
                    if (x < self.Width - 1)
                        sb.Append(',');
                }

                if (y < self.Height - 1)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
    }


    public class SparseMatrix<T> : IMatrix<T>
    {
        protected Dictionary<int, T> values = new Dictionary<int, T>();

        public SparseMatrix(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public virtual T this[int x, int y]
        {
            get
            {
                if(!values.TryGetValue(x + Width * y, out T v))
                    v = default;
                return v;
            }
            set { values[x + Width * y] = value; }
        }

        public int Width { get; }
        public int Height { get; }

        public T Get(int x, int y) => this[x, y];
    }

    public class SymmetricMatrix<T> : SparseMatrix<T>
    {
        public SymmetricMatrix(int n) : base(n, n) { }

        override public T this[int x, int y]
        {
            get
            {
                if (x < y)
                    return values[x + Width * y];
                else
                    return values[y + Width * x];
            }
            set
            {

                if (x < y)
                    values[x + Width * y] = value;
                else
                    values[y + Width * x] = value;
            }
        }
    }

    public class Matrix<T> : IMatrix<T>
    {
        private readonly T[,] data;

        public int Width => data.GetLength(0);

        public int Height => data.GetLength(1);

        public Matrix(int width, int height) : this(new T[width, height]) { }

        public Matrix(T[,] data)
        {
            this.data = data;
        }

        public T Get(int x, int y) => this[x, y];
        public T this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }
    }

}