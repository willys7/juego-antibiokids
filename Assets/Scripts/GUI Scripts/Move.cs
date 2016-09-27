using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public float speed;
	
	// Update is called once per frame
	void Update () 
	{
        float val = 1000000;
        speed = speed/val;
		transform.Translate(Vector3.left * speed  *  Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Destroy(gameObject);
	}
}
