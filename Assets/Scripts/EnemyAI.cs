using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 1f;
    public State state;
    public GameObject target;

	protected virtual void Start()
	{
		target = FindObjectOfType<PlayerMovement>().gameObject;
	}

	protected void UpdateState(State state)
	{
        this.state = state;
	}

	// Update is called once per frame
	void Update()
    {
        state.Update(Time.deltaTime);
    }
}
