using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Command
{
    public CommandHandler CommandHandler;

    public virtual void Execute() { }

    public virtual void Undo() { }
}

public class MoveCommand : Command
{
    private readonly Dictionary<Vertex, Vector3> _vertexPositions;

    public MoveCommand(List<Vertex> vertices)
    {
        _vertexPositions = new Dictionary<Vertex, Vector3>();
        foreach (Vertex vertex in vertices)
        {
            _vertexPositions.Add(vertex, vertex.Position);
        }
    }
    
    public override void Execute() { }

    public override void Undo()
    {
        foreach (var kvp in _vertexPositions)
        {
            kvp.Key.MoveTo(kvp.Value);
        }
    }
}

public class CreationCommand : Command
{
    public Vertex Vertex;
    public List<Vertex> ConnectedVertices;

    public CreationCommand(Vertex vertex)
    {
        Vertex = vertex;
        ConnectedVertices = vertex.GetConnectedVertices();
    }

    public override void Execute()
    {
    }

    public override void Undo()
    {
        Vertex oldVertex = GraphMesh.Instance.Graph.AddVertex(Vertex);
        foreach (Vertex vertex in ConnectedVertices)
        {
            GraphMesh.Instance.Graph.AddEdge(vertex, oldVertex);
        }
    }
}