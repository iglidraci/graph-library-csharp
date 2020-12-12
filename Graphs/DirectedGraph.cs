using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class DirectedGraph<T>:IGraph<T> where T:IComparable
    {
        public DirectedGraph()
        {
            adjacencyList = new Dictionary<T, HashSet<T>>();
            Count = 0;
        }

        private readonly Dictionary<T, HashSet<T>> adjacencyList;
        public int Count { get; set; }
        private int _orderCount = 1;

        public Dictionary<T, HashSet<T>> GetAdjacencyList() => adjacencyList;
        public void AddVertex(T vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
            {
                adjacencyList.Add(vertex, new HashSet<T>());
                Count++;
            }
        }
        public void AddVertexis(T[] vertexis)
        {
            foreach (T item in vertexis)
            {
                AddVertex(item);
            }
        }
        public void AddEdge(T fromNode, T toNode)
        {
            if (!adjacencyList.ContainsKey(fromNode) || !adjacencyList.ContainsKey(toNode))
                throw new Exception(message: "One of the nodes does not exist");
            adjacencyList[fromNode].Add(toNode);
        }
        public List<T> LinearOrder()
        {
            var linearOrder = new Stack<T>();
            var graph = GetAdjacencyList();
            var visited = new Dictionary<T, bool>();
            foreach (var key in adjacencyList.Keys)
            {
                visited.Add(key, false);
            }
            var list = new List<T>();
            foreach (T key in graph.Keys)
            {
                if(!visited[key])
                    FollowPathUntilCannotExtend(key, linearOrder, graph,visited);
            }
            while (linearOrder.Count != 0)
            {
                list.Add(linearOrder.Pop());
            }
            return list;
        }

        private void FollowPathUntilCannotExtend(T key, Stack<T> linearOrder, Dictionary<T, HashSet<T>> graph, Dictionary<T, bool> visited)
        {
            visited[key] = true;
            foreach (T neighbour in graph[key])
            {
                if(!visited[neighbour])
                    FollowPathUntilCannotExtend(neighbour, linearOrder, graph,visited);
            }
            linearOrder.Push(key);
            return;

        }
        public Dictionary<T,int> GetStronglyConnectedComponents()
        {
            var map = new Dictionary<T, int>();
            DirectedGraph<T> reversedGraph = new DirectedGraph<T>();
            ReversedGraph(reversedGraph);
            var preOrder = new Dictionary<T, int>();
            var postOrder = new Dictionary<T, int>();
            DepthFirstSearch(reversedGraph, preOrder, postOrder);
            var sortedDesc = from entry in postOrder orderby entry.Value descending select entry;
            var visited = new Dictionary<T, bool>();
            foreach (var key in adjacencyList.Keys)
            {
                visited.Add(key, false);
            }
            int label = 1;
            foreach (KeyValuePair<T,int> kvp in sortedDesc)
            {
                if (!visited[kvp.Key])
                {
                    Explore(kvp.Key, label, map, visited);
                    label++;
                }
            }
            return map;
        }

        private void Explore(T vertex, int label, Dictionary<T, int> map, Dictionary<T, bool> visited)
        {
            visited[vertex] = true;
            map[vertex] = label;
            foreach (T neighbor in adjacencyList[vertex])
            {
                if (!visited[neighbor])
                    Explore(neighbor, label, map, visited);
            }
        }

        private void DepthFirstSearch(DirectedGraph<T> reversedGraph, Dictionary<T, int> preOrder, Dictionary<T, int> postOrder)
        {
            var visited = new Dictionary<T, bool>();
            foreach (var key in reversedGraph.GetAdjacencyList().Keys)
            {
                visited.Add(key, false);
            }
            foreach (T vertex in reversedGraph.GetAdjacencyList().Keys)
            {
                if (!visited[vertex])
                {
                    Explore(vertex,visited,preOrder,postOrder,reversedGraph);
                }
            }
        }

        // this method is for exploring the reversed graph
        private void Explore(T vertex,Dictionary<T, bool> visited, Dictionary<T, int> preOrder, Dictionary<T, int> postOrder, DirectedGraph<T> reversedGraph)
        {
            visited[vertex] = true;
            Previsit(vertex, preOrder);
            _orderCount++;
            foreach (T neighbor in reversedGraph.GetAdjacencyList()[vertex])
            {
                if (!visited[neighbor])
                    Explore(neighbor,visited,preOrder,postOrder,reversedGraph);
            }
            PostVisit(vertex, postOrder);
            _orderCount++;
            return;
        }

        private void PostVisit(T vertex, Dictionary<T, int> postOrder)
        {
            postOrder.Add(vertex, _orderCount);
        }

        private void Previsit(T vertex, Dictionary<T, int> preOrder)
        {
            preOrder.Add(vertex, _orderCount);
        }
        private void ReversedGraph(DirectedGraph<T> reversedGraph)
        {
            foreach (T vertex in GetAdjacencyList().Keys)
            {
                reversedGraph.AddVertex(vertex);
            }
            foreach (T key in GetAdjacencyList().Keys)
            {
                foreach (T edge in GetAdjacencyList()[key])
                {
                    reversedGraph.AddEdge(edge, key);
                }
            }
        }
        public bool IsCyclic()
        {
            var visited = new Dictionary<T, bool>();
            var recursionStack = new Dictionary<T, bool>();
            foreach (T vertex in adjacencyList.Keys)
            {
                visited.Add(vertex, false);
                recursionStack.Add(vertex, false);
            }
            foreach (T vertex in adjacencyList.Keys)
            {
                if (IsCyclicInner(vertex, visited, recursionStack))
                    return true;
            }
            return false;
        }

        private bool IsCyclicInner(T vertex, Dictionary<T, bool> visited, Dictionary<T, bool> recursionStack)
        {
            if (recursionStack[vertex])
                return true;
            if (visited[vertex])
                return false;
            visited[vertex] = true;
            recursionStack[vertex] = true;
            foreach (T neighbour in adjacencyList[vertex])
            {
                if (IsCyclicInner(neighbour, visited, recursionStack))
                    return true;
            }
            recursionStack[vertex] = false;
            return false;
        }
        public LinkedList<T> ShortestPath(T start,T end)
        {
            CheckIfNodesExist(start, end);
            var path = new LinkedList<T>();
            var (dist, prev) = BreadthFirstSearch(start);
            if (dist[end] == this.Count)
                throw new Exception($"There is not path from {start} to {end}");
            while (end.CompareTo(start)!=0)
            {
                path.AddFirst(end);
                end = prev[end];
            }
            path.AddFirst(start);
            return path;
        }

        private void CheckIfNodesExist(T start, T end)
        {
            if (!adjacencyList.ContainsKey(start))
                throw new Exception("Starting point does not exist");
            if (!adjacencyList.ContainsKey(end))
                throw new Exception("Ending point does not exist");
        }

        private (Dictionary<T, int> dist,Dictionary<T,T> prev) BreadthFirstSearch(T start)
        {
            var distances = new Dictionary<T, int>();
            var previous = new Dictionary<T, T>();
            foreach (T key in adjacencyList.Keys)
            {
                distances.Add(key, this.Count); // a path cannot be equal to the count
                previous.Add(key, default);
            }
            distances[start] = 0;
            var queue = new Queue<T>();
            queue.Enqueue(start);
            while (queue.Count>0)
            {
                T currentNode = queue.Dequeue();
                foreach (T neighbour in adjacencyList[currentNode])
                {
                    if (distances[neighbour] == this.Count)
                    {
                        queue.Enqueue(neighbour);
                        distances[neighbour] = distances[currentNode] + 1;
                        previous[neighbour] = currentNode;
                    }
                }
            }
            return (distances,previous);
        }
    }
}
