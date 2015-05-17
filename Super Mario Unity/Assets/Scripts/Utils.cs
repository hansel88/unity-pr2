using UnityEngine;
using System.Collections;

// Helper functions not fit for any perticular object
public static class Utils
{
	// Checks if the contactpoint is on the head location
	public static bool ContactPointIsHead(this Transform t, Vector2 point, float margin, float offsetY = 0f, float height = 0.08f)
	{
		float width = 0.08f;
		return point.y > t.position.y + offsetY + (height - margin) && 
			(t.position.x - (width) < point.x && t.position.x + (width) > point.x);
	}

	// Save and load the number of lives
	public static void SaveLives(int lives)
	{
		PlayerPrefs.SetInt ("Lives", lives);
	}
	public static int LoadLives()
	{
		return PlayerPrefs.GetInt ("Lives", 3);
	}
}