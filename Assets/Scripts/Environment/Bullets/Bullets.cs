using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour
{
	public float		lifeSpan = 5;
	private float		startingTime;
	
	public int			damage = 1;
	
	public GameObject	explosionPrefab;
	public GameObject	collisionPrefab;
	
	void Start()
	{
		startingTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Invoke ("die", lifeSpan);
		/*
		if (Time.time - startingTime > lifeSpan)
		{
			die ();
		}*/
	}
	
	void die()
	{
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag == "Player")
		{
			col.gameObject.GetComponent<LifeManager>().takeHit(damage);
			Instantiate(collisionPrefab, col.contacts[0].point, Quaternion.identity);
			die ();
		}
	}
}
