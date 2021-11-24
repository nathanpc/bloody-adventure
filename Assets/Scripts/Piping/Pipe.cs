using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Defines a pipe object abstraction and keeps track of all the pipes around
/// the player.
/// </summary>
public class Pipe : MonoBehaviour {
	[SerializeField] private List<Pipe> nextPipes = null;
	[SerializeField] private Pipe previousPipe = null;
	[SerializeField] private GameObject middleConnection = null;
	[SerializeField] private GameObject pipesLoadingPoint = null;
	[SerializeField] private GameObject waypointContainer = null;
	[SerializeField] private Pipe nextPipeSelected = null;
	private TubeBuilder builder = null;
	private bool connectedWaypoints = false;

	public void Start() {
		NextPipes = new List<Pipe>();
		Builder = gameObject.GetComponentInParent<TubeBuilder>();
	}

	public void Update() {
		// Check if the player has already started to enter this pipe.
		if (!WaypointsConnected) {
			// Are we near the end?
			if (Builder.Follower.LastDestionationIsNear() &&
					(NextPipes.Count > 0)) {
				// Well, it's time for the player to choose its next path.
				SelectNextPipe();
				if (NextPipeSelected != null) {
					// Append the waypoints for the next pipe to guide the player.
					Builder.Follower.AppendWaypointContainer(
						NextPipeSelected.WaypointContainer);
				}

				// Done connecting the dots.
				WaypointsConnected = true;

				// Free up the previous pipe as we are already far enough from it.
				FreePreviousPipe();
			}
		}

		// Append the next pipes only when appropriate.
		if (NextPipes.Count == 0) {
			// Check if we are at the middle of the pipe.
			if (Vector3.Distance(PipesLoadingPoint.transform.position,
					Builder.Follower.controlledCharacter.transform.position) <= 0.5f) {
				AppendNextPipes();
			}
		}
	}

	/// <summary>
	/// Appends all available pipes to the module outputs.
	/// </summary>
	protected void AppendNextPipes() {
		// Instantiate a new pipe connected to the middle connector.
		GameObject pipeObject = Instantiate(Builder.GetNextPipe(),
			MiddleConnection.transform.position,
			MiddleConnection.transform.rotation, gameObject.transform.parent);
		
		// Add us as its previous pipe.
		Pipe pipe = pipeObject.GetComponent<Pipe>();
		pipe.PreviousPipe = this;

		// Add it to our next pipes list and select it as our next pipe to go.
		NextPipes.Add(pipe);
	}

	/// <summary>
	/// Frees up the previous previous pipe if there is one there.
	/// </summary>
	protected void FreePreviousPipe() {
		// Do we even have a previous pipe to nuke?
		if (PreviousPipe == null)
			return;

		// Nuke the bastard.
		PreviousPipe.Free(this);
		PreviousPipe = null;
	}

	/// <summary>
	/// Selects the next pipe the user wants to go through based on their
	/// tilt factor.
	/// </summary>
	public void SelectNextPipe() {
		// TODO: Tilt detection. :)
		if (NextPipes.Count == 0)
			Debug.LogWarning("For some reason we have no next pipes to select");

		// Select the next pipe using XKCD's approach to random selections.
		NextPipeSelected = NextPipes[0];
	}

	/// <summary>
	/// Frees up any resources being used by the pipe and its associated
	/// surroundings.
	/// </summary>
	/// <param name="protectedPipe">Pipe to protect from the impending doom.</param>
	public void Free(Pipe protectedPipe = null) {
		// Destroy the previous pipe.
		if (PreviousPipe != null)
			PreviousPipe.Free();

		// Destroy next pipes that aren't currently in use.
		foreach (Pipe pipe in NextPipes) {
			// Ignore null pipes.
			if (pipe == null)
				continue;

			// Protect a pipe.
			if (pipe == protectedPipe)
				continue;

			// Free it up.
			pipe.Free();
		}

		// Destroy ourself.
		Destroy(gameObject);
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

	/// <summary>
	/// Next pipe selected by the player to go through and have its waypoints
	/// connected.
	/// </summary>
	public Pipe NextPipeSelected {
		get { return nextPipeSelected; }
		set { nextPipeSelected = value; }
	}

	/// <summary>
	/// Path builder parent object.
	/// </summary>
	public TubeBuilder Builder {
		get { return builder; }
		set { builder = value; }
	}

	/// <summary>
	/// Point where we are free to append pipes to the current pipe.
	/// </summary>
	public GameObject PipesLoadingPoint {
		get { return pipesLoadingPoint; }
		set { pipesLoadingPoint = value; }
	}

	/// <summary>
	/// Pipe's internal waypoint container for the player's path.
	/// </summary>
	public GameObject WaypointContainer {
		get { return waypointContainer; }
		set { waypointContainer = value; }
	}

	/// <summary>
	/// Have we already connected the waypoints from this pipe to the next one
	/// that was selected by the user?
	/// </summary>
	public bool WaypointsConnected {
		get { return connectedWaypoints; }
		set { connectedWaypoints = value; }
	}
}
