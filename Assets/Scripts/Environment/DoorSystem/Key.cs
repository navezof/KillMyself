using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
	public enum EKeyType
	{
		RED,
		BLUE,
		GREEN
	};
	
	public EKeyType 	type;
	public GameObject	deathParticles;
	
	public bool			locked;
	
	private bool		dead = false;
	private float		timeOfDeath = 0;
	
	public KeySpawner	dispenser;
	
	void Update()
	{
		if (dead)
		{
			if (Time.time - timeOfDeath > 1)
			{
				dispenser.spawnKey();
				Destroy(this.gameObject);
			}
		}
	}
		
	public void die()
	{
		print ("CALLLED");
		deathParticles.SetActive(true);	
		transform.renderer.enabled = false;
		dead = true;
		timeOfDeath = Time.time;
	}
}
