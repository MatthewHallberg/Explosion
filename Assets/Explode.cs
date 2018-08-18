using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	private bool exploded = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !exploded) {
			exploded = true;
			Explosion ();
		}
	}

	//Force rigid body explosion
	void Explosion () {
		foreach (Transform child in this.transform) {
			child.gameObject.AddComponent<Rigidbody> ();
			child.gameObject.AddComponent<BoxCollider> ();
			child.gameObject.AddComponent<ParticleBehavior> ();
		}
	}

	public void SetExplosionFrame (float frame) {
		ParticleBehavior[] particles = FindObjectsOfType<ParticleBehavior> ();
		foreach (ParticleBehavior particle in particles) {
			particle.SetFrameCount (Mathf.RoundToInt(frame));
		}
	}
}
