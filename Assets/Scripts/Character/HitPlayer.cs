using UnityEngine;
using System.Collections;

public class HitPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<LifeManager>().takeHit(1);
		}
	}
	
	void OnCollisionStay(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			Character_Motor player = collision.gameObject.GetComponent<Character_Motor>();
			if (player != null) {
				//Using a rotation of plan in a orthogonal repert.
				float angle = this.transform.rotation.eulerAngles.y - player.transform.rotation.eulerAngles.y;

				angle = Mathf.PI * angle / 180;
				float xp = (collision.relativeVelocity.x * Mathf.Cos(angle)) - (collision.relativeVelocity.z * Mathf.Sin(angle));
				float yp = (collision.relativeVelocity.x * Mathf.Sin(angle)) + (collision.relativeVelocity.z * Mathf.Cos(angle));
				Player_Manager playerM = collision.gameObject.GetComponent<Player_Manager>();
				/*
				if (playerM.stunTarget.active == false && playerM.timer == 0) {
					player.HitMoveVector = new Vector3(-yp * 7.5f % 30, 0, xp * 7.5f% 30);
					playerM.stunTarget.active = true;
					}*/
			}
		}
	}
}
