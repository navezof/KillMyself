using UnityEngine;
using System.Collections;

public class Camera_Manager : MonoBehaviour
{
	// Link to the target.
	public Transform target;
	
	// Distance of the target
	public float distance = 10;
	// Maximum distance for the target
	float maxdistance = 15;
	// Minimum distance for the target
	float mindistance = 2;
	// Smooth of the scroll
	public float scrollSmooth = 10f;
	
	// height offset to the target
	public float height = 3;
	// maximum height offset
	float maxheight = 5;
	// minimum height offset
	float minheight = 0.1f;
	
	// Deadzone calibrator
	public float deadzone = 0.1f;
	
	// sensibility of the mouse
	public float sensibility = 6;
	// Min angle for y
	public float minY = 20;
	// Max Angle for y
	public float maxY = 55;
	
	//Final position after smooth
	private GameObject positionTarget;
	
	private float speedCoef = 0.2F; 
	public float speedCoefMin = 0.2F; 
	public float speedCoefMax = 2F;
	
	public float speedCoefDistMax = 9F;
	public float speedCoefDistMin = 6F;
	
	public float speedCoefAcc = 0.2F;
	public float speedCoefDec = 0.2F;
	
	// Rotation value
	float rotX;
	float rotY;
	float rotZ;
	
	Helper.ClipPlanePosition 	clipPlanePosition;
	
	public GameObject			backBuffer;
	public float				backBufferDistance = 10;
	
	float						nearestPointToCharacter = -1;
	Vector3						nearestPointToCharacterPosition;
	
	bool						cameraObstructionBool = true;
	public int					obstructionCheckLimit = 10;
	
	public float				moveValue;
	
	Vector3						beforeObstructionPosition;
	
	Vector3						viewOffset =  new Vector3(0, 1, 0);
	
	// Use this for initialization
	void Start ()
	{
		distance = Mathf.Clamp(distance, mindistance, maxdistance);
		height = Mathf.Clamp(height, minheight, maxheight);
		positionTarget = new GameObject("positionTarget");
		if (!backBuffer)
		{
			backBuffer = GameObject.Find("backBuffer").gameObject;
		}
		if (target != null)
		{
			initCamera();
		}
		else
		{
			// If there is not initial target
			if (GameObject.FindWithTag("Player") != null)
			{
				// find a Game Object with a player tag ...
				attachTarget(GameObject.FindWithTag("Player").transform);
			}
			else
			{
				// ... Or Instanciate a new one from the asset
				GameObject pl = (GameObject)Instantiate(Resources.Load("Character", typeof(GameObject)),
					new Vector3(0,0,0),
					new Quaternion(0,0,0,0)
					);
				if (pl == null)
					Debug.Log("test de GameObject");
				target = pl.transform;
			}
		}
	}
	
	// Last update is called after all the other updates.
	// The main camera should use the LastUpdate.
	void LateUpdate ()
	{
		int obstruction = 0;
		cameraObstructionBool = false;
				
		UserCameraRotation();
		UserCameraPosition();
	
		if (Vector3.Distance(transform.position,  positionTarget.transform.position) < speedCoefDistMin)
		{
   			speedCoef-= speedCoefDec;
   			if (speedCoef < speedCoefMin)
			{
			    speedCoef = speedCoefMin;
			}
	  	}
		else if (speedCoef < speedCoefMax)
		{
   			if (Vector3.Distance(transform.position,  positionTarget.transform.position) > speedCoefDistMax)
			{
    			speedCoef+= speedCoefAcc;
   			}
  		}
		
  		transform.position = Vector3.Lerp(this.transform.position, positionTarget.transform.position, 20F * Time.deltaTime * speedCoef);
		
		/*
		CameraObstructionCheck(obstruction);
		
		if (cameraObstructionBool)
		{
			beforeObstructionPosition = this.transform.position - target.transform.position;
			
		}
		
		do
		{
			obstruction++;
			CameraObstructionCheck(obstruction);
			/*if (cameraObstructionBool)
			{
				distance = Vector3.Distance(transform.position, target.transform.position + viewOffset);
				Debug.Log (transform.position + " " + (target.transform.position + viewOffset) + distance);
			}
		} while (cameraObstructionBool);
		*/
		

		transform.rotation = Quaternion.Slerp(transform.rotation, positionTarget.transform.rotation, 20F * Time.deltaTime * speedCoef);
		
	}
	
