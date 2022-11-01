using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
	private Vector3 travelLocation;
	private bool locationSet = false;
	private bool locationArrived = true;

	private float movementSpeed = 1f;
	private float timeToWait = 0.5f;

	private float timeWaited = 0f;

	private Vector3 lastKnownLocation;
	private State attackState;
	private EnemyAI ai;

	public PatrolState(Rigidbody2D rb, Animator anim, StateChanged callback) : base(rb, anim, callback)
	{
		lastKnownLocation = rb.transform.position;
		ai = rb.GetComponent<EnemyAI>();
	}

	public PatrolState SetAttackState(State s)
	{
		attackState = s;
		return this;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		// Check if enemy is in range
		if ( (ai.target.transform.position - _rb.transform.position).magnitude <= ai.detectionRange)
		{
			TransitionStates(attackState);
			return;
		}


		// Move towards the target
		if (locationSet && !locationArrived)
		{
			_rb.transform.position += deltaTime * movementSpeed * (travelLocation - _rb.transform.position).normalized;
			lastKnownLocation += deltaTime * movementSpeed * (travelLocation - _rb.transform.position).normalized;
		}

		// If we are near the intended location, wait for some time and then decide a new location
		if ((_rb.transform.position - travelLocation).magnitude < 0.5f)
		{
			locationArrived = true;
			locationSet = false;

			timeWaited += deltaTime;

			if (timeWaited >= timeToWait)
			{
				FindPath();
			}
			
		}

		if (_rb.transform.position != lastKnownLocation)
		{
			FindPath();
		}

		lastKnownLocation = _rb.transform.position;
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();
		FindPathGuarenteed();
	}

	protected override void OnExitState()
	{
		base.OnExitState();
	}

	protected override void TransitionStates(State state)
	{
		base.TransitionStates(state);
	}


	private void FindPathGuarenteed()
	{
		while (!FindPath());
	}

	private bool FindPath()
	{
		// Randonmly determine a location we can get to
		Vector2 offset = new Vector2(Random.Range(-5f, 5f), 0);
		Vector2 testPos = offset + (Vector2)_rb.transform.position;

		RaycastHit2D hit = Physics2D.BoxCast(testPos, new Vector2(0.15f, 3f), 0, Vector2.down, 0.15f, layerMask: LayerMask.GetMask("Wall"));
		RaycastHit2D hitHigh = Physics2D.BoxCast(testPos, new Vector2(0.5f, 0.15f), 0, Vector2.down, 0, layerMask: LayerMask.GetMask("Wall"));
		if (hit.collider != null && hitHigh.collider == null)
		{
			travelLocation = testPos;
			locationSet = true;
			locationArrived = false;
			timeWaited = 0f;
			return true;
		}

		return false;
	}
}
