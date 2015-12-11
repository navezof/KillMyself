using UnityEngine;
using System.Collections;

public class Spiral : SpawnerScript {
    public float angleSpeed  = 1;
    public float angleFireRate = 5;
    private float currentAngle = 0;
    public float angleOfFire = 80;
    private float currentAngleOfFireMax;
    private float currentAngleOfFireMin;
    private bool TestIsAngle;
    private float newAngle = 0;
    private float lastAngle = 0;
    public bool TempoMode = false;

	// Use this for initialization
	void Start () {
        base.Start();
       // Debug.Log("start angle 1 !!!" + transform.rotation.y + (angleOfFire / 2));
        this.rateOfSpawn = 0;
        currentAngleOfFireMax = angleOfFire;
        currentAngleOfFireMin = 0;
       transform.Rotate(new Vector3(0, -(angleOfFire / 2), 0));
        if (currentAngleOfFireMax > 360)
            currentAngleOfFireMax = currentAngleOfFireMax - 360;
        if (currentAngleOfFireMin < 0)
            currentAngleOfFireMin = 360 + currentAngleOfFireMin;
        currentAngle = angleOfFire / 2;
        lastAngle += angleSpeed;
        /*if (currentAngleOfFireMax < currentAngleOfFireMin) {
            float tmp = currentAngleOfFireMax;
            currentAngleOfFireMax = currentAngleOfFireMin;
            currentAngleOfFireMin = tmp;
        }*/
	}
	
	// Update is called once per frame
	void Update () {
      //  
        if (currentAngle <= angleOfFire)
        {
            if (lastAngle + angleFireRate >= newAngle)
            {
                newAngle += angleFireRate;
                this.fire();
            }
            lastAngle += Mathf.Sqrt(angleSpeed * angleSpeed);
        }
        if (currentAngle > angleOfFire && TempoMode) {
            angleSpeed *= -1;
            currentAngle = angleOfFire;
            Debug.Log("fire angle 1 !!!" +  angleSpeed);
        }
         if (currentAngle < 0 && TempoMode)
        {
            angleSpeed *= -1;
            currentAngle = 0;
            Debug.Log("fire angle 1 !!!" + angleSpeed);
        }
        currentAngle += angleSpeed;
        if (currentAngle > 360)
            currentAngle = currentAngle - 360;
        transform.Rotate(new Vector3(0, angleSpeed, 0));
	}
}
