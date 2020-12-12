using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class WeightedDirectedGraph<T> : IWeightedGraph<T> where T : IComparable<T>
    {
        private readonly Dictionary<T, Dictionary<T,int>> adjacencyList;
        public WeightedDirectedGraph()
        {
            adjacencyList = new Dictionary<T, Dictionary<T, int>>();
        }

        public void AddEdge(T first, T second, int weight)
        {
            if (!adjacencyList.ContainsKey(first) || !adjacencyList.ContainsKey(second))
                throw new Exception(message: "One of the nodes does not exist");
            adjacencyList[first].Add(second,weight);
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
        public WeightedDirectedGraph<T> GetReversedGraph()
        {
            var reversedGraph = new WeightedDirectedGraph<T>();
            foreach (T vertex in this.GetAdjacencyList().Keys)
            {
                reversedGraph.AddVertex(vertex);
            }
            foreach (T key in this.GetAdjacencyList().Keys)
            {
                foreach (var edge in this.GetAdjacencyList()[key])
                {
                    reversedGraph.AddEdge(edge.Key, key,edge.Value);
                }
            }
            return reversedGraph;
        }
    }
}
