using Drawing;
using UnityEngine;

public class EdgeRenderer : IRenderer
{
    private Edge _edge;
    private Color _color;
    private float _size;

    public EdgeRenderer(Edge edge)
    {
        _edge = edge;
        _color = Color.white;
        _size = 0.1f;
    }
    
    public override void Render()
    {
        if (_edge.Source.Position == _edge.Destination.Position) { return; }
        Draw.ingame.WireCylinder(_edge.Source.Position, _edge.Destination.Position, _size, _color);
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
