using System;

namespace Trees
{
    public class RedBlackTree<T> where T:IComparable<T>
    {
        private TreeNode<T> root;
        private TreeNode<T> insertedNode; //Set in InsertNode() method
        private TreeNode<T> nodeBeingDeleted; //Set in DeleteNode() method
        private bool siblingToRight;    //Sibling of curNode
        private bool parentToRight;     // of grand parent
        private bool nodeToDeleteRed;   //Color of deleted node
        public int Count { get; private set; }
        public TreeNode<T> Get(T value)
        {
            return root?.Get(value);
        }

        private void LeftRotate(ref TreeNode<T> node)
        {
            TreeNode<T> nodeParent = node.Parent;
            TreeNode<T> right = node.Right;
            TreeNode<T> temp = right.Left;
            right.Left = node;
            node.Parent = right;
            node.Right = temp;
            if (temp != null)
                temp.Parent = node;
            if (right != null)
                right.Parent = nodeParent;
            node = right;
        }
        private void RightRotate(ref TreeNode<T> node)
        {
            TreeNode<T> nodeParent = node.Parent;
            TreeNode<T> left = node.Left;
            TreeNode<T> temp = left.Right;
            left.Right = node;
            node.Parent = left;
            node.Left = temp;
            if (temp != null)
                temp.Parent = node;
            if (left != null)
                left.Parent = nodeParent;
            node = left;
        }

        public void Add(T item)
        {
            
            root = InsertNode(root, item, null);
            Count++;
            if (Count > 2)
            {
                TreeNode<T> parent, grandParent, greatGrandParent;
                GetNodesAbove(insertedNode, out parent, out grandParent, out greatGrandParent);
                FixTreeAfterInsertion(insertedNode, parent,grandParent,greatGrandParent);
            }
        }

        private void FixTreeAfterInsertion(TreeNode<T> child, 
                                            TreeNode<T> parent, TreeNode<T> grandParent, TreeNode<T> greatGrandParent)
        {
            if(grandParent != null)
            {
                TreeNode<T> uncle = (grandParent.Right == parent) ?
                    grandParent.Left : grandParent.Right;
                if(uncle != null && parent.Red && uncle.Red)
                {
                    uncle.Red = false;
                    parent.Red = false;
                    grandParent.Red = true;
                    TreeNode<T> higher = null;
                    TreeNode<T> stillHigher = null;
                    if (greatGrandParent != null)
                        higher = greatGrandParent.Parent;
                    if (higher != null)
                        stillHigher = higher.Parent;
                    FixTreeAfterInsertion(grandParent, greatGrandParent, higher, stillHigher);
                }
                else if(uncle == null || parent.Red && !uncle.Red)
                {
                    if (grandParent.Right == parent && parent.Right == child)
                    {
                        parent.Red = false;
                        grandParent.Red = true;
                        if (greatGrandParent != null)
                        {
                            if (greatGrandParent.Right == grandParent)
                            {
                                LeftRotate(ref grandParent);
                                greatGrandParent.Right = grandParent;
                            }
                            else
                            {
                                LeftRotate(ref grandParent);
                                greatGrandParent.Left = grandParent;
                            }
                        }
                        else
                        {
                            LeftRotate(ref root);
                        }
                    }
                    else if (grandParent.Left == parent && parent.Left == child)
                    {
                        parent.Red = false;
                        grandParent.Red = true;
                        if (greatGrandParent != null)
                        {
                            if (greatGrandParent.Right == grandParent)
                            {
                                RightRotate(ref grandParent);
                                greatGrandParent.Right = grandParent;
                            }
                            else
                            {
                                RightRotate(ref grandParent);
                                greatGrandParent.Left = grandParent;
                            }
                        }
                        else
                        {
                            RightRotate(ref root);
                        }
                    }
                    else if (grandParent.Right == parent && parent.Left == child)
                    {
                        child.Red = false;
                        grandParent.Red = true;
                        RightRotate(ref parent);
                        grandParent.Right = parent;
                        if(greatGrandParent != null)
                        {
                            if (greatGrandParent.Right == grandParent)
                            {
                                LeftRotate(ref grandParent);
                                greatGrandParent.Right = grandParent;
                            }
                            else
                            {
                                LeftRotate(ref grandParent);
                                greatGrandParent.Left = grandParent;
                            }
                        }
                        else
                        {
                            LeftRotate(ref root);
                        }
                    }
                    else if(grandParent.Left == parent && parent.Right == child)
                    {
                        child.Red = false;
                        grandParent.Red = true;
                        LeftRotate(ref parent);
                        grandParent.Left = parent;
                        if(greatGrandParent != null)
                        {
                            if (greatGrandParent.Right == grandParent)
                            {
                                RightRotate(ref grandParent);
                                greatGrandParent.Right = grandParent;
                            }
                            else
                            {
                                RightRotate(ref grandParent);
                                greatGrandParent.Left = grandParent;
                            }
                        }
                        else
                        {
                            RightRotate(ref root);
                        }
                    }
                }
                if (root.Red)
                {
                    root.Red = false;
                }
            }
        }

