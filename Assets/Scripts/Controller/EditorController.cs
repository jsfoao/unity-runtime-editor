using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public static EditorController Instance;
    public List<Vertex> SelectedVertices;

    public bool SelectedVertex()
    {
        return SelectedVertices.Count != 0;
    }

    private bool TwoSelectedVertex()
    {
        return SelectedVertices.Count == 2;
    }

    public void RemoveSelectedVertices()
    {
        InputHandler.Instance.CommandHandler.ExecuteCommand(new DeleteVertexCommand());
    }

    public void AddVertexToSelectedVertices()
    {
        InputHandler.Instance.CommandHandler.ExecuteCommand(new CreateVertexCommand());
    }
    
    public void SelectAll()
    {
        if (MouseController.Instance.SelectionMode != SelectionMode.Multiple) { return; }
        MouseController.Instance.GrabState = GrabState.Idling;
        
        foreach (Vertex vertex in GraphManager.Instance.Graph.Vertices)
        {
            vertex.Selectable.OnSelect();
        }
    }
    
    public void DeselectAll()
    {
        foreach (Vertex vertex in SelectedVertices)
        {
            vertex.Selectable.OnDeselect();
        }
        SelectedVertices.Clear();
    }

    public void JoinTwoVertices()
    {
        if (TwoSelectedVertex())
        {
            GraphManager.Instance.Graph.AddEdge(SelectedVertices[0], SelectedVertices[1]);
        }
    }

    public void SeparateTwoVertices()
    {
        if (!TwoSelectedVertex()) { return; }
        GraphManager.Instance.Graph.RemoveEdge(SelectedVertices[0], SelectedVertices[1]);
    }

    public void SetSingleSelectionMode()
    {
        MouseController.Instance.SelectionMode = SelectionMode.Singular;
    }
    
    public void SetMultipleSelectionMode()
    {
        MouseController.Instance.SelectionMode = SelectionMode.Multiple;
    }

    public void AddVertexOnOrigin()
    {
        GraphManager.Instance.Graph.AddVertex(new Vertex(Vector3.zero));
    }
    
    public void Undo()
    {
        InputHandler.Instance.CommandHandler.Undo();
    }
    
    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
        
        SelectedVertices = new List<Vertex>();
    }
}
