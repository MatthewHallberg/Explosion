using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehavior : MonoBehaviour {

	private Vector3 desiredPosition, startPosition;
	private bool shouldMove = false;

	public void Move (bool shouldExplode) {
		shouldMove = true;
		if (shouldExplode) {
			if (desiredPosition != null) transform.position = desiredPosition;
			startPosition = transform.position;
			desiredPosition = startPosition + (transform.forward * 2);
		} else {
			desiredPosition = startPosition;
		}
	}

	// Update is called once per frame
	void Update () {
		if (shouldMove) {
			transform.position = Vector3.Lerp (transform.position, desiredPosition, Time.deltaTime * 6f);
		}
	}
}
