using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	public int activateReward = 0;
	private Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OnActivate(bool disableBlock)
	{
		if (activateReward > 0)
		{
			GM.instance.Score += activateReward;
			GUIManager.instance.PopRewardText (transform.position, "+" + activateReward);
		}
		anim.SetTrigger ("ActivateTrigger");
		anim.SetBool ("DisableBlock", disableBlock);
	}
}