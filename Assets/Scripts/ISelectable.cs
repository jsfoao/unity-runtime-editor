using UnityEngine;

public abstract class ISelectable
{
    public abstract void OnSelect();
    public abstract void OnDeselect();
}

public class SelectableVertex : ISelectable
{
    private Vertex vertex;

    public SelectableVertex(Vertex vertex)
    {
        this.vertex = vertex;
    }
    
    public override void OnSelect()
    {
        Debug.Log($"<b>Selected Vertex</b>: {vertex}");
        Debug.Log($"Position: {vertex.Position}");
        
        EditorController.Instance.SelectedVertices.Add(vertex);
    }

    public override void OnDeselect()
    {
        if (!EditorController.Instance.SelectedVertices.Contains(vertex)) { return; }
        EditorController.Instance.SelectedVertices.Remove(vertex);
    }
}