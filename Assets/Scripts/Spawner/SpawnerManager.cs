using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour {

	// Use this for initialization
	public bool isActive = false;
	private bool lastState = false;
	
	void Start () {
		manage(isActive);
	}
	
	void manage(bool v) {
		
		for (int i = 0; i < this.transform.GetChildCount() ; i++) {
			this.transform.GetChild(i).GetComponent<SpawnerScript>().isActive = v;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (lastState != isActive) {
			lastState = isActive;
			manage(isActive);
			if (this.GetComponent<SpawnerScript>() != null)
				this.GetComponent<SpawnerScript>().isActive = isActive;
		}
	}
}
