using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ultimate class that will handle the actual creation of the world the
/// player will be inside of.
/// </summary>
public class TubeBuilder : MonoBehaviour {
	[SerializeField]
	private GameObject pipeContainer = null;
	[SerializeField]
	private GameObject currentPipe = null;

	// Start is called before the first frame update
	void Start() {

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
}
