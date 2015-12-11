using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
	public void activateEvent(string eventName)
	{
		for (int i = 0;i < transform.GetChildCount(); i++)
		{
			if (transform.GetChild(i).name == eventName)
			{
				transform.GetChild(i).GetComponent<Event>().activeEvent();
			}
		}
	}
}
