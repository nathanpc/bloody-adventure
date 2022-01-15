using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCellTrigger : MonoBehaviour {
	public ExplodeAction action;
	public GameObject rootNode;
	public ParticleSystem particles;
	public float dieSpeed = 0.1f;
	private bool isDying = false;

	void FixedUpdate() {
		// Check if is dying.
		if (isDying) {
			// Linearly scale down.
			float scale = transform.localScale.x;
			scale -= dieSpeed;

			// Bottom out the scale.
			if (scale < 0)
				scale = 0;

			transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	void OnTriggerEnter(Collider col) {
		// Check if the virus actually collided with us.
		if (col.tag == action.ActionableObjectTag) {
			DiseaseBehaviour diseaseBehaviour =
				col.transform.gameObject.GetComponent<DiseaseBehaviour>();

			// Take a hit at the thing.
			diseaseBehaviour.TakeHit(action.DamageAmount);
		}

		// Do a whole dance.
		//particles.Play();
		isDying = true;
		StartCoroutine(WaitToDie());
	}

	/// <summary>
	/// Coroutine function to wait for the particle effect to end before dying.
	/// </summary>
	IEnumerator WaitToDie() {
		// Wait for the particle show to stop.
		while (particles.isPlaying)
			yield return new WaitForSeconds(1);

		// Die.
		//Destroy(rootNode);
	}
}
