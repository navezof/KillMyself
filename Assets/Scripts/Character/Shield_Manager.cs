using UnityEngine;
using System.Collections;

public class Shield_Manager : MonoBehaviour {
    public GameObject render;
    private double coldownact;
    public double coldownbtact; // repacer en private

    public double coldownTimeBetweenActivation;
    public double coldownTimeInActivation;

    public int MaxShieldPower;
    public int InitialShieldPower;
    public int CurrentShieldPower;
    public int ShieldActivationCoast;

    private bool shildActif;

    public GameObject[] ShieldUI;

	// Use this for initialization
	void Start () {
        //Get Shield_colBox and render
        render = this.transform.FindChild("Shield_render").gameObject;
        shildActif = false;
        CurrentShieldPower = InitialShieldPower;
        coldownact = 0;
        coldownbtact = 0;
        powerOff();
	}
	
	// Update is called once per frame
	void Update () {
        if (coldownact > 0)
            coldownact -= Time.deltaTime;
        else if (shildActif == true)
            powerOff();
        if (coldownbtact > 0)
            coldownbtact -= Time.deltaTime;

        if (Input.GetButtonDown("shield"))
        {
            Debug.LogWarning("Passe le Input");
            if (isLoad())
            {
                Debug.LogWarning("Passe le isLoad");
                powerOn();
                Debug.LogWarning("Passe le powerOn");
            }
        }
	}

    private void powerOn()
    {
        render.SetActive(true);
        shildActif = true;
        coldownact = coldownTimeInActivation;
        coldownbtact = 0;
        CurrentShieldPower -= ShieldActivationCoast;
    }

    private void powerOff()
    {
        render.SetActive(false);
        shildActif = false;
        coldownact = 0;
        coldownbtact = coldownTimeBetweenActivation;

        int nbShieldUiOn = (CurrentShieldPower * ShieldUI.Length) / 100;
        Debug.Log("nb shield On UI: " + nbShieldUiOn);
        for (int i = 0; i < ShieldUI.Length; i++)
        {
            float newAlpha = 0;
            if (nbShieldUiOn > 0 && i <= nbShieldUiOn - 1)
            {
                newAlpha = 255;
            }
            GameObject current = ShieldUI[i];
            Color originalColour = current.renderer.material.color;
            current.renderer.material.color = new Color(originalColour.r, originalColour.g, originalColour.b, newAlpha);
        }
    }

    private bool isLoad()
    {
        if (CurrentShieldPower >= ShieldActivationCoast)
        {
            Debug.LogWarning("Passe le CurrentShieldPower");
            if (shildActif == false && coldownbtact <= 0f)
            {
                Debug.LogWarning("Passe le !shildActif && coldownbtact <= 0f");
                return true;
            }
        }
        return false;
    }
}
