using UnityEngine;
using System.Collections;

public class KeySpawner : MonoBehaviour
{
	// Key to be spawned
	public GameObject		key;
	
	void Start()
	{
		spawnKey();
	}
	
	public void spawnKey()
	{
		GameObject newKey = (GameObject) Instantiate(key, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
		newKey.GetComponent<Key>().dispenser = this;
	}
}
