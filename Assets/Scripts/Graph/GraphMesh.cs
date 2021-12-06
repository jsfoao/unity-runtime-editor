using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


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
        Vertex v1 = Graph.AddVertex(new Vertex(Vector3.zero));
        Vertex v2 = Graph.AddConnectedVertex(v1, new Vertex(new Vector3(10, 0, 0)));
        Vertex v3 = Graph.AddConnectedVertex(v1, new Vertex(new Vector3(0, 0, 10)));
        Vertex v4 = Graph.AddConnectedVertex(v3, new Vertex(new Vector3(10, 0, 10)));
        Graph.AddEdge(v2, v4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int index = Random.Range(0, Graph.Vertices.Length);
            Vertex vertex = Graph.Vertices[index];
            Vector3 position = new Vector3(
                Random.Range(vertex.Position.x - 5, vertex.Position.x + 5),
                Random.Range(vertex.Position.y - 5, vertex.Position.y + 5),
                Random.Range(vertex.Position.z - 5, vertex.Position.z + 5)
                );
            Graph.AddConnectedVertex(vertex, new Vertex(position));
        }
    }
}
