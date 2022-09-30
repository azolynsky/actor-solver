using Newtonsoft.Json;

var graph = new Graph();
List<ActorModel> actors;

using (StreamReader r = new StreamReader("core-data.json"))
{
    string json = r.ReadToEnd();
    actors = JsonConvert.DeserializeObject<List<ActorModel>>(json);
}

foreach (var actor in actors)
{
    var actorNode = new ActorNode(actor.Name);
    graph.addNode(actorNode);

    foreach(var movie in actor.Movies)
    {
        var movieNode = (MovieNode) graph.findNode(movie);
        if (movieNode == null){
            movieNode = new MovieNode(movie);
            graph.addNode(movieNode);
        }

        graph.addEdge(actorNode, movieNode);
    }
}

Console.WriteLine("Enter an actor's name");
string fromInput = Console.ReadLine();
var from = graph.findNode(fromInput);

Console.WriteLine("Enter another actor's name");
string toInput = Console.ReadLine();
var to = graph.findNode(toInput);

Console.WriteLine(string.Join(", ", graph.findShortestPath(from, to).Select(n => n.name)));

class ActorModel
{
    public string Name { get; set; }
    public string[] Movies { get; set; }
}

class Graph
{
    List<GraphNode> nodes;

    public Graph()
    {
        this.nodes = new List<GraphNode>();
    }

    public void addNode(GraphNode node)
    {
        this.nodes.Add(node);
    }

    public void addNeighbors(GraphNode node, List<GraphNode> neighbors)
    {
        node.addNeighbors(neighbors);
    }

    public GraphNode? findNode(string name)
    {
        return this.nodes.Find(n => n.name == name);
    }

    private void addEdge(GraphNode from, GraphNode to)
    {
        from.addNeighbor(to);
        to.addNeighbor(from);
    }

    public void addEdge(MovieNode from, ActorNode to)
    {
        addEdge((GraphNode)from, (GraphNode)to);
    }

    public void addEdge(ActorNode from, MovieNode to)
    {
        addEdge((GraphNode)from, (GraphNode)to);
    }

    public List<GraphNode> findShortestPath(GraphNode from, GraphNode to)
    {
        var queue = new Queue<GraphNode>();
        var visited = new HashSet<GraphNode>();
        var path = new Dictionary<GraphNode, GraphNode>();

        queue.Enqueue(from);
        visited.Add(from);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == to)
            {
                break;
            }

            foreach (var neighbor in current.getNeighbors())
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    path[neighbor] = current;
                }
            }
        }

        var shortestPath = new List<GraphNode>();
        var currentPath = to;

        while (currentPath != from)
        {
            shortestPath.Add(currentPath);
            currentPath = path[currentPath];
        }

        shortestPath.Add(from);
        shortestPath.Reverse();

        return shortestPath;
    }

    public List<GraphNode> getNodes()
    {
        return this.nodes;
    }

    public List<GraphNode> getNeighbors(GraphNode node)
    {
        return node.getNeighbors();
    }
}

interface IGraphNode
{
    public void addNeighbor(GraphNode node);
    public void addNeighbors(List<GraphNode> nodes);

    public List<GraphNode> getNeighbors();
}

abstract class GraphNode : IGraphNode
{
    public string name;
    public List<GraphEdge> edges;

    public void addNeighbor(GraphNode node)
    {
        this.edges.Add(new GraphEdge(this, node));
    }

    public void addNeighbors(List<GraphNode> nodes)
    {
        foreach (var node in nodes)
        {
            this.addNeighbor(node);
        }
    }

    public GraphNode(string name)
    {
        this.name = name;
        this.edges = new List<GraphEdge>();
    }

    public List<GraphNode> getNeighbors()
    {
        return edges.Select(e => e.to).ToList();
    }
}

class ActorNode : GraphNode
{
    public ActorNode(string name) : base(name) { }

    public void addNeighbor(MovieNode node)
    {
        this.edges.Add(new GraphEdge(this, node));
    }

    public void addNeighbors(List<MovieNode> nodes)
    {
        foreach (var node in nodes)
        {
            this.addNeighbor(node);
        };
    }
}

class MovieNode : GraphNode
{
    public MovieNode(string name) : base(name) { }

    public void addNeighbor(ActorNode node)
    {
        this.edges.Add(new GraphEdge(this, node));
    }

    public void addNeighbors(List<ActorNode> nodes)
    {
        foreach (var node in nodes)
        {
            this.addNeighbor(node);
        };
    }
}

class GraphEdge
{
    GraphNode from;
    public GraphNode to;

    public GraphEdge(GraphNode from, GraphNode to)
    {
        this.from = from;
        this.to = to;
    }
}
