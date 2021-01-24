using System;
using System.Linq;
using System.Collections.Generic;

namespace ContractingPlatform.Data
{   
    public class Graph {
        //private readonly List<string> _nodes;
        private readonly Dictionary<string, List<string>> _adj;
        
        public Graph() {
            //_nodes = new List<string>();
            _adj = new Dictionary<string, List<string>>();
        }
        
        public void AddEdge(string node1, string node2) { 
            if(!_adj.ContainsKey(node1))
                _adj[node1] = new List<string>();
            if(!_adj.ContainsKey(node2))
                _adj[node2] = new List<string>();
            _adj[node1].Add(node2);
            _adj[node2].Add(node1);
        }
        
        public Stack<string> ShortestPath(string source, string dest) {
            var path = new Dictionary<string, string>();
            var distance = new Dictionary<string, int>();
            foreach(var node in _adj.Keys) {
                distance[node] = -1;
            }
            distance[source] = 0;
            var q = new Queue<string>();
            q.Enqueue(source);
            while(q.Count > 0) {
                var node = q.Dequeue();
                foreach(var adj in _adj[node].Where(n => distance[n] == -1)) {
                    distance[adj] = distance[node] + 1;
                    path[adj] = node;
                    q.Enqueue(adj);
                }
            }
            var res = new Stack<string>();
            var cur = dest;
            var noPath = false;

            while(cur != source && !noPath) {
                res.Push(cur);
                if(path.ContainsKey(cur)) {
                    cur = path[cur];			
                }
                else {
                    noPath = true;				
                }
            }

            res.Push(source);

            if(noPath) {
                res.Clear();
            }

            return res;
        }
    }
        /*
        public class Graph
        {
            public List<Vertex> AdjacencyList { get; set; }
            public Graph()
            {
                AdjacencyList = new List<Vertex>();
            }

            /// <summary>
            /// Adds a new vertex to the graph
            /// </summary>
            /// <param name="newVertex">Name of the new vertex</param>
            /// <returns>Returns the success of the operation</returns>
            public bool AddVertex(string newVertex)
            {
                // We will keep the implementation simple and focus on the concepts
                // Ignore duplicate vertices.
                if (AdjacencyList.Find(v => v.Name == newVertex) != null) return true;

                // Add vertex to the graph
                AdjacencyList.Add(new Vertex(newVertex));
                return true;
            }

            /// <summary>
            /// Adds a new edge between two given vertices in the graph
            /// </summary>
            /// <param name="v1">Name of the first vertex</param>
            /// <param name="v2">Name of the second vertex</param>
            /// <returns>Returns the success of the operation</returns>
            public bool AddAnEdge(string v1, string v2)
            {
                // We will keep the implementation simple and focus on the concepts
                // Do not worry about handling invalid indexes or any other error cases.
                // We will assume all vertices are valid and already exist.

                // Add vertex v2 to the edges of vertex v1
                AdjacencyList.Find(v => v.Name == v1).Edges.Add(v2);

                // Add vertex v1 to the edges of vertex v2
                AdjacencyList.Find(v => v.Name == v2).Edges.Add(v1);

                return true;
            }

            /// <summary>
            /// Iteratively traverse the graph and return an array of vertex names
            /// </summary>
            /// <param name="startVertex">Name for the starting vertex from where the traversal should start.</param>
            /// <returns>Returns array of strings</returns>
            public List<string> BFSTraversal(string startVertex)
            {
                Vertex start = AdjacencyList.Find(v => v.Name == startVertex);
                if (start == null) return null;

                List<string> result = new List<string>();
                HashSet<string> visited = new HashSet<string>();
                Queue<Vertex> queue = new Queue<Vertex>();
                queue.Enqueue(start);

                while(queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    // If current vertex is already visited, move to the next vertex in the queue
                    if(visited.Contains(current.Name)) continue;

                    result.Add(current.Name);
                    visited.Add(current.Name);

                    foreach (var neighbor in current.Edges)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            queue.Enqueue(AdjacencyList.Find(v => v.Name == neighbor));
                        }
                    }
                }

                return result;

            }
        }

        public class Vertex
        {
            /// <summary>
            /// Name of the vertex
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// All the edges connected to the given vertex
            /// </summary>
            public List<string> Edges { get; set; }

            public Vertex() {}

            public Vertex(string name)
            {
                this.Name = name;
                this.Edges = new List<string>();
            }
        }
        */
}