using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour 
{	
	/*
	 * Make the player die if it collide
	 */
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			col.GetComponent<LifeManager>().die();
	}
}
