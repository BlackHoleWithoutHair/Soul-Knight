using Edgar.Graphs;
using Edgar.Legacy.GeneralAlgorithms.Algorithms.Graphs;
using System.Collections.Generic;

namespace Edgar.Unity
{
    public static class GraphUtilsGrid3D
    {
        private static readonly GraphUtils GraphUtils = new GraphUtils();

        /// <summary>
        /// Returns nodes that are inside a cycle (could be also inside multiple cycles).
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static HashSet<TNode> GetNodesInsideCycle<TNode>(IGraph<TNode> graph)
        {
            // The idea is that we get planar faces of the graph
            // and then each node that is included in at least 2 faces is inside a cycle

            var nodesInsideCycle = new HashSet<TNode>();
            var faces = GraphUtils.GetPlanarFaces(graph);
            var counter = new Dictionary<TNode, int>();

            foreach (var face in faces)
            {
                foreach (var node in face)
                {
                    if (counter.ContainsKey(node))
                    {
                        counter[node] += 1;
                    }
                    else
                    {
                        counter[node] = 1;
                    }
                }
            }

            foreach (var pair in counter)
            {
                var node = pair.Key;
                var count = pair.Value;

                if (count >= 2)
                {
                    nodesInsideCycle.Add(node);
                }
            }

            return nodesInsideCycle;
        }
    }
}