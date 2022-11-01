using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	public bool grantsShield;
	public bool grantsLaunch;
	public bool grantsJetpack;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out PlayerMovement pm))
		{
			pm.hasShield = pm.hasShield || grantsShield;
			pm.hasLaunch = pm.hasLaunch|| grantsLaunch;
			pm.hasJetpack = pm.hasJetpack || grantsJetpack;

			Destroy(gameObject);
		}
	}
}
