using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Trees.Trees;

namespace Trees
{
    public class TestNode : IComparable
    {
        public string Name { get; set; }
        public bool Is { get; set; }

        public int CompareTo(object obj)
        {
            return Name.CompareTo(obj);
        }
    }
    class Program
    {
        public static IEnumerable<int> ReadInts(string filePath)
        {
            using (TextReader reader = File.OpenText(filePath))
            {
                string lastLine;
                while ((lastLine = reader.ReadLine()) != null)
                {
                    yield return int.Parse(lastLine);
                }
            }
        }
        static void Main(string[] args)
        {
            var graph = new DirectedGraph<TestNode>();
            var node1 = new TestNode { Name = "Igli" };
            var node2 = new TestNode { Name = "Suzi" };
            var node3 = new TestNode { Name = "Nela" };
            var node4 = new TestNode { Name = "Anjeza" };
            graph.AddVertexis(new TestNode[] { node1, node2, node3, node4 });
            graph.AddEdge(node1, node2);
            graph.AddEdge(node1, node3);
        }

       

        public bool IsValidBST(TreeNode<int> root)
        {
            if (root == null)
                return true;
            var stack = new Stack<int>();
            var map = new Dictionary<int, bool>();
            TraverseInOrder(root, stack, map);
            if (map.ContainsKey(1))
                return false;
            return true;
        }
        public void TraverseInOrder(TreeNode<int> node, Stack<int> stack, Dictionary<int,bool> map)
        {
            if (node.Left != null)
            {
                TraverseInOrder(node.Left, stack,map);
            }
            Console.WriteLine(node.Value);
            if (stack.Count() == 0)
                stack.Push(node.Value);
            else
            {
                if (node.Value <= stack.Peek())
                {                    
                    map.Add(1,false);
                }
                else
                    stack.Push(node.Value);
            }

            if (node.Right != null)
                TraverseInOrder(node.Right, stack ,map);
            return;
        }
    }
}
