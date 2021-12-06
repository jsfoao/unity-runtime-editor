using Drawing;
using UnityEngine;

public class GridRenderer : MonoBehaviour, IRenderer
{
    private EditorGrid _gridInstance;
    public static GridRenderer Instance;
    [SerializeField] private Color gridColor;
    
    public void Render(Vector3 position)
    {
        for (int x = -_gridInstance.Size.x; x <= _gridInstance.Size.x; x++)
        {
            Draw.ingame.Line(
                new Vector3(x * _gridInstance.Offset, 0f, -_gridInstance.Size.y),
                new Vector3(x * _gridInstance.Offset, 0f, _gridInstance.Size.y),
                gridColor);
        }

        for (int z = -_gridInstance.Size.y; z <= _gridInstance.Size.y; z++)
        {
            Draw.ingame.Line(
                new Vector3(-_gridInstance.Size.x, 0f, z * _gridInstance.Offset),
                new Vector3(_gridInstance.Size.x, 0f, z * _gridInstance.Offset),
                gridColor);
        }
    }

    private void OnRenderObject()
    {
        Render(Vector3.zero);
    }

    private void Start()
    {
        _gridInstance = EditorGrid.Instance;
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
    }
}
