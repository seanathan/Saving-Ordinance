using UnityEngine;
using System.Collections;

public class EnemyEngineModule : MonoBehaviour {

    public float enginesMaxHP;

	public float enginesHP = 500.0f;

	public bool flash = false;
	public float enginePower = 1.0f;
    
    public bool drop = true;
    public GameObject winDrop;
    private bool EngineStart = true;


	void Start()
	{
		GetComponent<Renderer>().enabled = false;
        enginesMaxHP = enginesHP;
        EngineStart = false;

	}

    public void RestoreEngines()
    {
        if (EngineStart)
        {
            enginesMaxHP = enginesHP;
            EngineStart = false;
        }   
        enginePower = 1.0f;
        
        enginesHP = enginesMaxHP;
    }

    public void BulletHit(Ammunition bullet)
    {
        if (bullet.getShooter().gameObject != GetComponentInParent<LameShip>())
            BulletHit(bullet.damage);
    }

    public void BulletHit(BulletScript1 bullet)
    {
        if (bullet.getShooter().gameObject != GetComponentInParent<LameShip>())
            BulletHit(bullet.damage);
    }

    public void BulletHit(float damage)
    {
        if (enginesHP > 0f)
        {
            enginesHP -= damage;
            flash = true;
        }
    }
    

	void Update()
	{
        if (flash == true)
		{
			GetComponent<Renderer>().enabled = true;
			flash = false;
		}
		else
		{
			GetComponent<Renderer>().enabled = false;
		}
        
        if (enginesHP < enginesMaxHP/2 && enginesHP > 0)
        {

            enginePower = 0.5f;
            
        }

	}



	void LateUpdate()
	{
		if (enginesHP <= 0.0f) 
		{
			enginePower = 0f;

            if (drop == true && winDrop != null)
            {
                Instantiate(winDrop, transform.position, transform.rotation);
                drop = false;
            }
        }
	}
}
