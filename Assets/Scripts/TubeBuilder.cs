using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ultimate class that will handle the actual creation of the world the
/// player will be inside of.
/// </summary>
public class TubeBuilder : MonoBehaviour {
	private GameObject pipeContainer = null;
	[SerializeField]
	private GameObject currentPipe = null;
	[SerializeField]
	private PathFollower follower = null;

	// Start is called before the first frame update
	void Start() {
		Container = gameObject;
		Follower.SetWaypointContainer(currentPipe.GetComponent<Pipe>().WaypointContainer);
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Object that will hold the artery objects in the scene.
	/// </summary>
	public GameObject Container {
		get { return pipeContainer; }
		set { pipeContainer = value; }
	}

	/// <summary>
	/// Current object that the player is currently inside of.
	/// </summary>
	public GameObject CurrentTube {
		get { return currentPipe; }
		set { currentPipe = value; }
	}

	/// <summary>
	/// Player's path following controller.
	/// </summary>
	public PathFollower Follower {
		get { return follower; }
		set { follower = value; }
	}
}
