using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FallState : State
{
	public FallState(Rigidbody2D rb, Animator anim, StateChanged callback) : base(rb, anim, callback) { }

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		Vector2 pos = _rb.transform.position;
		pos.y -= 0.015f;

		RaycastHit2D hit = Physics2D.BoxCast(pos, new Vector2(0.15f, 3f), 0, Vector2.down, 0, layerMask: LayerMask.GetMask("Wall"));
		if (hit.collider != null)
		{
			TransitionStates(new WalkState(_rb, animator, _stateChangedCallback));
		}
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();
		
		if (hasAnimator)
			animator.SetTrigger("BeginFall");
	}

	protected override void OnExitState()
	{
		base.OnExitState();
		
		if (hasAnimator)
			animator.SetBool("Grounded", true);
	}
}