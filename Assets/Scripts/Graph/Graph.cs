using System.Collections.Generic;

public class Graph
{
    private List<Vertex> vertices;
    private HashSet<Edge> edges;
    
    public int Order => vertices.Count;
    public int Size => edges.Count;

    public Vertex[] Vertices => vertices.ToArray();

    public Graph()
    {
        vertices = new List<Vertex>();
        edges = new HashSet<Edge>();
    }
        
    public Vertex AddVertex(Vertex vertex)
    {
        vertices.Add(vertex);
        return vertex;
    }

    public void AddEdge(Vertex v1, Vertex v2)
    {
        edges.Add(v1.AddEdge(v2));
        edges.Add(v2.AddEdge(v1));
    }

    public Vertex AddConnectedVertex(Vertex v1, Vertex v2)
    {
        AddVertex(v2);
        AddEdge(v1, v2);
        return v2;
    }
}

public class Edge
{
    private Vertex source;
    private Vertex destination;
    
    public Vertex Source => source;
    public Vertex Destination => destination;

    public Edge(Vertex source, Vertex destination)
    {
        this.source = source;
        this.destination = destination;
    }
}