using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ultimate class that will handle the actual creation of the world the
/// player will be inside of.
/// </summary>
public class TubeBuilder : MonoBehaviour {
	[SerializeField] private GameObject currentPipe = null;
	[SerializeField] private PathFollower follower = null;
	[SerializeField] private List<GameObject> pipePrefabs = null;
	private GameObject pipeContainer = null;

	// Start is called before the first frame update
	void Start() {
		Container = gameObject;
		if (Prefabs == null)
			Prefabs = new List<GameObject>();

		// Starts entering the first pipe.
		Follower.SetWaypointContainer(
			currentPipe.GetComponent<Pipe>().WaypointContainer);
	}

	/// <summary>
	/// Get a random pipe from the prefabs list.
	/// </summary>
	/// <returns>Random pipe to continue the user's journey.</returns>
	public GameObject GetNextPipe() {
		return Prefabs[Random.Range(0, Prefabs.Count)];
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

	/// <summary>
	/// Pipe prefabs to build the veins with.
	/// </summary>
	public List<GameObject> Prefabs {
		get { return pipePrefabs; }
		set { pipePrefabs = value; }
	}
}
