using UnityEngine;
using System.Collections;

public class CollisionEntity : MonoBehaviour
{
	[HideInInspector]public BoxCollider2D boxCollider;
	private Rect jumpRect;
	private float jumpRectHeight = 0.05f;
	private CharacterMovement chrMove;

	public virtual void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}

	void Start()
	{
		chrMove = GM.instance.charManager.gameObject.GetComponent<CharacterMovement>();
	}

	public virtual void Update()
	{
		// Jump rect 
		/*Vector2 scale = boxCollider.size;
		scale.y = jumpRectHeight;
		Vector3 centerPoint = new Vector3( boxCollider.offset.x, boxCollider.offset.y, 0f);
		
		Vector3 worldPos = transform.TransformPoint (boxCollider.offset);
		
		jumpRect = new Rect(0f, 0f, scale.x, scale.y);
		jumpRect.center = new Vector2(worldPos.x, worldPos.y + boxCollider.bounds.extents.y);*/
		
		/*topLeft = new Vector3( jumpRect.xMin, jumpRect.yMax, worldPos.z);
		topRight = new Vector3( jumpRect.xMax, jumpRect.yMax, worldPos.z);
		btmLeft = new Vector3( jumpRect.xMin, jumpRect.yMin, worldPos.z);
		btmRight = new Vector3( jumpRect.xMax, jumpRect.yMin, worldPos.z);*/
		//SetJumpRect ();
	}

	// DEBUG
	/*Vector3 topLeft;
	Vector3 topRight;
	Vector3 btmLeft;
	Vector3 btmRight;

	void OnDrawGizmos()
	{
		// DEBUG
		Gizmos.color = Color.red;
		//Gizmos.DrawCube (jumpRect.center, jumpRect.size);
		Gizmos.DrawLine (topLeft, topRight);
		Gizmos.DrawLine (topRight, btmRight);
		Gizmos.DrawLine (btmRight, btmLeft);
		Gizmos.DrawLine (btmLeft, topLeft);
	}*/

	public virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.ground)) return;

		//print (string.Format ("Entity collision [{0} -> {1}]", gameObject.tag, other.collider.tag));

		if (other.collider.CompareTag (Tags.headCollider) && gameObject.CompareTag (Tags.player)) // other = head & this = player
		{
			chrMove.Jump (true);
			other.transform.SendMessage ("OnJumpHit", SendMessageOptions.DontRequireReceiver);
		}
		else if (other.collider.CompareTag (Tags.player) && !gameObject.CompareTag (Tags.headCollider)) // other = player & this != head
		{
			// TODO Send hitmsg to player
			print ("Mama mia!");
		}
		else if (other.collider.CompareTag (Tags.enemy))// && gameObject.CompareTag (Tags.player)) // other = enemy & this = player
		{
			//print ("Hit " + other.gameObject.name + " collider");
			other.gameObject.SendMessage ("OnCollide", transform, SendMessageOptions.DontRequireReceiver);
		}
		else if (other.collider.CompareTag (Tags.powerup) && gameObject.CompareTag (Tags.player)) // other = powerup & this = player
		{
			// TODO Send pickupmsg to powerup
			other.gameObject.SendMessage ("OnPickup", SendMessageOptions.DontRequireReceiver);
		}
		else if (other.collider.CompareTag (Tags.block) && JumpRectContains (other.contacts[0].point)) // other = block & this = contains(contact)
		{
			other.gameObject.SendMessage ("OnHit", SendMessageOptions.DontRequireReceiver);
			//print ("Hit block!");
		}

		if (other.collider.CompareTag (Tags.enemy)) // other = enemy
		{
			//print ("oaisjdoijwf2");
			//gameObject.SendMessage ("OnShellCollision", transform, SendMessageOptions.DontRequireReceiver);
		}
	}

	IEnumerator DelayedJump()
	{
		yield return new WaitForEndOfFrame();
		chrMove.Jump (true);
	}

	void SetJumpRect()
	{
		// TODO Replace with a collider
		// Jump rect 
		Vector2 colliderSize = new Vector3 (boxCollider.size.x * 0.9f, jumpRectHeight);
		Vector3 worldPos = transform.TransformPoint (boxCollider.offset);
		jumpRect = new Rect(0f, 0f, colliderSize.x, colliderSize.y);
		jumpRect.center = new Vector2(worldPos.x, worldPos.y + boxCollider.bounds.extents.y);

		// DEBUG
		// TODO Remove
		/*topLeft = new Vector3( jumpRect.xMin, jumpRect.yMax, worldPos.z);
		topRight = new Vector3( jumpRect.xMax, jumpRect.yMax, worldPos.z);
		btmLeft = new Vector3( jumpRect.xMin, jumpRect.yMin, worldPos.z);
		btmRight = new Vector3( jumpRect.xMax, jumpRect.yMin, worldPos.z);*/
	}

	// Checks if the jumprect contains the point
	public bool JumpRectContains(Vector2 point)
	{
		SetJumpRect ();
		return jumpRect.Contains (point);
	}
}