using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	public float duration = 10f;
	private float curDuration = 0;

	private void Update()
	{
		curDuration += Time.deltaTime;
		if (curDuration >= duration)
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(collision.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(collision.gameObject);
	}
}
