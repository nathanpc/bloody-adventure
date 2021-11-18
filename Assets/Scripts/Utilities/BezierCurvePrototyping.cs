using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A helper class to make bezier curve prototyping in containers a lot easier.
/// </summary>
public class BezierCurvePrototyping : MonoBehaviour {
	public int samplePoints = 10;
	public bool showLines = true;
	public bool showSamplePoints = false;
	public float samplePointMarkerSize = 0.1f;
	public Color gizmoColor = Color.yellow;

	private void OnDrawGizmos() {
		// Do we even want something shown?
		if (!showLines && !showSamplePoints)
			return;

		// Draw whatever we want.
		Gizmos.color = gizmoColor;
		DrawBezierPathLines();
	}

	/// <summary>
	/// Draws Bezier debug lines for positioning waypoints.
	/// </summary>
	public void DrawBezierPathLines() {
		// Get the points to draw.
		List<Vector3> positions = GetPositions();
		if (positions.Count == 0)
			return;

		// Sample the bezier curve.
		List<Vector3> bezierPoints =
			BezierCurve.SampleCurveWithPoints(positions, samplePoints);

		// Draw the lines and the sampled points.
		for (int i = 0; i < bezierPoints.Count; i++) {
			if (showLines && (i > 0))
				Gizmos.DrawLine(bezierPoints[i - 1], bezierPoints[i]);

			if (showSamplePoints)
				Gizmos.DrawWireSphere(bezierPoints[i], samplePointMarkerSize);
		}
	}

	/// <summary>
	/// Gets a list of points from our child <see cref="GameObject"/>s.
	/// </summary>
	/// <returns>Positions list.</returns>
	private List<Vector3> GetPositions() {
		List<Vector3> positions = new List<Vector3>();

		// Shallow copy the list.
		foreach (Transform transform in GetComponentsInChildren<Transform>()) {
			// Ignore containers.
			if (transform.gameObject.tag == "Container")
				continue;

			positions.Add(transform.position);
		}

		return positions;
	}
}
