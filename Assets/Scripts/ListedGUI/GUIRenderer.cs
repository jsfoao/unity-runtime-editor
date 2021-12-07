using Drawing;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GUIRenderer : MonoBehaviour
{
    public static GUIRenderer Instance;
    
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
    
    // GUI Labels
    private GUIListedLabel _guiGraphLabel;
    private GUIListedLabel _guiMouseLabel;
    
    private void HandleAxisRendering()
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
    
    void OnGUI()
    {
        _guiGraphLabel.Items["Vertices"].Value = GraphMesh.Instance.Graph.Order.ToString();
        _guiGraphLabel.Items["Edges"].Value = GraphMesh.Instance.Graph.Size.ToString();
        _guiGraphLabel.Draw();

        _guiMouseLabel.Items["Selecting"].Value = MouseController.Instance.Selecting.ToString();
        _guiMouseLabel.Items["Grabbing"].Value = MouseController.Instance.Grabbing.ToString();
        _guiMouseLabel.Items["GrabbedAxis"].Value = MouseController.Instance.GrabbedAxis.ToString();
        _guiMouseLabel.Items["HoveredAxis"].Value = MouseController.Instance.HoveredAxis.ToString();
        _guiMouseLabel.Draw();
        
        HandleAxisRendering();
    }
    
    private void SetupGUI()
    {
        // Mesh GUI
        _guiGraphLabel = new GUIListedLabel(new Vector2(20, 20), "Custom Mesh");
        _guiGraphLabel.CreateItem("Vertices");
        _guiGraphLabel.CreateItem("Edges");
        
        // Mouse GUI
        _guiMouseLabel = new GUIListedLabel(new Vector2(20, 100), "Mouse State");
        _guiMouseLabel.CreateItem("Selecting");
        _guiMouseLabel.CreateItem("Grabbing");
        _guiMouseLabel.CreateItem("GrabbedAxis");
        _guiMouseLabel.CreateItem("HoveredAxis");
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
        
        SetupGUI();

        _xColor = Color.red;
        _yColor = Color.green;
        _zColor = Color.blue;
        _xSelectedColor = new Color(1f, 0.5f, 0.5f, 1);
        _ySelectedColor = new Color(0.5f, 1f, 0.5f, 1);
        _zSelectedColor = new Color(0.5f, 0.5f, 1f, 1);
    }
}
