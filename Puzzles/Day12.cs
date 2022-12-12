using System.Buffers;

namespace AdventOfCode2022.Puzzles;

internal class Day12 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day12.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var graph = InitGraph(inputData);

        var startNode = graph.Single(n => n.Value.Marker == 'S').Value;
        var targetNode = graph.Single(n => n.Value.Marker == 'E').Value;

        return (GetShortestPathWithBfs(graph, startNode, targetNode).Count - 1).ToString();
    }

    private static string Part2(string[] inputData)
    {
        var graph = InitGraph(inputData);

        var startNodes = graph.Where(n => new[] { 'S', 'a' }.Contains(n.Value.Marker));
        var targetNode = graph.Single(n => n.Value.Marker == 'E').Value;

        return startNodes.Select(n => GetShortestPathWithBfs(graph, n.Value, targetNode))
            .Where(path => path.Count > 0)
            .Min(path => path.Count - 1).ToString();
    }

    private static Dictionary<(int, int), Node> InitGraph(string[] inputData)
    {
        return inputData
            .SelectMany((s, y) => s.Select((c, x) => new Node(x, y, c)))
            .ToDictionary(node => (node.X, node.Y), node => node);
    }

    private static HashSet<(int, int)> GetShortestPathWithBfs(Dictionary<(int, int), Node> graph, Node startNode, Node targetNode)
    {
        var queue = new Queue<Node>();
        var previous = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            foreach (var nextNode in node.GetAdjacentNodes(graph))
            {
                if (!previous.ContainsKey(nextNode))
                {
                    previous[nextNode] = node;
                    queue.Enqueue(nextNode);
                }
            }
        }

        // There is no path from start node to target node.
        if (!previous.ContainsKey(targetNode))
        {
            return new HashSet<(int, int)>();
        }

        var path = new HashSet<(int, int)>();

        var currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Location);
            currentNode = previous[currentNode];
        }

        path.Add(startNode.Location);
        _ = path.Reverse();

        return path;
    }

    private sealed class Node
    {
        public int X { get; }

        public int Y { get; }

        public int Height { get; }

        public char Marker { get; set; }

        public Node(int x, int y, char marker)
        {
            X = x;
            Y = y;
            Marker = marker;
            Height = Marker switch
            {
                'S' => 'a',
                'E' => 'z',
                _ => Marker
            };
        }

        public (int, int) Location => (X, Y);

        public IEnumerable<Node> GetAdjacentNodes(Dictionary<(int, int), Node> graph)
        {
            foreach (var (x, y) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                var pos = (X + x, Y + y);

                if (graph.TryGetValue(pos, out Node? value) && value.Height <= Height + 1)
                {
                    yield return graph[pos];
                }
            }
        }
    }
}
