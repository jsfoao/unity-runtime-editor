using Drawing;
using UnityEngine;

public class VertexRenderer : IRenderer
{
    private Vertex _vertex;
    private Color _color;
    private float _size;

    public VertexRenderer(Vertex vertex)
    {
        _vertex = vertex;
        _color = Color.red;
        _size = 0.2f;
    }

    public override void Render()
    {
        Draw.ingame.WireSphere(_vertex.Position, .2f, _color);
    }

    public void SetColor(Color color)
    {
        _color = color;
    }

    public void SetSize(float size)
    {
        _size = size;
    }
}
