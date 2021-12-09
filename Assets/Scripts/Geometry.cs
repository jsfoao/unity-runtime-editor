using UnityEngine;

public static class Geometry
{
    public static Vertex VertexIntersectMouseRay(Camera camera, float range = 0.25f)
    {
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        Transform cameraTransform = camera.transform;
        foreach (Vertex vertex in GraphManager.Instance.Graph.Vertices)
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
    
    public static Axis HoveringAxis(Camera camera, Vector3 position, float handleRange, out Vector3 xProjection, out Vector3 yProjection, out Vector3 zProjection)
    {
        Vector3 xPoint = position + Vector3.right;
        Vector3 yPoint = position + Vector3.up;
        Vector3 zPoint = position + Vector3.forward;
        Plane planeXZ = new Plane(position, xPoint, zPoint);
        Plane planeXY = new Plane(position, xPoint, yPoint);

        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        Vector3 intersectXZ = IntersectPlaneWithRay(planeXZ, mouseRay);
        Vector3 intersectXY = IntersectPlaneWithRay(planeXY, mouseRay);
        Vector3 relativeXZ = intersectXZ - position;
        Vector3 relativeXY = intersectXY - position;
        
        Vector3 _xProjection = new Vector3(Vector3.Dot(intersectXZ - position, Vector3.right), 0f, 0f);
        Vector3 _zProjection = new Vector3(0f, 0f, Vector3.Dot(intersectXZ - position, Vector3.forward));
        Vector3 _yProjection = new Vector3(0f, Vector3.Dot(intersectXY - position, Vector3.up), 0f);

        if ((_xProjection - relativeXZ).magnitude <= handleRange && relativeXZ.magnitude <= GUIRenderer.Instance.HandleLength && Mathf.Sign(relativeXZ.x) >= 0)
        {
            xProjection = _xProjection;
            yProjection = _yProjection;
            zProjection = _zProjection;
            return Axis.X;
        }
        if ((_zProjection - relativeXZ).magnitude <= handleRange && relativeXZ.magnitude <= GUIRenderer.Instance.HandleLength && Mathf.Sign(relativeXZ.z) >= 0)
        {
            xProjection = _xProjection;
            yProjection = _yProjection;
            zProjection = _zProjection;
            return Axis.Z;
        }
        if ((_yProjection - relativeXY).magnitude <= handleRange && relativeXY.magnitude <= GUIRenderer.Instance.HandleLength && Mathf.Sign(relativeXY.y) >= 0)
        {
            xProjection = _xProjection;
            yProjection = _yProjection;
            zProjection = _zProjection;
            return Axis.Y;
        }
        xProjection = _xProjection;
        yProjection = _yProjection;
        zProjection = _zProjection;
        return Axis.None;
    }
    
    private static Vector3 IntersectPlaneWithRay(Plane plane, Ray ray)
    {
        if (!plane.Raycast(ray, out float distance)) return Vector3.zero;
        Vector3 intersectionPoint = ray.GetPoint(distance);
        return intersectionPoint;
    }
}
