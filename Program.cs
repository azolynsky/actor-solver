var graph = new Graph();

var theProducers = new MovieNode("The Producers");
graph.addNode(theProducers);
var bueller = new MovieNode("Ferris Bueller's Day Off");
graph.addNode(bueller);
var warGames = new MovieNode("War Games");
graph.addNode(warGames);

var theBirdcage = new MovieNode("The Birdcage");
graph.addNode(theBirdcage);

var theLionKing = new MovieNode("The Lion King");
graph.addNode(theLionKing);

var mrsDoubtfire = new MovieNode("Mrs. Doubtfire");

var jtt = new ActorNode("Jonathan Taylor Thomas");
graph.addNode(jtt);
var matthewBroderick = new ActorNode("Matthew Broderick");
graph.addNode(matthewBroderick);
var nathanLane = new ActorNode("Nathan Lane");
graph.addNode(nathanLane);
var robinWilliams = new ActorNode("Robin Williams");
graph.addNode(robinWilliams);

graph.addEdge(jtt, theLionKing);
graph.addEdge(matthewBroderick, theLionKing);
graph.addEdge(matthewBroderick, theProducers);
graph.addEdge(matthewBroderick, bueller);
graph.addEdge(matthewBroderick, warGames);
graph.addEdge(nathanLane, theBirdcage);
graph.addEdge(nathanLane, theProducers);
graph.addEdge(nathanLane, theLionKing);
graph.addEdge(robinWilliams, theBirdcage);
graph.addEdge(robinWilliams, mrsDoubtfire);

Console.WriteLine(string.Join(", ", graph.findShortestPath(jtt, robinWilliams).Select(n => n.name)));

class Graph {
    List<GraphNode> nodes;

    public Graph(){
        this.nodes = new List<GraphNode>();
    }

    public void addNode(GraphNode node){
        this.nodes.Add(node);
    }

    public void addNeighbors(GraphNode node, List<GraphNode> neighbors){
        node.addNeighbors(neighbors);
    }

    private void addEdge(GraphNode from, GraphNode to){
        from.addNeighbor(to);
        to.addNeighbor(from);
    }

    public void addEdge(MovieNode from, ActorNode to){
        addEdge((GraphNode)from, (GraphNode)to);
    }

    public void addEdge(ActorNode from, MovieNode to){
        addEdge((GraphNode)from, (GraphNode)to);
    }

    public List<GraphNode> findShortestPath(GraphNode from, GraphNode to){
        var queue = new Queue<GraphNode>();
        var visited = new HashSet<GraphNode>();
        var path = new Dictionary<GraphNode, GraphNode>();

        queue.Enqueue(from);
        visited.Add(from);

        while(queue.Count > 0){
            var current = queue.Dequeue();

            if(current == to){
                break;
            }

            foreach(var neighbor in current.getNeighbors()){
                if(!visited.Contains(neighbor)){
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    path[neighbor] = current;
                }
            }
        }

        var shortestPath = new List<GraphNode>();
        var currentPath = to;

        while(currentPath != from){
            shortestPath.Add(currentPath);
            currentPath = path[currentPath];
        }

        shortestPath.Add(from);
        shortestPath.Reverse();

        return shortestPath;
    }

    public List<GraphNode> getNodes(){
        return this.nodes;
    }

    public List<GraphNode> getNeighbors(GraphNode node) {
        return node.getNeighbors();
    }
}

interface IGraphNode {
    public void addNeighbor(GraphNode node);
    public void addNeighbors(List<GraphNode> nodes);

    public List<GraphNode> getNeighbors();
}

abstract class GraphNode: IGraphNode {
    public string name;
    public List<GraphEdge> edges;

    public void addNeighbor(GraphNode node){
        this.edges.Add(new GraphEdge(this, node));
    }

    public void addNeighbors(List<GraphNode> nodes){
        foreach(var node in nodes){
            this.addNeighbor(node);
        }
    }

    public GraphNode(string name){
        this.name = name;
        this.edges = new List<GraphEdge>();
    }

    public List<GraphNode> getNeighbors() {
        return edges.Select(e => e.to).ToList();
    }
}

class ActorNode : GraphNode {
    public ActorNode(string name) : base(name) {}

    public void addNeighbor(MovieNode node){
        this.edges.Add(new GraphEdge(this, node));
    }

    public void addNeighbors(List<MovieNode> nodes){
        foreach (var node in nodes) {
            this.addNeighbor(node);
        };
    }
}

class MovieNode : GraphNode {
    public MovieNode(string name) : base(name) {}

    public void addNeighbor(ActorNode node){
        this.edges.Add(new GraphEdge(this, node));
    }

    public void addNeighbors(List<ActorNode> nodes){
        foreach (var node in nodes) {
            this.addNeighbor(node);
        };
    }
}

class GraphEdge {
    GraphNode from;
    public GraphNode to;

    public GraphEdge(GraphNode from, GraphNode to){
        this.from = from;
        this.to = to;
    }
}
