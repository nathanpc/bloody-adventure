using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple game cycle control utility class.
/// </summary>
public static class GameControl {
	/// <summary>
	/// Quits the game.
	/// </summary>
	public static void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif  // UNITY_EDITOR
	}
}
