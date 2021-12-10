using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class GUIRenderer : MonoBehaviour
{
    private GUIListedLabel _guiGraphLabel;
    private GUIListedLabel _guiVertexLabel;
    private GUIListedLabel _guiMouseLabel;
    private GUIListedLabel _guiCommandLabel;

    
    private void SetupGUILabels()
    {
        // Mesh GUI
        _guiGraphLabel = new GUIListedLabel(new Vector2(20, 20), "Custom Mesh");
        _guiGraphLabel.CreateItem("Vertices");
        _guiGraphLabel.CreateItem("Edges(Broken)");
        
        // Selected Vertex GUI
        _guiVertexLabel = new GUIListedLabel(new Vector2(20, 100), "Selected Vertex");
        _guiVertexLabel.CreateItem("Position");
        _guiVertexLabel.CreateItem("EdgesCount");
        
        // Mouse GUI
        _guiMouseLabel = new GUIListedLabel(new Vector2(20, 600), "Mouse State");
        _guiMouseLabel.CreateItem("SelectionMode");
        _guiMouseLabel.CreateItem("GrabState");
        _guiMouseLabel.CreateItem("SelectionState");
        _guiMouseLabel.CreateItem("GrabbedAxis");
        _guiMouseLabel.CreateItem("HoveredAxis");
        _guiMouseLabel.CreateItem("SelectedVertices");
        
        // Commands GUI
        _guiCommandLabel = new GUIListedLabel(new Vector2(20, 500), "Commands");
        _guiCommandLabel.CreateItem("LastCommand");
        _guiCommandLabel.CreateItem("StackCount");
    }
    void OnGUI()
    {
        #region GUI Labels
        _guiGraphLabel.Items["Vertices"].Value = GraphManager.Instance.Graph.Order.ToString();
        _guiGraphLabel.Items["Edges(Broken)"].Value = GraphManager.Instance.Graph.Size.ToString();
        _guiGraphLabel.Draw();

        if (EditorController.Instance.SelectedVertices.Count != 0)
        {
            _guiVertexLabel.Items["Position"].Value = EditorController.Instance.SelectedVertices[0].Position.ToString();
            _guiVertexLabel.Items["EdgesCount"].Value = EditorController.Instance.SelectedVertices[0].Edges.Count.ToString();   
        }
        else
        {
            _guiVertexLabel.Items["Position"].Value = "None";
            _guiVertexLabel.Items["EdgesCount"].Value = "None";
        }
        _guiVertexLabel.Draw();

        _guiMouseLabel.Items["SelectionMode"].Value = MouseController.Instance.SelectionMode.ToString();
        _guiMouseLabel.Items["GrabState"].Value = MouseController.Instance.GrabState.ToString();
        _guiMouseLabel.Items["SelectionState"].Value = MouseController.Instance.SelectionState.ToString();
        _guiMouseLabel.Items["GrabbedAxis"].Value = MouseController.Instance.GrabbedAxis.ToString();
        _guiMouseLabel.Items["HoveredAxis"].Value = MouseController.Instance.HoveredAxis.ToString();
        _guiMouseLabel.Items["SelectedVertices"].Value = EditorController.Instance.SelectedVertices.Count.ToString();
        _guiMouseLabel.Draw();

        if (InputHandler.Instance.CommandHandler.CommandStack.Count != 0)
        {
            _guiCommandLabel.Items["LastCommand"].Value = InputHandler.Instance.CommandHandler.LastCommand().Type.ToString();
            _guiCommandLabel.Items["StackCount"].Value = InputHandler.Instance.CommandHandler.CommandStack.Count.ToString();
        }
        else
        {
            _guiCommandLabel.Items["LastCommand"].Value = "None";
            _guiCommandLabel.Items["StackCount"].Value = 0.ToString();
        }
        _guiCommandLabel.Draw();
        #endregion
    }
    private void Awake()
    {
        SetupGUILabels();
    }
}
