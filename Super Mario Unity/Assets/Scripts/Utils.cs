using UnityEngine;
using System.Collections;

public static class Utils
{
	/// <summary>
	/// Checks if the point is on the head
	/// </summary>
	/// <returns><c>true</c>, if the point is on the head, <c>false</c> otherwise.</returns>
	/// <param name="t">Transform.</param>
	/// <param name="point">Contact point.</param>
	/// <param name="margin">Margin.</param>
	/// <param name="offsetY">Collider y offset.</param>
	/// <param name="height">Height of collider.</param>
	public static bool ContactPointIsHead(this Transform t, Vector2 point, float margin, float offsetY = 0f, float height = 0.08f)
	{
		float width = 0.08f;
		return point.y > t.position.y + offsetY + (height - margin) && 
			(t.position.x - (width) < point.x && t.position.x + (width) > point.x);
	}
}