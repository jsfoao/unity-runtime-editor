using Drawing;
using UnityEngine;

public class EditorRenderer : MonoBehaviour
{
    public static EditorRenderer Instance;
    
    [Header("Grid")] [SerializeField] private Color gridColor;
    [SerializeField] private Vector2Int size;
    [SerializeField] private float offset = 1f;
    
    [Header("Handles")]
    [SerializeField] public float HandleLength;
    private Color _xColor;
    private Color _yColor;
    private Color _zColor;
    private Color _xCurrentColor;
    private Color _yCurrentColor;
    private Color _zCurrentColor;
    private Color _xSelectedColor;
    private Color _ySelectedColor;
    private Color _zSelectedColor;

    private void Update()
    {
        RenderGraph();
        RenderGrid();
        RenderHandles();
    }

    private void RenderGraph()
    {
        if (GraphManager.Instance.Graph.Vertices.Length != 0)
        {
            GraphManager.Instance.Graph.Renderer.Render();
        }
    }
    private void RenderGrid()
    {
        for (int x = -size.x; x <= size.x; x++)
        {
            Draw.ingame.Line(
                new Vector3(x * offset, 0f, -size.y),
                new Vector3(x * offset, 0f, size.y), 
                gridColor);
        }

        for (int z = -size.y; z <= size.y; z++)
        {
            Draw.ingame.Line(
                new Vector3(-size.x, 0f, z * offset),
                new Vector3(size.x, 0f, z * offset), 
                gridColor);
        }
    }
    private void RenderHandles()
    {
        if (EditorController.Instance.SelectedVertices.Count == 0) { return; }

        Vector3 position = EditorController.Instance.SelectedVertices[0].Position;
        DrawAxisHandle(position, Vector3.right, _xCurrentColor);
        DrawAxisHandle(position, Vector3.up, _yCurrentColor);
        DrawAxisHandle(position, Vector3.forward, _zCurrentColor);
        
        switch (MouseController.Instance.HoveredAxis)
        {
            case Axis.X:
                ResetHandles();
                _xCurrentColor = _xSelectedColor;
                break;
            case Axis.Y:
                ResetHandles();
                _yCurrentColor = _ySelectedColor;
                break;
            case Axis.Z:
                ResetHandles();
                _zCurrentColor = _zSelectedColor;
                break;
            case Axis.None:        
                ResetHandles();
                break;
        }
    }
    private void ResetHandles()
    {
        _xCurrentColor = _xColor;
        _yCurrentColor = _yColor;
        _zCurrentColor = _zColor;
    }
    private void DrawAxisHandle(Vector3 position, Vector3 direction, Color color)
    {
        Vector3 edgePosition = position + direction * HandleLength;
        Draw.ingame.Line(position, edgePosition, color);
        Draw.ingame.SolidBox(edgePosition, Vector3.one * 0.2f, color);
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
        
        _xColor = Color.red;
        _yColor = Color.green;
        _zColor = Color.blue;
        _xSelectedColor = new Color(1f, 0.5f, 0.5f, 1);
        _ySelectedColor = new Color(0.5f, 1f, 0.5f, 1);
        _zSelectedColor = new Color(0.5f, 0.5f, 1f, 1);
    }
}
