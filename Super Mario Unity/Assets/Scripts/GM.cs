using UnityEngine;
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
			GUIManager.instance.ChangeScoreText (value); // Update GUI when setting the value
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
			GUIManager.instance.ChangeCoinText (value); // Update GUI when setting the value
		}
    }

	private int timer = 0;
	public int Timer
	{
		get { return this.timer; }
		set 
		{ 
			this.timer = value; 
			GUIManager.instance.ChangeTimeText (value); // Update GUI when setting the value
		}
	}
    #endregion

    public GameObject player;

	public bool frozenEntities = false; // For freezing the entities
	public bool frozenEntitiesCooldown = false;
	private float currentCountdownTime = 0;
	private const float secondRatio = 0.4f; // Seconds per in-game seconds
	private const int totalTime = 400; // Total time for a level
	[HideInInspector]public CharacterManager charManager;
	[HideInInspector]public AudioSource source;
	[HideInInspector]public bool isPaused = false;
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

		// Load the number of lives
		Lives = Utils.LoadLives ();

		// Find the references
		if (!player)
		{
			player = GameObject.FindGameObjectWithTag (Tags.player);
		}
		charManager = player.GetComponent<CharacterManager>();
		source = GetComponent<AudioSource>();
		
		// Reset the countdown
		ResetCountdown ();

		UpdateScreenBounds ();
    }

	void Update()
	{
		// Do the countdown
		DoTimerCountdown ();

		// Pausing
		if (Input.GetKeyDown("p"))
		{
			if(isPaused)
			{
				isPaused = false;
				Time.timeScale = 1f;
			}
			else
			{
				isPaused = true;
				Time.timeScale = 0f;
			}
		}
	}

    #region methods
    public void CheckGameOver()
    {
		// Save the highscore
        saveHighScore();

		// Check the timer
		if (Timer <= 0) // Less than 0
		{
			Application.LoadLevel ("Time_Up");
		}

		// Check the number of lives
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
		// Save scores
        saveHighScore();
		// Play SFX
		Instantiate(stageClear);
		Instantiate(fireworksSound);
		yield return new WaitForSeconds(7f);

		// Load main menu
		Application.LoadLevel ("Main_Title");
	}

	void DoTimerCountdown()
	{
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
		// Check the timer
		if (Timer == 0) // Out of time
		{
            source.Pause();
			StartCoroutine (charManager.Die (true));
		}
        else if(Timer == 100) // Low on time
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
		frozenEntities = true;
		frozenEntitiesCooldown = true;
	}

	public void UnFreezeEntities()
	{
		frozenEntities = false;
		StartCoroutine (ResetFreezeCooldown ());
	}

	// "Cooldown" for the freezing 
	// (when activating the rigidbody after freezing the collision event will be triggered, causing the entities to turn around)
	IEnumerator ResetFreezeCooldown()
	{
		yield return new WaitForSeconds(0.05f);
		frozenEntitiesCooldown = false;
	}

	[HideInInspector]public Vector3 camWorldTopRight; // Top right of screen in world coordinates
	[HideInInspector]public Vector3 camWorldBottomLeft; // Bottom left of screen in world coordinates

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

#if UNITY_EDITOR
	// DEBUG Just for reseting the score during test
	[ContextMenu("Reset highscore")]
	void ResetHighscore()
	{
        PlayerPrefs.SetInt("Highscore", 0);
	}
#endif
}