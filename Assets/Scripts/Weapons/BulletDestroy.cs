using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{

	public float timeSinceSpawn;
	public float bulletLife;

    // Start is called before the first frame update
    void Start()
    {
		timeSinceSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time > timeSinceSpawn + bulletLife) {
			Destroy(gameObject);
		}        
    }
}
