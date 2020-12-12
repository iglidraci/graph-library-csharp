using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public interface IWeightedGraph<T>
    {
        Dictionary<T, Dictionary<T,int>> GetAdjacencyList();
        void AddVertex(T vertex);
        void AddVertexis(T[] vertexis);
        void AddEdge(T first, T second, int weight);
        
    }    
}
