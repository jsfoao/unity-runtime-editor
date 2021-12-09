using UnityEngine;

public class GraphRenderer : IRenderer
{
    private Graph _graph;

    public GraphRenderer(Graph graph)
    {
        _graph = graph;
    }
    public override void Render()
    {
        foreach (Vertex vertex in _graph.Vertices)
        {
            vertex.Renderer.Render();
            foreach (Edge edge in vertex.Edges)
            {
                edge.Renderer.Render();
            }
        }
    }

    public void SetVerticesColor(Color color)
    {
        foreach (Vertex vertex in _graph.Vertices)
        {
            vertex.Renderer.SetColor(color);
        }
    }
    
    public void SetEdgesColor(Color color)
    {
        foreach (Vertex vertex in _graph.Vertices)
        {
            foreach (Edge edge in vertex.Edges)
            {
                edge.Renderer.SetColor(color);
            }
        }
    }
}
