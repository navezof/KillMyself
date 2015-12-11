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
    public float firePower = 10;
	public GameObject explosionPrefab;
    protected List<GameObject> objs = new List<GameObject>(); 
	public List<GameObject> keys = new List<GameObject>();
	private List<GameObject> keysCreated = new List<GameObject>();


    public bool  cadencedFire = false;
    public float timeToWaitToFire = 0;
    public float numberOfBulletPerRafale = 5;
    public float currentTimeFire = 0;
    protected float currentBullet = 0;

	public String BulletName;
	public int	IdBullet;
	public int KeyRate = 10;
    protected int currentKeyCreated = 0;

    protected void Start()
    {

	}
	
    protected void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "spawn", true);
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);
    }


    protected void createBullet()
    {
		System.Random rand = new System.Random();
		string name = BulletName + rand.Next(1,IdBullet  + 1);
			//Debug.Log(name);
        GameObject pl = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)),
                transform.position ,
                transform.rotation
                );
		pl.rigidbody.rotation = transform.rotation;
		Vector3 v =  Vector3.forward;
		//Debug.Log(v);
		pl.rigidbody.velocity =  transform.TransformDirection (v * firePower); 
		objs.Add(pl);	
	}

    protected void reSpawnBullet() 
	{
		
		System.Random rand = new System.Random();

		if (currentObj >= objs.Count) {currentObj = 0;}
		if (objs[currentObj] != null && explosionPrefab != null)
		{
			Instantiate(explosionPrefab, objs[currentObj].transform.position, objs[currentObj].transform.rotation);
		objs[currentObj].transform.position = transform.position;
		objs[currentObj].rigidbody.velocity = new Vector3(0,0,0);
		Vector3 v =  Vector3.forward;
		objs[currentObj].rigidbody.velocity =  transform.TransformDirection (v * firePower); 
		objs[currentObj].rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));
		}
        currentObj++;

	}

    protected void reSpwanKey()
	{
		System.Random rand = new System.Random();
		if (currentKeyCreated >= keysCreated.Count)
			currentKeyCreated = 0;
		GameObject key = keysCreated[currentKeyCreated++];
		if (key.GetComponent<Key>() != null && 
			(!key.GetComponent<Key>().locked)) {
				key.transform.position = transform.position;
				key.rigidbody.velocity = new Vector3(0,0,0);
				Vector3 v =  Vector3.forward;
				key.rigidbody.velocity =  transform.TransformDirection (v * firePower); 
				key.rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));
			}
	}

    protected void createKey() 
	{
		System.Random rand = new System.Random();
        if (keys[keysCreated.Count] != null) {
		    GameObject pl = (GameObject)Instantiate(keys[keysCreated.Count], transform.position, transform.rotation);
		    Vector3 v =  Vector3.forward;
		    pl.rigidbody.velocity =  transform.TransformDirection (v * firePower); // new Vector3(10 + rand.Next(20),-1 * rand.Next(25, 35),-1 * rand.Next(45, 55));
		    pl.rigidbody.AddRelativeTorque(rand.Next(8), rand.Next(8), rand.Next(8));
		    keysCreated.Add(pl);
        }
	}
	
	// Update is called once per frame

    protected bool spwan() {
        System.Random rand = new System.Random();
        if (isActive)
        {
            if (Time.time > LastSpawn + rateOfSpawn)
            {
                LastSpawn = Time.time;
                double r = rand.Next(0, KeyRate);
                //Debug.Log(r);
                if (r == 0 && keys.Count > 0)
                {
                    if (keysCreated.Count >= keys.Count)
                    {
                        reSpwanKey();
                        return true;
                    }
                    else
                    {
                        createKey();
                        return true;
                    }
                }
                else
                {
                    if (objs.Count < numberMaxObj)
                    {
                        createBullet();
                        return true;
                    }
                    else
                    {
                        reSpawnBullet();
                        return true;
                    }
                }
            }
        }
        else
        {
            if (objs.Count != 0)
            {
                objs.ForEach(delegate(GameObject b) { GameObject.Destroy(b); });
                objs.Clear();
            }
            if (keysCreated.Count != 0)
            {
                keysCreated.ForEach(delegate(GameObject k) { GameObject.Destroy(k); });
                keysCreated.Clear();
            }
        }
        return false;
    }

    public void Update() {
        fire();
    }

    protected void fire()
    {
        if (cadencedFire)
        {
            if (Time.time > currentTimeFire + timeToWaitToFire)
            {
                if (currentBullet >= numberOfBulletPerRafale)
                {
                    currentBullet = -1;
                    currentTimeFire = Time.time;
                }
               if (spwan())
                currentBullet++;
            }
        }
        else
            spwan();
	}
}
