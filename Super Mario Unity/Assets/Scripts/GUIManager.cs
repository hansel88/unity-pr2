using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	public static GUIManager instance;

	void Awake()
	{
		instance = this;
	}
}