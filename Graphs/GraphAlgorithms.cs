using System;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class GraphAlgorithms<T> where T : IComparable<T>
    {
        private IWeightedGraph<T> weightedGraph;
        private void SetWeightedGraph(IWeightedGraph<T> graph)
        {
            weightedGraph = graph;
        }
        public GraphAlgorithms()
        {
        }
        public LinkedList<T> ShortestPath(IGraph<T> graph, T start, T end)
        {
            CheckIfNodesExist(graph, start, end);
            var path = new LinkedList<T>();
            var (dist, prev) = BreadthFirstSearch(graph, start);
            if (dist[end] == graph.GetAdjacencyList().Count)
                throw new Exception($"There is not path from {start} to {end}");
            while (end.CompareTo(start) != 0)
            {
                path.AddFirst(end);
                end = prev[end];
            }
            path.AddFirst(start);
            return path;
        }

        private void CheckIfNodesExist(IGraph<T> graph, T start, T end)
        {
            if (!graph.GetAdjacencyList().ContainsKey(start))
                throw new Exception("Starting point does not exist");
            if (!graph.GetAdjacencyList().ContainsKey(end))
                throw new Exception("Ending point does not exist");
        }

        private (Dictionary<T, int> dist, Dictionary<T, T> prev) BreadthFirstSearch(IGraph<T> graph, T start)
        {
            var distances = new Dictionary<T, int>();
            var previous = new Dictionary<T, T>();
            foreach (T key in graph.GetAdjacencyList().Keys)
            {
                distances.Add(key, graph.GetAdjacencyList().Count); // a path cannot be equal to the count
                previous.Add(key, default);
            }
            distances[start] = 0;
            var queue = new Queue<T>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                T currentNode = queue.Dequeue();
                foreach (T neighbour in graph.GetAdjacencyList()[currentNode])
                {
                    if (distances[neighbour] == graph.GetAdjacencyList().Count)
                    {
                        queue.Enqueue(neighbour);
                        distances[neighbour] = distances[currentNode] + 1;
                        previous[neighbour] = currentNode;
                    }
                }
            }
            return (distances, previous);
        }
        public (Dictionary<T, int> distances,Dictionary<T,T> previous) Dijkstra(IWeightedGraph<T> graph, T start)
        {
            SetWeightedGraph(graph);
            var keys = weightedGraph.GetAdjacencyList().Keys;
            var dist = new Dictionary<T, int>();
            var prev = new Dictionary<T, T>();
            foreach (T key in keys)
            {
                dist.Add(key, int.MaxValue);
                prev.Add(key, default);
            }
            dist[start] = 0;
            var priorityQueue = new Dictionary<T, int>();
            foreach (T key in keys)
            {
                priorityQueue.Add(key, dist[key]);
            }
            while (priorityQueue.Count != 0)
            {
                T current = ExtractMin(priorityQueue);
                foreach (KeyValuePair<T, int> neighbour in weightedGraph.GetAdjacencyList()[current])
                {
                    T v = neighbour.Key;
                    int weight = neighbour.Value;
                    if (dist[v] > dist[current] + weight)
                    {
                        dist[v] = dist[current] + weight;
                        prev[v] = current;
                        priorityQueue[v] = dist[v]; // change priority
                    }

                }
            }
            return (dist,prev);
        }
        public Dictionary<T, int> BellmanFord(IWeightedGraph<T> graph, T start)
        {
            var dist = new Dictionary<T, int>();
            var prev = new Dictionary<T, T>();
            SetWeightedGraph(graph);
            var keys = weightedGraph.GetAdjacencyList().Keys;
            foreach (T key in keys)
            {
                dist.Add(key, int.MaxValue);
                prev.Add(key, default);
            }
            dist[start] = 0;
            var queue = new Queue<T>();
            for (int i = 1; i <= keys.Count - 1; i++) // repeat V-1 times
            {
                // put all the vertexes in a queue starting from Start node
                queue.Enqueue(start);
                foreach (T vertex in keys)
                {
                    if (vertex.CompareTo(start) != 0)
                        queue.Enqueue(vertex);
                }
                //try to relax all the edges of the graph
                while(queue.Count!=0)
                {
                    T current = queue.Dequeue();
                    foreach (var neighbour in weightedGraph.GetAdjacencyList()[current])
                    {
                        //relax
                        if (dist[neighbour.Key] > dist[current] + neighbour.Value) // dist[v] > dist[u] + w(u,v)
                        {
                            dist[neighbour.Key] = dist[current] + neighbour.Value;
                            prev[neighbour.Key] = current;
                        }
                    }
                }
            }
            return dist;
        }
        private T ExtractMin(Dictionary<T, int> priorityQueue)
        {
            T min = priorityQueue.Aggregate((l, r) => l.Value < r.Value ? l : r).Key; // find the key with the min value
            priorityQueue.Remove(min);
            return min;
        }
        public HashSet<T> DetectInfiniteArbitrage(IWeightedGraph<T> graph, T start)
        {
            var dist = new Dictionary<T, int>();
            var prev = new Dictionary<T, T>();
            var infiniteArbitrages = new HashSet<T>();
            var relaxed = new HashSet<T>(); // save the nodes relaxed in the V-th iteration
            SetWeightedGraph(graph);
            var keys = weightedGraph.GetAdjacencyList().Keys;
            foreach (T key in keys)
            {
                dist.Add(key, int.MaxValue);
                prev.Add(key, default);
            }
            dist[start] = 0;
            var queue = new Queue<T>();
            for (int i = 1; i <= keys.Count; i++) // repeat V times
            {
                // in tha last iteration save all the nodes relaxed in a set
                // put all the vertexes in a queue starting from Start node
                queue.Enqueue(start);
                foreach (T vertex in keys)
                {
                    if (vertex.CompareTo(start) != 0)
                        queue.Enqueue(vertex);
                }
                //try to relax all the edges of the graph
                while (queue.Count != 0)
                {
                    T current = queue.Dequeue();
                    foreach (var neighbour in weightedGraph.GetAdjacencyList()[current])
                    {
                        //relax
                        if (dist[neighbour.Key] > dist[current] + neighbour.Value) // dist[v] > dist[u] + w(u,v)
                        {
                            dist[neighbour.Key] = dist[current] + neighbour.Value;
                            prev[neighbour.Key] = current;
                            if(i == keys.Count)
                            {
                                relaxed.Add(neighbour.Key);
                            }
                        }
                    }
                }                
            }
            //queue must be empty by now so we can use the same queue to store the last iteration relaxed nodes
            foreach (T vertex in relaxed)
            {
                queue.Enqueue(vertex);
            }
            // do BFS with the queue and find all the nodes reachable from at least one of the nodes
            while (queue.Count > 0)
            {
                T currentNode = queue.Dequeue();
                foreach (var neighbour in weightedGraph.GetAdjacencyList()[currentNode])
                {
                    infiniteArbitrages.Add(neighbour.Key);
                }
            }
            return infiniteArbitrages;
        }
        public WeightedUndirectedGraph<T> Kruskal(WeightedUndirectedGraph<T> graph)
        {
            var subGraph = new WeightedUndirectedGraph<T>();
            var vertices = graph.GetAdjacencyList().Keys.ToArray();
            var disjointSet = new DisjointSet<T>(vertices);
            var priorityQueue = new PriorityQueue<(T from,T to), int>();
            foreach (T vertex in vertices)
            {
                foreach (KeyValuePair<T,int> kvp in graph.GetAdjacencyList()[vertex])
                {
                    priorityQueue.Enqueue((from: vertex, to: kvp.Key), kvp.Value);
                }
            }
            var sortedEdges = priorityQueue.GetQueue();
            while (sortedEdges.Count != 0)
            {
                var edge = sortedEdges.Dequeue();
                T u = disjointSet.Find(edge.Element.from);
                T v = disjointSet.Find(edge.Element.to);
                if (u.CompareTo(v) != 0)
                {
                    subGraph.AddVertexis(new T[] { edge.Element.from, edge.Element.to });
                    subGraph.AddEdge(edge.Element.from, edge.Element.to, edge.Priority);
                    disjointSet.Union(edge.Element.from, edge.Element.to);
                }
            }
            
            return subGraph;
        }
        public WeightedUndirectedGraph<T> Prim(WeightedUndirectedGraph<T> graph)
        {
            var subGraph = new WeightedUndirectedGraph<T>();
            var cost = new Dictionary<T, int>();
            var parent = new Dictionary<T, T>();
            var priorityQueue = new Dictionary<T, int>();
            var vertices = graph.GetAdjacencyList().Keys.ToArray();
            T initialVertex = vertices.First();
            foreach (T vertex in vertices)
            {
                if (vertex.CompareTo(initialVertex) == 0)
                    cost.Add(vertex, 0);
                else
                    cost.Add(vertex, int.MaxValue);
                parent.Add(vertex, default);
                priorityQueue.Add(vertex, cost[vertex]);
            }
            while (priorityQueue.Count>0)
            {
                T current = ExtractMin(priorityQueue);
                foreach (KeyValuePair<T,int> neighbour in graph.GetAdjacencyList()[current])
                {
                    if (priorityQueue.ContainsKey(neighbour.Key) && cost[neighbour.Key] > neighbour.Value)
                    {
                        cost[neighbour.Key] = neighbour.Value;
                        parent[neighbour.Key] = current;
                        priorityQueue[neighbour.Key] = cost[neighbour.Key];                        
                    }
                }
                
            }
            // build the sub graph from the parent data structure
            subGraph.AddVertexis(vertices);
            foreach (KeyValuePair<T, T> edge in parent)
            {
                if(edge.Value.CompareTo(default)!= 0)
                {
                    int weight = graph.GetAdjacencyList()[edge.Key][edge.Value];
                    subGraph.AddEdge(edge.Key, edge.Value, weight);
                    Console.WriteLine($"Edge from {edge.Key} to {edge.Value} with weight {weight} was added");
                }
            }
            return subGraph;
        }
        public (LinkedList<T> path,int distance) BidirectionalDijkstra(WeightedDirectedGraph<T> graph,T start, T end)
        {
            var reversedGraph = graph.GetReversedGraph();
            var dist = new Dictionary<T, int>();
            var distReverse = new Dictionary<T, int>();
            var vertecies = graph.GetAdjacencyList().Keys;
            var prev = new Dictionary<T, T>();
            var prevReverse = new Dictionary<T, T>();
            var proc = new HashSet<T>();
            var procReverse = new HashSet<T>();
            var priorityQueue = new Dictionary<T, int>();
            var priorityQueueReverse = new Dictionary<T, int>();
            foreach (T vertex in vertecies)
            {
                dist.Add(vertex, int.MaxValue);
                distReverse.Add(vertex, int.MaxValue);
                prev.Add(vertex, default);
                prevReverse.Add(vertex, default);
            }
            dist[start] = 0;
            distReverse[end] = 0;
            PopulateThePriorityQueue(dist, priorityQueue);
            PopulateThePriorityQueue(distReverse, priorityQueueReverse);
            do
            {
                T currentVertex = ExtractMin(priorityQueue);
                Process(currentVertex, graph, dist, prev, proc,priorityQueue);
                if (procReverse.Contains(currentVertex))
                    return ShortestPath(start, dist, prev, proc, end, distReverse, prevReverse, procReverse);
                T currentVertexInReverse = ExtractMin(priorityQueueReverse);
                Process(currentVertexInReverse, reversedGraph, distReverse, prevReverse, procReverse,priorityQueueReverse);
                if (proc.Contains(currentVertexInReverse))
                    return ShortestPath(start, dist, prev, proc, end, distReverse, prevReverse, procReverse);
            } while (true);
        }

        private static void PopulateThePriorityQueue(Dictionary<T, int> dist, Dictionary<T, int> priorityQueue)
        {
            foreach (var item in dist)
            {
                priorityQueue.Add(item.Key, item.Value);
            }
        }        
        private (LinkedList<T> path, int distance) ShortestPath(T start, Dictionary<T, int> dist, Dictionary<T, T> prev, HashSet<T> proc, T end, Dictionary<T, int> distReverse, Dictionary<T, T> prevReverse, HashSet<T> procReverse)
        {
            int distance = int.MaxValue;
            var path = new LinkedList<T>();
            T best = default;
            proc.UnionWith(procReverse); // proc+procReverse
            foreach (T u in proc)
            {
                if(dist[u] + distReverse[u] < distance)
                {
                    best = u;
                    distance = dist[u] + distReverse[u];
                }
            }
            var last = best;
            while (last.CompareTo(start)!=0)
            {
                path.AddFirst(last);
                last = prev[last];
            }
            last = best;
            while (last.CompareTo(end)!=0 && last.CompareTo(default)!=0)
            {
                last = prevReverse[last];
                path.AddLast(last);
            }
            return (path, distance);
        }

        private void Process(T currentVertex, WeightedDirectedGraph<T> graph, Dictionary<T, int> dist, Dictionary<T, T> prev, HashSet<T> proc, Dictionary<T, int> priorityQueue)
        {
            foreach (var neighboor in graph.GetAdjacencyList()[currentVertex])
            {
                if(dist[neighboor.Key] > dist[currentVertex] + neighboor.Value)
                {
                    dist[neighboor.Key] = dist[currentVertex] + neighboor.Value;
                    prev[neighboor.Key] = currentVertex;
                    priorityQueue[neighboor.Key] = dist[currentVertex] + neighboor.Value;
                }
            }
            proc.Add(currentVertex);
        }
    }

}
