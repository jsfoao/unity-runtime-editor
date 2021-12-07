using System;
using Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

public enum Axis
{
    None, X, Y, Z
}

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;
    [SerializeField] private float range = 1f;
    [SerializeField] private float handleSensitivity = 1f;
    private Camera _camera;
    public Axis HoveredAxis;

    private Vector3 xProjection;
    private Vector3 yProjection;
    private Vector3 zProjection;

    private Vector3 _clickedX;
    public bool Selecting;
    public bool Grabbing;
    public Axis GrabbedAxis;

    private EditorController _editorController;
    
    private Vector3 _dragOrigin;
    private Vertex _selectedVertex;

    private void Update()
    {
        if (_editorController.SelectedVertex())
        {
            HoveredAxis = HoveringAxis(_editorController.SelectedVertices[0].Position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (HoveredAxis == Axis.None)
            {
                Vertex newSelectedVertex = TrySelectVertex();
                if (newSelectedVertex != null)
                {
                    Selecting = true;
                    _selectedVertex?.Selectable.OnDeselect();
                    _selectedVertex = newSelectedVertex;
                    _selectedVertex.Selectable.OnSelect();
                    _dragOrigin = Vector3.zero;
                }
                else
                {
                    Selecting = false;
                    if (_selectedVertex != null)
                    {
                        _selectedVertex.Selectable.OnDeselect();
                    }
                }
            }

            if (!Grabbing)
            {
                switch (HoveredAxis)
                {
                    case Axis.X:
                        GrabbedAxis = Axis.X;
                        _dragOrigin = xProjection;
                        Grabbing = true;
                        break;
                    case Axis.Z:
                        GrabbedAxis = Axis.Z;
                        _dragOrigin = zProjection;
                        Grabbing = true;
                        break;
                    case Axis.Y:
                        GrabbedAxis = Axis.Y;
                        _dragOrigin = yProjection;
                        Grabbing = true;
                        break;
                    case Axis.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        if (Grabbing)
        {
            Vertex vertex = EditorController.Instance.SelectedVertices[0];
            switch (GrabbedAxis)
            {
                case Axis.X:
                    vertex.Position += xProjection - _dragOrigin;
                    break;
                case Axis.Y:
                    vertex.Position += yProjection - _dragOrigin;
                    break;
                case Axis.Z:
                    vertex.Position += zProjection - _dragOrigin;
                    break;
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            GrabbedAxis = Axis.None;
            Grabbing = false;
        }
    }

    private Vertex TrySelectVertex()
    {
        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        Transform cameraTransform = _camera.transform;
        foreach (Vertex vertex in GraphMesh.Instance.Graph.Vertices)
        {
            Plane vertexPlane = new Plane(vertex.Position, vertex.Position + cameraTransform.up, vertex.Position + cameraTransform.right);
            
            if (vertexPlane.Raycast(mouseRay, out float distanceMouse))
            {
                Vector3 intersectMouse = mouseRay.GetPoint(distanceMouse);
                if ((intersectMouse - vertex.Position).magnitude <= range)
                {
                    return vertex;
                }
            }
        }
        return null;
    }

    private Axis HoveringAxis(Vector3 position)
    {
        Vector3 xPoint = position + Vector3.right;
        Vector3 yPoint = position + Vector3.up;
        Vector3 zPoint = position + Vector3.forward;
        Plane planeXZ = new Plane(position, xPoint, zPoint);
        Plane planeXY = new Plane(position, xPoint, yPoint);

        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);

        Vector3 intersectXZ = IntersectPlaneWithRay(planeXZ, mouseRay);
        Vector3 intersectXY = IntersectPlaneWithRay(planeXY, mouseRay);
        Vector3 relativeXZ = intersectXZ - position;
        Vector3 relativeXY = intersectXY - position;
        
        xProjection = new Vector3(Vector3.Dot(intersectXZ - position, Vector3.right), 0f, 0f);
        zProjection = new Vector3(0f, 0f, Vector3.Dot(intersectXZ - position, Vector3.forward));
        yProjection = new Vector3(0f, Vector3.Dot(intersectXY - position, Vector3.up), 0f);

        if ((xProjection - relativeXZ).magnitude <= handleSensitivity && relativeXZ.magnitude <= GUIRenderer.Instance.HandleActualLength && Mathf.Sign(relativeXZ.x) >= 0)
        {
            return Axis.X;
        }
        if ((zProjection - relativeXZ).magnitude <= handleSensitivity && relativeXZ.magnitude <= GUIRenderer.Instance.HandleActualLength && Mathf.Sign(relativeXZ.z) >= 0)
        {
            return Axis.Z;
        }
        if ((yProjection - relativeXY).magnitude <= handleSensitivity && relativeXY.magnitude <= GUIRenderer.Instance.HandleActualLength && Mathf.Sign(relativeXY.y) >= 0)
        {
            return Axis.Y;
        }
        return Axis.None;
    }
    
    private Vector3 IntersectPlaneWithRay(Plane plane, Ray ray)
    {
        Vector3 intersectionPoint;
        if (!plane.Raycast(ray, out float distance)) return Vector3.zero;
        intersectionPoint = ray.GetPoint(distance);
        return intersectionPoint;
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
        
        if (Camera.main == null) { return; }

        HoveredAxis = Axis.None;
        _camera = Camera.main;
    }

    private void Start()
    {
        _editorController = EditorController.Instance;
    }
}
