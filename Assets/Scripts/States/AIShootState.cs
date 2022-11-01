using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootState : State
{
	RangedAI rai;

	private float timeSinceAttack = 0f;

	public AIShootState(Rigidbody2D rb, Animator anim, StateChanged callback) : base(rb, anim, callback)
	{
		rb.TryGetComponent(out rai);
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		timeSinceAttack += deltaTime;

		if (timeSinceAttack >= rai.attackDelay)
		{
			Attack();
		}

		if ( (rai.target.transform.position - rai.transform.position).magnitude > rai.detectionRange)
		{
			TransitionStates(new PatrolState(_rb, animator, _stateChangedCallback).SetAttackState(this));
		}
	}


	private void Attack()
	{
		timeSinceAttack = 0f;

		Projectile proj = Object.Instantiate(rai.projectile);
		proj.transform.position = _rb.transform.position;

		proj.SetDirection(rai.target.transform.position - rai.transform.position);
		proj.BeginMovement();

	}
}
