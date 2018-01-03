using UnityEngine;

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public float vCoordinate;
    public Vector3 tangent;
    public Vector3 normal;
 
    public OrientedPoint(Vector3 position, Quaternion rotation, float vCoordinate = 0)
    {
        this.position = position;
        this.rotation = rotation;
        this.vCoordinate = vCoordinate;
        this.tangent = new Vector3();
        this.normal = new Vector3();
    }

    public OrientedPoint(Vector3 position, Quaternion rotation, Vector3 tangent, Vector3 normal, float vCoordinate = 0)
    {
        this.position = position;
        this.rotation = rotation;
        this.tangent = tangent;
        this.normal = normal;
        this.vCoordinate = vCoordinate;
    }
 
    public Vector3 LocalToWorld(Vector3 point)
    {
        return position + rotation * point;
    }
 
    public Vector3 WorldToLocal(Vector3 point)
    {
        return Quaternion.Inverse(rotation) * (point - position);
    }
 
    public Vector3 LocalToWorldDirection(Vector3 dir)
    {
        return rotation * dir;
    }
}