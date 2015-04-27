using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour
{
    public static GM instance = null;

    #region score, lives and coins properties
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

	private int timer = 0;
	public int Timer
	{
		get { return this.timer; }
		set { this.timer = value; GUIManager.instance.ChangeTimeText (value);}
	}
    #endregion

    #region status and powerup properties

    public enum MarioStatus { Big, Small };

    private MarioStatus characterStatus;
    public MarioStatus CharacterStatus
    {
        get { return this.characterStatus; }
        set { this.characterStatus = value; }
    }

    private bool hasFireFly = false;
    public bool HasFireFly
    {
        get { return this.hasFireFly; }
        set { this.hasFireFly = value; }
    }

    private bool hasStar = false;
    public bool HasStar
    {
        get { return this.hasStar; }
        set { this.hasStar = value; }
    }

    private bool hasMushroom = false;
    public bool HasMushroom
    {
        get { return this.hasMushroom; }
        set { this.hasMushroom = value; }
    }
#endregion

	private float currentCountdownTime = 0;
	private const float secondRatio = 0.4f; // Seconds per in-game seconds
	private const int totalTime = 400; // Total time for a level

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

		ResetCountdown ();
    }

	void Update()
	{
		DoCountdown ();
	}

    #region methods
    private void checkGameOver()
    {
        //TODO
    }

	void DoCountdown()
	{
		currentCountdownTime += Time.deltaTime;
        if (Application.loadedLevel == 0)
        {
            if (currentCountdownTime >= secondRatio)
            {
                Timer -= 1;
                currentCountdownTime = 0f;
                CheckTimer();
            }
        }
	}

	void CheckTimer()
	{
		if (Timer <= 0)
		{
			// TODO Gameover
		}
	}

	void ResetCountdown()
	{
		Timer = totalTime;
	}
    #endregion

}
