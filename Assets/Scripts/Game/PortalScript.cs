using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{
	public LevelManager	levelManager;
	public string		nextLevelName;
	public GameObject	endCamera;
	
	public float		transitionTime;
	private float		transitionStartingTime;
	private bool		transition = false;
		
	// Use this for initialization
	void Start ()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transition)
		{
			if (Time.time - transitionStartingTime > transitionTime)
			{
				levelManager.setEnd(true);
				//Application.LoadLevel("Moutain");
			}
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			if (nextLevelName != "")
			{
				print ("Load : " + nextLevelName);
				endCamera.SetActive(true);
				transform.GetComponentInChildren<ParticleEmitter>().emit = true;
				transitionStartingTime = Time.time;
				transition = true;
			}
		}
	}
}
