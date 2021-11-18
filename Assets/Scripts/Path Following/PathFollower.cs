using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a <see cref="GameObject"/> that will be following
/// the child objects of another <see cref="GameObject"/>.
/// </summary>
public class PathFollower : MonoBehaviour {
	public GameObject controlledCharacter = null;
	public float movementSpeed = 2.0f;
	public float rotationSpeed = 1.0f;
	public bool ignoreRotation = false;
	public bool showDebug = false;
	public int bezierCurveSamples = 20;
	private Transform tCharacter;
	public List<Vector3> targets;
	private Vector3 characterForward;

	private void Start() {
		// Initialize some properties.
		targets = new List<Vector3>();
		if (controlledCharacter == null)
			controlledCharacter = this.gameObject;
		tCharacter = controlledCharacter.transform;
		characterForward = tCharacter.forward;
	}

	private void FixedUpdate() {
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
				Debug.Log("Arrived at: " + targets[0]);

			// Move to the next waypoint.
			MoveToNextWaypoint();
		}
	}

	// Draw gizmos in the editor.
	private void OnDrawGizmos() {
		DrawBezierPathLines(Color.yellow);
	}

	/*
	/// <summary>
	/// Gets waypoints transforms from a given container <see cref="GameObject"/>.
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	/// <returns>Waypoints tranforms list.</returns>
	private List<Transform> GetWaypointsTransforms(GameObject waypointsParent) {
		List<Transform> transforms = new List<Transform>();

		// Shallow copy the list.
		foreach (Transform transform in waypointsParent.GetComponentsInChildren<Transform>()) {
			// Ignore containers.
			if (transform.gameObject.tag == "Container")
				continue;

			transforms.Add(transform);
		}

		return transforms;
	}
	*/

	/// <summary>
	/// Gets waypoints positions from a given container <see cref="GameObject"/>.
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	/// <returns>Waypoints positions list.</returns>
	private List<Vector3> GetWaypointsPositions(GameObject waypointsParent) {
		List<Vector3> positions = new List<Vector3>();

		// Shallow copy the list.
		foreach (Transform transform in waypointsParent.GetComponentsInChildren<Transform>()) {
			// Ignore containers.
			if (transform.gameObject.tag == "Container")
				continue;

			positions.Add(transform.position);
		}

		return positions;
	}

	/// <summary>
	/// Sets the current waypoints container <see cref="GameObject"/>
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	public void SetWaypointContainer(GameObject waypointsParent) {
		List<Vector3> positions = GetWaypointsPositions(waypointsParent);
		List<Vector3> bezierPoints =
			BezierCurve.SampleCurveWithPoints(positions, bezierCurveSamples);

		targets.AddRange(bezierPoints);
	}

	/// <summary>
	/// Appends a new waypoints container <see cref="GameObject"/> to the
	/// current waypoint list.
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints to be appended.</param>
	public void AppendWaypointContainer(GameObject waypointsParent) {
		// Yeah, it's just an alias for now. When I have time I'll clean this
		// horrible crap up.
		SetWaypointContainer(waypointsParent);
	}

	/// <summary>
	/// Moves the character towards the current waypoint.
	/// </summary>
	private void MoveTowardsWaypoint() {
		// Just sit still if we are waiting at the waypoint for a timer to finish.
		if (IsAtWaypoint(true))
			return;

		// Move our object towards the waypoint.
		float moveStep = movementSpeed * Time.deltaTime;
		tCharacter.position = Vector3.MoveTowards(tCharacter.position, targets[0], moveStep);

		// Rotate the object towards the waypoint.
		if (!ignoreRotation) {
			float rotationStep = rotationSpeed * Time.deltaTime;

			Vector3 newDirection = Vector3.RotateTowards(tCharacter.forward,
				targets[0] - tCharacter.position, rotationStep, 0.0f);
			tCharacter.rotation = Quaternion.LookRotation(newDirection);
		}
	}

	/// <summary>
	/// Starts moving the character to its next waypoint.
	/// </summary>
	public void MoveToNextWaypoint() {
		// Remove the first element to go to the next one.
		targets.RemoveAt(0);

		characterForward = tCharacter.forward;
		if (targets.Count > 0) {
			// Look at the next target.
			//tCharacter.LookAt(targets[0]);

			//if (lastWaypoint == null)
			//	return;

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
		Vector3 posTarget = targets[0];

		// Check if we have reached the waypoint already.
		atWaypoint = Vector3.Distance(posCharacter, posTarget) < 0.1f;
		if (ignoreTimer)
			return atWaypoint;

		if (atWaypoint) {
			/*
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
			*/
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
	/// Checks if the target character is near its final destination.
	/// </summary>
	/// <param name="closeEnoughDistance">What distance do we think is near
	/// enough.</param>
	/// <returns><code>True</code> if the character is near its final
	/// destination.</returns>
	public bool LastDestionationIsNear(float closeEnoughDistance = 0.15f) {
		// If the next target is not the last one, then we have nothing to do.
		if (targets.Count != 1)
			return false;

		return Vector3.Distance(tCharacter.position, targets[0])
			< closeEnoughDistance;
	}

	/// <summary>
	/// Gets the <see cref="GameObject"/> of the character that this path follower is controlling.
	/// </summary>
	/// <returns><see cref="GameObject"/> of the character.</returns>
	public GameObject GetControlledCharacter() {
		return controlledCharacter;
	}

	/// <summary>
	/// Draws Bezier debug lines for positioning waypoints.
	/// </summary>
	public void DrawBezierPathLines(Color gizmoColor) {
		if (targets.Count == 0)
			return;

		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere(targets[0], 0.1f);
		for (int i = 1; i < targets.Count; i++) {
			Gizmos.DrawLine(targets[i - 1], targets[i]);
			Gizmos.DrawSphere(targets[i], 0.1f);
		}
	}
}
