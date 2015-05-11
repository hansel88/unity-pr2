using UnityEngine;
using System.Collections;

public class BlockPowerup : Block
{
	public enum BlockContent {Coin, Mushroom, Fireflower, Star, OneUp};
	public BlockContent content = BlockContent.Coin;
	public int activateCount = 1;
	public GameObject[] itemPrefabs;
	public Transform itemMoverParent;
	private bool isActive = true;
	private int curActivateCount;
	private Powerup curPowerup;

	 void OnActivate(CharacterManager charManager)
	{
		isActive = curActivateCount < activateCount;
		if (!isActive) return;
		curActivateCount ++;

		if (content == BlockContent.Coin)
		{
			// TODO Pop coin
			Instantiate (itemPrefabs[(int)content], itemMoverParent.position, Quaternion.identity);
		}
		else
		{
			// Change item to fireflower if player already has mushroom or fireflower
			if (content == BlockContent.Mushroom && (int)charManager.curState >= (int)PlayerState.Mushroom) 
			{
				content = BlockContent.Fireflower;
			}

			//GetComponent<BoxCollider2D>().enabled = false;
			GameObject powerupObj = Instantiate (itemPrefabs[(int)content], itemMoverParent.position, Quaternion.identity) as GameObject;
			curPowerup = powerupObj.GetComponent<Powerup>();
			curPowerup.canMove = false;
			curPowerup.boxCollider.enabled = false;
			//curPowerup.SetActiveState (false);
			curPowerup.isActive = false;
			curPowerup.rBody.isKinematic = true;
			powerupObj.transform.SetParent (itemMoverParent);
		}

		base.OnActivate (curActivateCount >= activateCount);
	}

	public void StartPowerup()
	{
		if (curPowerup)
		{
			curPowerup.canMove = true;
			curPowerup.rBody.isKinematic = false;
			curPowerup.isActive = true;
			//GetComponent<BoxCollider2D>().enabled = true;
			curPowerup.boxCollider.enabled = true;
			curPowerup.transform.SetParent (null);
			curPowerup = null;
			//curPowerup.rBody.isKinematic = false;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!isActive) return;

		if (other.collider.CompareTag (Tags.player))
		{
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			//if (charManager.ValidHeadHit (other.contacts[0].point, charManager.charCollider))
			if (charManager.ValidHeadHit (other.contacts[0].point, charManager.transform.position, charManager.charCollider.size,
			                              charManager.charCollider.offset, charManager.charCollider.bounds));
			{
				OnActivate (charManager);
			}
		}
		else
		{

		}
	}
}