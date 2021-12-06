using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public static EditorController Instance;
    public List<Vertex> SelectedVertices;
    private void Awake()
    {
        SelectedVertices = new List<Vertex>();
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
    }
}
