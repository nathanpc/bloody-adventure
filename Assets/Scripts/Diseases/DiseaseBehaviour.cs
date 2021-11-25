using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstracts away the random behaviours of our diseases.
/// </summary>
public abstract class DiseaseBehaviour : MonoBehaviour {
	[SerializeField] private int _hitPoints = 100;

	/// <summary>
	/// Inflicts damage to the object.
	/// </summary>
	/// <param name="amount">Amount of damage to take.</param>
	public virtual void TakeHit(int amount) {
		HitPoints -= amount;
	}

	/// <summary>
	/// Well... Our <see cref="GameObject"/> will go to a better place...
	/// </summary>
	public virtual void Die() {
		if (gameObject != null)
			Destroy(gameObject);
	}

	/// <summary>
	/// Are we supposed to be dead yet?
	/// </summary>
	/// <returns>True if our <see cref="HitPoints"/> are at 0.</returns>
	public bool IsSupposedDead() {
		return HitPoints == 0;
	}

	/// <summary>
	/// Amount of "life" the object has.
	/// </summary>
	public int HitPoints {
		get { return _hitPoints; }
		set {
			_hitPoints = value;

			// Do not allow negative values.
			if (_hitPoints < 0)
				_hitPoints = 0;
		}
	}
}
