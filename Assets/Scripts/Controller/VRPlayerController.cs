using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller abstraction class for a VR controller.
/// </summary>
public class VRPlayerController : MonoBehaviour, IPlayerController {
	[SerializeField] private GameObject _playerModel;
	[SerializeField] private float _leanSpeed = 1.0f;
	[SerializeField] private float _rotationSpeed = 1.0f;
	public ActionBase[] dissolveAction;
	private ActionBase _currentAction = null;

	void Start() {
		CurrentAction = dissolveAction[0];
	}

	// Update is called once per frame
	void Update() {
		// Are we done yet?
		if (Escape())
			GameControl.Quit();

		// Lean in X and Y. Ignore Z because we are not in VR.
		Vector3 lean = new Vector3(HorizontalLean(), VerticalMovement(), 0);
		transform.Translate(lean * LeanSpeed * Time.deltaTime);

		// Rotate the player around.
		transform.Rotate(0, HorizontalRotation() * RotationSpeed, 0);
		transform.Rotate(-VerticalRotation() * RotationSpeed, 0, 0);

		// Fire?
		if (MainFire()) {
			CurrentAction.ExecuteAction();
		} else {
			CurrentAction.StopAction();
		}
	}

	public float HorizontalLean() {
		return Input.GetAxis("Horizontal");
	}

	public float VerticalMovement() {
		return Input.GetAxis("Vertical");
	}

	public float HorizontalRotation() {
		return Input.GetAxis("Mouse X");
	}

	public float VerticalRotation() {
		return Input.GetAxis("Mouse Y");
	}

	public bool MainFire() {
		return Input.GetAxis("Trigger") >= 0.99f;
	}

	public bool Escape() {
		return Input.GetButtonDown("Cancel");
	}

	/// <summary>
	/// Model of the actual player.
	/// </summary>
	public GameObject PlayerModel {
		get { return _playerModel; }
		set { _playerModel = value; }
	}

	/// <summary>
	/// Current action of the player.
	/// </summary>
	public ActionBase CurrentAction {
		get { return _currentAction; }
		set { _currentAction = value; }
	}

	/// <summary>
	/// Speed of the player leaning.
	/// </summary>
	public float LeanSpeed {
		get { return _leanSpeed; }
		set { _leanSpeed = value; }
	}

	/// <summary>
	/// Speed of the player rotating.
	/// </summary>
	public float RotationSpeed {
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}
}