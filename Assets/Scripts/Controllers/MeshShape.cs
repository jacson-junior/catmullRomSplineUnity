using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshShape : MonoBehaviour {

	public AnimationCurve shape;
	[RangeAttribute(1,20)]
	public int subdivisions;

	List<Vector2> verts;
	List<Vector2> normals;

	public ExtrudeShape es { get; set; }

	void Awake()
	{
		List<Vector2> verts = new List<Vector2>();
		List<Vector2> normals = new List<Vector2>();

		for (int i = 1; i < shape.keys.Length; i++)
		{
			Keyframe start = shape.keys[i-1];
			Keyframe end = shape.keys[i];

			float difference = Mathf.Abs(start.time - end.time);

			verts.Add(new Vector2(start.time, start.value));
			normals.Add(Vector2.up);

			for (float j = (start.time + difference/subdivisions); j < end.time; j += difference/subdivisions)
			{
				verts.Add(new Vector2(j, shape.Evaluate(j)));
				normals.Add(Vector2.up);
			}

		}
		verts.Add(new Vector2(shape.keys[shape.length - 1].time, shape.keys[shape.length - 1].value));
		normals.Add(new Vector2(0,1));

		es = new ExtrudeShape(verts.ToArray(), normals.ToArray());
	}

	Vector3 CalculateNormal(Vector3 tangent, Vector3 up)
    {
        Vector3 binormal = Vector3.Cross(up, tangent);
        return Vector3.Cross(tangent, binormal);
    }

}
