using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour
{
	// Count the time elapsed since the start of the game
	public float	timer;	
	// if true the menu is displayed
	private bool	menu = false;
	
	void Start ()
	{
		timer = Time.time;
	}
	
	/*
	 * Call the other drawing functions if needed
	 */
	void Update ()
	{
		timer = Time.time;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (menu)
				menu = false;
			else
				menu = true;
		}
	}
	
	void OnGUI()
	{
		drawTimer();
		if (menu)
			drawMenu();
	}
	
	
	/*
	 * Draw the timer on the top of the screen 
	 */
	void drawTimer()
	{
		GUI.Label(new Rect(Screen.width/2, 10, 300, 30), timer.ToString());
	}
	
	/*
	 * Draw the menu allowing to restart or quit the application
	 */
	void drawMenu()
	{
		GUI.Box(new Rect(10, Screen.height - Screen.height/6, 110, 100), "Menu");
		if (GUI.Button(new Rect(15, Screen.height - Screen.height/8, 100, 30), "Try again."))
		{
			Application.LoadLevel("Moutain");
		}
		if (GUI.Button(new Rect(15, Screen.height - Screen.height/14, 100, 30), "Quit."))
		{
			Application.Quit();
		}
	}
}
