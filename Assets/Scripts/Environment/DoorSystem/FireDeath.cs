using UnityEngine;
using System.Collections;

public class FireDeath : MonoBehaviour
{
	private bool dead = false;
	
	// Update is called once per frame
	void Update ()
	{
		if (dead)
		{
			for (int i = 0; i < transform.GetChildCount(); i++)
			{
				if (transform.GetChild(i).GetComponent<ParticleEmitter>() != null)
					transform.GetChild(i).GetComponent<ParticleEmitter>().emit = false;
				else
					transform.GetChild(i).GetComponent<Light>().enabled = false;
			}
		}
	}
	
	public void die()
	{
		dead = true;
	}
}
