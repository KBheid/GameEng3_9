using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class State
{
	public delegate void StateChanged(State state);

	protected Rigidbody2D _rb;
	protected Animator animator;

	protected StateChanged _stateChangedCallback;

	protected bool hasAnimator;

	protected State(Rigidbody2D rb, Animator anim, StateChanged callback)
	{
		_rb = rb;
		animator = anim;
		_stateChangedCallback = callback;

		hasAnimator = anim != null;
	}

	protected virtual void OnEnterState() { }
	protected virtual void OnExitState() { }
	public virtual void Update(float deltaTime) { }
	// pressed 
	public virtual void Input(KeyCode key, bool pressed) { }

	protected virtual void TransitionStates(State state)
	{
		OnExitState();
		_stateChangedCallback(state);
		state.OnEnterState();
	}

	public static void SetStateNoExit(State state)
	{
		state.OnEnterState();
	}

	public static void TransitionStates(State oldState, State newState)
	{
		oldState.TransitionStates(newState);
	}
}