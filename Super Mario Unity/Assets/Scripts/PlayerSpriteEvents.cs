using UnityEngine;
using System.Collections;

public class PlayerSpriteEvents : MonoBehaviour
{
	public void PowerupDone()
	{
		GM.instance.UnFreezeEntities ();
	}
}