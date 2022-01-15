using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller abstraction class for a "normal" computer controller,
/// aka keyboard and mouse.
/// </summary>
public class DesktopPlayerController : ControllerBase {
	public override float HorizontalRotation() {
		return Input.GetAxis("Mouse X");
	}

	public override float VerticalRotation() {
		return Input.GetAxis("Mouse Y");
	}
}
