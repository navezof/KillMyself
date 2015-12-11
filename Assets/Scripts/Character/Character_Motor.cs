using UnityEngine;
using System.Collections;

public class Character_Motor : MonoBehaviour
{
	// Instance of this class
	public static Character_Motor	Instance;
	
	// Instance of the animation manager
	public Animation_Manager		animationManager;
	
		// Vector representing the speed of the character
	public float			speedLimit = 10;
	public float			airSpeedLimit = 2;
	
	public float			forwardSpeedLimit = 15;
	public float			backwardSpeedLimit = 7;
	public float			strafeSpeedLimit = 8;
	public float			fallingSpeedLimit = 9;

	// Reference of the 3D model (not yet used)
	public Transform		Model;

	// Vector representing the movement of the character
	public Vector3 			MoveVector;
	public Vector3 			HitMoveVector;
	
	// Gravity power
	public float			gravityForce = -1.1f;
	public float			currentForce;
	public float			currentYSpeed;
	
	// height tranform to player's foot
	public float			distanceFoot = 2.0f;
	public bool				onTheFloor = true;
	
	//
	public float			terminalGravityVelocity = -3;
	
	//Slide
	public float			minimumSlide = 0.2f;
	public float			slideDeadzone = 0.1f;
	public float 			lockControlLimit = 0.8f;
	private bool			lockControl = false;
	
	private Vector3			slideVector;	

	private Vector3 		impulsionVector;
	private Vector3			lastPosition;
	
	// Initalize the variables
	public void Awake()
	{
		Instance = this;
		MoveVector = Vector3.zero;
		impulsionVector = Vector3.zero;
		speedLimit = 10;
		currentForce = 0;
		lastPosition = Vector3.zero;
	}
	
	// Main function, call the other functions of the class
	public void UpdateMotor()
	{
		//if lockControl then don't use new MoveVector position, set by controllers.
		if (lockControl)
			MoveVector = Vector3.zero;
		lockControl = false;
		AlignWithCamera();
		checkRayCast(true);
		animationManager.CurrentMotionState();
		MoveCharacter();
	}
	
	public float MoveSpeed()
	{
		switch (animationManager.CharacterMotionState)
		{
		case Animation_Manager.MotionStateList.Forward:
			return (forwardSpeedLimit);
		case Animation_Manager.MotionStateList.LeftForward:
			return (forwardSpeedLimit);
		case Animation_Manager.MotionStateList.RightForward:
			return (forwardSpeedLimit);
			
		case Animation_Manager.MotionStateList.Left:
			return (strafeSpeedLimit);
		case Animation_Manager.MotionStateList.Right:
			return (strafeSpeedLimit);
			
		case Animation_Manager.MotionStateList.Backward:
			return (backwardSpeedLimit);
		case Animation_Manager.MotionStateList.LeftBackward:
			return (backwardSpeedLimit);
		case Animation_Manager.MotionStateList.RightBackward:
			return (backwardSpeedLimit);
			
		case Animation_Manager.MotionStateList.Stationary:
			return (fallingSpeedLimit);	
		default:
			return (0);
		};
	}
	
	
	// Apply the moveVector to the character position.
	// We also normalize the vector and multiply by the speed
	public void MoveCharacter()
	{
		float fortement = 5f;
		MoveVector = transform.TransformDirection(MoveVector);
		
		// Normalize already check the magnitude so we don't need to do it.
		MoveVector.Normalize();
		Vector3 tmpVect = HitMoveVector * Time.deltaTime;
		if (HitMoveVector.x > 0.1f) {
			HitMoveVector.x -= fortement * Time.deltaTime;
		} else {
			HitMoveVector.x = 0f;
		}
		
		if (HitMoveVector.z > 0.1f ) {
			HitMoveVector.z -= fortement * Time.deltaTime;
		} else if (HitMoveVector.z > 0) {
		
		} else {
			HitMoveVector.z = 0f;
		}
		fortement += (HitMoveVector.x + HitMoveVector.y) / 2 * 2.5f * Time.deltaTime;
		ProcessMotion();
		if (onTheFloor)
		{
			MoveVector = MoveVector * MoveSpeed() * Time.deltaTime;
		}
		else
		{
			MoveVector = new Vector3(impulsionVector.x + MoveVector.x * airSpeedLimit * Time.deltaTime,
										MoveVector.y * MoveSpeed() * Time.deltaTime,
										impulsionVector.z + MoveVector.z * airSpeedLimit * Time.deltaTime);
		}
		this.GetComponent<CharacterController>().Move(MoveVector + tmpVect);
		MoveVector = Vector3.zero;
		checkRayCast(false);
		if (onTheFloor && currentForce == 0) {
			this.GetComponent<CharacterController>().Move(transform.TransformDirection(slideVector));
		}
	
		Camera.mainCamera.GetComponent<Camera_Manager>().updateMove();
	}
	
