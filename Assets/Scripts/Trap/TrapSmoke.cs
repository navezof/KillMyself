using UnityEngine;
using System.Collections;

public class TrapSmoke : Trap
{
	public GameObject	obstaclePrefab;
	
	// Use this for initialization
	void Start ()
	{
		_type = Trap.ETrapeType.SMOKE;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public override void activeTrap(Transform collided, GameObject collid)
	{
		Instantiate (obstaclePrefab, collided.position, Quaternion.identity); 
		print("Activation of  : " + _type + " trap on " + collided.position);
	}
}
