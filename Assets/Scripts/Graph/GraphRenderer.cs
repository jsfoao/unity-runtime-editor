using Drawing;
using UnityEngine;

[RequireComponent(typeof(GraphMesh))]
public class GraphRenderer : MonoBehaviour, IRenderer
{
    private GraphMesh _graphMesh;

    public void RenderVertices()
    {
        if (_graphMesh.Graph.Vertices.Length == 0) { return; }
        foreach (Vertex vertex in _graphMesh.Graph.Vertices)
        {
            Draw.ingame.WireSphere(
                vertex.Position, 
                .2f,
                vertex.Color);
        }  
    }

    public void RenderEdges()
    {
        foreach (Vertex vertex in _graphMesh.Graph.Vertices)
        {
            if (vertex.Edges.Count == 0) { continue; }
            foreach (Edge edge in vertex.Edges)
            {
                if (edge.Source.Position == edge.Destination.Position)
                {
                    continue;
                }
                
                Draw.ingame.WireCylinder(
                    edge.Source.Position, 
                    edge.Destination.Position, 
                    .1f, Color.white);
            }
        }
    }
    public void Render(Vector3 position)
    {
        RenderVertices();
        RenderEdges();
    }
    
    private void Update()
    {
        Render(Vector3.zero);
    }

    private void Awake()
    {
        _graphMesh = GetComponent<GraphMesh>();
    }
}
