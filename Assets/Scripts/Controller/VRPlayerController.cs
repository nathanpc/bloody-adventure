using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller abstraction class for a VR controller.
/// </summary>
public class VRPlayerController : ControllerBase {
	public override bool MainFire() {
		return Input.GetAxis("Trigger") >= 0.99f;
	}
}
