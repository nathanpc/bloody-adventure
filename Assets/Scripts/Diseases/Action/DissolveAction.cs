using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action script for the cholesterol dissolver spray.
/// </summary>
public class DissolveAction : ActionBase {
	[SerializeField] private string _dissolvableObjectTag;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public override void ExecuteAction() {
		RaycastHit hit;

		// Raycast and check if we actually have hit some cholesterol.
		if (DoCameraRaycast(out hit)) {
			// Check if we can actually dissolve the item that was hit.
			if (hit.transform.tag == DissolvableObjectTag) {
				DiseaseBehaviour diseaseBehaviour =
					hit.transform.gameObject.GetComponent<DiseaseBehaviour>();

				diseaseBehaviour.TakeHit();
			}
		}
	}

	/// <summary>
	/// Which <see cref="GameObject"/> tag is actually used for dissolvable items?
	/// </summary>
	public string DissolvableObjectTag {
		get { return _dissolvableObjectTag; }
		set { _dissolvableObjectTag = value; }
	}
}
