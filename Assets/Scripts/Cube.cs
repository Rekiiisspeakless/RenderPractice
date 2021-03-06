﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour {
	public int sizeX, sizeY, sizeZ;
	private Mesh mesh;
	private Vector3[] vertices;

	void Awake(){
		//StartCoroutine(Generate ());
		Generate ();
	}

	private void Generate(){
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh();
		mesh.name = "Procedure Cube";
		CreateVertices ();
		CreateTriangles ();
	}

	private void CreateVertices(){
		//WaitForSeconds wait = new WaitForSeconds (0.05f);

		int cornerVertices = 8;
		int edgeVertices = 4 * (sizeX - 1) * (sizeY - 1) * (sizeZ - 1);
		int faceVertices = 2 * ((sizeX - 1) * (sizeY - 1) + 
			(sizeY - 1) * (sizeZ - 1) + (sizeZ - 1) * (sizeX - 1));
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
		int v = 0;
		for (int y = 0; y <= sizeY; ++y) {
			for (int x = 0; x <= sizeX; ++x) {
				vertices [v++] = new Vector3 (x, y, 0f);
				//yield return wait;
			}
			for (int z = 1; z <= sizeZ; ++z) {
				vertices [v++] = new Vector3 (sizeX, y, z);
				//yield return wait;
			}
			for (int x = sizeX - 1; x >= 0; --x) {
				vertices [v++] = new Vector3 (x, y, sizeZ);
				//yield return wait;
			}
			for (int z = sizeZ - 1; z > 0; --z) {
				vertices [v++] = new Vector3 (0f, y, z);
				//yield return wait;
			}
		}


		for (int z = 1; z < sizeZ; ++z) {
			for (int x = 1; x < sizeX; ++x) {
				vertices [v++] = new Vector3 (x, sizeY, z);
				//yield return wait;
			}
		}
		

		for (int z = 1; z < sizeZ; ++z) {
			for (int x = 1; x < sizeX; ++x) {
				vertices [v++] = new Vector3 (x, 0f, z);
				//yield return wait;
			}
		}
		mesh.vertices = vertices;
	}

	private void CreateTriangles(){
		int quads = 2 * (sizeX * sizeY + sizeY * sizeZ + sizeZ * sizeX);
		int[] triangles = new int[quads * 6];
		int ring = 2 * (sizeX + sizeZ); 
		int t = 0, v = 0;
		for (int y = 0; y < sizeY; ++y, v++) {
			for (int q = 0; q < ring - 1; ++q, ++v) {
				t = SetQuad (triangles, t, v, v + 1, v + ring, v + ring + 1);
			}
			t = SetQuad (triangles, t, v, v - ring + 1, v + ring, v + 1);
		}
		t = CreateTopFace (triangles, t, ring);
		t = CreateBottomFace(triangles, t, ring);
		mesh.triangles = triangles;
	}

	private int CreateTopFace(int[] triangles, int t, int ring){
		int v = ring * sizeY;
		for(int x = 0; x < sizeX - 1; x ++, v++){
			t = SetQuad (triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad (triangles, t, v, v + 1, v + ring - 1,  v + 2);
		int vMin = ring * (sizeY + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;
		for (int z = 1; z < sizeZ - 1; ++z, vMin--, vMid++, vMax++) {
			t = SetQuad (triangles, t, vMin, vMid, vMin - 1, vMid + sizeX - 1);
			for (int x = 1; x < sizeX - 1; ++x, vMid++) {
				t = SetQuad (triangles, t, vMid, vMid + 1, vMid + sizeX - 1, vMid + sizeX);
			}
			t = SetQuad (triangles, t, vMid, vMax, vMid + sizeX - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMin - 2);
		for (int x = 1; x < sizeX - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
		return t;
	}

	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (sizeX - 1) * (sizeZ - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < sizeX - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= sizeX - 2;
		int vMax = v + 2;

		for (int z = 1; z < sizeZ - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + sizeX - 1, vMin + 1, vMid);
			for (int x = 1; x < sizeX - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + sizeX - 1, vMid + sizeX, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + sizeX - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < sizeX - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}


	private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11){
		triangles [i] = v00;
		triangles [i + 1] = triangles [i + 4] = v01;
		triangles [i + 2] = triangles [i + 3] = v10;
		triangles [i + 5] = v11;
		return i + 6;
	}

	private void OnDrawGizmos(){
		if (vertices == null) {
			return;
		}
		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; ++i) {
			Gizmos.DrawSphere (vertices[i], 0.1f);
		}
			
	}
}
