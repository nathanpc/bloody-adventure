using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A little utility class to help configure some platform-specific behaviour
/// and tailor the game to each platform.
/// </summary>
public class PlatformConfigurator : MonoBehaviour {
	[Header("Controller")]
	public DesktopPlayerController desktopController;
	public VRPlayerController vrController;
	public MobilePlayerController mobileController;
	public bool usingVR = false;
	[Header("Camera")]
	public GameObject regularCamera;
	public GameObject vrCameraRig;
	[Header("Effects")]
	public List<GameObject> fog;

	// Start is called before the first frame update
	void Start() {
		// Determine the optimal configuration based on the runtime platform.
		switch (Application.platform) {
		// Desktop
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.LinuxEditor:
		case RuntimePlatform.LinuxPlayer:
		case RuntimePlatform.WindowsEditor:
		case RuntimePlatform.WindowsPlayer:
			SetupForDesktop();
			if (usingVR)
				SetupVirtualReality();

			break;
		// Mobile
		case RuntimePlatform.IPhonePlayer:
		case RuntimePlatform.Android:
			SetupForMobile();
			break;
		// Others
		default:
			throw new System.Exception("Unsupported platform. Please implement it in " +
				"the PlatformConfigurator.");
		}
	}

	/// <summary>
	/// Sets up everything to run on a desktop.
	/// </summary>
	public void SetupForDesktop() {
		// Camera
		regularCamera.SetActive(true);
		vrCameraRig.SetActive(false);

		// Controller.
		mobileController.enabled = false;
		desktopController.enabled = true;

		// Effects.
		ChangeFogState(true);
	}

	/// <summary>
	/// Sets up everything for VR. You must first setup the actual platform
	/// like desktop or mobile before calling this.
	/// </summary>
	public void SetupVirtualReality() {
		// Camera
		regularCamera.SetActive(false);
		vrCameraRig.SetActive(true);

		// Controller.
		desktopController.enabled = false;
		vrController.enabled = true;

		// Effects.
		ChangeFogState(true);
	}

	/// <summary>
	/// Sets up everything to run on a mobile device.
	/// </summary>
	public void SetupForMobile() {
		// Camera
		regularCamera.SetActive(true);
		vrCameraRig.SetActive(false);

		// Controller.
		desktopController.enabled = false;
		mobileController.enabled = true;

		// Effects.
		ChangeFogState(false);
	}

	/// <summary>
	/// Sets the state of the fog effect objects.
	/// </summary>
	/// <param name="state">Should we activate them?</param>
	private void ChangeFogState(bool state) {
		foreach (GameObject obj in fog) {
			gameObject.SetActive(state);
		}
	}
}
