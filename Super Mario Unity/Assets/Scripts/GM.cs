using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour
{
    public static GM instance = null;

    private int score = 0;
    public int Score
    {
        get { return this.score; }
        set { this.score = value; }
    }

    private int lives = 0;
    public int Lives
    {
        get { return this.lives; }
        set { this.lives = value; }
    }

    private int coins = 0;
    public int Coins
    {
        get { return this.coins; }
        set { this.coins = value; }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }


}
