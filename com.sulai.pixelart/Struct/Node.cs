using Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Struct
{

    public interface INode<T>
    {
        List<INode<T>> Children { get; }

    }

    public static class INodeExtension
    {
        public static IEnumerable<T> Leaves<T>(this INode<T> root)
        {
            switch (root)
            {
                case LeafNode<T> leaf:
                    yield return leaf.data;
                    break;
                case BinaryNode<T> node:
                    foreach (var n in node.Children)
                        foreach (var v in Leaves(n))
                            yield return v;
                    break;
                default:
                    break;
            }
        }
    }

    public class BinaryNode<T> : INode<T>
    {
        public BinaryNode<T> left;
        public BinaryNode<T> right;
        public float height = 0;

        public BinaryNode() { }

        public float Height()
        {
            var lh = left.Height();
            var rh = right.Height();
            return (lh > rh) ? lh : rh;
        }

        public List<INode<T>> Children
        {
            get
            {
                if (left == null)
                    if (right == null)
                        return new List<INode<T>>();
                    else
                        return new List<INode<T>>() { right };
                else
                {
                    if (right == null)
                        return new List<INode<T>>() { left };
                    else
                        return new List<INode<T>>() { left, right };
                }
            }
        }
    }
    public class LeafNode<T> : BinaryNode<T>
    {
        public T data;
        public LeafNode(T data)
        {
            this.data = data;
        }
    }
}