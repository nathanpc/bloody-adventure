using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstraction class to allow us to control the player with a whole
/// plethora of controller types.
/// </summary>
public abstract class ControllerBase : MonoBehaviour, IPlayerController {
	[SerializeField] private GameObject _playerModel;
	[SerializeField] private float _leanSpeed = 1.0f;
	[SerializeField] private float _rotationSpeed = 1.0f;
	public ActionBase[] actions;
	private int _actionIndex = 0;

	public virtual void Start() {
		// Clear all of the actions first.
		foreach (ActionBase action in actions)
			SetActionActive(action, false);

		// Select the default action.
		SelectAction(0);
	}

	// Update is called once per frame
	public virtual void Update() {
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

		// Switch weapon?
		if (SwitchTool())
			SelectAction(ActionIndex + 1);
	}

	/// <summary>
	/// Selects an action based on its index.
	/// </summary>
	/// <param name="actionIndex">Index of the new action.</param>
	public void SelectAction(int actionIndex) {
		// Disable the current action.
		SetActionActive(CurrentAction, false);

		// Wrap around the index in case its over the amount of actions we have.
		if (actionIndex >= actions.Length)
			actionIndex = 0;

		// Enable the new current action.
		_actionIndex = actionIndex;
		SetActionActive(CurrentAction, true);
	}

	/// <summary>
	/// Sets the active state of an action <see cref="GameObject"/>.
	/// </summary>
	/// <param name="action">Action to have its active state changed.</param>
	/// <param name="state">New active state.</param>
	protected void SetActionActive(ActionBase action, bool state) {
		action.parentObject.SetActive(state);
	}

	public virtual float HorizontalLean() {
		return Input.GetAxis("Horizontal");
	}

	public virtual float VerticalMovement() {
		return Input.GetAxis("Vertical");
	}

	public virtual float HorizontalRotation() {
		return Input.GetAxis("Mouse X");
	}

	public virtual float VerticalRotation() {
		return Input.GetAxis("Mouse Y");
	}

	public virtual bool SwitchTool() {
		return Input.GetButtonDown("Switch Tool");
	}

	public virtual bool MainFire() {
		return Input.GetButton("Fire1");
	}

	public virtual bool Escape() {
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
	/// Current action index.
	/// </summary>
	private int ActionIndex {
		get { return _actionIndex; }
	}

	/// <summary>
	/// Current action of the player.
	/// </summary>
	public ActionBase CurrentAction {
		get { return actions[ActionIndex]; }
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
