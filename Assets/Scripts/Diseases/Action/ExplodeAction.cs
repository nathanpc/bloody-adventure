using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action script for the COVID exploder cell.
/// </summary>
public class ExplodeAction : ActionBase {
	[SerializeField] private int _damageAmount = 1;
	[SerializeField] private string _dissolvableObjectTag;
	//[SerializeField] private ParticleSystem _particles;
	public GameObject rigidBodyPrefab;
	public GameObject instantiateInPlace;
	public float throwForce;
	private GameObject _cell;

	// Start is called before the first frame update
	void Start() {
		// Get the cell!
		Cell = gameObject;
	}

	public override void ExecuteAction() {
		GameObject rigidCell = Instantiate(rigidBodyPrefab,
			instantiateInPlace.transform.position, instantiateInPlace.transform.rotation);
		
		// Setup the explode trigger.
		WhiteCellTrigger cellTrigger = rigidCell.GetComponentInChildren<WhiteCellTrigger>();
		cellTrigger.action = this;

		Vector3 direction = Vector3.right;
		direction = instantiateInPlace.transform.forward;

		// Throw the cell!
		rigidCell.GetComponentInChildren<Rigidbody>().AddRelativeForce(
			direction * throwForce, ForceMode.Impulse);
	}

	public override void StopAction() {
	}

	/// <summary>
	/// Cell object.
	/// </summary>
	public GameObject Cell {
		get { return _cell; }
		set { _cell = value; }
	}

	/*
	/// <summary>
	/// Spraying particles.
	/// </summary>
	public ParticleSystem SprayParticles {
		get { return _particles; }
		set { _particles = value; }
	}
	*/

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
