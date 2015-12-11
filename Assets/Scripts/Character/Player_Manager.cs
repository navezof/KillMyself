using UnityEngine;
using System.Collections;

public class Player_Manager : MonoBehaviour {

	public Character_Motor motor;
	public  Shield_Manager shield;
    public float gold;
	public int numberOfGold = 0;
    public float goldMultiplicator;
	
	public float timer = 0f;
	public int nbLifeRemaning = 3;
	
	public KeyCode suicideKey = KeyCode.F;
	
	void Start() {
        init();
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Shield")
        {
            shield.CurrentShieldPower = (shield.ShieldActivationCoast + shield.CurrentShieldPower) % shield.MaxShieldPower;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Gold")
        {
			numberOfGold += 1;
            gold += 1 * goldMultiplicator;
            goldMultiplicator += 0.1f;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Bad")
        {
            endHit();
            Destroy(hit.gameObject);
        }
    }

	void Update () {
        if (goldMultiplicator > 1)
        {
            if (timer >= 1f)
            {
                goldMultiplicator -= 0.1f;
                timer = 0f;
            }
            else
                timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(suicideKey))
		{
			init ();
			BroadcastMessage("die", 0);
		}
	}
	
	public void init()
	{
        gold = 0;
        goldMultiplicator = 0;
		nbLifeRemaning = 3;
		timer = 0.1f;
	}
	
	void endHit()
	{
		BroadcastMessage("die");
		/*
		if (nbLifeRemaning > 0)
		{
			BroadcastMessage("die");
		}
		else
		{
			BroadcastMessage("die", 0);
		}
		*/
	}
}
