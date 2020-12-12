using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees.Trees
{
    public class AvlTree<T> where T:IComparable<T>
    {
        private TreeNode<T> _root;
        public int Count { get; set; }
        public AvlTree()
        {
            _root = null;
            Count = 0;
        }
        public int GetHeight(TreeNode<T> node) => node == null ? -1 : node.Height;


        // binary search on the avl tree to find the value
        public TreeNode<T> Find(T value) => _root?.Get(value);
        

        public void Insert(T value)
        {
            _root = InserValue(_root, value);
            Count++;
        }
        public void Remove(T value)
        {
            _root = DeleteValue(_root, value);
            Count--;
        }

        private TreeNode<T> DeleteValue(TreeNode<T> node, T value)
        {
            //1) delete as in a normal bst
            if (node == null)
                throw new InvalidOperationException(message: $"The value {value} is not in the AVL tree");
            int difference = node.Value.CompareTo(value);
            if (difference > 0) // current node value is bigger than the value we want to delete
                node.Left = DeleteValue(node.Left, value);
            else if (difference < 0) // current node value is less than the value we want to delete
                node.Right = DeleteValue(node.Right, value);
            else // we found the node we want to delete
            {
                // case when the node has no left child
                if (node.Left == null)
                {
                    // no children or only right child
                    node = node.Right;
                }
                else if(node.Right == null)
                {
                    // only left child
                    node = node.Left;
                }
                else // node has two children
                {
                    // first get the min value in the right subtree
                    T rightSubTreeMinimum = LeftMost(node.Right);
                    // call deleteNode with the min value, it has to be the first case we already solved
                    node.Right = DeleteValue(node.Right, rightSubTreeMinimum);
                    // assign the node you want to delete the value of the right subtree minimum
                    node.Value = rightSubTreeMinimum;
                    
                }
            }
            if (node == null)
                return node;
            // 2) Set the height of the current node
            node.Height = CalculateTheHeight(node);
            //3)get the balance factor of the node
            int balance = GetBalanceFactor(node);
            //right right rotate
            if (balance > 1 && GetBalanceFactor(node.Left)>=0)
                return RightRotate(node);
            if (balance > 1 && GetBalanceFactor(node.Left) < 0)
                return LeftRightRotate(node);
            if (balance < -1 && GetBalanceFactor(node.Right) <= 0)
                return LeftRotate(node);
            if (balance < -1 && GetBalanceFactor(node.Right) > 0)
                return RightLeftRotate(node);
            return node;
        }
        private T LeftMost(TreeNode<T> node)
        {
            if (node.Left == null)
                return node.Value;
            else
                return LeftMost(node.Left);
        }
        private T GetMinimum(TreeNode<T> node)
        {
            var current = node;
            while (current.Left != null)
                current = current.Left;
            return current.Value;
        }

        private TreeNode<T> InserValue(TreeNode<T> node, T value)
        {
            // 1) just insert the value as a simple bst
            if (node == null)
            {
                return new TreeNode<T>(value);
            }
            else if (node.Value.CompareTo(value) > 0)
            {
                //insert in the left as a simple BST
                node.Left = InserValue(node.Left, value);
            }
            else if (node.Value.CompareTo(value) < 0)
            {
                //insert in the right as a simple BST
                node.Right = InserValue(node.Right, value);
            }
            else return node;
            // or you can just throw an exception that no duplicate values are allowed in the avl tree

            //2) update the height of the node
            node.Height = CalculateTheHeight(node);

            // 3) get the balance factor for the node and check all the 4 cases if it is unbalanced
            int balanceFactor = GetBalanceFactor(node);
            if(balanceFactor > 1) // here we are in the case where the left subtree is taller than it should have been
            {
                int leftLeftHeight = GetHeight(node.Left.Left);
                int leftRightHeight = GetHeight(node.Left.Right);
                if (leftLeftHeight >= leftRightHeight)
                    return RightRotate(node);
                else
                    return LeftRightRotate(node);
            }
            if(balanceFactor < -1) // the right subtree is taller than it should have been
            {
                int rightRightHeight = GetHeight(node.Right.Right);
                int rightLeftHeight = GetHeight(node.Right.Left);
                if (rightRightHeight >= rightLeftHeight)
                    return LeftRotate(node);
                else
                    return RightLeftRotate(node);
            }
            return node;
        }

        private TreeNode<T> RightLeftRotate(TreeNode<T> node)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        private TreeNode<T> LeftRotate(TreeNode<T> node)
        {
            var newRoot = node.Right;
            node.Right = newRoot.Left;
            newRoot.Left = node;
            node.Height = CalculateTheHeight(node);
            newRoot.Height = CalculateTheHeight(newRoot);
            return newRoot;
        }

        private TreeNode<T> LeftRightRotate(TreeNode<T> node)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        private TreeNode<T> RightRotate(TreeNode<T> node)
        {
            var newRoot = node.Left;
            node.Left = newRoot.Right;
            newRoot.Right = node;
            node.Height = CalculateTheHeight(node);
            newRoot.Height = CalculateTheHeight(node);
            return newRoot;
        }

        private int CalculateTheHeight(TreeNode<T> node) => 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

        public int GetBalanceFactor(TreeNode<T> node) => GetHeight(node.Left) - GetHeight(node.Right);

    }
}
