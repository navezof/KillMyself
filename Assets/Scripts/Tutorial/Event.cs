using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour
{
	public GameObject	allEvents;
	public GameObject	player;
	public Camera		mainCamera;
	public Camera		eventCamera;
	
	public bool			playerBlock = true;
	//public bool		cameraMovement = false;
	
	//public Transform[]  cameras;
	public string[]		texts;
	
	private bool		done = false;
	public bool			active = false;
	public int			eventNumber = 0;
	
	private Rect		boxPosition;
	private Rect		textPosition;
	private Rect		nextPosition;
	
	// Use this for initialization
	void Start ()
	{
		if (!player)
			player = GameObject.Find("Character");
		if (!allEvents)
			allEvents = GameObject.Find("Events").gameObject;
		if (!mainCamera)
			mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		//mainCamera = Camera.mainCamera;
		boxPosition = new Rect(10, Screen.height - Screen.height / 10, Screen.width - 20, Screen.height/10);
		textPosition = new Rect(20, Screen.height - Screen.height / 14, Screen.width - 40, Screen.height/10);
		nextPosition = new Rect(Screen.width - Screen.width / 6, Screen.height - Screen.height / 24, Screen.width - 40, Screen.height/10);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (active)
		{
			if (Input.GetMouseButtonDown(1))
				eventNumber++;
			if (eventNumber >= texts.Length)
			{
				done = true;
				active = false;
				if (playerBlock)
					setBlock(true);
			}
		}		
	}
	
	void OnGUI()
	{
		if (active)
			drawText();
	}
	
	/*
	void moveCamera()
	{
		if (eventNumber < cameras.Length)
		{
			if (cameras[eventNumber] != null)
			{
				mainCamera.GetComponent<Camera_Manager>().sleep = true;
				mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
															cameras[eventNumber].position, 
															Time.deltaTime);
				mainCamera.transform.LookAt(cameras[eventNumber].GetChild(0).transform);
			}
			else
			{
				mainCamera.GetComponent<Camera_Manager>().sleep = false;
			}
		}
	}
	*/
	
	void drawText()
	{
		GUI.Box(boxPosition, "");
		if (eventNumber < texts.Length)
			GUI.Label(textPosition, texts[eventNumber]);
		GUI.Label(nextPosition, "<i>Click left mouse button for next</i>");
	}
	
	void setBlock(bool blocked)
	{
		player.GetComponentInChildren<Grab>().block(blocked);
		player.GetComponent<Character_Manager>().enabled = blocked;
		mainCamera.GetComponent<Camera_Manager>().enabled = blocked;
		if (eventCamera != null)
		{
			eventCamera.enabled = !blocked;
			mainCamera.enabled = blocked;
		}
	}
	
	public void activeEvent()
	{
		desactiveAll();
		active = true;
	}
	
	void desactiveAll()
	{
		for (int i = 0; i < allEvents.transform.GetChildCount();i++)
		{
			allEvents.transform.GetChild(i).GetComponent<Event>().active = false;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player" && !done)
		{
			activeEvent();
			if (playerBlock)
				setBlock(false);
		}
	}
}
