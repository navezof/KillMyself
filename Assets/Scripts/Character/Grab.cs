using UnityEngine;
using System.Collections;

public class Grab : MonoBehaviour
{
	public GameObject		spotlight;
	
	private bool			canClick = true;
	
	private GameObject		grabbedObject;
	private GameObject  	lastObjectInRange;
			
	// Use this for initialization
	void Start ()
	{
		grabbedObject = null;
		if ((spotlight = transform.GetComponentInChildren<Light>().gameObject) == null)
		{
			Debug.Log("Error : Light missing on grab script");
			return ;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canClick)
		{
			if (Input.GetMouseButtonDown(0) && grabbedObject == null)
			{
				grab();
			}
			else if (Input.GetMouseButtonDown(0) && grabbedObject != null)
			{
				drop();
			}
		}
		if (grabbedObject)
		{
			grabbedObject.transform.position = transform.position;
			spotlight.SetActive(false);
		}
		if (lastObjectInRange != null)
			spotlight.transform.LookAt(lastObjectInRange.transform.position);
		else
			spotlight.SetActive(false);
	}
	
	public void grab()
	{
		if (lastObjectInRange)
		{
			grabbedObject = lastObjectInRange;
			grabbedObject.transform.position = transform.position;
		}
	}
	
	public void drop()
	{
		grabbedObject = null;
	}
	
	public void block(bool blocked)
	{
		canClick = blocked;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Key")
		{
			if (col.transform.GetComponent<Key>().locked == false)
			{
				lastObjectInRange = col.gameObject;
				spotlight.SetActive(true);
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Key")
		{
			if (col.gameObject == lastObjectInRange)
				lastObjectInRange = null;
		}
	}
}