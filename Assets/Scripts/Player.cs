using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

	public int wallDamage = 1;
	public int pointsPerSoda = 20;
	public int pointsPerFood = 10;
	public float restartLevelDelay = 1f;
	public Text foodText;

	private Animator animator;
	private int food;

	protected override void Start () {
		animator = GetComponent<Animator>();

		food = GameManager.instance.playerFoodPoints;

		foodText.text = "Food: " + food;

		base.Start();
	}

	private void OnDisable() {
		if (food > 0)
			GameManager.instance.playerFoodPoints = food;
	}

	protected override void AttemptMove<T>(int xDir, int yDir) {
		food--;
		foodText.text = "Food: " + food;

		base.AttemptMove<T>(xDir, yDir);
		RaycastHit2D hit;

		if (Move(xDir, yDir, out hit))
		{
			//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
		}

		CheckGameOver ();
		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove<T>(T component) {
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);
		animator.SetTrigger("Chop");
	}

	private void Restart() {
		SceneManager.LoadScene(0);
	}

	private void CheckGameOver() {
		if (food <= 0)
			GameManager.instance.GameOver();
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			other.gameObject.SetActive(false);
		} else if (other.tag == "Soda") {
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerSoda + " Food: " + food;
			other.gameObject.SetActive(false);
		}
	}

	public void LoseFood(int loss) {
		animator.SetTrigger("Hit");
		foodText.text = "-" + loss + " Food: " + food;
		food -= loss;
		CheckGameOver();
	}

	void Update () {
		if (!GameManager.instance.playersTurn) return;

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw("Horizontal");
		vertical = (int)Input.GetAxisRaw("Vertical");

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0)
			AttemptMove<Wall> (horizontal, vertical);

	}
}
