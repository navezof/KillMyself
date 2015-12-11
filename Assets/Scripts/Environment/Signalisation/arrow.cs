using UnityEngine;
using System.Collections;

public class arrow : MonoBehaviour
{
	bool		characterClose = false;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		Color col = this.transform.renderer.material.color;
		
		if (characterClose && col.a <= 255)
		{
			col.a += 0.5f;
		}
		else if (col.a > 0)
		{
			col.a -= 0.5f;
		}
		this.transform.renderer.material.color = col;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			characterClose = true;
	}
	
	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
			characterClose = false;
	}
}
