using UnityEngine;
using System.Collections;

public class TrapJump : Trap
{
	GameObject			obstaclePrefab;
	public float 		jumpForce = 40;
	
	
	// Use this for initialization
	void Start ()
	{
		_type = Trap.ETrapeType.SPRING;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public override void activeTrap(Transform collided, GameObject collid)
	{
        //TODO remove this trap;
		print("Activation of  : " + _type + " trap on " + collided.position);
	}
}
