using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstracts away a bezier curve in a nice way.
/// </summary>
public class BezierCurve : List<Vector3> {
	/// <summary>
	/// Constructs the Bezier curve object and already pre-populate the points
	/// for a quadratic curve.
	/// </summary>
	/// <param name="p0">First point of the curve.</param>
	/// <param name="p1">Middle point of the curve.</param>
	/// <param name="p2">Last point of the curve.</param>
	public BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2) : base() {
		Add(p0);
		Add(p1);
		Add(p2);
	}

	/// <summary>
	/// Samples a bezier curve from a list of points.
	/// </summary>
	/// <param name="points">List of points to use to create the curve.</param>
	/// <param name="samples">Number of samples to take for each section of the curve.</param>
	/// <returns>List of sampled points forming a nice bezier curve.</returns>
	public static List<Vector3> SampleCurveWithPoints(List<Vector3> points,
			int samples = 20) {
		List<Vector3> bezierPoints = new List<Vector3>();

		// Go through the points list sampling a nice smooth curve.
		while (points.Count >= 3) {
			// Build Bezier curve.
			BezierCurve bezier = new BezierCurve(points[0], points[1], points[2]);

			// Sample the curve.
			for (float i = 0.0f; i < (1f + (0.5f / samples)); i += (1 / samples)) {
				// Aren't floats beautiful?
				if (i > 1.0f)
					i = 1.0f;

				bezierPoints.Add(bezier.GetPointAt(i));
			}

			// Go to the next curve in the list.
			points.RemoveAt(0);
			points.RemoveAt(0);
		}

		return bezierPoints;
	}

	/// <summary>
	/// Adds a point to the bezier curve list of points to use for the calculation.
	/// </summary>
	/// <remarks>This is just an alias of <see cref="Add"/></remarks>
	/// <param name="point">New point to be added to the list.</param>
	public void AddPoint(Vector3 point) {
		Add(point);
	}

	/// <summary>
	/// Gets the <see cref="Vector3"/> point of the curve at a specific length in
	/// the curve.
	/// </summary>
	/// <param name="t">Where in the curve we want the point to get, from 0 to 1.</param>
	/// <returns>Point in the curve we requested.</returns>
	public Vector3 GetPointAt(float t) {
		Vector3 point = new Vector3();

		// Let's make sure T is in range.
		if ((t < 0.0f) || (t > 1.0f))
			throw new Exception("T variable out of range. Should be from 0 to 1.");

		// Check if we can handle the number of points.
		if (Count != 3)
			throw new Exception("Wrong number of points for calculation. Can only do 3 for now.");

		// Calculate points.
		point.x = ((1 - t) * (((1 - t) * this[0].x) + (t * this[1].x))) + (t * (((1 - t) * this[1].x) + (t * this[2].x)));
		point.y = ((1 - t) * (((1 - t) * this[0].y) + (t * this[1].y))) + (t * (((1 - t) * this[1].y) + (t * this[2].y)));
		point.z = ((1 - t) * (((1 - t) * this[0].z) + (t * this[1].z))) + (t * (((1 - t) * this[1].z) + (t * this[2].z)));

		return point;
	}
}
