using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public State currentState;
	public GameObject forcefieldPrefab;

	private JetpackState jetpackState;
	private State lastState;

	Animator _animator;
	Rigidbody2D _rb;
	SpriteRenderer _renderer;

	public bool hasShield = false;
	public bool hasLaunch = false;
	public bool hasJetpack = false;

	// Start is called before the first frame update
	void Start()
	{
		_animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody2D>();
		_renderer = GetComponent<SpriteRenderer>();

		currentState = new WalkState(_rb, _animator, OnStateChange);
		jetpackState = new JetpackState(_rb, _animator, OnStateChange);
	}

	void OnStateChange(State state)
	{
		lastState = currentState;
		currentState = state;
	}

	// Update is called once per frame
	void Update()
	{
		float xMovement = Input.GetAxisRaw("Horizontal");
		_renderer.flipX = xMovement != 1 && (xMovement == -1 || _renderer.flipX);

		if (hasJetpack && currentState != jetpackState && lastState != jetpackState && Input.GetKeyDown(KeyCode.T))
		{
			jetpackState.SetReturnState(currentState);
			State.TransitionStates(currentState, jetpackState);
		}

		if (Input.GetKeyDown(KeyCode.E) && hasShield)
		{
			Instantiate(forcefieldPrefab).transform.position = transform.position;
		}

		currentState.Update(Time.deltaTime);
	}

	private void OnGUI()
	{
		// Send input to current state
		Event e = Event.current;

		switch (e.type)
		{
			case EventType.KeyDown:
				currentState.Input(e.keyCode, true);
				break;

			case EventType.KeyUp:
				currentState.Input(e.keyCode, false);
				break;
		}
	}

}
