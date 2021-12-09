using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CommandType
{
    None, Position, Create, Delete
}

public enum BehaviourType
{
    KeyDown, KeyUp, Hold
}

[Serializable]
public class Control
{
    public KeyCode Key;
    public BehaviourType Behaviour;
    public UnityEvent Event;

    public Control(KeyCode key, BehaviourType behaviourType = BehaviourType.KeyDown)
    {
        Key = key;
        Event = new UnityEvent();
        Behaviour = behaviourType;
    }

    public void AddAction(UnityAction call)
    {
        Event.AddListener(call);
    }
}
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
        Vertex oldVertex = GraphManager.Instance.Graph.AddVertex(_vertex);
        foreach (Vertex vertex in _connectedVertices)
        {
            GraphManager.Instance.Graph.AddEdge(vertex, oldVertex);
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
            GraphManager.Instance.Graph.RemoveVertex(vertex);
        }
    }
}