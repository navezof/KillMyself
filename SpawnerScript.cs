using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class SpawnerScript : MonoBehaviour
{

	// Use this for initialization
    public bool isActive = false;
    public float rateOfSpawn = 0.1f;
    public int numberMaxObj = 50;
    public float LastSpawn = 0;
	public int	currentObj = 0;
    private List<GameObject> objs = new List<GameObject>(); 
	public List<GameObject> keys = new List<GameObject>();
	private List<GameObject> keysCreated = new List<GameObject>();
	
	
	public String BulletName;
	public int	IdBullet;
	public int KeyRate = 10;
	private int currentKeyCreated = 0;
	void Start () {

	}
	
    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "spawn", true);
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);
    }
	
	
	private void createBullet() {
		System.Random rand = new System.Random();
		string name = BulletName + rand.Next(1,IdBullet  + 1);
			Debug.Log(name);
        GameObject pl = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)),
                transform.position ,
                transform.rotation
                );
		pl.rigidbody.rotation = transform.rotation;
		Vector3 v =  Vector3.forward;
		if (v.x == 0) v.x = -0.5f + (float)rand.Next(10) / 10.0f;
		if (v.y == 0) v.y = -0.5f + (float)rand.Next(10) / 10.0f;
		v.x *= 10 + rand.Next(20);
		v.z *= 10 + rand.Next(20);
		Debug.Log(v);
		pl.rigidbody.velocity =  transform.TransformDirection (v); // new Vector3(10 + rand.Next(20),-1 * rand.Next(25, 35),-1 * rand.Next(45, 55));
		pl.rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));
		objs.Add(pl);	
	}
	
	private void reSpawnBullet() 
	{
		System.Random rand = new System.Random();
		currentObj++;
		if (currentObj >= objs.Count) {currentObj = 0;}
		objs[currentObj].transform.position = transform.position;
		objs[currentObj].rigidbody.velocity = new Vector3(0,0,0);
		Vector3 v =  Vector3.forward;
		if (v.x == 0) v.x = -0.5f + (float)rand.Next(10) / 10.0f;
		if (v.y == 0) v.y = -0.5f + (float)rand.Next(10) / 10.0f;
		v.x *= 10 + rand.Next(20);
		v.z *= 10 + rand.Next(20);
		Debug.Log(v);
		objs[currentObj].rigidbody.velocity =  transform.TransformDirection (v); 
		objs[currentObj].rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));

	}
	
	private void reSpwanKey()
	{
		System.Random rand = new System.Random();
		if (currentKeyCreated >= keysCreated.Count)
			currentKeyCreated = 0;
		GameObject key = keysCreated[currentKeyCreated++];
		if (key.GetComponent<Key>() != null && 
			(!key.GetComponent<Key>().locked || !key.GetComponent<Key>().used) ) {
				key.transform.position = transform.position;
				key.rigidbody.velocity = new Vector3(0,0,0);
				Vector3 v =  Vector3.forward;
				if (v.x == 0) v.x = -0.5f + (float)rand.Next(10) / 10.0f;
				if (v.y == 0) v.y = -0.5f + (float)rand.Next(10) / 10.0f;
				v.x *= 10 + rand.Next(20);
				v.z *= 10 + rand.Next(20);
				Debug.Log(v);
				key.rigidbody.velocity =  transform.TransformDirection (v); 
				key.rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));
			}
	}
	
	private void createKey() 
	{
		System.Random rand = new System.Random();
		GameObject pl = (GameObject)Instantiate(keys[keysCreated.Count], transform.position, transform.rotation);
		Vector3 v =  Vector3.forward;
		if (v.x == 0) v.x = -0.5f + (float)rand.Next(10) / 10.0f;
		if (v.y == 0) v.y = -0.5f + (float)rand.Next(10) / 10.0f;
		v.x *= 10 + rand.Next(20);
		v.z *= 10 + rand.Next(20);
		Debug.Log(v);
		pl.rigidbody.velocity =  transform.TransformDirection (v); // new Vector3(10 + rand.Next(20),-1 * rand.Next(25, 35),-1 * rand.Next(45, 55));
		pl.rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));
		keysCreated.Add(pl);
	}
	
	// Update is called once per frame
	void Update () {
		System.Random rand = new System.Random();
		if (isActive) 
		{
	        if (Time.time > LastSpawn + rateOfSpawn) {
	            LastSpawn = Time.time;
				double r = rand.Next(0, KeyRate);
				Debug.Log(r);
				if (r == 0 && keys.Count > 0) {
					if (keysCreated.Count >= keys.Count) {
						reSpwanKey();
					}
					else {
						createKey();
					}				
				}
				else {
					if (objs.Count < numberMaxObj) {
						createBullet();
					}
					else {
						reSpawnBullet();
					}
				}
	        }
		}
		else {
			if (objs.Count != 0)
			{
				objs.ForEach(delegate(GameObject b) {GameObject.Destroy(b);});
				objs.Clear();
			}
			if (keysCreated.Count != 0) 
			{
				keysCreated.ForEach(delegate(GameObject k) {GameObject.Destroy(k);});
				keysCreated.Clear();
			}
		}
	}
}
