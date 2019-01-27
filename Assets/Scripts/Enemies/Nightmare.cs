using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightmare : Enemy {

	[SerializeField] private Rigidbody2D r2d;
	[SerializeField] private SpriteRenderer sr;
	[SerializeField] private float movementSpeed;

	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private float projectileVelocity;
	private float _attackTimer = 0f;

	// Start is called before the first frame update
	public override void Start(){
		base.Start();

		if (r2d == null)
			r2d = GetComponent<Rigidbody2D>();

		if (sr == null)
			sr = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    public override void Update() {
		base.Update();

		Vector2 direction = (player.position - transform.position).normalized;
		if (direction.x < 0) {
			if (!sr.flipX)
				sr.flipX = true;
		} else {
			if (sr.flipX)
				sr.flipX = false;
		}

		direction *= movementSpeed;
		r2d.velocity = direction;

		if (_attackTimer >= attackDelay) {
			Fire();
			_attackTimer = 0f;
		} else {
			_attackTimer += Time.deltaTime;
		}
	}

	private void Fire() {
		GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();

		Vector2 direction = (player.position - transform.position).normalized;
		direction *= projectileVelocity;
		projRb.velocity = direction;
	}
}
