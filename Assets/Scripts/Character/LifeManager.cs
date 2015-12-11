using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour
{
	public LevelManager			levelManager;
	
	// Spawnpoint used currently by the player
	public GameObject			currentSpawnPoint;
	
	public GameObject			explosionPlayer;
	
	public int					currentLife;
	public int					maxLife = 3;
	
	public GameObject			lifeCounter;
	
	private bool				startRotating;
	
	private float				lastDeath;
	private float				respawningTime = 3.0f;
	private bool				dead = false;
	
	// Use this for initialization
	void Start ()
	{
		currentLife = maxLife;
		
		if (currentSpawnPoint == null)
			currentSpawnPoint = GameObject.Find("Spawnpoint_1").gameObject;
		if (lifeCounter == null)
		{
			if ((lifeCounter = GameObject.Find("lifeCounter").gameObject) == null)
			{
				Debug.Log("Error : No lifeCounter");
				return; 
			}
		}
		if (explosionPlayer == null)
		{
			if ((explosionPlayer = GameObject.Find("explosion_player").gameObject) == null)
			{
				Debug.Log("Error : No explosion player");
				return;
			}
		}
		if ((levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>()) == null)
		{
			Debug.Log("Error : No levelManager");
			return;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dead)
		{
			if (Time.time - lastDeath > respawningTime)
				respawn();
		}
		if (startRotating)
			rotateLife();
	}
	
	void rotateLife()
	{
		if (currentLife == 2)
		{
			//print ("1 LF rot z ; " + (lifeCounter.transform.eulerAngles.z));
			if (lifeCounter.transform.eulerAngles.z < 310)
				lifeCounter.transform.Rotate(0, 0, 1);
			else
				startRotating = false;
		}
		if (currentLife == 1)
		{
			//print ("2 LF rot z ; " + lifeCounter.transform.eulerAngles.z);
			if (lifeCounter.transform.eulerAngles.z > 0.1f)
				lifeCounter.transform.Rotate(0, 0, 1);
			else
				startRotating = false;
		}
	}

	public void changeCurrentSpawnPoint(GameObject newSpawnPoint)
	{
		if (currentSpawnPoint != null)
			currentSpawnPoint.GetComponent<SpawnPoint>().activate(false);
		currentSpawnPoint = newSpawnPoint;
	}
	
	public void die()
	{
		if (dead == false)
		{
			dead = true;
			GetComponent<AudioSource>().Play();
			GetComponentInChildren<MeshRenderer>().enabled = false;
			lifeCounter.GetComponent<MeshRenderer>().enabled = false;
			explosionPlayer.GetComponent<ParticleEmitter>().emit = true;
			GetComponent<Character_Manager>().enabled = false;
			GetComponent<Player_Manager>().init();
			lastDeath = Time.time;
			currentLife--;
		}
	}
	
	public void takeHit(int damage)
	{
		if (dead == false)
		{
			BroadcastMessage("drop");
			die ();
		}
	}
	
	void respawn()
	{
		if (currentLife <= 0)
				levelManager.setEnd(false);
		
		transform.position = currentSpawnPoint.transform.position + new Vector3(0, -1, 0);
		transform.rotation = currentSpawnPoint.transform.rotation;
		dead = false;
		startRotating = true;
		GetComponentInChildren<MeshRenderer>().enabled = true;
		lifeCounter.GetComponent<MeshRenderer>().enabled = true;
		explosionPlayer.GetComponent<ParticleEmitter>().emit = false;
		GetComponent<Character_Manager>().enabled = true;
		Camera.mainCamera.GetComponent<Camera_Manager>().initCamera();
	}
}