        private void GetNodesAbove(TreeNode<T> curNode, out TreeNode<T> parent, 
                                    out TreeNode<T> grandParent, out TreeNode<T> greatGrandParent)
        {
            parent = null;
            grandParent = null;
            greatGrandParent = null;
            if (curNode != null)
                parent = curNode.Parent;
            if (parent != null)
                grandParent = parent.Parent;
            if (grandParent != null)
                greatGrandParent = grandParent.Parent;
        }

        private TreeNode<T> InsertNode(TreeNode<T> node, T item, TreeNode<T> parent)
        {
            if (node == null)
            {
                TreeNode<T> newNode = new TreeNode<T>(item, parent);
                if (Count > 0)
                    newNode.Red = true;
                else
                    newNode.Red = false;
                insertedNode = newNode;
                return newNode;
            }
            else if (item.CompareTo(node.Value) < 0)
            {
                node.Left = InsertNode(node.Left, item, node);
                return node;
            }
            else if (item.CompareTo(node.Value) > 0)
            {
                node.Right = InsertNode(node.Right, item, node);
                return node;
            }
            else
                throw new InvalidOperationException("Cannot add duplicate object into RBT");

        }
        public TreeNode<T> GetRootNode()
        {
            return root;
        }
       
        public void Remove(T item)
        {
            if (Count > 1)
            {
                root = DeleteNode(root, item, null);
                Count--;
                if (Count == 0)
                    root = null;
                TreeNode<T> curNode = null;
                if (nodeBeingDeleted.Right != null)
                    curNode = nodeBeingDeleted.Right;
                TreeNode<T> parent, sibling, grandParent;
                if (curNode == null)
                    parent = nodeBeingDeleted.Parent;
                else
                    parent = curNode.Parent;
                GetParentGrandParentSibling(curNode, parent, out sibling, out grandParent);
                if (curNode != null && curNode.Red)
                    curNode.Red = false;
                else if (!nodeToDeleteRed && !nodeBeingDeleted.Red)
                    FixTreeAfterDeletion(curNode, parent, sibling, grandParent);
                root.Red = false;
            }
        }

        private TreeNode<T> DeleteNode(TreeNode<T> node, T item, TreeNode<T> parent)
        {
            if (node == null)
                throw new InvalidOperationException("Value is not in the tree");
            if (item.CompareTo(node.Value) < 0)
                node.Left = DeleteNode(node.Left, item, node);
            else if (item.CompareTo(node.Value) > 0)
                node.Right = DeleteNode(node.Right, item, node);
            else if (item.CompareTo(node.Value) == 0)
            {
                nodeToDeleteRed = node.Red;
                nodeBeingDeleted = node;
                if (node.Left == null)
                {
                    //No children or only a right children
                    node = node.Right;
                    if (node != null)
                        node.Parent = parent;
                }
                else if (node.Right == null)
                {
                    //Only a left child
                    node = node.Left;
                    node.Parent = parent;
                }
                else
                {
                    //two children
                    //deletes using the leftmost node of the right subtree
                    T replaceWithValue = LeftMost(node.Right);
                    node.Right = DeleteLeftMost(node.Right, node);
                    node.Value = replaceWithValue;
                }
            }
            return node;
        }

        private TreeNode<T> DeleteLeftMost(TreeNode<T> node, TreeNode<T> parent)
        {
            if(node.Left == null)
            {
                nodeBeingDeleted = node;
                if (node.Right != null)
                    node.Parent = parent;
                return node.Right;
            }
            else
            {
                node.Left = DeleteLeftMost(node.Left, node);
                return node;
            }
        }

