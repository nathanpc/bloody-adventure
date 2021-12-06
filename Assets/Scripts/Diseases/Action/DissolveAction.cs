using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action script for the cholesterol dissolver spray.
/// </summary>
public class DissolveAction : ActionBase {
	[SerializeField] private GameObject _sprayer;
	[SerializeField] private int _damageAmount = 1;
	[SerializeField] private string _dissolvableObjectTag;
	private ParticleSystem particles;

	// Start is called before the first frame update
	void Start() {
		// Get the pretty particles!
		if (Sprayer != null)
			particles = Sprayer.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update() {

	}

	public override void ExecuteAction() {
		RaycastHit hit;

		// Show a nice spray!
		particles.Play();

		// Raycast and check if we actually have hit some cholesterol.
		if (DoCameraRaycast(out hit)) {
			// Check if we can actually dissolve the item that was hit.
			if (hit.transform.tag == DissolvableObjectTag) {
				DiseaseBehaviour diseaseBehaviour =
					hit.transform.gameObject.GetComponent<DiseaseBehaviour>();

				diseaseBehaviour.TakeHit(DamageAmount);
			}
		}
	}

	/// <summary>
	/// Sprayer object.
	/// </summary>
	public GameObject Sprayer {
		get { return _sprayer; }
		set { _sprayer = value; }
	}

	/// <summary>
	/// Amount of damage to inflict.
	/// </summary>
	public int DamageAmount {
		get { return _damageAmount; }
		set { _damageAmount = value; }
	}

	/// <summary>
	/// Which <see cref="GameObject"/> tag is actually used for dissolvable items?
	/// </summary>
	public string DissolvableObjectTag {
		get { return _dissolvableObjectTag; }
		set { _dissolvableObjectTag = value; }
	}
}
