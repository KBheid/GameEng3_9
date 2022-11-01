using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackState : State
{
	private bool movingRight = false;
	private bool movingLeft = false;
	private bool movingUp = false;

	private bool firstInputIgnored = false;

	private bool lastMovementWasLeft;
	private float maxFuel = 100f;
	private float curFuel;

	private float useAndRechargeRate = 10f;

	private float lastTime = -1;
	private float allowedMinForJetpack = 30f;
	private float lastGravity;

	private State returnState;


	public JetpackState(Rigidbody2D rb, Animator anim, StateChanged callback) : base(rb, anim, callback)
	{
		curFuel = maxFuel;
	}

	public override void Input(KeyCode key, bool pressed)
	{
		base.Input(key, pressed);

		if (key == KeyCode.D)
		{
			movingRight = pressed;
			lastMovementWasLeft = false;
		}
		if (key == KeyCode.A)
		{
			movingLeft = pressed;
			lastMovementWasLeft = true;
		}
		if (key == KeyCode.Space)
		{
			movingUp = pressed;
		}

		if (key == KeyCode.T)
		{
			if (firstInputIgnored)
			{
				TransitionStates(returnState);
			}

			firstInputIgnored = true;
			
		}
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		if (movingRight)
		{
			_rb.AddForce(new Vector2(500 * deltaTime, 0));
		}
		if (movingLeft)
		{
			_rb.AddForce(new Vector2(-500 * deltaTime, 0));
		}
		if (movingUp)
		{
			_rb.AddForce(new Vector2(0, 150 * deltaTime));
		}

		if (!movingRight && !movingLeft && !movingUp)
		{
			_rb.velocity *= 0.9999f * 1-deltaTime;
		}

		if (hasAnimator)
			animator.SetBool("Moving", movingLeft || movingRight);

		curFuel -= useAndRechargeRate * deltaTime;

		if (curFuel <= 0)
		{
			TransitionStates(returnState);
		}
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();
		if (lastTime > 0)
		{
			float timeSince = Time.time - lastTime;
			curFuel = Mathf.Clamp(curFuel + timeSince*useAndRechargeRate, 0, maxFuel);
		}

		if (curFuel < allowedMinForJetpack)
		{
			TransitionStates(returnState);
			return;
		}

		lastGravity = _rb.gravityScale;
		_rb.gravityScale = 0;
		firstInputIgnored = false;

		movingRight = false; movingLeft = false; movingUp = false;
	}

	public void SetReturnState(State s)
	{
		if (s != this)
			returnState = s;
	}

	protected override void OnExitState()
	{
		base.OnExitState();
		lastTime = Time.time;
		_rb.gravityScale = lastGravity;
	}
}
