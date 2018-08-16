using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ExplosionController : MonoBehaviour {

	public Transform newParent;

	private bool exploded = false;
	private Material objectMat;

	private void Update () {
		if (Input.GetMouseButtonDown (0)){
			Explode ();
		}
	}

	void Explode () {
		exploded = !exploded;
		StartCoroutine (SplitMesh ());
	}

	IEnumerator SplitMesh () {

		print ("exploding");

		int submesh = 0;

		Mesh m = GetComponent<MeshFilter> ().mesh;
		// Get the vertex liss
		Vector3 [] verts = m.vertices;
		Vector3 [] norms = m.normals;
		Vector2 [] uvs = m.uv;

		// Get the triangles and material for the given submesh
		int [] tris = m.GetTriangles (submesh);
		objectMat = GetComponent<MeshRenderer> ().sharedMaterials [submesh];

		// Do for all triangles
		for (int i = 0; i < tris.Length; i += 3) {
			// Create the game object and assign it as a child to the parent. Give it a name that identifies
			// its submesh and triangle index
			GameObject go = new GameObject ();
			go.name = i.ToString();
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localScale = Vector3.one;
			go.transform.SetParent (newParent);

			// Now create the mesh for the new child object
			Mesh newMesh = new Mesh ();
			newMesh.name = go.name;
			newMesh.vertices = new Vector3 [] { verts [tris [i]], verts [tris [i + 1]], verts [tris [i + 2]] };
			newMesh.normals = new Vector3 [] { norms [tris [i]], norms [tris [i + 1]], norms [tris [i + 2]] };
			newMesh.uv = new Vector2 [] { uvs [tris [i]], uvs [tris [i + 1]], uvs [tris [i + 2]] };
			newMesh.triangles = new int [] { 0, 1, 2 };
			go.AddComponent<MeshFilter> ().sharedMesh = newMesh;
			go.AddComponent<MeshRenderer> ().sharedMaterials = new Material [] { objectMat };
			yield return null;
		}
		yield return new WaitForEndOfFrame ();
		CombineMeshes ();
	}

	void CombineMeshes () {
		MeshFilter [] meshFilters = newParent.GetComponentsInChildren<MeshFilter> ();
		CombineInstance [] combine = new CombineInstance [meshFilters.Length];
		int count = 1;
		for (int i = 0; i < meshFilters.Length; i += count) {
			print (meshFilters [i].gameObject.name);
			combine [i].mesh = meshFilters [i].sharedMesh;
			combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
		}
		//create new combined mesh
		GameObject go = new GameObject ();
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		go.transform.SetParent (newParent);
		go.AddComponent<MeshFilter> ();
		go.GetComponent<MeshFilter> ().mesh = new Mesh ();
		go.GetComponent<MeshFilter> ().mesh.CombineMeshes (combine);
		go.AddComponent<MeshRenderer> ().sharedMaterials = new Material [] { objectMat };
	}
}