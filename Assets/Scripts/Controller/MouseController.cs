using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

public enum Axis
{
    None, X, Y, Z
}

public enum SelectionMode
{
    Singular, Multiple
}

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;
    [SerializeField] private float range = 1f;
    [SerializeField] private float handleSensitivity = 1f;
    private Camera _camera;
    public Axis HoveredAxis;

    private Vector3 _xProjection;
    private Vector3 _yProjection;
    private Vector3 _zProjection;

    private Vector3 _clickedX;
    public bool Selecting;
    public bool Grabbing;
    public Axis GrabbedAxis;

    private EditorController _editorController;
    
    private Vector3 _dragOrigin;

    public SelectionMode SelectionMode;

    private void Update()
    {
        if (_editorController.SelectedVertex())
        {
            HoveredAxis = HoveringAxis(_editorController.SelectedVertices[0].Position);
        }

        // Join two edges
        if (Input.GetKeyDown(KeyCode.F))
        {
            // todo can't add edge when already has (EdgeChecker)
            if (_editorController.TwoSelectedVertex())
            {
                GraphMesh.Instance.Graph.AddEdge(_editorController.SelectedVertices[0], _editorController.SelectedVertices[1]);
            }
        }

        // Delete selected edges
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (!_editorController.SelectedVertex()) { return; }
            _editorController.RemoveSelectedVertices();
            _editorController.DeselectAll();
        }

        // Add edge from edge
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_editorController.SelectedVertex()) { return; }
            _editorController.AddVertexToSelectedVertices();
        }
        
        // Switch between single and multiple selection mode
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SelectionMode = SelectionMode.Multiple;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            SelectionMode = SelectionMode.Singular;
        }
        
        // Vertex Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (HoveredAxis == Axis.None)
            {
                Vertex newSelectedVertex = TrySelectVertex();
                if (newSelectedVertex == null)
                {
                    if (SelectionMode == SelectionMode.Singular)
                    {
                        Selecting = false;
                        _editorController.DeselectAll();
                    }
                    return;
                }
                
                switch (SelectionMode)
                {
                    case SelectionMode.Singular:
                        Selecting = true;
                        _editorController.DeselectAll();
                        newSelectedVertex.Selectable.OnSelect();
                        _dragOrigin = Vector3.zero;
                        break;
                    case SelectionMode.Multiple:
                        Selecting = true;
                        newSelectedVertex.Selectable.OnSelect();
                        _dragOrigin = Vector3.zero;
                        break;
                }
            }

            if (!Grabbing)
            {
                switch (HoveredAxis)
                {
                    case Axis.X:
                        GrabbedAxis = Axis.X;
                        _dragOrigin = _xProjection;
                        Grabbing = true;
                        break;
                    case Axis.Z:
                        GrabbedAxis = Axis.Z;
                        _dragOrigin = _zProjection;
                        Grabbing = true;
                        break;
                    case Axis.Y:
                        GrabbedAxis = Axis.Y;
                        _dragOrigin = _yProjection;
                        Grabbing = true;
                        break;
                    case Axis.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // Behaviour when grabbing
        if (Grabbing)
        {
            foreach (Vertex vertex in EditorController.Instance.SelectedVertices)
            {
                switch (GrabbedAxis)
                {
                    case Axis.X:
                        vertex.Position += _xProjection - _dragOrigin;
                        break;
                    case Axis.Y:
                        vertex.Position += _yProjection - _dragOrigin;
                        break;
                    case Axis.Z:
                        vertex.Position += _zProjection - _dragOrigin;
                        break;
                }
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
        
        _xProjection = new Vector3(Vector3.Dot(intersectXZ - position, Vector3.right), 0f, 0f);
        _zProjection = new Vector3(0f, 0f, Vector3.Dot(intersectXZ - position, Vector3.forward));
        _yProjection = new Vector3(0f, Vector3.Dot(intersectXY - position, Vector3.up), 0f);

        if ((_xProjection - relativeXZ).magnitude <= handleSensitivity && relativeXZ.magnitude <= GUIRenderer.Instance.HandleLength && Mathf.Sign(relativeXZ.x) >= 0)
        {
            return Axis.X;
        }
        if ((_zProjection - relativeXZ).magnitude <= handleSensitivity && relativeXZ.magnitude <= GUIRenderer.Instance.HandleLength && Mathf.Sign(relativeXZ.z) >= 0)
        {
            return Axis.Z;
        }
        if ((_yProjection - relativeXY).magnitude <= handleSensitivity && relativeXY.magnitude <= GUIRenderer.Instance.HandleLength && Mathf.Sign(relativeXY.y) >= 0)
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
        SelectionMode = SelectionMode.Singular;
    }

    private void Start()
    {
        _editorController = EditorController.Instance;
    }
}
