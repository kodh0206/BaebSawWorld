using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	class Brick
	{	
		//벽돌의 인덱스
		public readonly int index;
		//존재?
		public bool exist;
		//빈블락
		public readonly GameObject emptyBrick;
		//벽돌 컨트롤러
		public BrickController brick;

		//벽돌 생성자
		public Brick(int index, GameObject emptyBrick)
		{
			this.index = index;
			this.emptyBrick = emptyBrick;
		}
	}
	
	event Action OnLevelUp = delegate { };

	[Range(0f, 1f)]
	public float randomIconProbability;
	[Range(0f, 2f)]
	public float timerSpeed = 1f;
	//빈블락
	public GameObject emptyBrickPrefab;
	//벽돌 블락
	public BrickController brickPrefab;
	//필드 애니메이션
	public Animator fieldAnimator;

	[Header("UI")]
	public RectTransform fieldTransform;//세포

	//public Text levelText;
	//public Text experienceText;
	//public Text maxExperienceText;
	public Text boxTimer;
	public Image boxImage;
	public GameObject fullText; 
	public Button boxButton;

	[Header("SFX")]
	public PlaySfx clickSfx;
	public PlaySfx landingSfx;
	public PlaySfx mergingSfx;
	public PlaySfx mainSfx;
	public PlaySfx feverSfx;
	public PlaySfx punch;
	[Header("VFX")]
	public ParticleSystem spawnEffect;
	public ParticleSystem openEffect;
	public ParticleSystem mergeEffect;

	/// General //
	public int progresslevel;
	//총벽돌 갯수 
	int totalBricksCount = 3;
	Vector2Int minCoords;
	Vector2Int maxCoords;
	Vector2Int bricksCount = new Vector2Int(3,2);
	int ProgressLevel; //오픈레밸 
	public int spawnlevel;
	public double currenciesPerSeconds;
	//필드
	Brick[,] field;
	readonly List<Vector2Int> freeCoords = new List<Vector2Int>();
	
	/// Level Stats ///
	[SerializeField]
	readonly int[,] fieldIndexes =
	{
		{39,37,36,38,40},
		{29,27,26,28,30},
		{24,1,2,3,25},
		{20,4,5,6,17},
		{19,8,7,9,16},
		{21,11,10,12,18},
		{22,14,13,15,23},
		{34,32,31,33,35}
	};
	const int maxBricksCount = 35;
	readonly int[] startingLevelsStats = {40, 46, 74, 110};
	int currentExperienceLevel;
	int currentExperience;
	int prevLevelMaxExperience;
	int levelMaxExperience = 40;
	int maxOpenLevel;
	const float spawnVerticalOffset = 500f;
	public float spawnTime = 10f;
	float timer;
	float imageDelta = 0.1f;
	int userLevel;
	
	static readonly int BigField = Animator.StringToHash("Big");
	static readonly int SmallField = Animator.StringToHash("Small");

    //edit
    //idle time increase code
	//check Line start from 244
    [Header("IdleTime")]

    public float time_increase = 1.0f;
    public float current_time = 0f;
    public DateTime closedTime;
	private bool userIdle = false;
	private float timeSinceLastInput;
	public float idleLength = 30.0f;

    double currenciesPerSecond;
	Vector2Int BricksCount
	{
		get
		{
			bricksCount.x = maxCoords.y - minCoords.y + 1;
			bricksCount.y = maxCoords.x - minCoords.x + 1;
			return bricksCount;
		}
	}
	//피버모드 
	private float feverGauge = 0;  // 피버 게이지
	private bool isFeverMode = false; // 피버 모드 활성화 여부
	private float feverModeDuration = 30f; // 피버 모드 지속 시간
	private float feverModeRemaining = 0; // 피버 모드 남은 시간

	//계란까는 레밸
	int OpenLevel{
		get => OpenLevel;
		set
		{
			OpenLevel= value;
			//levelText.text = (currentExperienceLevel + 1).ToString();
		}
	}
	int CurrentExperienceLevel
	{
		get => currentExperienceLevel;
		set
		{
			currentExperienceLevel = value;
			//levelText.text = (currentExperienceLevel + 1).ToString();
		}
	}

	int LevelMaxExperience
	{
		get => levelMaxExperience;
		set
		{
			levelMaxExperience = value;
			//maxExperienceText.text = levelMaxExperience.ToString();
		}
	}
	
	GameState gameState;

	void Awake()
	{	 
		if(!PlayerPrefs.HasKey("SpawnLevel")){
			spawnlevel = 0;

		}
		else if(PlayerPrefs.HasKey("SpawnLevel"))
		{
			spawnlevel =PlayerPrefs.GetInt("SpawnLevel");
		}
		if(!PlayerPrefs.HasKey("SpawnTime"))
		{
			spawnTime =10f;
		}
		else if(PlayerPrefs.HasKey("SpawnTime")){
			spawnTime = PlayerPrefs.GetFloat("SpawnTime");
		}
		
		Debug.Log("소환 레벨 LV"+spawnlevel);
		Debug.Log("소환 시간"+spawnTime+"초");
		
		timer = spawnTime;
		boxImage.fillAmount = 0;
		boxButton.onClick.AddListener(OnBoxClick);
		
		gameState = UserProgress.Current.GetGameState<GameState>(name);
		if (gameState == null)
		{
			gameState = new GameState();
			UserProgress.Current.SetGameState(name, gameState);
		}
		UserProgress.Current.CurrentGameId = name;

		InitField();

		if (!LoadGame())
		{
			
			CurrentExperienceLevel = 0;
			LevelMaxExperience = startingLevelsStats[0];
			gameState.Score = 0;
			gameState.MaxOpenLevel=0;
			UpdateLevelExperience();
			spawnlevel=1;
			spawnTime=10f;
			progresslevel =1;
		
		}
		
		UpdateCoords();
		OnLevelUp += UpdateFieldSize;
		MergeController.Purchased += SpawnBrick;
		MergeController.RewardUsed += SpawnBrick;
		MergeController.Purchased += UpdateLevelExperience;
		MergeController.RewardUsed += UpdateLevelExperience;
		CalculateCurrenciesPerSecond();
		Debug.Log("머지컨트롤러 확인:"+gameState.MaxOpenLevel);

	}
	
	void Update()
	{
		UpdateSpawnTimer(true);
		CalculateCurrenciesPerSecond();
		Debug.Log("초당 재화 :" + GetCurrenciesPerSecond() );

		if (isFeverMode)
    	{
        // 남은 시간 감소
        	feverModeRemaining -= Time.deltaTime;
        	// 남은 시간이 없으면 피버 모드 해제
        	if (feverModeRemaining <= 0)
        	{	
            	isFeverMode = false;
            	feverGauge = 0;
            	// 여기서 벽돌 생성 속도와 수익을 원래대로 설정
        	}
    	}

		//edited
		//idle check
		//

		if (Input.touchCount > 0 || Input.anyKey)
		{
			timeSinceLastInput = 0;
			Debug.Log("touched: ");
		}
		else
		{
			timeSinceLastInput += Time.deltaTime;
			Debug.Log("not touched: ");

		}
		userIdle = timeSinceLastInput >= idleLength ? true : false;
		Debug.Log("idle status: " + userIdle);

		IdleStatus(userIdle);
	}

	//게임로드 
	bool LoadGame()
	{	//벽돌현황 
		BrickState[] bricks = gameState.GetField();
		
		if (bricks == null || bricks.Length != field.GetLength(0) * field.GetLength(1))
			return false;
		
		totalBricksCount = gameState.BricksCount;
		maxOpenLevel =gameState.MaxOpenLevel;
		
		MergeController.Instance.UpdateState(gameState.MaxOpenLevel, gameState.GetPresetsPrices(), gameState.GetRewardTime());
		UpdateCoords();

		for (int i = 0; i < field.GetLength(0); i++)
		{
			for (int j = 0; j <field.GetLength(1); j++)
			{
				if (bricks[i * field.GetLength(1) + j].level < 0) continue;
				
				SpawnBrick(new Vector2Int(i, j), bricks[i * field.GetLength(1) + j].level, 
					(BrickType)bricks[i * field.GetLength(1) + j].type, bricks[i * field.GetLength(1) + j].open == 0);

					
			}
		}
		MergeController.Instance.FreeSpace = freeCoords.Count > 0;
		return true;
	}
	//게임 저장
	void SaveGame()
	{	
		BrickState[] bricks = new BrickState[field.GetLength(0) * field.GetLength(1)];
		for (int i = 0; i < field.GetLength(0); i++)
		{
			for (int j = 0; j < field.GetLength(1); j++)
			{
				bricks[i * field.GetLength(1) + j] = field[i, j].brick != null ? 
					new BrickState(field[i, j].brick.Level, field[i, j].brick.Open, (int)field[i, j].brick.Type) : new BrickState(-1);
			}
		}

		IEnumerable<MergePreset> presets = MergeController.Instance.GetPresets();
		double[] presetPrices = new double[presets.Count()];
		for (int j = 0; j < presetPrices.Length; j++)
			presetPrices[j] = presets.ElementAt(j).Price;
		//저장
		gameState.BricksCount = totalBricksCount;
		gameState.MaxOpenLevel = MergeController.Instance.MaxOpenLevel;
		gameState.SetField(bricks);
		gameState.SetPresetsPrices(presetPrices);
		gameState.SetRewardTimer(MergeController.Instance.RewardTimer);
		UserProgress.Current.UserLevels = MergeController.Instance.MaxOpenLevel+1;
		UserProgress.Current.SaveGameState(name);
	}
	//필드 생성 
	void InitField()
	{
		field = new Brick[fieldIndexes.GetLength(0), fieldIndexes.GetLength(1)];

		for (int i = 0; i < fieldIndexes.GetLength(0); i++)
		{
			for (int j = 0; j < fieldIndexes.GetLength(1); j++)
				field[i,j] = new Brick(fieldIndexes[i,j], BrickObject(emptyBrickPrefab, false));
		}
		
		minCoords = new Vector2Int(field.GetLength(0), field.GetLength(1));
		maxCoords = new Vector2Int(0, 0);
	}

	void UpdateCoords()
	{
		freeCoords.Clear();
		
		for(int i = 0; i < field.GetLength(0); i++)
		{
			for (int j = 0; j < field.GetLength(1); j++)
			{
				if (field[i, j].index <= totalBricksCount)
				{
					field[i, j].exist = true;
					minCoords.x = Mathf.Min(i, minCoords.x);
					minCoords.y = Mathf.Min(j, minCoords.y);

					maxCoords.x = Mathf.Max(i, maxCoords.x);
					maxCoords.y = Mathf.Max(j, maxCoords.y);
				}

				if (field[i, j].brick == null && field[i, j].index <= totalBricksCount)
					freeCoords.Add(new Vector2Int(i, j));
			}
		}
		MergeController.Instance.FreeSpace = freeCoords.Count > 0;
		UpdateField();
	}
	
	void UpdateFieldSize()
	{
		if(totalBricksCount < maxBricksCount)
			totalBricksCount++;
		UpdateCoords();
	}
	
	void UpdateField()
	{
		for (int i = 0; i < field.GetLength(0); i++)
		{
			for (int j = 0; j < field.GetLength(1); j++)
			{
				if (!field[i, j].exist) continue;
				if (field[i, j].brick)
					SetBrick(field[i,j].brick.gameObject, new Vector2Int(i, j));
				SetBrick(field[i,j].emptyBrick, new Vector2Int(i, j));
				
			}
		}
	}
	
	void HighLightField(BrickController brick, bool highlight)
	{
		for (int i = 0; i < field.GetLength(0); i++)
		{
			for (int j = 0; j < field.GetLength(1); j++)
			{
				if (field[i, j].brick)
					field[i,j].brick.HighlightBrick(brick, highlight);
			}
		}
	}

	GameObject BrickObject(GameObject prefab, bool active = true)
	{
		GameObject brick = Instantiate(prefab, fieldTransform);
		brick.gameObject.SetActive(active);
		return brick;
	}
	//블록 생성
	void SpawnBrick(int level = 0, BrickType type = BrickType.Default)
	{
		MergeController.Instance.FreeSpace = freeCoords.Count > 0;
		if (freeCoords.Count <= 0)
			return;

		Vector2Int coords = freeCoords[Random.Range(0, freeCoords.Count)];
		if(level == 0) 
			MergeController.Instance.CheckForRandomPreset(randomIconProbability, out level, ref type);

		if (type == BrickType.Random)
			UpdateLevelExperience(level, type);
		
		SpawnBrick(coords, level, type);
		
	}

	void SpawnBrick(Vector2Int coords, int level, BrickType type, bool open = false)
	{
	
		Vector2 position = GetBrickPosition(coords);
		Vector2 spawnPoint = new Vector2(position.x, position.y);
		MergePreset preset = MergeController.Instance.GetPresset(level);
		
		field[coords.x,coords.y].brick = BrickObject(brickPrefab.gameObject).GetComponent<BrickController>();
		field[coords.x,coords.y].brick.RectTransform.sizeDelta = BrickSize(BricksCount);
		field[coords.x,coords.y].brick.SetBrick(preset, level, type, open);
		field[coords.x,coords.y].brick.DoLandingAnimation(spawnPoint, position);
		field[coords.x,coords.y].brick.OpenClick += BrickOnClick;
		field[coords.x,coords.y].brick.PointerUp += BrickOnPointerUp;
		field[coords.x,coords.y].brick.PointerDown += BrickOnPointerDown;

		freeCoords.Remove(coords);
		landingSfx.Play();
		SpawnEffect(spawnEffect, field[coords.x,coords.y].brick.gameObject);
	


		
		SaveGame();
	}
	
	void SetBrick(GameObject brick, Vector2Int coords)
	{
		brick.SetActive(true);
		Vector2 brickPosition = GetBrickPosition(coords);
		brick.GetComponent<RectTransform>().anchoredPosition = brickPosition;
		brick.GetComponent<RectTransform>().sizeDelta = BrickSize(BricksCount);
	}

	void SpawnEffect(ParticleSystem prefab, GameObject brick)
	{
		ParticleSystem effect = Instantiate(prefab, fieldTransform);
		Vector3 effectSpawnPosition = brick.transform.position;
		effectSpawnPosition.z = -1;
		effect.transform.position = effectSpawnPosition;
		effect.Play();
	}

	void BrickOnClick(BrickController brick)
	{
		SpawnEffect(openEffect, brick.gameObject);
		
	}
	public void PlayPunch(){
		punch.Play();
	}

	void BrickOnPointerDown(BrickController brick)
	{
		HighLightField(brick, true);
	}
	
	void BrickOnPointerUp(BrickController brick, Vector2 position)
	{
		HighLightField(brick, false);
		
		Vector2Int cachedCoords = BrickPositionToCoords(brick.CachedPosition, brick.RectTransform.pivot);
		Vector2Int coords = BrickPositionToCoords(position, brick.RectTransform.pivot);

		if(!field[coords.x, coords.y].exist)
		{
			brick.RectTransform.anchoredPosition = GetBrickPosition(cachedCoords);
		}
		else if (field[coords.x, coords.y].brick == null || coords == cachedCoords)
		{
			brick.RectTransform.anchoredPosition = GetBrickPosition(coords);
			if (coords != cachedCoords)
			{
				field[coords.x, coords.y].brick = brick;
				field[cachedCoords.x, cachedCoords.y].brick = null;

				freeCoords.Remove(freeCoords.Find(c => c == coords));
				freeCoords.Add(cachedCoords);
			}
		}
		else if (MergeBricks(brick, field[coords.x, coords.y].brick))
		{
			brick.RectTransform.anchoredPosition = GetBrickPosition(coords);
			field[coords.x, coords.y].brick = brick;
			field[cachedCoords.x, cachedCoords.y].brick = null;

			mergingSfx.Play();
			freeCoords.Add(cachedCoords);
			//UpdateLevelExperience(brick.Level);
			 IncreaseFeverGauge(1); //게이지 채우기
			SpawnEffect(mergeEffect, brick.gameObject);
		}
		else
		{
			BrickController targetBrick = field[coords.x, coords.y].brick;

			field[coords.x, coords.y].brick.IsLandingCheck();
			field[coords.x, coords.y].brick.RectTransform.anchoredPosition = GetBrickPosition(cachedCoords);
			field[coords.x, coords.y].brick = field[cachedCoords.x, cachedCoords.y].brick;

			field[cachedCoords.x, cachedCoords.y].brick = targetBrick;
			brick.RectTransform.anchoredPosition = GetBrickPosition(coords);
		}
		SaveGame();
	}

	Vector2 GetBrickPosition(Vector2 coords)
	{
		coords = new Vector2(coords.y - minCoords.y, coords.x - minCoords.x);
		Vector2 brickSize = BrickSize(BricksCount);

		RectTransform brickTransform = brickPrefab.GetComponent<RectTransform>();
		Vector2 brickPosition = Vector2.Scale(coords, brickSize);
		Vector2 offset = new Vector2(brickSize.x * BricksCount.x / 2, brickSize.y * BricksCount.y / 2);
		brickPosition += Vector2.Scale(brickSize, brickTransform.pivot) - offset;

		return brickPosition;
	}
	
	Vector2Int BrickPositionToCoords(Vector2 position, Vector2 pivot)
	{
		Vector2 brickSize = BrickSize(BricksCount);
		Vector2 offset = new Vector2(brickSize.x * BricksCount.x / 2, brickSize.y * BricksCount.y / 2);
		Vector2 coords = (position + offset - Vector2.Scale(brickSize, pivot)) / brickSize;
		
		coords.x = Mathf.Clamp(coords.x, 0 , BricksCount.x - 1);
		coords.y = Mathf.Clamp(coords.y, 0 , BricksCount.y - 1);

		Vector2Int result = Vector2Int.RoundToInt(coords);
		result = new Vector2Int(result.y  + minCoords.x , result.x + minCoords.y);

		return result;
	}

	Vector2Int BrickSize(Vector2Int count)
	{
		int size = (int)(fieldTransform.rect.width / count.x);
		
		if (size * count.y > fieldTransform.rect.height)
			size = (int) (fieldTransform.rect.height / count.y);
		return new Vector2Int(size, size);
	}

	static bool MergeBricks(BrickController brick, BrickController targetBrick)
	{
		if (brick.Level != targetBrick.Level || !brick.Open || !targetBrick.Open) return false;
		
		brick.LevelUp(MergeController.Instance.GetPresset(brick.Level + 1));
		MergeController.Instance.UpdateMaxOpenLevel(brick.Level);
		
		Destroy(targetBrick.gameObject);
	
		return true;
	}
	//벽돌구매
	public void BuyBrick(){
		UpdateField();
		UpdateFieldSize();
		SpawnBrick();
		
	}
	void UpdateLevelExperience(int value = 0, BrickType brickType = BrickType.Default)
	{
		gameState.Score += value;
		
	
		SaveGame();
	}


	

	int GetLevelMaxExperience()
	{
		int result;
		if (CurrentExperienceLevel < startingLevelsStats.Length)
		{
			result = startingLevelsStats[CurrentExperienceLevel];
			if (CurrentExperienceLevel > 1)
				prevLevelMaxExperience = startingLevelsStats[CurrentExperienceLevel - 1];
		}
		else
		{
			result = LevelMaxExperience * 2 - prevLevelMaxExperience + totalBricksCount;
			prevLevelMaxExperience = LevelMaxExperience;
		}
		return result;
	}
	
	void UpdateSpawnTimer(bool smooth)
	{
		fullText.gameObject.SetActive(freeCoords.Count <= 0);
		boxTimer.transform.parent.gameObject.SetActive(freeCoords.Count > 0);
		
		timer = smooth ? timer - Time.deltaTime * timerSpeed : timer - 1;
		float seconds = Mathf.Max(1, Mathf.CeilToInt(timer % 60f));
		
		if (freeCoords.Count <= 0)
		{
			timer = spawnTime;
			boxImage.fillAmount = 0;
			return;
		}

		if (timer <= 0)
		{
			timer = spawnTime;
			SpawnBrick(spawnlevel);
		}
		
		float value = timer < spawnTime ? boxImage.fillAmount : 0f;
		float targetValue = 1f + imageDelta - seconds / spawnTime;
		
		boxTimer.text = seconds.ToString();
		boxImage.fillAmount = smooth
			? Mathf.Lerp(value, targetValue, Time.deltaTime * timerSpeed)
			: targetValue;
	}

	public void OnBoxClick()
	{
		UpdateSpawnTimer(false);
	}
	
	public void MinimizeCurrentGame(bool value)
	{
		if (!value)
		{
			MaximizeCurrentGame();
			return;
		}

		ResetTriggers();
		Time.timeScale = 0;
		fieldAnimator.SetTrigger(SmallField);
	}
	
	void MaximizeCurrentGame()
	{
		Time.timeScale = 1;
		ResetTriggers();
		fieldAnimator.SetTrigger(BigField);
	}
	
	void ResetTriggers()
	{
		fieldAnimator.ResetTrigger(BigField);
		fieldAnimator.ResetTrigger(SmallField);
	}

	void CalculateCurrenciesPerSecond()
    {
        double totalCurrenciesPerSecond = 0;

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j].brick != null)
                {
                    double brickCurrenciesPerSecond = field[i, j].brick.DPS;
                    totalCurrenciesPerSecond += brickCurrenciesPerSecond;
                }
            }
        }

        currenciesPerSecond = totalCurrenciesPerSecond;
    }

	double GetCurrenciesPerSecond()
    {
        return currenciesPerSecond;
    }

	void IncreaseFeverGauge(float amount)
	{
		feverGauge += amount;
		CheckFeverMode();
	}

	void CheckFeverMode()
	{
		if (feverGauge >= 100 && !isFeverMode)  // 게이지가 100 이상이면 피버 모드 활성화
		{
			isFeverMode = true;
			feverModeRemaining = feverModeDuration;
			// 여기서 벽돌 생성 속도와 수익을 두 배로 설정
		}
	}
    //edit
    //idle time increase code 

    void IdleStatus(bool status)
    {
		if (status)
        {
			Debug.Log("we are in");
			//no needs for time record
			string lastTimeString = PlayerPrefs.GetString("LastTime", DateTime.Now.ToString());
			
			Debug.Log("time 1: " + lastTimeString);

			AddingIdleTime();

			InvokeRepeating("IncreaseTime", 0f, 1f);	
		}
		else
        {
			//no needs for time record
			OnApplicationQuit();
			Debug.Log("time exit before refresh: " + current_time);
			current_time = 0;
			Debug.Log("time value: " + current_time);
			//Debug.Log("time exit: " + DateTime.Now);
		}

	}

    void AddingIdleTime()
    {
        TimeSpan idleTime = DateTime.Now - closedTime;
        float idleValue = (float)idleTime.TotalSeconds * time_increase;
        current_time += idleValue;
    }

    private void IncreaseTime()
    {
        current_time += time_increase;
		//Debug.Log("time exteding: " + current_time);
    }

    private void OnApplicationQuit()
    {
		// 게임을 닫을 때 현재 시간을 저장합니다.
		//no needs for time record
		PlayerPrefs.SetString("LastTime", DateTime.Now.ToString());
    }


}