using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player action abstraction interface.
/// </summary>
public abstract class ActionBase : MonoBehaviour {
	[Header("General")]
	public GameObject parentObject;

	[Header("Interaction")]
	[SerializeField] private string _actionableObjectTag;
	[SerializeField] private int _damageAmount = 1;
	[SerializeField] private float _maxDistance = 1.0f;

	[Header("Effects")]
	[SerializeField] private ParticleSystem _particles;

	[Header("Sounds")]
	public AudioSource audioSource;

	/// <summary>
	/// Executes an action depending on what's needed from the user.
	/// </summary>
	public abstract void ExecuteAction();

	/// <summary>
	/// Ran when the user stops executing an action.
	/// </summary>
	public virtual void StopAction() {
	}

	/// <summary>
	/// Performs a raycast from the center of the camera.
	/// </summary>
	/// <param name="hit">If true is returned, this will contain the hit
	/// information.</param>
	/// <returns>True if a collider has been hit.</returns>
	protected bool DoCameraRaycast(out RaycastHit hit) {
		Ray origin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		if (Physics.Raycast(origin, out hit, MaxDistance)) {
#if UNITY_EDITOR
			Debug.DrawRay(origin.origin, hit.point, Color.blue);
#endif  // UNITY_EDITOR

			return true;
		}

		return false;
	}

	/// <summary>
	/// Which <see cref="GameObject"/> tag is actually able to be interacted
	/// with from this action.
	/// </summary>
	public string ActionableObjectTag {
		get { return _actionableObjectTag; }
		set { _actionableObjectTag = value; }
	}

	/// <summary>
	/// Amount of damage to inflict.
	/// </summary>
	public int DamageAmount {
		get { return _damageAmount; }
		set { _damageAmount = value; }
	}

	/// <summary>
	/// Maximum distance the action can have an effect.
	/// </summary>
	public float MaxDistance {
		get { return _maxDistance; }
		set { _maxDistance = value; }
	}

	/// <summary>
	/// Fancy particles.
	/// </summary>
	public ParticleSystem Particles {
		get { return _particles; }
		set { _particles = value; }
	}
}
