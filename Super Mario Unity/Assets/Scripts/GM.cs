﻿using UnityEngine;
using System.Collections;
using System.Threading;

public class GM : MonoBehaviour
{
    public static GM instance = null;

    #region score, lives and coins properties
    private int score = 0;
    public int Score
    {
        get { return this.score; }
        set 
		{ 
			this.score = value; 
			GUIManager.instance.ChangeScoreText (value);
		}
    }

	private int lives = 3;
    public int Lives
    {
        get { return this.lives; }
        set 
		{ 
			this.lives = value;
			Utils.SaveLives (value);
		}
    }

    private int coins = 0;
    public int Coins
    {
        get { return this.coins; }
        set 
		{
			this.coins = value; 
			GUIManager.instance.ChangeCoinText (value);
		}
    }

	private int timer = 0;
	public int Timer
	{
		get { return this.timer; }
		set 
		{ 
			this.timer = value; 
			GUIManager.instance.ChangeTimeText (value);
		}
	}
    #endregion

    public GameObject player;
	private bool playerIsAlive = true;
	public bool PlayerIsAlive   
	{ 
		get { return playerIsAlive; } 
		set { playerIsAlive = value; charMove.canMove = value; }
	}
	public bool frozenEntities = false;
	public bool frozenEntitiesCooldown = false;
	private float currentCountdownTime = 0;
	private const float secondRatio = 0.4f; // Seconds per in-game seconds
	private const int totalTime = 400; // Total time for a level
	private CharacterMovement charMove;
	[HideInInspector]public CharacterManager charManager;
	[HideInInspector]public AudioSource source;
    public GameObject gameOverSound;
    public GameObject timerWarningSound;
    public GameObject fireworksSound;
    public GameObject stageClear;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

		Lives = Utils.LoadLives ();

		if (!player)
		{
			player = GameObject.FindGameObjectWithTag (Tags.player);
		}
		charManager = player.GetComponent<CharacterManager>();
		charMove = player.GetComponent<CharacterMovement>();

		ResetCountdown ();

		UpdateScreenBounds ();

		source = GetComponent<AudioSource>();
    }

	void Update()
	{
		DoTimerCountdown ();
	}

    #region methods
    public void CheckGameOver()
    {
        saveHighScore();
		if (Timer <= 0)
		{
			Application.LoadLevel ("Time_Up");
		}
		if (Lives <= 0) // Out of lives --> game over
		{
       		Destroy(Instantiate(gameOverSound), 10);
			Application.LoadLevel ("Game_Over");
		}
		else // Have more lives --> continue
		{
			Application.LoadLevel ("Loading_Level");
		}
    }

	public IEnumerator StageClear()
	{
        saveHighScore();
		Instantiate(stageClear);
		Instantiate(fireworksSound);
		yield return new WaitForSeconds(7f);

		Application.LoadLevel ("Main_Title");
	}

	void DoTimerCountdown()
	{
		// Don't countdown if player is dead
		if (!playerIsAlive) return;

		// Countdown
		currentCountdownTime += Time.deltaTime;
		if (currentCountdownTime >= secondRatio)
		{
			Timer --;
			currentCountdownTime = 0f;
			CheckTimer();
		}
	}

	void CheckTimer()
	{
		if (Timer == 0)
		{
			// TODO Gameover
            source.Pause();
			StartCoroutine (charManager.Die (true));
		}
        else if(Timer == 100)
        {
            source.Pause();
            Destroy(GameObject.Instantiate(timerWarningSound), 15);
            source.PlayDelayed(3f);
        }

	}

	void ResetCountdown()
	{
		Timer = totalTime;
	}

	public void FreezeEntities()
	{
		print ("Freeze entities");
		frozenEntities = true;
		frozenEntitiesCooldown = true;
	}

	public void UnFreezeEntities()
	{
		print ("UnFreeze entities");
		frozenEntities = false;
		StartCoroutine (ResetFreezeCooldown ());
	}

	IEnumerator ResetFreezeCooldown()
	{
		yield return new WaitForSeconds(0.05f);
		frozenEntitiesCooldown = false;
	}

	[HideInInspector]public Vector3 camWorldTopRight; // Top right of screen in world coordinates
	[HideInInspector]public Vector3 camWorldBottomLeft; // Bottom left of screen in world coordinates
	//[HideInInspector]public Vector3 camWorldCenter; // Center of screen in world coordinates

	/// <summary>
	/// Updates the screen bounds.
	/// </summary>
	public void UpdateScreenBounds()
	{
		Camera cam = Camera.main;
		camWorldBottomLeft = cam.ScreenToWorldPoint (new Vector3 (0f, 0f));
		camWorldTopRight = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth, cam.pixelHeight));
		//camWorldCenter = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f));
	}
	#endregion

    public void saveHighScore()
    {
        PlayerPrefs.SetInt("currentScore", this.Score);

        int HighestScore = PlayerPrefs.GetInt("Highscore");

        //Saving this score as the highest if it is
        if(this.Score > HighestScore || HighestScore == 0)
		{
			print (string.Format ("OldHS: {0}, NewHS: {1}", HighestScore, Score));
            PlayerPrefs.SetInt("Highscore", this.Score);
		}
    }

	[ContextMenu("Reset highscore")]
	void ResetHighscore()
	{
        PlayerPrefs.SetInt("Highscore", 0);
	}

	void OnLevelWasLoaded(int id)
	{
		/*Vector3 entrancePoint = GameObject.FindGameObjectWithTag (Tags.entrancePoint).transform.position;
		transform.localPosition = new Vector3(entrancePoint.x, transform.position.y);
		charMove.transform.SetParent (null);
		charMove.transform.position = entrancePoint;*/
	}
}

public enum DeathType
{
	JumpedOn,
	ShellHit,
	OutOfMap
}
