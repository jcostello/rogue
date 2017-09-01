using System;
﻿using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {

		public int maximum;
		public int minumum;

		public Count(int minumum, int maximum) {
			this.minumum = minumum;
			this.maximum = maximum;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList() {
		gridPositions.Clear();

		for (int x=1; x < columns - 1; x++) {
			for (int y=1; y < rows -1 ; y++) {
				gridPositions.Add(new Vector3(x, y, 0));
			}
		}
	}

	void BoardSetup() {
		boardHolder = new GameObject("Board").transform;

		for (int x=-1; x < columns + 1; x++) {
			for (int y=-1; y < rows + 1; y++) {
                GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
                
                if(x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
                
                GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);

		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minumum, int maximum) {
		int objectCount = Random.Range(minumum, maximum) + 1;

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	void SpanwEnemies(int level) {
		int enemyCount = (int) Mathf.Log(level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
	}

	void SpanwWallsAndItems() {
		LayoutObjectAtRandom(wallTiles, wallCount.minumum, wallCount.maximum);
		LayoutObjectAtRandom(foodTiles, foodCount.minumum, foodCount.maximum);
	}

	void SpanwExit() {
		Instantiate(exit, new Vector3(columns -1, rows-1, 0f), Quaternion.identity);
	}

	public void SetupBoard(int level) {
		BoardSetup();
		InitializeList();
		SpanwWallsAndItems();
		SpanwEnemies(level);
		SpanwExit();
	}
}
