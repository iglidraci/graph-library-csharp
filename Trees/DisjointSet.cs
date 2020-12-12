using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class DisjointSet<T> where T : IComparable<T>
    {
        private readonly Dictionary<T, T> _parent;
        private readonly Dictionary<T,int> _rank;
        public int GetCount()
        {
            return _parent.Count;
        }                
        public DisjointSet(T[] nodes)
        {
            _parent = new Dictionary<T, T>();
            _rank = new Dictionary<T, int>();
            Initialize(nodes);
        }
        private void Initialize(T[] nodes)
        {
            foreach (T node in nodes)
            {
                _parent.Add(node, node); // each node in a different set
                _rank.Add(node, 0); // each node has rank 0
            }
        }
        // find the root of the tree that node is in
        public T Find(T node)
        {
            if (_parent[node].CompareTo(node) == 0)
                return node;
            else
            {
                // we recursivly call to find the parent
                // we use dynamic programming to decrease the height of the tree
                T result = Find(_parent[node]);
                _parent[node] = result;
                return result;
            }
        }
        public void Union(T first,T second)
        {
            T firstRoot = Find(first);
            T secondRoot = Find(second);
            int firstRank = _rank[first];
            int secondRank = _rank[second];
            // if the two nodes are in the same tree dont do anything just return
            if (firstRoot.CompareTo(secondRoot) == 0)
                return;
            if (firstRank > secondRank)
            {
                //the first tree has a greater height so we attach the second tree to the first tree
                _parent[secondRoot] = firstRoot;
                return;
            }
            else if(secondRank > firstRank)
            {
                _parent[firstRoot] = secondRoot;
                return;
            }
            else
            {
                // just assign the second tree to the first tree root and increase the height
                _parent[secondRoot] = firstRoot;
                _rank[firstRoot]++;
                return;
            }
        }
        
    }
}
