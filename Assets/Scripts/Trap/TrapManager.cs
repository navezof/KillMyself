using UnityEngine;
using System.Collections;

public class TrapManager : MonoBehaviour
{
	public Trap[]	traps;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public void ActiveTrap(Transform trans, GameObject collid)
	{
		int i;
		
		i = Random.Range(0, traps.Length);
		print ("Trap : " + i);
		traps[i].activeTrap(trans, collid);
	}
}
