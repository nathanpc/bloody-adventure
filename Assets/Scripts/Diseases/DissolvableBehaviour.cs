using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How a dissolvable object behaves.
/// </summary>
public class DissolvableBehaviour : DiseaseBehaviour {
	[Header("Effects")]
	[SerializeField] private ParticleSystem _particles;

	private float _originalScale;

	void Start() {
		// Get the original scale value.
		OriginalScale = transform.localScale.x;
	}

	public override void TakeHit(int amount) {
		// Let's take a hit first.
		base.TakeHit(amount);

		// Make sure we don't end up with a tiny 1HP point.
		base.TakeHit(5);
		if (HitPoints < 15)
			base.TakeHit(15);

		// Die?
		if (IsSupposedDead())
		StartCoroutine(WaitToDie());

		// Scale the model according to our hit count.
		float scale = OriginalScale * ((float)HitPoints / 100);
		transform.localScale = new Vector3(scale, scale, scale);

		// Show pretty particles.
		Particles.Play();

		// Play a hit sound.
		if (audioSource.isPlaying)
			audioSource.Stop();
		audioSource.Play();
	}

	/// <summary>
	/// Coroutine function to wait for the particle effect to end before dying.
	/// </summary>
	IEnumerator WaitToDie() {
		// Wait for the particle show or sound to stop.
		while (Particles.isPlaying || audioSource.isPlaying)
			yield return new WaitForSeconds(1);

		Die();
	}

	/// <summary>
	/// Original scale of the object to be used as a reference.
	/// </summary>
	protected float OriginalScale {
		get { return _originalScale; }
		set { _originalScale = value; }
	}

	/// <summary>
	/// Particle systems associated with the component to make pretty splashy
	/// animation.
	/// </summary>
	protected ParticleSystem Particles {
		get { return _particles; }
		set { _particles = value; }
	}
}
