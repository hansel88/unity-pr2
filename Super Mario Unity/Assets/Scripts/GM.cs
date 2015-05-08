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
        set 
		{ 
			this.score = value; 
			GUIManager.instance.ChangeScoreText (value);
		}
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

    #region status and powerup properties

    public enum MarioStatus { Big, Small };

    private MarioStatus characterStatus = MarioStatus.Small;
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

	// TODO Remove
	public enum PowerupItem{Mushroom, Fireflower, Star};
	public enum MarioPowerupStatus{Small, Big, Fireflower, Star};
	public MarioPowerupStatus marioPowerupStatus;

	public GameObject player;
	private bool playerIsAlive = true;
	public bool PlayerIsAlive 
	{ 
		get {return playerIsAlive;} 
		set {playerIsAlive = value; charMove.canMove = value;}
	}
	public bool frozenEntities = false;
	private float currentCountdownTime = 0;
	private const float secondRatio = 0.4f; // Seconds per in-game seconds
	private const int totalTime = 400; // Total time for a level
	private CharacterMovement charMove;
	[HideInInspector]public CharacterManager charManager;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

		if (!player)
		{
			player = GameObject.FindGameObjectWithTag (Tags.player);
		}
		charManager = player.GetComponent<CharacterManager>();
		charMove = player.GetComponent<CharacterMovement>();

		ResetCountdown ();

		UpdateScreenBounds ();
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

	public void PowerPlayerUp(PowerupItem powerup)
	{
		// TODO Improve
		if (marioPowerupStatus == MarioPowerupStatus.Small && powerup == PowerupItem.Mushroom)
		{
			marioPowerupStatus = MarioPowerupStatus.Big;
			// TODO Animate getting mushroom
		}
		if (marioPowerupStatus == MarioPowerupStatus.Small && powerup == PowerupItem.Fireflower)
		{
			marioPowerupStatus = MarioPowerupStatus.Fireflower;
			// TODO Animate from small to fireflower
		}
		if (marioPowerupStatus == MarioPowerupStatus.Big && powerup == PowerupItem.Fireflower)
		{
			marioPowerupStatus = MarioPowerupStatus.Fireflower;
			// TODO Animate from big to firelfower
		}
		if (marioPowerupStatus == MarioPowerupStatus.Fireflower && (powerup == PowerupItem.Mushroom || powerup == PowerupItem.Fireflower))
		{
			// TODO Reward player with points
		}
	}

	public void PowerPlayerDown()
	{

	}

	void DoCountdown()
	{
		// Don't countdown if player is dead
		if (!playerIsAlive) return;

		// Countdown
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

	public void FreezeEntities()
	{
		frozenEntities = true;
	}

	public void UnFreezeEntities()
	{
		frozenEntities = false;
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
}

public enum DeathType
{
	JumpedOn,
	ShellHit,
	OutOfMap
}
