using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
	TrapManager	trapManager;
	
	// Use this for initialization
	void Start ()
	{
		trapManager = GameObject.Find("TrapManager").GetComponent<TrapManager>();
}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	void OnTriggerEnter(Collider col)
	{
		GameObject collid;
		
		collid = col.gameObject;
		if (collid.tag == "Player")
		{	
			trapManager.ActiveTrap(transform, collid);
		}
	}
}