        private T LeftMost(TreeNode<T> node)
        {
            if (node.Left == null)
                return node.Value;
            else
                return LeftMost(node.Left);
        }

        private void FixTreeAfterDeletion(TreeNode<T> curNode, TreeNode<T> parent, 
                                            TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            TreeNode<T> siblingLeftChild = null;
            TreeNode<T> siblingRightChild = null;
            if (sibling != null && sibling.Left != null)
                siblingLeftChild = sibling.Left;
            if (sibling != null && sibling.Right != null)
                siblingRightChild = sibling.Right;
            bool siblingRed = (sibling != null && sibling.Red);
            bool siblingLeftRed = (siblingLeftChild != null && siblingLeftChild.Red);
            bool siblingRightRed = (siblingRightChild != null && siblingRightChild.Red);
            if (parent != null && !parent.Red && siblingRed && !siblingLeftRed && !siblingRightRed)
                Case1(curNode, parent, sibling, grandParent);
            else if (parent != null && !parent.Red && !siblingRed && !siblingLeftRed && !siblingRightRed)
                Case2A(curNode, parent, sibling, grandParent);
            else if(parent!=null && parent.Red && !siblingRed && !siblingLeftRed && !siblingRightRed)
                Case2B(curNode, parent, sibling, grandParent);
            else if(siblingToRight && !siblingRed && siblingLeftRed && !siblingRightRed)
                Case3(curNode, parent, sibling, grandParent);
            else if(!siblingToRight && !siblingRed && !siblingLeftRed && siblingRightRed)
                Case3P(curNode, parent, sibling, grandParent);
            else if(siblingToRight && !siblingRed && siblingRightRed)
                Case4(curNode, parent, sibling, grandParent);
            else if(!siblingToRight && !siblingRed && siblingLeftRed)
                Case4P(curNode, parent, sibling, grandParent);
        }

        private void Case4P(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            sibling.Red = parent.Red;
            sibling.Left.Red = false;
            parent.Red = false;
            if(grandParent != null)
            {
                if (parentToRight)
                {
                    RightRotate(ref parent);
                    grandParent.Right = parent;
                }
                else
                {
                    RightRotate(ref parent);
                    grandParent.Left = parent;
                }
            }
            else
            {
                RightRotate(ref parent);
                root = parent;
            }
        }

        private void Case4(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            sibling.Red = parent.Red;
            sibling.Right.Red = false;
            parent.Red = false;
            if(grandParent != null)
            {
                if (parentToRight)
                {
                    LeftRotate(ref parent);
                    grandParent.Right = parent;
                }
                else
                {
                    LeftRotate(ref parent);
                    grandParent.Left = parent;
                }
            }
            else
            {
                LeftRotate(ref parent);
                root = parent;
            }
        }

        private void Case3P(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            if (parent.Right == curNode)
            {
                sibling.Red = true;
                sibling.Right.Red = false;
                LeftRotate(ref sibling);
                parent.Left = sibling;
            }
            Case4P(curNode, parent, sibling, grandParent);
        }

        private void Case3(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            if(parent.Left == curNode)
            {
                sibling.Red = true;
                sibling.Left.Red = false;
                RightRotate(ref sibling);
                parent.Right = sibling;
            }
            Case4(curNode, parent, sibling, grandParent);
        }

        private void Case2B(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            if(sibling != null)
                sibling.Red=!sibling.Red;
            curNode = parent;
            curNode.Red = !curNode.Red;
        }

        private void Case2A(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            if (sibling != null)
                sibling.Red = !sibling.Red;
            curNode = parent;
            if (curNode != root)
            {
                parent = curNode.Parent;
                GetParentGrandParentSibling(curNode, parent, out sibling, out grandParent);
                TreeNode<T> siblingLeftChild = null;
                TreeNode<T> siblingRightChild = null;
                if (sibling != null && sibling.Left != null)
                    siblingLeftChild = sibling.Left;
                if (sibling != null && sibling.Right != null)
                    siblingRightChild = sibling.Right;
                bool siblingRed = (sibling != null && sibling.Red);
                bool siblingLeftRed = (siblingLeftChild != null && siblingLeftChild.Red);
                bool siblingRightRed = (siblingRightChild != null && siblingRightChild.Red);
                if (parent != null && !parent.Red && !siblingRed && !siblingLeftRed && !siblingRightRed)
                    Case2A(curNode, parent, sibling, grandParent);
                else if (parent != null && parent.Red && !siblingRed && !siblingLeftRed && !siblingRightRed)
                    Case2B(curNode, parent, sibling, grandParent);
                else if (siblingToRight && !siblingRed && !siblingLeftRed && siblingRightRed)
                    Case3(curNode, parent, sibling, grandParent);
                else if (!siblingToRight && !siblingRed && !siblingLeftRed && siblingRightRed)
                    Case3P(curNode, parent, sibling, grandParent);
                else if (siblingToRight && !siblingRed && siblingRightRed)
                    Case4(curNode, parent, sibling, grandParent);
                else if (!siblingToRight && !siblingRed && siblingLeftRed)
                    Case4P(curNode, parent, sibling, grandParent);
            }
        }

