using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class SplineMeshConstructor : MonoBehaviour {

	SplineConstructor sc;
	MeshShape ms;

    Mesh mesh;
	void Start()
	{
		sc = GetComponent<SplineConstructor>();
		ms = GetComponent<MeshShape>();
		MeshFilter mf = GetComponent<MeshFilter>();

		mesh = new Mesh();
		Extrude(ref mesh, ms.es, sc.oPoints.ToArray());

		mf.sharedMesh = mesh;
	}

	public void Extrude(ref Mesh mesh, ExtrudeShape shape, OrientedPoint[] path)
    {
        int vertsInShape = shape.verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.Lines.Length * segments * 2;
        int triIndexCount = triCount * 3;
 
        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
 
        // Generate all of the vertices and normals
        for (int i = 0; i < path.Length; i++)
        {
            int offset = i*vertsInShape;
            for (int j = 0; j < vertsInShape; j++)
            {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld(shape.verts[j]);
                normals[id] = path[i].LocalToWorldDirection(shape.normals[j]);
                uvs[id] = new Vector2( shape.Ucoords[j], i / ((float)edgeLoops) );
            }
        }
 
        // Generate all of the triangles
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int offset = i * vertsInShape;
            for (int l = 0; l < shape.Lines.Length; l += 2)
            {
                int a = offset + shape.Lines[l];
                int b = offset + shape.Lines[l] + vertsInShape;
                int c = offset + shape.Lines[l + 1] + vertsInShape;
                int d = offset + shape.Lines[l + 1];
                triangleIndices[ti++] = a;
                triangleIndices[ti++] = b;
                triangleIndices[ti++] = c;
                triangleIndices[ti++] = c;
                triangleIndices[ti++] = d;
                triangleIndices[ti++] = a;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangleIndices;
    }

    void OnDrawGizmos() {
        if (mesh != null)
        {
            foreach (Vector3 vertice in mesh.vertices)
            {
                var oldColor = Gizmos.color;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(vertice, 0.05f);
                Gizmos.color = oldColor;
            }
            foreach (Vector3 normals in mesh.normals)
            {
                var oldColor = Gizmos.color;            
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + normals.normalized);
                Gizmos.color = oldColor;            
            }
        }
    }
}
