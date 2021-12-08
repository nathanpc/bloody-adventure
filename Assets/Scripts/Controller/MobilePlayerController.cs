using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller abstraction class for a mobile touchscreen controller.
/// </summary>
public class MobilePlayerController : MonoBehaviour, IPlayerController {
	[SerializeField] private GameObject _playerModel;
	[SerializeField] private float _leanSpeed = 1.0f;
	[SerializeField] private float _rotationSpeed = 1.0f;
	public ActionBase currentAction = null;
	private Vector3 touchStartPosition;
	private float touchSpeedCompensation = 0.005f;

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
		if (MainFire())
			currentAction.ExecuteAction();
	}

	public float HorizontalLean() {
		return 0f;
	}

	public float VerticalMovement() {
		return 0f;
	}

	public float HorizontalRotation() {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);

			switch (touch.phase) {
			case TouchPhase.Began:
				touchStartPosition = touch.position;
				break;
			case TouchPhase.Moved:
				return (touch.position.x - touchStartPosition.x) *
					touchSpeedCompensation;
			case TouchPhase.Ended:
				break;
			}
		}

		return 0f;
	}

	public float VerticalRotation() {
		// Check if this is the first touch.
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);

			switch (touch.phase) {
			case TouchPhase.Began:
				touchStartPosition = touch.position;
				break;
			case TouchPhase.Moved:
				return (touch.position.y - touchStartPosition.y) *
					touchSpeedCompensation;
			case TouchPhase.Ended:
				break;
			}
		}

		return 0f;
	}

	public bool MainFire() {
		return Input.touchCount > 1;
		/*
		// Check if we are already controlling the camera.
		if (Input.touchCount > 1) {
			// Get the raycast of the touch.
			Touch touch = Input.GetTouch(1);
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;

			// Check if we hit something.
			if (Physics.Raycast(ray, out hit)) {
				// Has the touch input ended?
				switch (touch.phase) {
				case TouchPhase.Canceled:
				case TouchPhase.Ended:
					return false;
				}

				// Check if we hit the current tool.
				return hit.transform.tag == "Tool";
			}
		}

		return false;
		*/
	}

	public bool Escape() {
		return false;
	}

	/// <summary>
	/// Model of the actual player.
	/// </summary>
	public GameObject PlayerModel {
		get { return _playerModel; }
		set { _playerModel = value; }
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
