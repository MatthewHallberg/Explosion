﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	private bool exploded = false;

	private void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Explosion ();
		}
	}

	public void OnContentPlaced () {
		StartCoroutine (PlacedRoutine ());
	}

	IEnumerator PlacedRoutine () {
		yield return new WaitForSeconds (1f);
		Explosion ();
	}

	//Force rigid body explosion
	public void Explosion () {
		if (!exploded) {
			exploded = true;
			//add physics and stuff to children
			foreach (Transform child in this.transform) {
				child.gameObject.AddComponent<Rigidbody> ();
				child.gameObject.AddComponent<BoxCollider> ();
				child.gameObject.AddComponent<ParticleBehavior> ();
			}
		}
	}

	public void SetExplosionFrame (float frame) {
		ParticleBehavior[] particles = FindObjectsOfType<ParticleBehavior> ();
		foreach (ParticleBehavior particle in particles) {
			particle.SetFrameCount (Mathf.RoundToInt(frame));
		}
	}
}
