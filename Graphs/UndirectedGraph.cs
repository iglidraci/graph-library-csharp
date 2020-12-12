using System;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class UndirectedGraph<T>:IGraph<T> where T:IComparable<T>
    {
        public UndirectedGraph()
        {
            adjacencyList = new Dictionary<T, HashSet<T>>();
            Count = 0;
        }
        private readonly Dictionary<T, HashSet<T>> adjacencyList;
        public int Count { get; set; }

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
        public void AddEdge(T firstNode, T secondNode)
        {
            if (!adjacencyList.ContainsKey(firstNode) || !adjacencyList.ContainsKey(secondNode))
                throw new Exception(message: "One of the nodes does not exist");
            adjacencyList[firstNode].Add(secondNode);
            adjacencyList[secondNode].Add(firstNode);

        }
        // gives us a map with key each of our vertexis and an integer that correspond to the label
        // shortly, each connected component should have the same label
        public Dictionary<T,int> FindConnectedComponents()
        {
            var map = new Dictionary<T, int>();
            var visited = new Dictionary<T, bool>();
            foreach (var key in adjacencyList.Keys)
            {
                visited.Add(key, false);
            }
            int label = 1;
            foreach (T vertex in adjacencyList.Keys)
            {
                if (!visited[vertex])
                {
                    Explore(vertex, label, map,visited);
                    label++;
                }
            }
            return map;
        }

        private void Explore(T vertex, int label, Dictionary<T, int> map, Dictionary<T, bool> visited)
        {
            visited[vertex] = true;
            map[vertex] = label;
            foreach (T neighbor in adjacencyList[vertex] )
            {
                if (!visited[neighbor])
                    Explore(neighbor, label, map, visited);
            }
        }
        public LinkedList<T> ShortestPath(T start, T end)
        {
            CheckIfNodesExist(start, end);
            var path = new LinkedList<T>();
            var (dist, prev) = BreadthFirstSearch(start);
            if (dist[end] == this.Count)
                throw new Exception($"There is not path from {start} to {end}");
            while (end.CompareTo(start) != 0)
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

        private (Dictionary<T, int> dist, Dictionary<T, T> prev) BreadthFirstSearch(T start)
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
            while (queue.Count > 0)
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
            return (distances, previous);
        }
        public bool IsBipartite()
        {
            if (Count == 0)
                throw new Exception("There is no graph built");
            T source = adjacencyList.Keys.ElementAt(0);
            var vertexColors = new Dictionary<T, bool?>(); // true represents red and false represent blue
            foreach (T key in adjacencyList.Keys)
            {
                vertexColors.Add(key, null);
            }
            vertexColors[source] = true;
            // start the BFS algorithm
            var queue = new Queue<T>();
            queue.Enqueue(source);
            while (queue.Count > 0)
            {
                T currentNode = queue.Dequeue();
                foreach (T neighbour in adjacencyList[currentNode])
                {
                    if (vertexColors[neighbour] == vertexColors[currentNode])
                        return false;
                    else if(vertexColors[neighbour] == null)
                    {
                        queue.Enqueue(neighbour);
                        vertexColors[neighbour] = !vertexColors[currentNode];
                    }
                }
            }
            return true;
        }
    }
}
