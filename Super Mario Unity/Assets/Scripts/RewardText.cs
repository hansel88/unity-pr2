using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RewardText : MonoBehaviour
{
	public float speed = 10f;
	public float duration = 1f;
	public Text textReward;

	void Update()
	{
		transform.position += Vector3.up * speed * Time.deltaTime;
	}

	public void Initialize(string text, Vector3 pos, Transform parent)
	{
		textReward.text = text;
		transform.SetParent (parent, false);
		transform.position = pos;
		gameObject.SetActive (true);
		Invoke ("Deactivate", duration);
	}

	void Deactivate()
	{
		gameObject.SetActive (false);
	}
}