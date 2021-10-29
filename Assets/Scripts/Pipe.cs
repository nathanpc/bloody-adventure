using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Defines a pipe object abstraction and keeps track of all the pipes around
/// the player.
/// </summary>
public class Pipe : MonoBehaviour {
	[SerializeField]
	private List<Pipe> nextPipes = null;
	[SerializeField]
	private Pipe previousPipe = null;
	[SerializeField]
	private GameObject middleConnection = null;
	public bool appe = false;

	public void Start() {
		NextPipes = new List<Pipe>();
		AppendPipes();
	}

	public void Update() {
		
	}

	/// <summary>
	/// Appends all available pipes to the module outputs.
	/// </summary>
	public void AppendPipes() {
		if (!appe)
			return;
		// Middle connected pipe.
		GameObject pipeObject = Instantiate((GameObject)Resources.Load("Prefabs/Test/StraightPipeTest",
			typeof(GameObject)), MiddleConnection.transform.position,
			MiddleConnection.transform.rotation);
		pipeObject.transform.parent = gameObject.transform.parent;
		Pipe pipe = pipeObject.GetComponent<Pipe>();
		pipe.previousPipe = this;
		nextPipes.Add(pipe);
	}

	/// <summary>
	/// Frees up any resources being used by the pipe and its associated
	/// surroundings.
	/// </summary>
	public void Free() {
		// Destroy the previous pipe.
		if (previousPipe != null)
			previousPipe.Free();

		// Destroy next pipes that aren't currently in use.
		foreach (Pipe pipe in NextPipes) {
			// Ignore null pipes.
			if (pipe == null)
				continue;

			if (!pipe.InUse)
				pipe.Free();
		}

		// Destroy ourself.
		Destroy(gameObject);
	}

	/// <summary>
	/// Checks if the pipe is currently in use.
	/// </summary>
	public bool InUse {
		get { return false; }
	}

	/// <summary>
	/// Middle connection point for a pipe.
	/// </summary>
	public GameObject MiddleConnection {
		get { return middleConnection; }
		set { middleConnection = value; }
	}

	/// <summary>
	/// List of pipes the player has the option of going through next.
	/// </summary>
	public List<Pipe> NextPipes {
		get { return nextPipes; }
		set { nextPipes = value; }
	}

	/// <summary>
	/// Previous object the player was inside of to get here.
	/// </summary>
	public Pipe PreviousPipe {
		get { return previousPipe; }
		set { previousPipe = value; }
	}
}
