using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
	
	public float explosionTimerInSecond;
	// Use this for initialization
	void Start () {
		StartCoroutine(selfdestruct());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 IEnumerator selfdestruct() {
		yield return  new WaitForSeconds(explosionTimerInSecond);
		Destroy(gameObject);
	}
}