	// Attach a new target to the Camera and init the camera pos
	void attachTarget(Transform p_target)
	{
		target = p_target;
		initCamera();
	}
	
	float checkNearestPointToCharacter(Vector3 targetLookAtPosition, Vector3 nearClipPlanePosition)
	{
		RaycastHit hit;
		float distancePlayerNCP;
		
		if (Physics.Linecast(targetLookAtPosition, nearClipPlanePosition, out hit))
		{
			if (hit.transform.tag != "Player")
			{
				distancePlayerNCP = Vector3.Distance(targetLookAtPosition, hit.point);
				//if (nearestPointToCharacter == -1)
				//	nearestPointToCharacter = distancePlayerNCP;
				if (nearestPointToCharacter > distancePlayerNCP)
				{
					nearestPointToCharacterPosition = hit.point; 
					nearestPointToCharacter = distancePlayerNCP;
					//print ("Nearest PC Found : " + nearestPointToCharacter);
					return (nearestPointToCharacter);
				}
			}
		}
		return (-1);
	}
	
	float CameraCollisionPointsCheck(Vector3 targetLookAtPosition, Vector3 cameraPositionAfterSmoothing)
	{			
		clipPlanePosition = Helper.NearClipPlane(cameraPositionAfterSmoothing);
		
		nearestPointToCharacter = 999;
		
		if ((checkNearestPointToCharacter(targetLookAtPosition, clipPlanePosition.upperRight) == -1) &&
			(checkNearestPointToCharacter(targetLookAtPosition, clipPlanePosition.lowerRight) == -1) &&
			(checkNearestPointToCharacter(targetLookAtPosition, clipPlanePosition.lowerLeft) == -1) &&
			(checkNearestPointToCharacter(targetLookAtPosition, clipPlanePosition.upperLeft) == -1) &&
			(checkNearestPointToCharacter(targetLookAtPosition, backBuffer.transform.position) == -1))
			nearestPointToCharacter = -1;
		//print (">>>>>> Nearest Point To character : " + nearestPointToCharacter);
		
		//DEBUG
		drawDebugVision();

		return (nearestPointToCharacter);
	}
	
	void drawDebugVision()
	{
		Debug.DrawLine(clipPlanePosition.upperLeft, clipPlanePosition.lowerLeft, Color.green);
		Debug.DrawLine(clipPlanePosition.upperLeft, clipPlanePosition.upperRight, Color.green);
		Debug.DrawLine(clipPlanePosition.lowerRight, clipPlanePosition.upperRight, Color.green);
		Debug.DrawLine(clipPlanePosition.lowerRight, clipPlanePosition.lowerLeft, Color.green);

		Debug.DrawLine(target.transform.position + viewOffset, clipPlanePosition.upperLeft, Color.red);
		Debug.DrawLine(target.transform.position + viewOffset, clipPlanePosition.upperRight, Color.red);
		Debug.DrawLine(target.transform.position + viewOffset, clipPlanePosition.lowerLeft, Color.red);
		Debug.DrawLine(target.transform.position + viewOffset, clipPlanePosition.lowerRight, Color.red);
		
		Debug.DrawLine(target.transform.position + viewOffset, backBuffer.transform.position, Color.red);
	}
	
	// Update the camera position after a mouse move
	void UserCameraPosition()
	{		
	   //positionTarget.transform.position = new Vector3(target.position.x - (distance * Mathf.Sin(rotY * Mathf.Deg2Rad)), target.position.y, target.position.z - (distance * Mathf.Cos(rotY *  Mathf.Deg2Rad)));
	   positionTarget.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
	   positionTarget.transform.Translate(viewOffset);
	   positionTarget.transform.Translate(new Vector3(0, 0, -distance));
	}
	
