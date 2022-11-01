using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float damage;
    public float range = 15f;

    public float knockback = 1f;

    private bool initialized = false;
    private float distMoved = 0;

    public void BeginMovement()
	{
        initialized = true;
	}

    public void SetSpeed(float newSpeed)
	{
        speed = newSpeed;
	}

    public void SetDamage(float newDamage)
	{
        damage = newDamage;
	}

    public void SetDirection(float x, float y)
	{
        direction = new Vector2(x, y).normalized;
	}

    public void SetDirection(Vector2 vec)
	{
        direction = vec.normalized;
	}

    // Update is called once per frame
    void Update()
    {
        if (initialized)
		{
            transform.position += speed * Time.deltaTime * (Vector3) direction;
            distMoved += (speed * Time.deltaTime * (Vector3)direction).magnitude;

            if (distMoved >= range)
                Destroy(gameObject);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out PlayerMovement pm)) {
            pm.GetComponent<Rigidbody2D>().AddForce( (pm.transform.position - transform.position).normalized*knockback );
		}
	}
}
