using UnityEngine;
using System.Collections;

public class TransitionScript : MonoBehaviour
{
	public TextMesh		finish_text;
	public TextMesh		title_text;
	public LevelManager	levelManager;
	
	private Rect		tryAgainPos = new Rect(Screen.width/10, Screen.height - Screen.height/4, 100, 50);
	public string		tryAgainText = "Try again.";

	private Rect		nextLevelPos = new Rect(Screen.width/2, Screen.height - Screen.height/4, 100, 50);
	public string		nextLevelText = "Next level";
	
	private Rect		quitPos = new Rect(Screen.width - Screen.width/8, Screen.height - Screen.height/4, 100, 50);
	public string 		quitText = "Exit game";
	
	private Rect		tryAgainNoticePos = new Rect(Screen.width/2, Screen.height/2, 100, 100);
	public string		tryAgainNotice = "You will restart the level with all your score, gold and lives you currently have. Are you ok with that?";
	
	private Rect		tryAgainYesAnswerPos = new Rect(Screen.width/4, Screen.height/3, 100, 50);
	public string		tryAgainYesAnswer = "Yes!";
	
	private Rect		tryAgainNoAnswerPos = new Rect(Screen.width - Screen.width/4, Screen.height/3, 100, 50);
	public string		tryAgainNoAnswer = "No I want to start from scratch!";
	
	public Rect			scorePos = new Rect(10, Screen.height/2, 500, 500);
	
	
	void Start ()
	{
		if ((levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>()) == null)
		{
			return;
		}
		if (!levelManager.success)
		{
			title_text.text = "Game Over!";
			finish_text.text = "";
		}
		else
		{
			title_text.text = "Congratulation!";
			finish_text.text = "You just finished <b>" + levelManager.prevLevel + "</b>";
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(tryAgainPos, tryAgainText))
			tryAgainGUI();
		if (levelManager)
		{
			if (levelManager.success)
			{
				drawScoreGUI();
				if (GUI.Button(nextLevelPos, nextLevelText))
				{
					Application.LoadLevel(levelManager.nextLevel);
				}
			}
		}
		if (GUI.Button(quitPos, quitText))
		{
			Application.Quit();
		}
	}
	
	void drawScoreGUI()
	{
		print ("DRawed?");
		GUI.Label(new Rect(20, Screen.height/3, 300, 50), "Time : " + levelManager.time);
		GUI.Label(new Rect(20, Screen.height/3 + 50, 300, 50), "Gold collected : " + levelManager.goldCollected + "/" + levelManager.totalGold);
		GUI.Label(new Rect(20, Screen.height/3 + 110, 300, 50), "Gold value : " + levelManager.goldvalue);
		GUI.Label(new Rect(20, Screen.height/3 + 170, 300, 50), "Total Score : " + levelManager.time * ((levelManager.goldvalue * levelManager.difficulty) + levelManager.goldCollected));
	}
	
	void tryAgainGUI()
	{
		if (levelManager.success)
		{
			GUI.Box (tryAgainNoticePos, tryAgainNotice);
			if (GUI.Button(tryAgainYesAnswerPos, tryAgainYesAnswer))
			{
				// PlayerInformation
				DontDestroyOnLoad(levelManager);
			}
		}
		Application.LoadLevel(levelManager.prevLevel);
	}
	
	
}
