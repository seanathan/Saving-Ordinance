using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour {

    public string alias;

    //public GameObject tracer;       // Core Bullet Effect
    public string fireEffectAlias;
    public HitEffect fireEffect;   // Muzzle Flash

    public string hitEffectAlias;
    public HitEffect hitEffect;    // Explosion
    public enum trackType
    {
        dumb,   // fires straight ahead
        beam,   // fires as solid beam
        smart   // maneuvers toward target
    }

    public trackType tracking;

    public float velocity = 500;    //initial or max velocity

    public int damage = 10; //damage in hit points
    public float impactForce = 0;   // Impact force? 0 otherwise
    public float smartForce = 0;       // force of agility in tracking

    [Header("Range")]

    public float range = 1000;  //total distance till abort
    private float travelled = 0;
    private Rigidbody shotrb;



    public enum bulletState
    {
        waiting,    //initializing
        firing,     //Just leaving hardpoint
        traveling,  //traveling through space
        hit,        //successful impact
        miss        //timed out / no impact
    }
    public bulletState status = bulletState.waiting;

    private Vector3 initPosition;

    public GameObject smartTarget;  //if smart, null otherwise
    private GameObject shooter;      //originator
    public EnemyShipModular.mobAffiliation shooterIff;
    
    public Ammunition FireThis(Transform Muzzle)
    {
        Ammunition shot = Fire(this, Muzzle);
        return shot;
    }

    public Ammunition FireThis(Transform Muzzle, GameObject SmartTarget)
    {
        Ammunition shot = Fire(this, Muzzle, SmartTarget);
        return shot;
    }

    public static Ammunition Fire(Ammunition AmmoToLoad, Transform Muzzle, GameObject Target = null)
    {
        if (AmmoToLoad == null)
            return null;


        AmmoToLoad.MuzzleBang(Muzzle);

        GameObject shotgo = Instantiate(AmmoToLoad.gameObject, Muzzle.position, Muzzle.rotation);
        Ammunition shot = shotgo.GetComponent<Ammunition>();


        shot.setShooter(Muzzle.gameObject);

        shot.shotrb = shot.GetComponent<Rigidbody>();

        shot.status = bulletState.firing;

        shot.initPosition = Muzzle.position;

        shot.smartTarget = Target;

        //get source velocity;
        if (shot.shotrb)
        {
            Vector3 InitialVelocity = Vector3.zero;
            
            if (Muzzle.GetComponentInParent<Rigidbody>())
                InitialVelocity = Muzzle.GetComponentInParent<Rigidbody>().velocity;

            shot.shotrb.velocity = Muzzle.forward * shot.velocity;
            shot.shotrb.velocity += InitialVelocity;

        }

        if (shot.shotrb)
            shot.shotrb.AddRelativeForce(Vector3.forward * shot.smartForce);


        return shot;
    }
    
    public void setShooter(GameObject newShooter)
    {
        if (!newShooter)
        {
            //Default to player if no shooter assigned

            shooterIff = EnemyShipModular.mobAffiliation.Ally;
            shooter = ScoreKeeper.playerAlive;
            return;
        }

        //passes IFF to round
        shooter = newShooter;

        EnemyShipModular shooterMob = shooter.GetComponentInParent<EnemyShipModular>();
        if (shooterMob != null)
            shooterIff = shooterMob.iff;
    }
    
    public GameObject getShooter()
    {
        if (shooter != null)
            return shooter;

        return ScoreKeeper.playerAlive;
    }


    void Update()
    {
        if (status == bulletState.waiting)
            return;


        
        

        //if beam, have the turret control it
        //    if (beam)
        //       transform.parent = shooter.transform;

        //force



        //travel distance
        travelled += velocity * Time.deltaTime;

        if (travelled > range)
            status = bulletState.miss;
        
        //remove if ended last frame
        if (status == bulletState.hit || status == bulletState.miss)
            Destroy(gameObject);


        transform.SetParent(cleanup.getCleaner());
    }


    void OnTriggerEnter(Collider other)
    {
        EnemyShipModular ship = other.GetComponentInParent<EnemyShipModular>();
        if (ship != null)
        {
            //don't hit if self!
            if (ship.gameObject == shooter)
                return;
        }

        Hit(other);
    }
    

    void Hit(Collider other)
    {
        other.SendMessageUpwards("BulletHit", this, SendMessageOptions.DontRequireReceiver);

        if (AmmoManager.checkAlias(hitEffectAlias))
            Instantiate(AmmoManager.getHit(hitEffectAlias).gameObject, transform.position, transform.rotation);

        status = bulletState.hit;


        //alt kinetic force
        if (impactForce > 0)
        {
            if (other.GetComponentInParent<Rigidbody>())
            {
                other.GetComponentInParent<Rigidbody>().AddForceAtPosition(impactForce * GetComponentInParent<Rigidbody>().velocity, transform.position);
            }
        }
    }

    private void OnValidate()
    {

        fireEffect = AmmoManager.getHit(fireEffectAlias);
        
        hitEffect = AmmoManager.getHit(hitEffectAlias);
        
    }

    void MuzzleBang(Transform Muzzle)
    {

        if (AmmoManager.checkAlias(fireEffectAlias))
            Instantiate(AmmoManager.getHit(fireEffectAlias).gameObject, Muzzle.position, Muzzle.rotation);
        
    }


    public string getAlias()
    {
        if (alias.Length > 0)
            return alias;
        else
            return (gameObject.name);
    }
}
