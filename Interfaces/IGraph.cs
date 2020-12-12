using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public interface IGraph<T>
    {
        Dictionary<T, HashSet<T>> GetAdjacencyList();
        void AddVertex(T vertex);
        void AddVertexis(T[] vertexis);
        void AddEdge(T first, T second);
    }
}
