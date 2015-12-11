using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
	public GameObject gateLight;
	public GameObject sparkle;
	// Associated spawner
    public List<GameObject>   spawnersToActiveList = new List<GameObject>();
    public List<GameObject>   spawnersInTheSceneList = new List<GameObject>();
	// Number of this spawn
	public int			spawnNumber;
	//public GameObject	bombingSound;
	
	//public GameObject[] indications;
	
	private bool 		activated;
	
	// Use this for initialization
	void Start ()
	{
		/*if (spawners && !activated)
			activateAllSpawner(false);
		activate(false);*/
	}
		
	public void activate(bool v)
	{
		gateLight.transform.renderer.material.color = Color.green;
		if (spawnersToActiveList != null)
		{
        	activateAllSpawner(true);
			sparkle.SetActive(true);
		}
		//if (bombingSound != null)
		//	bombingSound.SetActive(true);

		if (!v)
		{
			gateLight.transform.renderer.material.color = Color.red;
			activateAllSpawner(false);
			sparkle.SetActive(false);
			//if (bombingSound != null)
			//	bombingSound.SetActive(false);
		}
	}
	
	void activateAllSpawner(bool v)
	{
		if (spawnersToActiveList != null)
		{
            for (int i = 0; i < spawnersInTheSceneList.Count; i++)
			{
                spawnersInTheSceneList[i].GetComponent<SpawnerManager>().isActive = false;
			}
			
			for (int i = 0; i < spawnersToActiveList.Count; i++)
			{
                spawnersToActiveList[i].GetComponent<SpawnerManager>().isActive = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			col.transform.GetComponent<LifeManager>().changeCurrentSpawnPoint(this.gameObject);
			activate(true);
		}
	}
}
