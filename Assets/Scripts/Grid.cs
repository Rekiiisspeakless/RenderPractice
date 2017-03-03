using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter)) ]

public class Grid : MonoBehaviour {

	public int xSize, ySize;
	private Vector3[] vertices; 
	private Mesh mesh;

	void Awake(){
		//StartCoroutine (Generate());
		Generate();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Generate(){
		//WaitForSeconds wait = new WaitForSeconds (0.05f);
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];

		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural grid";
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4 (1f, 0f, 0f, -1f);
		//create vertice
		for (int i = 0, y = 0; y <= ySize; ++y) {
			for (int x = 0; x <= xSize; ++x, ++i) {
				vertices [i] = new Vector3 (x, y);
				//uv coordinate
				uv [i] = new Vector2 ((float)x / xSize, (float)y / ySize);
				//tangents
				tangents[i] = tangent;

				//yield return wait;
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;
		//create triangle
		int[] triangles = new int[xSize * ySize * 6];

		for (int i = 0, j = 0, y = 0; y < ySize; ++j, ++y) {
			for (int x = 0; x < xSize; ++x, ++j, i += 6) {
				triangles [i] = j;
				triangles [i + 1] = triangles [i + 4] = j + xSize + 1;
				triangles [i + 2] = triangles [i + 3] = j + 1;
				triangles [i + 5] = j + xSize + 2;
				mesh.triangles = triangles;
				//calculate normals
				mesh.RecalculateNormals ();
				//yield return wait;
			}
		}

		//clockwise
		/*triangles [0] = 0;
		triangles [1] = xSize + 1;
		triangles [2] = 1;
		triangles [3] = 1;
		triangles [4] = xSize + 1;
		triangles [5] = xSize + 2;
		mesh.triangles = triangles;*/
	}

	void OnDrawGizmos(){
		if (vertices == null)
			return;
		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; ++i) {
			Gizmos.DrawSphere (vertices[i], 0.1f);
		}
	}
}
