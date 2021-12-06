using Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

public enum Axis
{
    None, X, Y, Z
}
public class MouseController : MonoBehaviour
{
    [SerializeField] private float _range = 1f;
    private Camera _camera;
    private Axis _hoveredAxis;

    private Vector3 _intersectX;
    private Vector3 _intersectY;
    private Vector3 _intersectZ;

    private Vector3 _clickedX;

    private void Update()
    {
        if (EditorController.Instance.SelectedVertices.Count != 0)
        {
            NewCalculateHandles(EditorController.Instance.SelectedVertices[0].Position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_hoveredAxis == Axis.None)
            {
                TrySelection();
            }
            if (_hoveredAxis == Axis.X)
            {
                Vertex vertex = EditorController.Instance.SelectedVertices[0];
                vertex.Position += _intersectX.normalized - new Vector3(0.9f, 0f, 0f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            
        }
    }

    private void TrySelection()
    {
        Ray _mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        Transform cameraTransform = _camera.transform;
        foreach (Vertex vertex in GraphMesh.Instance.Graph.Vertices)
        {
            Plane vertexPlane = new Plane(vertex.Position, vertex.Position + cameraTransform.up, vertex.Position + cameraTransform.right);
            
            if (vertexPlane.Raycast(_mouseRay, out float distanceMouse))
            {
                Vector3 intersectMouse = _mouseRay.GetPoint(distanceMouse);
                
                // Select vertex
                if ((intersectMouse - vertex.Position).magnitude <= _range)
                {
                    vertex.Selectable.OnSelect();
                }
                // Deselect
                else
                {
                    vertex.Selectable.OnDeselect();
                }
            }
        }
    }

    private void NewCalculateHandles(Vector3 position)
    {
        Vector3 xPoint = position + Vector3.right;
        Vector3 yPoint = position + Vector3.up;
        Vector3 zPoint = position + Vector3.forward;
        Plane _planeXZ = new Plane(position, xPoint, zPoint);
        Plane _planeXY = new Plane(position, xPoint, yPoint);
        Plane _planeYZ = new Plane(position, yPoint, zPoint);
        
        Ray _mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        
        // Plane XZ
        if (_planeXZ.Raycast(_mouseRay, out float distanceXZ))
        {
            Vector3 intersectXZ = _mouseRay.GetPoint(distanceXZ);
            Vector3 posToIntersect = intersectXZ - position;
            
            Vector3 xProjection = new Vector3(Vector3.Dot(posToIntersect, Vector3.right), 0f, 0f);
            Vector3 zProjection = new Vector3(0f, 0f,  Vector3.Dot(posToIntersect, Vector3.forward));

            if ((xProjection - posToIntersect).magnitude < 0.1f && posToIntersect.magnitude <= GUIRenderer.Instance.HandlesLength && Mathf.Sign(posToIntersect.x) >= 0)
            {
                OnHoverAxisIn();
                Draw.ingame.Circle(intersectXZ, Vector3.up, 0.3f, Color.yellow);
            }
            else
            {
                OnHoverAxisOut();
            }
            
            Draw.ingame.Circle(intersectXZ, Vector3.up, 0.2f, Color.red);
            Draw.ingame.Line(position, intersectXZ, Color.red);
            
            Draw.ingame.Circle(position + xProjection, Vector3.up, 0.1f, Color.red);
            Draw.ingame.Circle(position + zProjection, Vector3.up, 0.1f, Color.blue);
            
            Draw.ingame.Line(xProjection + position , posToIntersect + position, Color.white);
            Draw.ingame.Line(zProjection + position, posToIntersect + position, Color.white);
        }
    }

    private void OnHoverAxisIn()
    {
        _hoveredAxis = Axis.X;
    }

    private void OnHoverAxisOut()
    { 
        _hoveredAxis = Axis.None;
    }
    
    private void Awake()
    {
        if (Camera.main == null) { return; }

        _hoveredAxis = Axis.None;
        _camera = Camera.main;
    }
}
