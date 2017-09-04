using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 1f;
	public float turnDelay = .05f;
	public BoardManager boardManager;
	public static GameManager instance = null;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private int level = 0;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		enemies = new List<Enemy>();
		boardManager = GetComponent<BoardManager>();
		InitGame();
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		level++;

		InitGame();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	void InitGame() {
		doingSetup = true;

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text>();
		levelText.text = "Day " + level;
		levelImage.SetActive(true);
		Invoke ("HideLevelImage", levelStartDelay);

		enemies.Clear();
		boardManager.SetupBoard(level);
	}

	private void HideLevelImage() {
		levelImage.SetActive(false);
		doingSetup = false;
	}

	public void GameOver() {
		levelText.text = "After " + level + " days you starved.";
		enabled = false;
	}

	void Update () {
		if (playersTurn || enemiesMoving || doingSetup) return;

		StartCoroutine(MoveEnemies());
	}

	public void AddEnemyToList(Enemy enemy) {
		enemies.Add(enemy);
	}

	IEnumerator MoveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0)
			yield return new WaitForSeconds(turnDelay);

		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}

}
