using System.Collections.Generic;
using UnityEngine;

public enum CommandType
{
    None, Position, Create, Delete
}
// TODO Undo System: Commands Example
public class Command
{
    public CommandType Type;
    public CommandHandler CommandHandler;

    public virtual void Execute() { }

    public virtual void Undo() { }
}

public class VertexPositionCommand : Command
{
    private readonly Dictionary<Vertex, Vector3> _vertexPositions;
    private readonly List<Vertex> _vertices;

    public VertexPositionCommand(List<Vertex> vertices)
    {
        _vertices = vertices;
        _vertexPositions = new Dictionary<Vertex, Vector3>();

        // Uncomment for debugging
        Type = CommandType.Position;
    }

    public override void Execute()
    {
        foreach (Vertex vertex in _vertices)
        {
            _vertexPositions.Add(vertex, vertex.Position);
        }
    }

    public override void Undo()
    {
        foreach (var kvp in _vertexPositions)
        {
            kvp.Key.MoveTo(kvp.Value);
        }
    }
}

public class DeleteVertexCommand : Command
{
    private readonly Vertex _vertex;
    private List<Vertex> _connectedVertices;

    public DeleteVertexCommand(Vertex vertex)
    {
        _vertex = vertex;
    }

    public override void Execute()
    {
        _connectedVertices = _vertex.GetConnectedVertices();
        
        // Uncomment for debugging
        Type = CommandType.Delete;
    }

    public override void Undo()
    {
        Vertex oldVertex = GraphMesh.Instance.Graph.AddVertex(_vertex);
        foreach (Vertex vertex in _connectedVertices)
        {
            GraphMesh.Instance.Graph.AddEdge(vertex, oldVertex);
        }
    }
}

public class CreateVertexCommand : Command
{
    private readonly List<Vertex> _vertices;

    public CreateVertexCommand(List<Vertex> vertices)
    {
        _vertices = vertices;
    }

    public override void Execute()
    {
        // Uncomment for debugging
        Type = CommandType.Create;
    }

    public override void Undo()
    {
        foreach (Vertex vertex in _vertices)
        {
            GraphMesh.Instance.Graph.RemoveVertex(vertex);
        }
    }
}