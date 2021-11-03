using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of utilities that Unity should've implemented to make our life
/// a lot easier.
/// </summary>
public static class UnityManipulator {
	/// <summary>
	/// Converts a list of <see cref="Transform"/> into a list of their positions.
	/// </summary>
	/// <param name="list">Transforms list.</param>
	/// <returns>List with Transforms positions.</returns>
	public static List<Vector3> ListTransformsPosition(List<Transform> list) {
		List<Vector3> posList = new List<Vector3>();

		foreach (Transform transform in list)
			posList.Add(transform.position);

		return posList;
	}
}
