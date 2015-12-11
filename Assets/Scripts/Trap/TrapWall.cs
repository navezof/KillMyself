using UnityEngine;
using System.Collections;

public class TrapWall : Trap
{
	public GameObject	obstaclePrefab;
	
	// Use this for initialization
	void Start ()
	{
		_type = Trap.ETrapeType.WALL;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public override void activeTrap(Transform collided, GameObject collid)
	{
		Vector3 pos = this.gameObject.transform.position;
		GameObject created;
		
		pos = new Vector3(collided.position.x + 1, collided.position.y, collided.position.z);
		created = (GameObject)Instantiate(obstaclePrefab, pos, Quaternion.identity);
		Debug.Log(collid.transform.rotation.eulerAngles.y);
		created.transform.Rotate(0, collided.transform.rotation.eulerAngles.y, 0);
		//created.transform.rotation =  new Quaternion(0, collid.transform.rotation.y, 0, 0);
		Debug.Log(created.transform.rotation.eulerAngles.y);
		print("Activation of  : " + _type + " trap on " + collided.position);
	}
}
