using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchState : State
{
	private float _maxCharge = 3f;
	private float _timeCharged = 0f;

	public LaunchState(Rigidbody2D rb, Animator anim, StateChanged callback) : base(rb, anim, callback) { }

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		_timeCharged = Mathf.Clamp(_timeCharged + deltaTime, 0, _maxCharge);
		if (_timeCharged == _maxCharge)
		{
			Launch();
		}
	}

	public override void Input(KeyCode key, bool pressed)
	{
		base.Input(key, pressed);

		if (key == KeyCode.R && pressed == false)
		{
			Launch();
		}
	}

	private void Launch()
	{
		_rb.AddForce(new Vector2(0, 650 * _timeCharged / _maxCharge));
		TransitionStates(new FallState(_rb, animator, _stateChangedCallback));
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();

		if (hasAnimator)
		{
			animator.SetTrigger("BeginJump");
			animator.SetBool("Grounded", false);
		}
	}
}
