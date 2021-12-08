using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Vertex
{
    public Vector3 Position;
    public HashSet<Edge> Edges;
    
    [NonSerialized] public Color Color;

    public SelectableVertex Selectable;

    public Vertex(Vector3 position)
    {
        Position = position;
        Edges = new HashSet<Edge>();
        Color = Color.red;

        Selectable = new SelectableVertex(this);
    }
    public Edge AddEdge(Vertex target)
    {
        Edge edge = new Edge(this, target);
        Edges.Add(edge);
        return edge;
    }

    public void RemoveEdge(Vertex target)
    {
        foreach (Edge edge in Edges)
        {
            if (edge.Destination != target) continue;
            Edges.Remove(edge);
            return;
        }
    }

    public void RemoveAllEdges()
    {
        foreach (Edge edge in Edges)
        {
            edge.Source.RemoveEdge(this);
        }
    }
}