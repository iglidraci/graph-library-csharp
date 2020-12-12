using System;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class Bst<T> where T: IComparable<T>
    {
        private TreeNode<T> root;
        public TreeNode<T> Get(T value)
        {
            return root?.Get(value);
        }

        public T Min()
        {
            if (root == null)
                throw new InvalidOperationException(message: "Empty tree");
            return root.Min();
        }
        public T Max()
        {
            if (root == null)
                throw new InvalidOperationException(message: "Empty tree");
            return root.Max();
        }
        public void Insert(T value)
        {
            if (root == null)
                root = new TreeNode<T>(value);
            else
                root.Insert(value);
        }
        public IEnumerable<T> TraverseInOrder()
        {
            if (root != null)
                return root.TraverseInOrder();
            else
                return Enumerable.Empty<T>();
        }
        public void Remove(T value)
        {
            root = Remove(root, value);
        }
        public TreeNode<T> Remove(TreeNode<T> subtreeRoot, T value)
        {
            if (subtreeRoot == null)
                return null;
            int compareTo = value.CompareTo(subtreeRoot.Value);
            if (compareTo < 0)
            {
                subtreeRoot.Left = Remove(subtreeRoot.Left, value);
            }
            else if (compareTo > 0)
            {
                subtreeRoot.Right = Remove(subtreeRoot.Right, value);
            }
            else
            {
                if (subtreeRoot.Left == null)
                {
                    return subtreeRoot.Right;
                }
                if (subtreeRoot.Right == null)
                {
                    return subtreeRoot.Left;
                }
                subtreeRoot.Value = subtreeRoot.Right.Min();
                subtreeRoot.Right = Remove(subtreeRoot.Right, subtreeRoot.Value);

            }
            return subtreeRoot;

        }

    }
}
