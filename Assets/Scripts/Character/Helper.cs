using UnityEngine;
using System.Collections;

public class Helper : MonoBehaviour
{
	public struct ClipPlanePosition
	{
		public Vector3 upperRight;
		public Vector3 lowerRight;
		public Vector3 lowerLeft;
		public Vector3 upperLeft;
	}
	static public float CameraClamp(float angleToClamp, float min, float max)
	{
		return (Mathf.Clamp(angleToClamp, min, max));
	}
	
	static public ClipPlanePosition NearClipPlane(Vector3 SmoothedCameraPosition)
	{
		Transform 	camera = Camera.mainCamera.transform;
		Camera 		cameraScript = camera.GetComponent<Camera>();
		
		float fov;
		float nearClipPlaneHeight;
		float nearClipPlaneWidth;
				
		ClipPlanePosition clipPlanePosition = new ClipPlanePosition();


		fov = (cameraScript.fieldOfView * Mathf.Deg2Rad) / 2;
		nearClipPlaneHeight = Mathf.Tan(fov) * cameraScript.nearClipPlane;
		nearClipPlaneWidth = (nearClipPlaneHeight * cameraScript.aspect);
		
		clipPlanePosition.upperRight = Vector3.zero;
		clipPlanePosition.upperRight.x += nearClipPlaneWidth;
		clipPlanePosition.upperRight.y += nearClipPlaneHeight;
		clipPlanePosition.upperRight.z += cameraScript.nearClipPlane;
		
		clipPlanePosition.lowerRight = Vector3.zero;
		clipPlanePosition.lowerRight.x += nearClipPlaneWidth;
		clipPlanePosition.lowerRight.y -= nearClipPlaneHeight;
		clipPlanePosition.lowerRight.z += cameraScript.nearClipPlane;
		
		clipPlanePosition.lowerLeft = Vector3.zero;
		clipPlanePosition.lowerLeft.x -= nearClipPlaneWidth;
		clipPlanePosition.lowerLeft.y -= nearClipPlaneHeight;
		clipPlanePosition.lowerLeft.z += cameraScript.nearClipPlane;
		
		clipPlanePosition.upperLeft = Vector3.zero;
		clipPlanePosition.upperLeft.x -= nearClipPlaneWidth;
		clipPlanePosition.upperLeft.y +=  nearClipPlaneHeight;
		clipPlanePosition.upperLeft.z +=  cameraScript.nearClipPlane;
		
		
		clipPlanePosition.upperRight = camera.transform.TransformPoint(clipPlanePosition.upperRight);
		clipPlanePosition.upperLeft = camera.transform.TransformPoint(clipPlanePosition.upperLeft);
		clipPlanePosition.lowerRight = camera.transform.TransformPoint(clipPlanePosition.lowerRight);
		clipPlanePosition.lowerLeft = camera.transform.TransformPoint(clipPlanePosition.lowerLeft);
		/*print ("======================>>>>>");
		print ("CPP UpperRight : " + clipPlanePosition.upperRight);
		print ("CPP lowerRight : " + clipPlanePosition.lowerRight);
		print ("CPP lowerLeft : " + clipPlanePosition.lowerLeft);
		print ("CPP upperLeft : " + clipPlanePosition.upperLeft);
		print ("<<<<<<======================");*/
		return (clipPlanePosition);
	}
}
