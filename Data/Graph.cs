using System;
using System.Linq;
using System.Collections.Generic;

namespace ContractingPlatform.Data
{   
    public class Graph {
        //private readonly List<string> _nodes;
        public Dictionary<string, List<string>> AdjacencyList {get;set;}
        
        public Graph() {
            //_nodes = new List<string>();
            AdjacencyList = new Dictionary<string, List<string>>();
        }
        
        public void AddEdge(string node1, string node2) { 
            if(!AdjacencyList.ContainsKey(node1))
                AdjacencyList[node1] = new List<string>();
            if(!AdjacencyList.ContainsKey(node2))
                AdjacencyList[node2] = new List<string>();
            AdjacencyList[node1].Add(node2);
            AdjacencyList[node2].Add(node1);
        }
        
        public Stack<string> ShortestPath(string source, string dest) {
            var path = new Dictionary<string, string>();
            var distance = new Dictionary<string, int>();
            foreach(var node in AdjacencyList.Keys) {
                distance[node] = -1;
            }
            distance[source] = 0;
            var q = new Queue<string>();
            q.Enqueue(source);
            while(q.Count > 0) {
                var node = q.Dequeue();
                foreach(var adj in AdjacencyList[node].Where(n => distance[n] == -1)) {
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
}