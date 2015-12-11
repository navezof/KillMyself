using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public string		prevLevel;
	public string		nextLevel;
	
	public float		goldvalue;
	public int			goldCollected = 0;
	public int			totalGold = 0;
	public GameObject	golds;
	public int			difficulty = 1;
	public float		time;
	public bool			success = false;
	public bool			continueUsed = false;
	
	private float		endGold = 0;
	
	private Player_Manager	player;
	
	void Start()
	{
		player = GameObject.Find("Character").GetComponent<Player_Manager>();
		foreach (Transform child in golds.transform)
			totalGold++;
	}
	
	public void setEnd(bool succeded)
	{
		goldvalue = player.gold;
		goldCollected = player.numberOfGold;
		time = player.GetComponent<PlayerGUI>().timer;
		success = succeded;
		
		loadNextLevel();
	}
	
	public void loadNextLevel()
	{		
		DontDestroyOnLoad(this.gameObject);
		Application.LoadLevel("Transition");
	}
}
