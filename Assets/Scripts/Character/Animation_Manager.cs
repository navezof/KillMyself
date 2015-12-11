using UnityEngine;
using System.Collections;

public class Animation_Manager : MonoBehaviour
{
	public enum MotionStateList 
	{
		Stationary,
		Forward,
		Backward,
		Left,
		Right,
		LeftForward,
		RightForward,
		LeftBackward,
		RightBackward,
		Sliding,
		NoMoveSliding,
		Jumping,
		Strafing,
		Dead
	};
	
	public MotionStateList CharacterMotionState;
	
	public Character_Motor	characterMotorScript;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public void CurrentMotionState()
	{
		bool bForward = false;
		bool bRight = false;
		bool bLeft = false;
		bool bBackward = false;
		
		if (characterMotorScript.MoveVector.z > 0)
			bForward = true;
		if (characterMotorScript.MoveVector.z < 0)
			bBackward = true;
		if (characterMotorScript.MoveVector.x > 0)
			bRight = true;
		if (characterMotorScript.MoveVector.x < 0)
			bLeft = true;
		
		if (bForward)
		{
			CharacterMotionState = MotionStateList.Forward;
			if (bLeft)
				CharacterMotionState = MotionStateList.LeftForward;
			if (bRight)
				CharacterMotionState = MotionStateList.RightForward;
		}
		else if (bBackward)
		{
			CharacterMotionState = MotionStateList.Backward;
			if (bLeft)
				CharacterMotionState = MotionStateList.LeftBackward;
			if (bRight)
				CharacterMotionState = MotionStateList.RightBackward;
		}
		else if (bLeft)
			CharacterMotionState = MotionStateList.Left;
		else if (bRight)
			CharacterMotionState = MotionStateList.Right;
		else
			CharacterMotionState = MotionStateList.Stationary;
		//print ("Current motion state : " + CharacterMotionState.ToString());
	}
}
