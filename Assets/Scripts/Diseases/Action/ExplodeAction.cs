using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action script for the COVID exploder cell.
/// </summary>
public class ExplodeAction : ActionBase {
	[Header("White Cell")]
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

		// Play the throw sound.
		if (audioSource.isPlaying)
			audioSource.Stop();
		audioSource.Play();

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
}
