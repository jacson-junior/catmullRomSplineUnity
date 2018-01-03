using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ExtrudeShape
{
    public Vector2[] verts;
    public Vector2[] normals;
 
    IEnumerable<int> LineSegment(int i)
    {
        yield return i;
        yield return i + 1;
    }
 
    int[] lines;
    public int[] Lines
    {
        get
        {
            if (lines == null)
            {
                lines = Enumerable.Range(0, verts.Length - 1)
                    .SelectMany(i => LineSegment(i))
                    .ToArray();
            }
 
            return lines;
        }
    }

    float[] ucoords;
    public float[] Ucoords
    {
        get
        {
            if (ucoords == null)
            {
                ucoords = Enumerable.Range(0, verts.Length)
                    .Select(i =>  i / (float)verts.Length )
                    .ToArray();
            }
 
            return ucoords;
        }
    }

    public ExtrudeShape(Vector2[] verts, Vector2[] normals){
        this.verts = verts;
        this.normals = normals;
    }
};