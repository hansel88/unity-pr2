using UnityEngine;
using System.Collections;

public static class Utils
{

	public static bool ContactPointIsHead(this Transform t, Vector2 point, float margin, float offsetY = 0f, float height = 0.08f)
	{
		float width = 0.08f;
		return point.y > t.position.y + offsetY + (height - margin) && 
			(t.position.x - (width) < point.x && t.position.x + (width) > point.x);
	}
}

/*using UnityEngine;
using System.Collections;

public static class Utils
{
	// Checks if the point inside the x bounds of the collider and above the head
	// TODO Use collider as this parameter
	public static bool ContactPointIsHead(this BoxCollider2D c, Vector2 point, float margin)
	{
		Vector2 size = c.size;
		Vector2 offset = c.offset;
		Transform t = c.transform;
		return point.y > t.position.y + offset.y + size.y - margin && 
			(t.position.x - (size.x) < point.x && t.position.x + (size.x) > point.x);
	}
}*/