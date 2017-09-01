using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public BoardManager boardManager;

	private int level = 3;

	public static GameManager instance = null;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		boardManager = GetComponent<BoardManager>();
		InitGame();
	}

	void InitGame() {
		boardManager.SetupBoard(level);
	}

	// Update is called once per frame
	void Update () {

	}
}
