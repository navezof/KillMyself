using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour
{
	public enum ETrapeType
	{
		SPRING,
		SMOKE,
		WALL
	};
	
	public ETrapeType _type;
	
	public virtual void activeTrap(Transform position, GameObject collid)	{}
}
