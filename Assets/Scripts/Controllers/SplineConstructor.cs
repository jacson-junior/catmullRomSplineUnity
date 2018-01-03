using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Splines.CatmullRom;
using System.Linq;
using System;
using Utils;

//[ExecuteInEditMode]
public class SplineConstructor : MonoBehaviour {

    public Transform prefab;
	public float step;
    public List<Transform> transformPoints;
    public List<OrientedPoint> oPoints;
	float[] sampledLengths;
    Vector3[] points;
	Spline spline;
    List<Vector3[]> lines;
    List<float> samples;
	// Use this for initialization
	void Awake () {
		spline = new Spline();
        lines = new List<Vector3[]>();
        oPoints = new List<OrientedPoint>();
        points = transformPoints.Select(x => x.position).ToArray();
        if (points.Length > 4)
        {
		    GenerateSamples();
        }
	}
	
	void GenerateSamples()
    {
        lines = new List<Vector3[]>();
        for (int i = 0; i < points.Length-3; i++)
        {
            Vector3 prevPoint = points[i+1];
            Vector3 pt;
            float total = 0;
            
            samples = new List<float>();
            Vector3[] selectedPoints = points.Skip(i).Take(4).ToArray();

            for (float f = 0f; f < 1.0f; f += step)
            {
                pt = spline.GetPoint(f, selectedPoints);
                total += (pt - prevPoint).magnitude;
                samples.Add(total);
                sampledLengths = samples.ToArray();
                oPoints.Add(GetOrientedPoint(f, selectedPoints));

                lines.Add(new Vector3[]{ prevPoint, pt });
                prevPoint = pt;
            }
    
            pt = spline.GetPoint(1, selectedPoints);
            lines.Add(new Vector3[]{ prevPoint, pt });

            samples.Add(total + (pt - prevPoint).magnitude);
        }
    }

    public OrientedPoint GetOrientedPoint(float t, Vector3[] sPoints)
    {
        Vector3 tangent, normal;
        Quaternion orientation;
 
        Vector3 point = spline.GetPoint(t, sPoints, out tangent, out normal, out orientation);
 
        return new OrientedPoint(point, orientation, tangent, normal, sampledLengths.Sample(t));
    }

    void OnDrawGizmos() {
        if (lines != null)
        {
            foreach (Vector3[] line in lines)
            {
                Gizmos.DrawLine(line[0], line[1]);
            }
        }
        if (oPoints != null)
        {
            foreach (OrientedPoint point in oPoints)
            {
                var oldColor = Gizmos.color;
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point.position, 0.1f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(point.position + point.tangent.normalized, point.position - point.tangent.normalized);
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(point.position, point.position + point.normal.normalized);
                Gizmos.color = oldColor;
            }
        }
    }

    public void CreatePoint() {
        Transform tr = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        tr.parent = transform;
        tr.name = transformPoints.Count.ToString();
        transformPoints.Add(tr);
        if (transformPoints.Count > 4)
        {
		    GenerateSamples();
        }
    }

    void Update()
    {
        transformPoints = transformPoints.Where(item => item != null).ToList();
    }

    private static void DestroyElement(float index)
    {
        Destroy(GameObject.Find(index.ToString()));
    }
}
