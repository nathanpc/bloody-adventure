using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of every waypoint.
/// </summary>
public class WaypointBase : MonoBehaviour {
	protected GameObject controlledObject;
	protected PathFollower follower;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Sets the <see cref="PathFollower"/> object that reached this waypoint.
	/// </summary>
	/// <param name="follower"><see cref="PathFollower"/> that called this waypoint.</param>
	public void SetPathFollower(PathFollower follower) {
		this.follower = follower;
		controlledObject = follower.GetControlledCharacter();
	}

	/// <summary>
	/// Gets the type of waypoint.
	/// </summary>
	/// <returns>Name of the waypoint type.</returns>
	public virtual string WaypointType() {
		throw new NotImplementedException();
	}
}
