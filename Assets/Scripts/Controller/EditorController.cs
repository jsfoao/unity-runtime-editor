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
