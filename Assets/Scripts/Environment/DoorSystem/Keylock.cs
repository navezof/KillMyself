using UnityEngine;
using System.Collections;

public class Keylock : MonoBehaviour
{
	public Grab	grabberScript;
	
	// Door the keylock open
	public GameObject 	door;
	private bool		opened = false;
	
	// Number of keys needed to open the door
	public int redNeeded;
	public int blueNeeded;
	public int greenNeeded;
	
	public GameObject redPrefab;
	public GameObject bluePrefab;
	public GameObject greenPrefab;
	
	//public Transform[]	flameSlots;
	public ArrayList flameSlots = new ArrayList();
	public ArrayList keySlots = new ArrayList();
	
	public float	doorOpeningSpeed = 0.05f;
	private float	openPosition;
	
	// Use this for initialization
	void Start ()
	{
		if (grabberScript == null)
			grabberScript = GameObject.Find("Grabber").gameObject.GetComponent<Grab>();
		reduceKeys();
		createFlame();
		openPosition = door.transform.position.y - 4;
	}
	
	void Update()
	{
		for (int i = 0; i < keySlots.Count ; i++)
		{
			GameObject key = (GameObject) keySlots[i];
			key.transform.position = new Vector3(transform.position.x,
												transform.position.y + 1 * i,
												transform.position.z);
		}
		if (opened == true && door.transform.position.y >= openPosition)
		{
			door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - doorOpeningSpeed, door.transform.position.z);
		}
	}
	
	void reduceKeys()
	{
		redNeeded = Mathf.Clamp(redNeeded, 0, 3);
		blueNeeded = Mathf.Clamp(blueNeeded, 0, 3);
		greenNeeded = Mathf.Clamp(greenNeeded, 0, 3);
	}
	
	void createFlame()
	{		
		int index = 0;
		
		GameObject flame = null;
		for (int redCreated = redNeeded ; redCreated > 0; redCreated--)
		{	
			flame = (GameObject) Instantiate(redPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			flame.transform.parent = door.transform;
			flame.transform.localPosition = new Vector3(-0.4f + (0.4f * index), 0.2f, -2);
			flameSlots.Add(flame);
			index++;
		}
		for (int blueCreated = blueNeeded; blueCreated > 0; blueCreated--)
		{	
			flame = (GameObject) Instantiate(bluePrefab, new Vector3(0, 0, 0), Quaternion.identity);
			flame.transform.parent = door.transform;
			flame.transform.localPosition = new Vector3(-0.4f + (0.4f * index), 0.2f, -2);
			flameSlots.Add(flame);
			index++;
		}
		for (int greenCreated = greenNeeded; greenCreated > 0; greenCreated--)
		{	
			flame = (GameObject) Instantiate(greenPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			flame.transform.parent = door.transform;
			flame.transform.localPosition = new Vector3(-0.4f + (0.4f * index), 0.2f, -2);
			if (flameSlots.Count < 3)
				flameSlots.Add(flame);
			index++;
		}
	}
	
	void placeKey(GameObject key)
	{
		keySlots.Add(key);
		key.GetComponent<Key>().locked = true;
		removeFlame(key.GetComponent<Key>().type);
	}
	
	void removeFlame(Key.EKeyType type)
	{
		for (int i = 0; i < door.transform.GetChildCount(); i++)
		{
			Transform flame = door.transform.GetChild(i);
			if (flame.name.ToUpper().Contains(type.ToString()))
			{
				flame.GetComponent<FireDeath>().die();
				return;
			}
		}
	}
	
	void openDoor()
	{
		if (opened == false)
		{
			GetComponentInChildren<MeshRenderer>().enabled = true;
			//door.animation.Play();
			opened = true;
		}
	}
	/*
	 * If the collider is a key, remove one needed of the door.
	 * Destroy the door if the number of keys is reached.
	 */
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Key")
		{
			Key	keyscript = col.GetComponent<Key>();
			if (keyscript.locked == true)
				return;
			keyscript.locked = true;
			grabberScript.drop();
			
			if (keyscript.type == Key.EKeyType.RED)
			{	
				if (redNeeded <= 0)
					keyscript.die();
				else
				{
					redNeeded--;
					placeKey(col.gameObject);
				}
			}
			else if (keyscript.type == Key.EKeyType.BLUE)
			{
				if (blueNeeded <= 0)
					keyscript.die();
				else
				{
					blueNeeded--;
					placeKey(col.gameObject);
				}
			}
			else if (keyscript.type == Key.EKeyType.GREEN)
			{
				if (greenNeeded <= 0)
					keyscript.die();
				else
				{
					greenNeeded--;
					placeKey(col.gameObject);
				}
			}
		}
		if (redNeeded == 0 && blueNeeded == 0 && greenNeeded == 0)
			openDoor();
	}
}
