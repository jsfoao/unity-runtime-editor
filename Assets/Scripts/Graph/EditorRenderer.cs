using UnityEngine;

[RequireComponent(typeof(GraphManager))]
public class EditorRenderer : MonoBehaviour
{
    private GraphManager _graphManager;

    private void Update()
    {
        if (_graphManager.Graph.Vertices.Length != 0)
        {
            _graphManager.Graph.Renderer.Render();
        }
    }

    private void Awake()
    {
        _graphManager = GetComponent<GraphManager>();
    }
}
