using UnityEngine;
using System.Collections;

public class CollisionEntity : MonoBehaviour
{
	public BoxCollider2D boxCollider;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}
	public Rect topRect;
	public float size = 0.1f;
	public virtual void Update()
	{
		// old rect
		/*Bounds b = boxCollider.bounds;
		topRect = new Rect(b.center.x - b.extents.x, b.center.x + b.extents.x, 
		                   b.center.y - b.extents.y - size, b.center.y + b.extents.y + size);
		topRect.xMin = topRect.x;
		topRect.xMax = topRect.y;
		topRect.yMin = -topRect.x;
		topRect.yMax = -topRect.y;*/


		// rect 
		Vector2 scale = boxCollider.size;
		scale.y = size;
		Vector3 centerPoint = new Vector3( boxCollider.offset.x, boxCollider.offset.y, 0f);
		
		Vector3 worldPos = transform.TransformPoint (boxCollider.offset);
		
		topRect = new Rect(0f, 0f, scale.x, scale.y);
		topRect.center = new Vector2(worldPos.x, worldPos.y + boxCollider.bounds.extents.y);
		
		topLeft = new Vector3( topRect.xMin, topRect.yMax, worldPos.z);
		topRight = new Vector3( topRect.xMax, topRect.yMax, worldPos.z);
		btmLeft = new Vector3( topRect.xMin, topRect.yMin, worldPos.z);
		btmRight = new Vector3( topRect.xMax, topRect.yMin, worldPos.z); 
	}
	Vector3 topLeft;
	Vector3 topRight;
	Vector3 btmLeft;
	Vector3 btmRight;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		//Gizmos.DrawCube (topRect.center, topRect.size);
		Gizmos.DrawLine (topLeft, topRight);
		Gizmos.DrawLine (topRight, btmRight);
		Gizmos.DrawLine (btmRight, btmLeft);
		Gizmos.DrawLine (btmLeft, topLeft);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.collider.CompareTag (Tags.ground))
		{


			//Rect topRect = new Rect(b.center - b.extents.x, b.center + b.extents.x, 
			//                       b.center - b.extents.y - size, b.center + b.extents.y + size);

			if (other.collider.CompareTag (Tags.player))
			{
				// TODO Send hitmsg to player
				print ("Hit player");
			}
			else if (other.collider.CompareTag (Tags.enemy))
			{
				// OR compare to the entitys colliders (store reference on them)
				/*print (other.collider);
				return;
				if (other.collider.GetType () == typeof(EdgeCollider2D) && gameObject.CompareTag (Tags.player))
				{
					print ("Hit " + other.gameObject.name + " edgecollider");
				}
				else if (other.collider.GetType () == typeof(BoxCollider2D))
				{
					print ("Hit " + other.gameObject.name + " boxcollider");
				}*/
				if (other.collider.GetComponent<CollisionEntity>().topRect.Contains (other.contacts[0].point)
				    && gameObject.CompareTag (Tags.player))
				{
					print ("Hit " + other.gameObject.name + " toprect");
					// TODO Jump player
					other.gameObject.SendMessage ("OnJumpHit", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					print ("Hit " + other.gameObject.name + " collider");
					other.gameObject.SendMessage ("OnCollide", SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (other.collider.CompareTag (Tags.powerup))
			{
				// TODO Send pickupmsg to powerup
				print ("Hit powerup");
			}
		}
	}
}