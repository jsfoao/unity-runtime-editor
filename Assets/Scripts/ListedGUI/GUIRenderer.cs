using Drawing;
using UnityEngine;

public class GUIRenderer : MonoBehaviour
{
    public static GUIRenderer Instance;
    private GUIListedLabel _guiGraphLabel;
    private GUIListedLabel _guiMouseLabel;
    [SerializeField] public float HandlesLength;
    [SerializeField] private float _arrowheadSize;
    void OnGUI()
    {
        _guiGraphLabel.Items["Vertices"].Value = GraphMesh.Instance.Graph.Order.ToString();
        _guiGraphLabel.Items["Edges"].Value = GraphMesh.Instance.Graph.Size.ToString();
        _guiGraphLabel.Draw();

        _guiMouseLabel.Items["Grabbing"].Value = MouseController.Instance.Grabbing.ToString();
        _guiMouseLabel.Items["GrabbedAxis"].Value = MouseController.Instance.GrabbedAxis.ToString();
        _guiMouseLabel.Items["HoveredAxis"].Value = MouseController.Instance.HoveredAxis.ToString();
        _guiMouseLabel.Draw();
        
        DrawHandles();
    }
    
    private void SetupGUI()
    {
        // Mesh GUI
        _guiGraphLabel = new GUIListedLabel(new Vector2(20, 20), "Custom Mesh");
        _guiGraphLabel.CreateItem("Vertices");
        _guiGraphLabel.CreateItem("Edges");
        
        // Mouse GUI
        _guiMouseLabel = new GUIListedLabel(new Vector2(20, 100), "Mouse State");
        _guiMouseLabel.CreateItem("Grabbing");
        _guiMouseLabel.CreateItem("GrabbedAxis");
        _guiMouseLabel.CreateItem("HoveredAxis");
    }

    public void DrawHandles()
    {
        if (EditorController.Instance.SelectedVertices.Count == 0) { return; }

        Vector3 position = EditorController.Instance.SelectedVertices[0].Position;
        Draw.ingame.Line(position, position + Vector3.right * HandlesLength, Color.red);
        Draw.ingame.Line(position, position + Vector3.up * HandlesLength, Color.green);
        Draw.ingame.Line(position, position + Vector3.forward * HandlesLength, Color.blue);
        
        Draw.ingame.SolidBox(position + Vector3.right * HandlesLength, _arrowheadSize, Color.red);
        Draw.ingame.SolidBox(position + Vector3.up * HandlesLength, _arrowheadSize, Color.green);
        Draw.ingame.SolidBox(position + Vector3.forward * HandlesLength, _arrowheadSize, Color.blue);
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
