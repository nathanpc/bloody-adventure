using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How a dissolvable object behaves.
/// </summary>
public class DissolvableBehaviour : DiseaseBehaviour {
	private float _originalScale;

	void Start() {
		// Get the original scale value.
		OriginalScale = transform.localScale.x;
	}

	public override void TakeHit() {
		// Let's take a hit first.
		base.TakeHit();

		// Die?
		if (IsSupposedDead()) {
			Die();
			return;
		}

		// Scale the model according to our hit count.
		float scale = OriginalScale * ((float)HitPoints / 100);
		transform.localScale = new Vector3(scale, scale, scale);

		Debug.Log(HitPoints);
	}

	/// <summary>
	/// Original scale of the object to be used as a reference.
	/// </summary>
	protected float OriginalScale {
		get { return _originalScale; }
		set { _originalScale = value; }
	}
}
