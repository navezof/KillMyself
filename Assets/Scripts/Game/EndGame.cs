using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour
{
	// Model used for the end animation
	public GameObject		model;
	public LevelManager		levelManager;
	// Camera of the end animation
	public Camera			camera;
	
	private bool			gameover;
	
	// Actual index of the text displayed
	private int 				index = 0;
	
	void Start ()
	{
		index = 0;
		model.SetActive(false);
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}
	
	/*
	 * Get the key or mosue input to display the different texts
	 */
	void Update ()
	{
		if (gameover == true)
		{
			if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
			{
				index++;
			}
		}
	}
	
	/*
	 * Launch and initilize all the parameters to launch correctly the animation.
	 */
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			col.GetComponent<Character_Manager>().enabled = false;
			col.gameObject.SetActive(false);
			Camera.mainCamera.enabled = false;
			camera.enabled = true;
			model.SetActive(true);
			model.gameObject.GetComponent<Animation>().Play();
			gameover = true;
		}
	}
	
	/*
	 * Display the text
	 */
	void OnGUI()
	{
		if (gameover)
		{
			if (index > 0)
				GUI.Box(new Rect(Screen.width/4, 30, 400, 30), "And he died and the world was safe again");
			if (index > 1)
				GUI.Box(new Rect(Screen.width/4, 70, 400, 30), "...");
			if (index > 2)
				GUI.Box(new Rect(Screen.width/4, 110, 400, 30), "Maybe.");
			if (index > 3)
				GUI.Box(new Rect(Screen.width/4, 150, 400, 30), "His last words were : \"Blub blub, it's hot!\"");
			if (index > 4)
				GUI.Box(new Rect(Screen.width/4, 190, 400, 30), "Well, duh, it's a volcano after all.");
			if (index > 5)
				GUI.Box(new Rect(Screen.width/4, 230, 400, 30), "...");
			if (index > 6)
				GUI.Box(new Rect(Screen.width/4, 270, 400, 30), "It was fun!");
			if (index > 7)
				levelManager.setEnd(true);
		}
	}
	
	void displayRetry()
	{
		GUI.Box(new Rect(Screen.width/4, 310, 400, 30), "Should we try again?");
		if (GUI.Button(new Rect(Screen.width/12, Screen.height - Screen.height/4, 100, 40), "Hell yeah!"))
		{
			Application.LoadLevel("Moutain");
		}
		if (GUI.Button(new Rect(Screen.width - Screen.width/4, Screen.height - Screen.height/4, 100, 40), "Not again!"))
		{
			Application.Quit();
		}				
	}
}
