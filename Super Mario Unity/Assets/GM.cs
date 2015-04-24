using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Timers;
using System;

public class GM : MonoBehaviour
{
    public static GM instance = null;

    private int score = 0;
    public int Score
    {
        get { return this.score; }
        set { this.score = value; }
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }


}
