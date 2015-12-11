using UnityEngine;
using System.Collections;

public class TimedParticlesDestructor : MonoBehaviour
{
	public float objectLifeSpan = 1;
	public float particleLifeSpan = 1;	
	
	void Awake()
	{
	
		Invoke("stopEmit", objectLifeSpan);
	}
	
	void stopEmit()
	{
		this.GetComponent<ParticleEmitter>().emit = false;
		Invoke("destroy", particleLifeSpan);
	}
	
	// Use this for initialization
	void destroy()
	{
		GameObject.Destroy(this.gameObject);
	}
}
