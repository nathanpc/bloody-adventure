using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a <see cref="GameObject"/> that will be following
/// the child objects of another <see cref="GameObject"/>.
/// </summary>
public class PathFollower : MonoBehaviour {
	public GameObject controlledCharacter = null;
	public GameObject waypointContainer = null;
	public float movementSpeed = 2.0f;
	public float rotationSpeed = 1.0f;
	public bool autoStart = false;
	public bool ignoreY = true;
	public bool ignoreRotation = false;
	public bool useWaypointRotation = false;
	public bool showDebug = false;
	private Transform tCharacter;
	private List<Transform> targets;
	private Vector3 characterForward;

	// Start is called before the first frame update
	private void Start() {
		// Initialize some properties.
		targets = new List<Transform>();
		if (controlledCharacter == null)
			controlledCharacter = this.gameObject;
		tCharacter = controlledCharacter.transform;
		characterForward = tCharacter.forward;

		// Auto start the path following?
		if (autoStart) {
			// Do we even have a waypoint container.
			if (waypointContainer == null)
				return;

			SetWaypointContainer(waypointContainer);
		}
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

	// Draw gizmos in the editor.
	private void OnDrawGizmos() {
		DrawBezierPathLines(Color.yellow);
	}

	/// <summary>
	/// Draws Bezier debug lines for positioning waypoints.
	/// </summary>
	public void DrawBezierPathLines(Color gizmoColor) {
		List<Vector3> points = BezierCurve.SampleCurveWithPoints(
			UnityManipulator.ListTransformsPosition(GetWaypointsTransforms(waypointContainer)));
		Gizmos.color = gizmoColor;

		for (int i = 1; i < points.Count; i++) {
			Gizmos.DrawLine(points[i - 1], points[i]);
		}
	}

	/// <summary>
	/// Draws straight debug lines for positioning waypoints.
	/// </summary>
	public void DrawStraightPathLines(Color gizmoColor) {
		// Go through GameObject childs.
		Vector3 startPos = controlledCharacter.transform.position;
		foreach (Transform transform in GetWaypointsTransforms(waypointContainer)) {
			// Show a nice line between the waypoints.
			Gizmos.color = gizmoColor;
			Gizmos.DrawLine(startPos, transform.position);
			Gizmos.DrawWireSphere(transform.position, 0.1f);

			startPos = transform.position;
		}
	}

	/// <summary>
	/// Gets waypoints tranforms from a given container <see cref="GameObject"/>.
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	private List<Transform> GetWaypointsTransforms(GameObject waypointsParent) {
		List<Transform> transforms = new List<Transform>();

		// Shallow copy the list.
		foreach (Transform transform in waypointsParent.GetComponentsInChildren<Transform>()) {
			// Ignore ourself.
			if (transform.gameObject == waypointContainer)
				continue;

			transforms.Add(transform);
		}

		return transforms;
	}

	/// <summary>
	/// Sets the current waypoints container <see cref="GameObject"/>
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	public void SetWaypointContainer(GameObject waypointsParent) {
		targets.AddRange(GetWaypointsTransforms(waypointsParent));
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
