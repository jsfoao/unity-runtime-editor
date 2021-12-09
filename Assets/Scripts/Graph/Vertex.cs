using System;
using System.Collections.Generic;
using UnityEngine;

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
            if (edge.Destination == target)
            {
                Edges.Remove(edge);
                return;
            }
        }
    }

    public List<Vertex> GetConnectedVertices()
    {
        List<Vertex> connectedVertices = new List<Vertex>();
        foreach (Edge edge in Edges)
        {
            connectedVertices.Add(edge.Destination);
        }
        return connectedVertices;
    }

    public void MoveTo(Vector3 position)
    {
        Position = position;
    }
}