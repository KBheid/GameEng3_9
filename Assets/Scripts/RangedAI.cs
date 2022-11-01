using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : EnemyAI
{
    public Projectile projectile;
    public float attackDelay;
    public float damage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        state = new PatrolState(GetComponent<Rigidbody2D>(), null, UpdateState)
            .SetAttackState(new AIShootState(GetComponent<Rigidbody2D>(), null, UpdateState));
        
        State.SetStateNoExit(state);
    }

    // Update is called once per frame
    void Update()
    {
        state.Update(Time.deltaTime);
    }
}
