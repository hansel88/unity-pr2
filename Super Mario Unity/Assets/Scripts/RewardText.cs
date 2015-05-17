using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Handles the movement for the pickuptext
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
		// Set the text
		textReward.text = text;

		// Set the parent and position the object
		transform.SetParent (parent, false);
		transform.position = pos;

		// Activate
		gameObject.SetActive (true);

		// Deactivate in a certian amount of time
		Invoke ("Deactivate", duration);
	}

	void Deactivate()
	{
		gameObject.SetActive (false);
	}
}