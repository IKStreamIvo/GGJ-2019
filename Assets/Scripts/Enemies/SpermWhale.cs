using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpermWhale : Enemy {

	[SerializeField] private Rigidbody2D r2d;
	[SerializeField] private SpriteRenderer sr;
	[SerializeField] private float movementSpeed;
	
    // Start is called before the first frame update
    public override void Start()
    {
		base.Start();
		if (r2d == null)
			r2d = GetComponent<Rigidbody2D>();

		if (sr == null)
			sr = GetComponent<SpriteRenderer>();
    }

	public override void Update() {
		base.Update();

		
		Vector2 direction = (player.position - transform.position).normalized;
		if(direction.x < 0) {
			if (!sr.flipX)
				sr.flipX = true;
		} else {
			if (sr.flipX)
				sr.flipX = false;
		}

		direction *= movementSpeed;
		r2d.velocity = direction;

	}
}
