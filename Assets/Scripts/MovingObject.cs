﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;
	private float inverseMoveTime;

	protected virtual void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		rigidBody = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);

		boxCollider.enabled = false;

		hit = Physics2D.Linecast(start, end, blockingLayer);

		boxCollider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine(SmoothMovement(end));
			return true;
		}

		return false;

	}

	protected IEnumerator SmoothMovement(Vector3 endPosition) {
		float squareRemainingDistance = (transform.position - endPosition).sqrMagnitude;
		while (squareRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(rigidBody.position, endPosition, inverseMoveTime * Time.deltaTime);
			rigidBody.MovePosition(newPosition);
			squareRemainingDistance = (transform.position - endPosition).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove<T>(int xDir, int yDir)
		where T:Component {

		RaycastHit2D hit;
		bool canMove = Move(xDir, yDir, out hit);

		if (hit.transform == null)
			return;

		T hitComponent = hit.transform.GetComponent<T>();

		if (!canMove && hitComponent != null)
			OnCantMove (hitComponent);

	}
 
	protected abstract void OnCantMove<T>(T component)
		where T:Component;
	
	void Update () {
		
	}
}
