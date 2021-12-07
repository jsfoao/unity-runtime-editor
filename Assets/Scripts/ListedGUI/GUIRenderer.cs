using Drawing;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GUIRenderer : MonoBehaviour
{
    public static GUIRenderer Instance;
    private GUIListedLabel _guiGraphLabel;
    private GUIListedLabel _guiMouseLabel;
    [SerializeField] public Vector3 HandleLength;
    [SerializeField] public float HandleActualLength;
    [SerializeField] public Vector3 HandleSize;
    [SerializeField] private float _selectedHandleSize;

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
        
        DrawAllHandles();

        switch (MouseController.Instance.HoveredAxis)
        {
            case Axis.X:
                ResetHandleSizes();
                HandleSize.x = _selectedHandleSize;
                break;
            case Axis.Y:
                ResetHandleSizes();
                HandleSize.y = _selectedHandleSize;
                break;
            case Axis.Z:
                ResetHandleSizes();
                HandleSize.z = _selectedHandleSize;
                break;
            case Axis.None:        
                ResetHandleSizes();
                break;
        }
    }

    private void ResetHandleSizes()
    {
        HandleSize.x = 0.2f;
        HandleSize.y = 0.2f;
        HandleSize.z = 0.2f;
    }

    private void DrawAllHandles()
    {
        if (EditorController.Instance.SelectedVertices.Count == 0) { return; }

        Vector3 position = EditorController.Instance.SelectedVertices[0].Position;
        DrawAxisHandle(position, Vector3.right, HandleSize.x, HandleLength.x, Color.red);
        DrawAxisHandle(position, Vector3.up, HandleSize.y, HandleLength.y, Color.green);
        DrawAxisHandle(position, Vector3.forward, HandleSize.z, HandleLength.z, Color.blue);
    }

    private void DrawAxisHandle(Vector3 position, Vector3 direction, float size, float length, Color color)
    {
        Vector3 edgePosition = position + direction * length;
        Draw.ingame.Line(position, edgePosition, color);
        Draw.ingame.SolidBox(edgePosition, Vector3.one * size, color);
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
    }
}