        private void Case1(TreeNode<T> curNode, TreeNode<T> parent, TreeNode<T> sibling, TreeNode<T> grandParent)
        {
            if (siblingToRight)
            {
                parent.Red = !parent.Red;
                sibling.Red = !sibling.Red;
                if(grandParent != null)
                {
                    if (parentToRight)
                    {
                        LeftRotate(ref parent);
                        grandParent.Right = parent;
                    }
                    else if (!parentToRight)
                    {
                        LeftRotate(ref parent);
                        grandParent.Left = parent;
                    }
                }
                else
                {
                    LeftRotate(ref parent);
                    root = parent;
                }
                grandParent = sibling;
                parent = parent.Left;
                parentToRight = false;
            }
            else if (!siblingToRight)
            {
                parent.Red = !parent.Red;
                sibling.Red = !sibling.Red;
                if(grandParent != null)
                {
                    if (parentToRight)
                    {
                        RightRotate(ref parent);
                        grandParent.Right = parent;
                    }
                    else if (!parentToRight)
                    {
                        RightRotate(ref parent);
                        grandParent.Left = parent;
                    }
                }
                else
                {
                    RightRotate(ref parent);
                    root = parent;
                }
                grandParent = sibling;
                parent = parent.Right;
                parentToRight = true;
            }
            if (parent.Right == curNode)
            {
                sibling = parent.Left;
                siblingToRight = false;
            }
            else if (parent.Left == curNode)
            {
                sibling = parent.Right;
                siblingToRight = true;
            }
            TreeNode<T> siblingLeftChild = null;
            TreeNode<T> siblingRightChild = null;
            if (sibling != null && sibling.Left != null)
                siblingLeftChild = sibling.Left;
            if (sibling != null && sibling.Right != null)
                siblingRightChild = sibling.Right;
            bool siblingRed = (sibling != null && sibling.Red);
            bool siblingLeftRed = (siblingLeftChild != null && siblingLeftChild.Red);
            bool siblingRightRed = (siblingRightChild != null && siblingRightChild.Red);
            if (parent.Red && !siblingRed && !siblingLeftRed && !siblingRightRed)
                Case2B(curNode, parent, sibling, grandParent);
            else if (siblingToRight && !siblingRed && siblingLeftRed && !siblingRightRed)
                Case3(curNode, parent, sibling, grandParent);
            else if (!siblingToRight && !siblingRed && !siblingLeftRed && siblingRightRed)
                Case3P(curNode, parent, sibling, grandParent);
            else if (siblingToRight && !siblingRed && siblingRightRed)
                Case4(curNode, parent, sibling, grandParent);
            else if (!siblingToRight && !siblingRed && siblingLeftRed)
                Case4P(curNode, parent, sibling, grandParent);

        }

        private void GetParentGrandParentSibling(TreeNode<T> curNode, 
                                                TreeNode<T> parent, out TreeNode<T> sibling, 
                                                out TreeNode<T> grandParent)
        {
            sibling = null;
            grandParent = null;
            if (parent != null)
            {
                if(parent.Right == curNode)
                {
                    siblingToRight = false;
                    sibling = parent.Left;
                }
                if (parent.Left == curNode)
                {
                    siblingToRight = true;
                    sibling = parent.Right;
                }
            }
            if (parent != null)
                grandParent = parent.Parent;
            if(grandParent != null)
            {
                if (grandParent.Right == parent)
                    parentToRight = true;
                if (grandParent.Left == parent)
                    parentToRight = false;
            }
        }

    }
}
