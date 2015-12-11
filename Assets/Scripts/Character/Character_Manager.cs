using UnityEngine;
using System.Collections;

public class Character_Manager : MonoBehaviour
{
	static Character_Manager Instance;
    // Reference to the CharacterMotor for managing the keyboard input.
    static Character_Motor _characterMotor;

	float	yvalue = 0;
	// CharacterManager need a CameraManager to manage the data of the camera.
    Camera_Manager _cameraManager;

	
	void Awake()
	{
		_characterMotor = GetComponent<Character_Motor>();
		Instance = this;
	
		Camera_Manager.MainCameraCheck();		
	}

	public static Character_Motor getCharacterMotor()
	{
	  return _characterMotor;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Check if there is a maincamera in the scene
		if (Camera.mainCamera == null)
			return ;
		
		ControllerInput();
		// Force an update of the Character_Motor
		Character_Motor.Instance.UpdateMotor();
	}

    // Send the input to the other script in order to manage the input
    void ControllerInput()
	{
		var deadzone = 0.1f;
		
		//Character_Motor.Instance.MoveVector = Vector3.zero;
		// Check for deadzone and modify the MoveVector
		if (Mathf.Abs(Input.GetAxis("Vertical")) > deadzone)
			Character_Motor.Instance.MoveVector += new Vector3(0, 0,  Input.GetAxis("Vertical"));
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadzone)
			Character_Motor.Instance.MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		yvalue = Character_Motor.Instance.MoveVector.y;
	}
}
