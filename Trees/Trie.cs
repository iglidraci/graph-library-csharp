using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class Trie
    {
        private readonly string[] _patterns;
        private readonly DirectedGraph<char> trie;
        public Trie(string [] patterns)
        {
            _patterns = patterns;
            trie = new DirectedGraph<char>();
            trie.AddVertex('*');// the root is represented by star char
            BuildTheTrie();
        }

        private void BuildTheTrie()
        {
            foreach (string pattern in _patterns)
            {
                char currentNode = '*';
                for (int i = 0; i < pattern.Length; i++)
                {
                    char currentSymbol = pattern[i];
                    if (trie.GetAdjacencyList()[currentNode].Contains(currentSymbol))
                        currentNode = currentSymbol;
                    else
                    {
                        trie.AddVertex(currentSymbol);
                        trie.AddEdge(currentNode, currentSymbol);
                        currentNode = currentSymbol;
                    }
                }
            }
        }
        public DirectedGraph<char> GetTheTrie()
        {
            return trie;
        }
    }
}
