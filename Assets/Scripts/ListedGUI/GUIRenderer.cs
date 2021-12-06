using Drawing;
using UnityEngine;

public class GUIRenderer : MonoBehaviour
{
    public static GUIRenderer Instance;
    private GUIListedLabel _guiGraphLabel;
    [SerializeField] public float HandlesLength;
    [SerializeField] private float _arrowheadSize;
    void OnGUI()
    {
        _guiGraphLabel.Items["Vertices"].Value = GraphMesh.Instance.Graph.Order.ToString();
        _guiGraphLabel.Items["Edges"].Value = GraphMesh.Instance.Graph.Size.ToString();
        _guiGraphLabel.Draw();
        
        DrawHandles();
    }
    
    private void SetupGUI()
    {
        // Mesh GUI
        _guiGraphLabel = new GUIListedLabel(new Vector2(20, 20), "Custom Mesh");
        _guiGraphLabel.CreateItem("Vertices");
        _guiGraphLabel.CreateItem("Edges");
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
