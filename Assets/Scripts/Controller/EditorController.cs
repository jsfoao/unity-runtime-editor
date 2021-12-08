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

    public bool TwoSelectedVertex()
    {
        return SelectedVertices.Count == 2;
    }

    public void RemoveSelectedVertices()
    {
        InputEntity.Instance.CommandHandler.ExecuteCommand(new CreationCommand(SelectedVertices[0]));
        foreach (Vertex vertex in SelectedVertices)
        {
            GraphMesh.Instance.Graph.RemoveVertex(vertex);
        }
        DeselectAll();
    }

    public void AddVertexToSelectedVertices()
    {
        List<Vertex> addedVertex = new List<Vertex>();
        foreach (Vertex vertex in SelectedVertices)
        {
            Vertex newVertex = GraphMesh.Instance.Graph.AddConnectedVertex(vertex, new Vertex(vertex.Position));
            addedVertex.Add(newVertex);
        }
        DeselectAll();
        foreach (Vertex vertex in addedVertex)
        {
            vertex.Selectable.OnSelect();
        }
    }

    public void SelectAll()
    {
        foreach (Vertex vertex in GraphMesh.Instance.Graph.Vertices)
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
