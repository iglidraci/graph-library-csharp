using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class WeightedUndirectedGraph<T> : IWeightedGraph<T> where T : IComparable<T>
    {
        private readonly Dictionary<T, Dictionary<T, int>> adjacencyList;
        public WeightedUndirectedGraph()
        {
            adjacencyList = new Dictionary<T, Dictionary<T, int>>();
        }
        public void AddEdge(T first, T second, int weight)
        {
            if (!adjacencyList.ContainsKey(first) || !adjacencyList.ContainsKey(second))
                throw new Exception(message: "One of the nodes does not exist");
            adjacencyList[first].Add(second, weight);
            adjacencyList[second].Add(first, weight);
        }

        public void AddVertex(T vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
            {
                adjacencyList.Add(vertex, new Dictionary<T, int>());
            }
        }

        public void AddVertexis(T[] vertexis)
        {
            foreach (T item in vertexis)
            {
                AddVertex(item);
            }
        }

        public Dictionary<T, Dictionary<T, int>> GetAdjacencyList()
        {
            return adjacencyList;
        }
    }
}
