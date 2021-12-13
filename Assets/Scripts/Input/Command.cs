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

public class RecordPositionCommand : Command
{
    private readonly Dictionary<Vertex, Vector3> _vertexPositions;
    private readonly List<Vertex> _vertices;

    public RecordPositionCommand(List<Vertex> vertices)
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
        CommandHandler.CommandStack.Push(this);
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
    private Vertex _vertex;
    private List<Vertex> _connectedVertices;

    public override void Execute()
    {
        EditorController editorController = EditorController.Instance;
        if (!editorController.SelectedVertex())
        {
            return;
        }
        _vertex = editorController.SelectedVertices[0];
        _connectedVertices = _vertex.GetConnectedVertices();
        foreach (Vertex vertex in editorController.SelectedVertices)
        {
            GraphManager.Instance.Graph.RemoveVertex(vertex);
        }
        editorController.DeselectAll();
        MouseController.Instance.ResetStates();
        
        Type = CommandType.Delete;
        CommandHandler.CommandStack.Push(this);
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
    private List<Vertex> _vertices;

    public override void Execute()
    {
        EditorController editorController = EditorController.Instance;

        if (!editorController.SelectedVertex()) { return; }
        List<Vertex> addedVertex = new List<Vertex>();
        _vertices = addedVertex;
        
        foreach (Vertex vertex in editorController.SelectedVertices)
        {
            Vertex newVertex = GraphManager.Instance.Graph.AddConnectedVertex(vertex, new Vertex(vertex.Position));
            addedVertex.Add(newVertex);
        }

        editorController.DeselectAll();
        foreach (Vertex vertex in addedVertex)
        {
            vertex.Selectable.OnSelect();
        }
        
        CommandHandler.CommandStack.Push(this);
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