	// We rotate the character to have the same rotation of the camera
	public void AlignWithCamera()
	{
		if (MoveVector != Vector3.zero)
		{
			if (Camera.mainCamera == null)
				return;
			this.transform.rotation = Quaternion.Euler(new Vector3(0, Camera.mainCamera.transform.eulerAngles.y, 0));
		}
	}
	
	public void ProcessMotion()
	{
		ApplyGravity();
		currentYSpeed += currentForce;
		currentYSpeed = Mathf.Clamp(currentYSpeed, terminalGravityVelocity, -terminalGravityVelocity);
		//Slide
		MoveVector += slideVector;
		// Gravity or Jump
		MoveVector.y = MoveVector.y + (currentYSpeed * Time.deltaTime);
	}
	
	public	void checkRayCast(bool slide)
	{
		RaycastHit hit;
		onTheFloor = false;

		if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y + distanceFoot,transform.position.z), Vector3.down, out hit))
		{
			if (hit.distance <= distanceFoot || !(hit.distance >= distanceFoot + slideDeadzone * 2)){
				onTheFloor = true;
				calculateSlide(hit.normal, hit.distance, slide);
			}
		}	
	}
	
	void calculateSlide(Vector3 hit, float hitDist, bool slide)
	{
		float xn = 0;
		float moveValue = 1f;
		float zn = 0;
		
		if (hit.x <= -slideDeadzone || hit.x >= slideDeadzone) {
			if (hit.x > slideDeadzone) {
				xn += hit.x * moveValue;
			} else if (hit.x < -slideDeadzone) {
				xn += hit.x * moveValue;
			}
		} else {
			xn = 0;
		}
		if (hit.z <= -slideDeadzone || hit.z >= slideDeadzone) {
			if (hit.z > slideDeadzone) {
				zn += hit.z * moveValue;

			} else if (hit.z < -slideDeadzone) {
				zn += hit.z * moveValue;
			}
		} else {
			zn = 0;
		}
		slideVector = new Vector3(xn, 0, zn);
	
		if (hit.x >= lockControlLimit || hit.x <= -lockControlLimit
			|| hit.z >= lockControlLimit || hit.z <= -lockControlLimit)
			lockControl = true;
		else 
			lockControl = false;
		
		if (xn != 0 || zn != 0 || !slide)
			replaceSlide(slide);
	}
	
	public void replaceSlide(bool slide)
	{
		RaycastHit	hit;
		
		if (!slide)
			slideVector = Vector3.zero;
			
		Vector3	newPosition = transform.TransformDirection(MoveVector) + slideVector;
		Vector3 rayPosition = newPosition + this.transform.position;
		if (Physics.Raycast(rayPosition, Vector3.down, out hit) && hit.distance < 2)
		{
			slideVector = new Vector3(slideVector.x, -hit.distance, slideVector.z);
		}
	}
	
	public void ApplyGravity()
	{
		if(!onTheFloor)
		{
			if (currentForce > terminalGravityVelocity)
			{
				if (currentYSpeed > terminalGravityVelocity)
					currentForce += gravityForce;
				else
					currentForce = 0;
			}
		} else if (onTheFloor && currentYSpeed < 0) {
			currentForce = 0;
			currentYSpeed = 0;
			impulsionVector = Vector3.zero;
		}
	}
}
