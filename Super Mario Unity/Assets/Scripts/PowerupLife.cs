using UnityEngine;
using System.Collections;

public class PowerupLife : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();
		
		GM.instance.Lives ++;
		GUIManager.instance.PopRewardText(transform.position, "+1up");
	}
}