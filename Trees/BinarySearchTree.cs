using System;
using System.Collections.Generic;

namespace Trees
{
    public class BinarySearchTree<T> where T:IComparable<T>
    {
        private TreeNode<T> _root;
        public BinarySearchTree()
        {
            _root = null;
        }
        public BinarySearchTree(T value)
        {
            _root.Value = value;
            _root.Right = null;
            _root.Left = null;
        }
        private TreeNode<T> Get()
        {
            return _root;
        }
        public void Insert(T value)
        {
            TreeNode<T> currentNode = Get();
            TreeNode<T> newNode = new TreeNode<T>
            {
                Value = value,
                Left = null,
                Right = null
            };
            if (currentNode == null)
                _root=newNode;
            else
            {
                
                while (true)
                {
                    int compare = value.CompareTo(currentNode.Value);
                    if (compare < 0)
                    {
                        // Go left
                        if (currentNode.Left == null)
                        {
                            currentNode.Left = newNode;
                            return;
                        }
                        else
                        {
                            currentNode = currentNode.Left;
                        }
                    }
                    else
                    {
                        if(currentNode.Right == null)
                        {
                            currentNode.Right = newNode;
                            return;
                        }
                        else
                        {
                            currentNode = currentNode.Right;
                        }
                    }
                }
            }
        }
        public T GetRootValue()
        {
            return _root.Value;
        }
        public TreeNode<T> GetRoot()
        {
            return _root;
        }
        // get back here does not work
        public void Remove(T value)
        {
            if (_root == null)
                return;
            TreeNode<T> currentNode = _root;
            TreeNode<T> parent = null;
            while (currentNode != null)
            {
                int compare = value.CompareTo(currentNode.Value);
                if (compare < 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.Left;
                }
                else if (compare > 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.Right;
                }
                else
                {
                    // if it is the root to remove just set it to null
                    if (parent == null)
                        _root = null;
                    // 1) Node to be removed has no child
                    if (HasNoChild(currentNode))
                    {
                        if (parent.Right.Value.CompareTo(value) == 0)
                            parent.Right = null;
                        else
                            parent.Left = null;
                        return;
                    }
                    // 2) Node to be removed has one child
                    else if (HasOneChildOnly(currentNode))
                    {
                        if(currentNode.Left == null)
                        {
                            if (parent.Right.Value.CompareTo(value) == 0)
                                parent.Right = currentNode.Right;
                            else
                                parent.Left = currentNode.Right;
                        }
                        else
                        {
                            if (parent.Right.Value.CompareTo(value) == 0)
                                parent.Right = currentNode.Left;
                            else
                                parent.Left = currentNode.Left;
                        }
                        return;
                    }
                    // 3) Node to be removed has 2 children
                    else
                    {
                        TreeNode<T> parentToTheMinimumValueNode = currentNode;
                        TreeNode<T> minimumValueNode = currentNode.Right;
                        while(minimumValueNode.Left != null)
                        {
                            parentToTheMinimumValueNode = minimumValueNode;
                            minimumValueNode = minimumValueNode.Left;

                        }
                        currentNode.Value = minimumValueNode.Value;
                        parentToTheMinimumValueNode.Left = null;
                        return;
                    }
                    

                }
            }
        }
        public List<T> BreadthFirstSearch()
        {
            var list = new List<T>();
            var queue = new Queue<TreeNode<T>>();
            var currentNode = GetRoot();
            queue.Enqueue(currentNode);
            while(queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                list.Add(currentNode.Value);
                if(currentNode.Left != null)
                {
                    queue.Enqueue(currentNode.Left);
                }
                if(currentNode.Right != null)
                {
                    queue.Enqueue(currentNode.Right);
                }
            }
            return list;
        }
        public List<T> BreadthFirstSearchRecursive()
        {
            var queue = new Queue<TreeNode<T>>();
            var list = new List<T>();
            queue.Enqueue(GetRoot());
            return BFSR(queue,list);
        }

        private List<T> BFSR(Queue<TreeNode<T>> queue, List<T> list)
        {
            if (queue.Count == 0)
                return list;
            var current = queue.Dequeue();
            list.Add(current.Value);
            if (current.Left != null)
                queue.Enqueue(current.Left);
            if (current.Right != null)
                queue.Enqueue(current.Right);
            return this.BFSR(queue, list);
        }
        public List<T> DepthFirstSearch()
        {
            var list = new List<T>();
            return TraverseInOrder(_root,list);
        }

        private List<T> TraverseInOrder(TreeNode<T> node, List<T> list)
        {
            if (node.Left != null)
                TraverseInOrder(node.Left, list);
            list.Add(node.Value);
            if (node.Right != null)
                TraverseInOrder(node.Right, list);
            return list;
        }

        private bool HasOneChildOnly(TreeNode<T> currentNode)
        {
            if (currentNode.Left != null && currentNode.Right == null)
                return true;
            if (currentNode.Left == null && currentNode.Right != null)
                return true;
            return false;
        }

        private bool HasNoChild(TreeNode<T> currentNode)
        {
            if (currentNode.Left == null && currentNode.Right == null)
                return true;
            return false;
        }
        
    }
}
