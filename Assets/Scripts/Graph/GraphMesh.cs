using UnityEngine;

public class GraphMesh : MonoBehaviour
{
    public static GraphMesh Instance;
    public Graph Graph;
    private GUIListedLabel _guiMesh;
    private GUIListedLabel _guiVertex;
    
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
        
        Graph = new Graph();
        Graph.AddVertex(new Vertex(Vector3.zero));
    }
}
