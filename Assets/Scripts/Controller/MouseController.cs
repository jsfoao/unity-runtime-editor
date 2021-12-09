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

public enum GrabState
{
    Disabled, Idling, Grabbing
}

public enum SelectionState
{
    Deselected, Selected
}

public class MouseController : MonoBehaviour
{
    public static MouseController Instance; 
    [Tooltip("Minimum distance to vertex to select")] 
    [SerializeField] [Range(0f, 1f)]
    private float range;
    [Tooltip("Minimum distance to handle to select")] 
    [SerializeField] [Range(0f, 1f)]
    private float handleRange;
    private Camera _camera;

    private Vector3 _xProjection;
    private Vector3 _yProjection;
    private Vector3 _zProjection;
    
    private EditorController _editorController;
    
    private Vector3 _dragOrigin;
    
    [NonSerialized] public Axis HoveredAxis;
    [NonSerialized] public Axis GrabbedAxis;
    [NonSerialized] public SelectionMode SelectionMode;
    [NonSerialized] public GrabState GrabState;
    [NonSerialized] public SelectionState SelectionState;

    private void Update()
    {
        if (_editorController.SelectedVertex())
        {
            HoveredAxis = Geometry.HoveringAxis(_camera, _editorController.SelectedVertices[0].Position, handleRange,
                out _xProjection, 
                out _yProjection,
                out _zProjection);
        }

        if (GrabState == GrabState.Grabbing)
        {
            OnClickHover();
        }
    }

    #region Events
    public void OnClickDown()
    {
        if (HoveredAxis == Axis.None)
        {
            Vertex newSelectedVertex = Geometry.VertexIntersectMouseRay(Camera.main, range);
            if (newSelectedVertex == null)
            {
                if (SelectionMode == SelectionMode.Singular) { OnDeselect(); }
                return;
            }
                
            switch (SelectionMode)
            {
                case SelectionMode.Singular:
                    OnSelect(newSelectedVertex, false);
                    GrabState = GrabState.Idling;
                    break;
                case SelectionMode.Multiple:
                    OnSelect(newSelectedVertex, true);
                    GrabState = GrabState.Idling;
                    break;
            }
        }

        if (GrabState == GrabState.Idling)
        {
            switch (HoveredAxis)
            {
                case Axis.X:
                    GrabbedAxis = Axis.X;
                    _dragOrigin = _xProjection;
                    GrabState = GrabState.Grabbing;
                    OnGrab();
                    break;
                case Axis.Z:
                    GrabbedAxis = Axis.Z;
                    _dragOrigin = _zProjection;
                    GrabState = GrabState.Grabbing;
                    OnGrab();
                    break;
                case Axis.Y:
                    GrabbedAxis = Axis.Y;
                    _dragOrigin = _yProjection;
                    GrabState = GrabState.Grabbing;
                    OnGrab();
                    break;
                case Axis.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void OnClickUp()
    {
        if (GrabState == GrabState.Grabbing)
        {
            GrabbedAxis = Axis.None;
            GrabState = GrabState.Idling;
        }
    }

    private void OnClickHover()
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
    #endregion

    #region VertexSelection
    private void OnSelect(Vertex vertex, bool multiple)
    {
        if (!multiple) { _editorController.DeselectAll(); }
        vertex.Selectable.OnSelect();
        _dragOrigin = Vector3.zero;
        SelectionState = SelectionState.Selected;
    }

    private void OnDeselect()
    {
        SelectionState = SelectionState.Deselected;
        GrabState = GrabState.Disabled;
        _editorController.DeselectAll();
    }

    private void OnGrab()
    {
        if (!_editorController.SelectedVertex()) { return; }

        Command recordPosition = new VertexPositionCommand(_editorController.SelectedVertices);
        InputHandler.Instance.CommandHandler.ExecuteCommand(recordPosition);
    }

    public void ResetStates()
    {
        SelectionState = SelectionState.Deselected;
        GrabState = GrabState.Disabled;
        HoveredAxis = Axis.None;
    }
    #endregion
    
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
        
        ResetStates();
    }

    private void Start()
    {
        _editorController = EditorController.Instance;
    }
}
