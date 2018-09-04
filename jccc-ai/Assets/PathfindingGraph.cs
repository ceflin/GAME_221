using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{

    public abstract class Pathfinder
    {
        protected PathfindingGraph graph;
        public Pathfinder(PathfindingGraph graph)
        {
            this.graph = graph;
        }

        public List<PathfindingConnection> Pathfind(PathfindingNode from, PathfindingNode to, out List<PathfindingNode> nodesVisited)
        {
            if (!graph.connectionsByNode.ContainsKey(from) || !graph.connectionsByNode.ContainsKey(to)) throw new Exception("Invalid pathfinding operation for this graph");

            return DoPathfind(from, to, out nodesVisited);  
        }

        protected abstract List<PathfindingConnection> DoPathfind(PathfindingNode from, PathfindingNode to, out List<PathfindingNode> nodesVisited);
    }

    public enum PathfindingStrategy
    {
        Dijkstra,
        AStarEuclideanUnmodified,
        AStarEuclideanUnderestimating,
        AStarEuclideanOverestimating,
        AStarManhattan,
        AStarZero
    }

    public class PathfinderFactory
    {
        private static PathfinderFactory instance = new PathfinderFactory();
        private PathfinderFactory() { }
        public static PathfinderFactory Instance { get { return instance; } }

        public Pathfinder GetPathfinder(PathfindingGraph graph, PathfindingStrategy strategy)
        {
            switch (strategy)
            {
                case PathfindingStrategy.Dijkstra:
                    return new DijkstraPathfinder(graph);
                case PathfindingStrategy.AStarEuclideanUnmodified:
                    return new AStarPathfinder(graph, 1f, new EuclideanHeuristic());
                case PathfindingStrategy.AStarEuclideanUnderestimating:
                    return new AStarPathfinder(graph, 0.75f, new EuclideanHeuristic());
                case PathfindingStrategy.AStarEuclideanOverestimating:
                    return new AStarPathfinder(graph, 1.5f, new EuclideanHeuristic());
                case PathfindingStrategy.AStarManhattan:
                    return new AStarPathfinder(graph, 1.0f, new ManhattanHeuristic());
                case PathfindingStrategy.AStarZero:
                    return new AStarPathfinder(graph, 0.0f, new ZeroHeuristic());
            }
            return null;
        }

    }

    public class DijkstraPathfinder : Pathfinder
    {
        public DijkstraPathfinder(PathfindingGraph graph)
            :base(graph)
        {

        }

        internal class ListEntry
        {
            public PathfindingNode node = null;
            public PathfindingNode origin = null;
            public float costSoFar = 0;
            public PathfindingConnection connection = null;

            public ListEntry(PathfindingNode node, PathfindingNode origin, float costSoFar, PathfindingConnection connection)
            {
                this.node = node;
                this.origin = origin;
                this.costSoFar = costSoFar;
                this.connection = connection;
            }
        }

        protected override List<PathfindingConnection> DoPathfind(PathfindingNode from, PathfindingNode to, out List<PathfindingNode> nodesVisited)
        {
            nodesVisited = new List<PathfindingNode>();
            if (from == to) return new List<PathfindingConnection>();

            // Two lists, with costs
            Dictionary<PathfindingNode, ListEntry> open = new Dictionary<PathfindingNode, ListEntry>();
            Dictionary<PathfindingNode, ListEntry> closed = new Dictionary<PathfindingNode, ListEntry>();

            // Start with the first node
            open.Add(from, new ListEntry(from, null, 0, null));

            while (!closed.ContainsKey(to))
            {
                PathfindingNode candidate = null;
                foreach (PathfindingNode openNode in open.Keys)
                {
                    if (candidate == null || open[openNode].costSoFar < open[candidate].costSoFar) candidate = openNode;
                }
                if (candidate == null) return null;     // No candidate? No path

                nodesVisited.Add(candidate);
                foreach (PathfindingConnection connection in graph.connectionsByNode[candidate])
                {
                    PathfindingNode other = connection.to;
                    if (!closed.ContainsKey(other))
                    {
                        if (open.ContainsKey(other))
                        {
                            // Replace if candidate cost plus connection cost is less than cost so far
                            if (open[candidate].costSoFar + connection.cost < open[other].costSoFar)
                            {
                                open[other].origin = candidate;
                                open[other].costSoFar = open[candidate].costSoFar + connection.cost;
                                open[other].connection = connection;
                            }
                        }
                        else
                        {
                            open.Add(other, new ListEntry(other, candidate, open[candidate].costSoFar + connection.cost, connection));
                        }
                    }
                }
                closed.Add(candidate, open[candidate]);
                open.Remove(candidate);

            }

            // Retrieve the path as a list of connections; find the last node in the closed list
            ListEntry lastEntry = closed[to];
            List<PathfindingConnection> connections = new List<PathfindingConnection>();
            while (lastEntry.connection.from != from)
            {
                connections.Add(lastEntry.connection);
                lastEntry = closed[lastEntry.connection.from];
            }
            connections.Add(lastEntry.connection);
            connections.Reverse();
            return connections;
        }
    }

    public abstract class Heuristic
    {
        public abstract float Estimate(PathfindingNode source, PathfindingNode goal);
    }

    public class EuclideanHeuristic : Heuristic
    {
        public override float Estimate(PathfindingNode source, PathfindingNode goal)
        {
            return Vector3.Distance(source.characteristicPoint, goal.characteristicPoint);
        }
    }

    public class ManhattanHeuristic : Heuristic
    {
        public override float Estimate(PathfindingNode source, PathfindingNode goal)
        {
            return Mathf.Abs(source.characteristicPoint.x - goal.characteristicPoint.x)
                + Mathf.Abs(source.characteristicPoint.y - goal.characteristicPoint.y)
                + Mathf.Abs(source.characteristicPoint.z - goal.characteristicPoint.z);
        }
    }

    public class ZeroHeuristic : Heuristic
    {
        public override float Estimate(PathfindingNode source, PathfindingNode goal)
        {
            return 0f;
        }
    }

    public class AStarPathfinder : Pathfinder
    {
        private float heuristicFactor = 1.0f;
        private Heuristic heuristic;
        public AStarPathfinder(PathfindingGraph graph, float heuristicFactor, Heuristic heuristic)
            : base(graph)
        {
            this.heuristicFactor = heuristicFactor;
            this.heuristic = heuristic;
        }

        internal class ListEntry
        {
            public PathfindingNode node = null;
            public PathfindingNode origin = null;
            public float costSoFar = 0;
            public PathfindingConnection connection = null;
            public float heuristicValue = 0;

            public float estimatedCost { get { return costSoFar + heuristicValue; } }
            public ListEntry(PathfindingNode node, PathfindingNode origin, float costSoFar, float heuristicValue, PathfindingConnection connection)
            {
                this.node = node;
                this.origin = origin;
                this.costSoFar = costSoFar;
                this.connection = connection;
                this.heuristicValue = heuristicValue;
            }
        }

        protected override List<PathfindingConnection> DoPathfind(PathfindingNode from, PathfindingNode to, out List<PathfindingNode> nodesVisited)
        {
            nodesVisited = new List<PathfindingNode>();
            if (from == to) return new List<PathfindingConnection>();

            // Two lists, with costs
            Dictionary<PathfindingNode, ListEntry> open = new Dictionary<PathfindingNode, ListEntry>();
            Dictionary<PathfindingNode, ListEntry> closed = new Dictionary<PathfindingNode, ListEntry>();

            // Start with the first node
            open.Add(from, new ListEntry(from, null, 0, 0, null));

            while (!closed.ContainsKey(to))
            {
                PathfindingNode candidate = null;
                foreach (PathfindingNode openNode in open.Keys)
                {
                    if (candidate == null || open[openNode].estimatedCost < open[candidate].estimatedCost) candidate = openNode;
                }
                if (candidate == null) return null;     // No candidate? No path
                nodesVisited.Add(candidate);
                foreach (PathfindingConnection connection in graph.connectionsByNode[candidate])
                {
                    PathfindingNode other = connection.to;
                    if (!closed.ContainsKey(other))
                    {
                        if (open.ContainsKey(other))
                        {
                            // Replace if candidate cost plus connection cost is less than cost so far
                            if (open[candidate].costSoFar + connection.cost < open[other].costSoFar)
                            {
                                open[other].origin = candidate;
                                open[other].costSoFar = open[candidate].costSoFar + connection.cost;
                                open[other].connection = connection;
                            }
                        }
                        else
                        {
                            open.Add(other, new ListEntry(other, candidate, open[candidate].costSoFar + connection.cost, heuristic.Estimate(other, to) * heuristicFactor, connection));
                        }
                    }
                    else
                    {
                        // Have to check anyway!
                        if (open[candidate].costSoFar + connection.cost < closed[other].costSoFar)
                        {
                            closed[other].origin = candidate;
                            closed[other].costSoFar = open[candidate].costSoFar + connection.cost;
                            closed[other].connection = connection;
                            open.Add(other, closed[other]);
                            closed.Remove(other);
                        }
                    }
                }
                closed.Add(candidate, open[candidate]);
                open.Remove(candidate);

            }

            // Retrieve the path as a list of connections; find the last node in the closed list
            List<PathfindingConnection> connections = new List<PathfindingConnection>();
            ListEntry lastEntry = closed[to];



            while (lastEntry.origin != from)
            {
                connections.Add(lastEntry.connection);
                lastEntry = closed[lastEntry.origin];
            }
            connections.Add(lastEntry.connection);
            connections.Reverse();
            return connections;
        }
    }

    public class PathfindingGraph
    {
        public Dictionary<PathfindingNode, List<PathfindingConnection>> connectionsByNode = new Dictionary<PathfindingNode, List<PathfindingConnection>>();

        public void AddDirectional(PathfindingNode from, PathfindingNode to, float cost)
        {
            if (!connectionsByNode.ContainsKey(from)) connectionsByNode.Add(from, new List<PathfindingConnection>());
            connectionsByNode[from].Add(new PathfindingConnection(from, to, cost));
        }

        public void AddBidirectional(PathfindingNode from, PathfindingNode to, float cost)
        {
            AddDirectional(from, to, cost);
            AddDirectional(to, from, cost);
        }

    }

    public class PathfindingNode
    {
        public Vector3 characteristicPoint;
        public int id;
        private static int ID = 0;

        public PathfindingNode(Vector3 characteristicPoint)
        {
            this.characteristicPoint = characteristicPoint;
            this.id = ID++;
        }
        
    }

    public class PathfindingConnection
    {
        public PathfindingNode from;
        public PathfindingNode to;
        public float cost;

        public PathfindingConnection(PathfindingNode from, PathfindingNode to, float cost)
        {
            this.from = from;
            this.to = to;
            this.cost = cost;
        }
    }
}
