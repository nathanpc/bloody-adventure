using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action script for the cholesterol dissolver spray.
/// </summary>
public class DissolveAction : ActionBase {
	[Header("Spray Bottle")]
	public GameObject trigger;
	
	private GameObject _sprayer;
	private bool triggered = false;

	// Start is called before the first frame update
	void Start() {
		// Get the sprayer!
		Sprayer = gameObject;
	}

	public override void ExecuteAction() {
		RaycastHit hit;

		// Pull the trigger.
		if (!triggered) {
			trigger.transform.localRotation = Quaternion.Euler(0, 0, -32);
			triggered = true;
		}

		// Show a nice spray!
		Particles.Play();

		// Play the spray sound.
		if (!audioSource.isPlaying)
			audioSource.Play();

		// Raycast and check if we actually have hit some cholesterol.
		if (DoCameraRaycast(out hit)) {
			// Check if we can actually dissolve the item that was hit.
			if (hit.transform.tag == ActionableObjectTag) {
				DiseaseBehaviour diseaseBehaviour =
					hit.transform.gameObject.GetComponent<DiseaseBehaviour>();

				// Take a hit at the thing.
				diseaseBehaviour.TakeHit(DamageAmount);
			}
		}
	}

	public override void StopAction() {
		// Depress the trigger.
		if (triggered)
			trigger.transform.localRotation = Quaternion.Euler(0, 0, 0);

		triggered = false;
	}

	/// <summary>
	/// Sprayer object.
	/// </summary>
	public GameObject Sprayer {
		get { return _sprayer; }
		set { _sprayer = value; }
	}
}
