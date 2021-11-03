using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a <see cref="GameObject"/> that will be following
/// the child objects of another <see cref="GameObject"/>.
/// </summary>
public class PathFollower : MonoBehaviour {
	public GameObject waypointContainer;
	public float movementSpeed = 2.0f;
	public float rotationSpeed = 1.0f;
	public bool ignoreY = true;
	public bool ignoreRotation = false;
	public bool useWaypointRotation = false;
	public bool showDebug = false;
	private GameObject controlledCharacter;
	private Transform tCharacter;
	private List<Transform> targets;
	private Vector3 characterForward;

	// Start is called before the first frame update
	private void Start() {
		// Initialize some properties.
		targets = new List<Transform>();
		controlledCharacter = this.gameObject;
		tCharacter = controlledCharacter.transform;
		characterForward = tCharacter.forward;
	}

	// Update is called once per frame
	private void Update() {
		// Check if we have reached our final destination.
		if (IsAtFinalDestination()) {
			if (showDebug)
				Debug.Log("Path Follower: Final waypoint no last animation play stop animation");

			return;
		}

		// Move to the current waypoint.
		MoveTowardsWaypoint();

		// Check if we have reached our waypoint.
		if (IsAtWaypoint()) {
			if (showDebug)
				Debug.Log("Arrived at: " + targets[0].gameObject.name);

			// Move to the next waypoint.
			MoveToNextWaypoint();
		}
	}

	/// <summary>
	/// Sets the current waypoints container <see cref="GameObject"/>
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	public void SetWaypointContainer(GameObject waypointsParent) {
		// Go through GameObject childs.
		foreach (Transform transform in waypointsParent.GetComponentsInChildren<Transform>()) {
			// Ignore if we got ourself.
			if (transform.gameObject == waypointsParent)
				continue;

			// Add to the list of targets.
			targets.Add(transform);
		}
	}

	/// <summary>
	/// Moves the character towards the current waypoint.
	/// </summary>
	private void MoveTowardsWaypoint() {
		// Just sit still if we are waiting at the waypoint for a timer to finish.
		if (IsAtWaypoint(true))
			return;

		// Fix problems with glitches when ignoring the Y.
		if (ignoreY)
			targets[0].position = new Vector3(targets[0].position.x, tCharacter.position.y,
				targets[0].position.z);

		// Move our object towards the waypoint.
		float moveStep = movementSpeed * Time.deltaTime;
		tCharacter.position = Vector3.MoveTowards(tCharacter.position, targets[0].position, moveStep);

		// Rotate the object towards the waypoint.
		if (!ignoreRotation) {
			float rotationStep = rotationSpeed * Time.deltaTime;

			// Use waypoint rotation?
			if (useWaypointRotation) {
				tCharacter.rotation = Quaternion.Lerp(tCharacter.rotation, targets[0].rotation, rotationStep);
			} else {
				// Rotate towards the next waypoint.
				//Vector3 direction = targets[0].position - tCharacter.position;
				//Quaternion toRotation = Quaternion.FromToRotation(characterForward, direction);
				//tCharacter.rotation = Quaternion.RotateTowards(tCharacter.rotation, toRotation, 10);
			}
		}
	}

	/// <summary>
	/// Starts moving the character to its next waypoint.
	/// </summary>
	public void MoveToNextWaypoint() {
		// Remember the last animation and waypoint.
		WaypointBase lastWaypoint = targets[0].GetComponent<WaypointBase>();

		// Remove the first element to go to the next one.
		targets.RemoveAt(0);

		characterForward = tCharacter.forward;
		if (targets.Count > 0) {
			if (!useWaypointRotation)
				tCharacter.LookAt(targets[0].position);

			if (lastWaypoint == null)
				return;

			if (showDebug)
				Debug.Log("Path Follower: Move to next waypoint play run animation");
		}
	}

	/// <summary>
	/// Checks if the target character is at the current waypoint.
	/// </summary>
	/// <param name="ignoreTimer">Should we ignore the timer?</param>
	/// <returns><code>True</code> if the character is at the current waypoint.</returns>
	public bool IsAtWaypoint(bool ignoreTimer = false) {
		bool atWaypoint = false;
		Vector3 posCharacter = tCharacter.position;
		Vector3 posTarget = targets[0].position;

		// Ignore the Y coordinate?
		if (ignoreY) {
			posCharacter.y = 0;
			posTarget.y = 0;
		}

		// Check if we have reached the waypoint already.
		atWaypoint = Vector3.Distance(posCharacter, posTarget) < 0.1f;
		if (ignoreTimer)
			return atWaypoint;

		if (atWaypoint) {
			// Stop for a while?
			WaypointStop stop = targets[0].GetComponent<WaypointStop>();
			if (stop != null) {
				stop.SetPathFollower(this);
				stop.StartTimer();
			}

			// A change of speed maybe?
			WaypointSpeed speed = targets[0].GetComponent<WaypointSpeed>();
			if (speed != null) {
				speed.SetPathFollower(this);
				speed.SetPathFollowerSpeeds();
			}

			// Make sure we can continue after a stop.
			if (stop != null)
				return stop.TimesUp();
		}

		return atWaypoint;
	}

	/// <summary>
	/// Checks if the target character is at its final destination.
	/// </summary>
	/// <returns><code>True</code> if the character is at its final destination.</returns>
	public bool IsAtFinalDestination() {
		return targets.Count == 0;
	}

	/// <summary>
	/// Gets the <see cref="GameObject"/> of the character that this path follower is controlling.
	/// </summary>
	/// <returns><see cref="GameObject"/> of the character.</returns>
	public GameObject GetControlledCharacter() {
		return controlledCharacter;
	}
}