	// Update the camera rotation after a mouse move
	void UserCameraRotation()
	{
		float xaxis = Input.GetAxis("Mouse X");
		float yaxis = Input.GetAxis("Mouse Y");
		float scroll = -Input.GetAxis("Mouse ScrollWheel");
		var newRot = Quaternion.identity;
		
		Transform testAngle;
		
		if (Mathf.Abs(yaxis) > deadzone)
		{
			rotX += (yaxis * sensibility);
			rotX = Mathf.Clamp(rotX, minY, maxY);
		//	testAngle = positionTarget.transform;
		//	testAngle.RotateAround(target.transform.position, Vector3.left, yaxis * sensibility);
		//	if (!(testAngle.eulerAngles.x <= maxY && testAngle.eulerAngles.x >= minY))
		//	{
		//		positionTarget.transform.RotateAround(target.transform.position, Vector3.left, -yaxis * sensibility);
		//	}
		}
		if (Mathf.Abs(xaxis) > deadzone)
		{
			this.rotY += (xaxis * sensibility);
		}
		if (Mathf.Abs(scroll) > deadzone)
		{
			distance += scroll * scrollSmooth;
			distance = Helper.CameraClamp(distance, mindistance, maxdistance);
		}
		newRot.eulerAngles = new Vector3(rotX, rotY, rotZ);
		positionTarget.transform.rotation = newRot;
	}
	
	// Initialize the position of the camera after a target as been assigned
	public void initCamera()
	{
		var newRot = Quaternion.identity;
		
		this.rotX = target.rotation.eulerAngles.x;
		this.rotY = target.rotation.eulerAngles.y;
		this.rotZ = target.rotation.eulerAngles.z;
	    //this.transform.position = new Vector3(target.position.x - (distance * Mathf.Sin(rotY * Mathf.Deg2Rad)), target.position.y, target.position.z - (distance * Mathf.Cos(rotY *  Mathf.Deg2Rad)));
		newRot.eulerAngles = new Vector3(rotX, rotY, rotZ);
		this.transform.rotation = newRot;
		this.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
		this.transform.Translate(viewOffset);
		this.transform.Translate(new Vector3(0, 0, -distance));
		//this.transform.LookAt(target.position);
		//if (positionTarget != null)
		//{
		//	positionTarget.transform.rotation = target.rotation;
		//    positionTarget.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
		//	positionTarget.transform.Translate(new Vector3(0, height, -distance), target.transform);
		//	positionTarget.transform.LookAt(target.position);
		//}
	}
	
	// Update the camera position after a move of the transform
	public bool updateMove()
	{
		var newRot = Quaternion.identity;
		
		//this.transform.position = new Vector3(target.position.x - (distance * Mathf.Sin(rotY * Mathf.Deg2Rad)), target.position.y, target.position.z - (distance * Mathf.Cos(rotY *  Mathf.Deg2Rad)));
		newRot.eulerAngles = new Vector3(rotX, rotY, rotZ);
		this.transform.rotation = newRot;
		this.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
		this.transform.Translate(new Vector3(0, 0, -distance));
		this.transform.Translate(viewOffset);
		return true;
	}
	
	
	// Check the presence of a main camera
	public static bool MainCameraCheck()
	{
		if (Camera.mainCamera)
			return (true);
		// If no camera are present then a new camera is created and assigned to a target
		GameObject camera = new GameObject("MainCamera");
		camera.AddComponent<Camera>();
		camera.transform.tag = "MainCamera";
		camera.AddComponent<Camera_Manager>();
		(camera.GetComponent<Camera_Manager>()).attachTarget(GameObject.FindWithTag("Player").transform);
		return (false);
	}
	
	bool CameraObstructionCheck(int obstructionCheck)
	{
		//print ("Check obstruction : " + obstructionCheck);
		if (obstructionCheck > obstructionCheckLimit)
		{
			transform.position = nearestPointToCharacterPosition;
			cameraObstructionBool = false;
			return (cameraObstructionBool);
		}
		if (CameraCollisionPointsCheck(target.transform.position + viewOffset, positionTarget.transform.position) == -1)
		{
			if (cameraObstructionBool)
			{
				if (checkNearestPointToCharacter(target.transform.position, beforeObstructionPosition) == -1)
					transform.position = beforeObstructionPosition;
			}
			cameraObstructionBool = false;
		}
		else
		{
			transform.Translate(Vector3.forward * 6f * Time.deltaTime);
			cameraObstructionBool = true;
		}
		return (cameraObstructionBool);
	}
}
