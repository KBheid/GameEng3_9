using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WalkState : State
{
	private bool movingLeft;
	private bool movingRight;

	private bool lastMovementWasLeft;

	public WalkState(Rigidbody2D rb, Animator anim, StateChanged callback) : base(rb, anim, callback) { }

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
			TransitionStates(new JumpState(_rb, animator, _stateChangedCallback));
		}

		if (_rb.velocity.y < -0.5f)
		{
			TransitionStates(new FallState(_rb, animator, _stateChangedCallback));
		}

		if (_rb.TryGetComponent(out PlayerMovement pm))
		{
			if (pm.hasLaunch && key == KeyCode.R)
			{
				TransitionStates(new LaunchState(_rb, animator, _stateChangedCallback));
			}
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

		if (hasAnimator)
			animator.SetBool("Moving", movingLeft || movingRight);
	}
}
