using UnityEngine;
using System.Collections;

public class Loading_Level : MonoBehaviour {

    float timer;
    float maxDelay;

	// Use this for initialization
	void Start () {
        timer = 0;
        maxDelay = 2;

        if (timer < maxDelay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Application.LoadLevel("Level_01");
        }
               
    }
}
