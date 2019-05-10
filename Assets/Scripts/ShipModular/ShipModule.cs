using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
{
	public float HP;
	public float maxHP;
	public int status;
	public GameObject hitEffect;
	public GameObject destructEffect;
	public int capCost;

	public void Restore()

	public void Disable()

	public void Destruct()

	public void Flash()
	}

	public void GetOwner()

	public void GetHP()
}
*/

public class ShipModule : MonoBehaviour {

	public enum moduleStatus{

		error = -1,

		//should always start waiting
		waiting = 0,
		//DO NOT OPERATE BELOW 0

		offline = 4,
		restoring = 5,
		//operational when > 5
		damaged = 9,

		online = 10
	}	public moduleStatus status;

	private SpaceShip _ship;

	public int maxHP;
	private float _HP;
	
	public Transform[] hardPoints;

	public GameObject damageEffect;

	//hit box is a red-colored collider that will detect hits to module and flash when hit
	public GameObject hitBox;

	//most recent entry
	public string moduleMessage;

	[Multiline]
	public string moduleLog;

	public bool Online{
		get{
			return (int)status > 5;
		}
		set{
			if (value)
				Activate();
			else
				Deactivate();
		}
	}
	
	private void OnValidate()
	{
		//check if there is more than one ship module on the same gameobject

		if (GetComponents<ShipModule>().Length > 1){
			status = moduleStatus.error;
			Log("ERROR: Multiple modules on same gameobject");
			gameObject.SetActive(false);
		}
	}

	public int HP{
		//If you're trying to do decimal damage, use HIT.	

		get {if (_HP > 0)
				return Mathf.RoundToInt(_HP);
			else return 0;
		}
		set {
			_HP = value;
			if (_HP > maxHP)
				_HP = maxHP;
		}

		//set { }
	}

	public float HPratio{
		get{ return _HP / (float)maxHP; }
	}

	public void Hit(float damage){
		//absolute value only.  Damage only
		damage = Mathf.Abs(damage);

		if (status == moduleStatus.online)
		{
			HitFlash = true;
			
			_HP -= damage;
		}
		if (HP == 0)
		{
			Deactivate();
			return;
		}
	}

	public SpaceShip Ship
	{
		get {
			if (status == moduleStatus.waiting)
				return null;
			else if (_ship == null)
			{
				//ERROR CATCH.  Kill this gameObject
				status = moduleStatus.error;
				Log(modName + " lost their parent ship");
				Deactivate();
				return null;
			}
			else
				return _ship;
		}
		set
		{
			//valid claim only if in same tree, and only if unclaimed
			if (_ship == null) {
				SpaceShip[] allParentShips = GetComponentsInParent<SpaceShip>();
				foreach (SpaceShip shiptest in allParentShips){
					if (shiptest == value){
						_ship = value;
						Log(name + " now belongs to " + value.name);
						Activate();
						return;

					}
				}
				Log(value.shipName + " is not a valid parent of " + name);
				
			}
		}
	}

	public void Log(string entry)
	{
		string logLine = Time.time + "\t" + entry + "\n";
		moduleLog += logLine;
	//	Debug.Log(ship + " " + logLine);
		moduleMessage = entry;
	}

	public string modName{
		get{
			string _modName = "";
			if (Ship != null)
				_modName += Ship.shipName + " ";

			_modName += getModuleType() + name;
			return _modName;
		}
	}

	public string getModuleType(){
		string modType = "";

		return modType;

	}

	//restores if deactivated
	public void Activate(){
		if (_ship == null)
		{

			Log(_ship.ToString());

			Log(modName + " can not activate. No owner found.");
			return;
		}
		
		if (!_ship.isNeutralized())
		{
			if (status == moduleStatus.offline){ }
				Log(modName + " has restored power");

			if (status == moduleStatus.waiting)
				Log(modName + " now online");

			status = moduleStatus.online;
			HP = maxHP;
			ShowDamage = false;
		}
		else
		{
			status = moduleStatus.offline;
			HP = 0;
			ShowDamage = false;
		}
	}
	
	public void Deactivate(){
		HP = 0;
		if (status == moduleStatus.damaged || status == moduleStatus.online)
		{
			status = moduleStatus.offline;
			Log(modName + " has been disabled.");
			ShowDamage = true;
		}
		if (status == moduleStatus.error){
			gameObject.SetActive(false);
			//just kill this if it's in error;
		}
	}

	private bool ShowDamage{
		set{
			if (damageEffect != null)
				damageEffect.SetActive(value);
		}
	}

	public bool HitFlash
	{
		get{
			if (hitBox == null)
				return false;
			Renderer hitEffect = hitBox.GetComponent<Renderer>();
			if (hitEffect != null)
			{
				return (hitEffect.enabled);
			}
			else return false;
		}
		set{
			if (hitBox == null)
				return;
			Renderer hitEffect = hitBox.GetComponent<Renderer>();
			if (hitEffect != null)
			{
				if (hitEffect.enabled == true)
					hitEffect.enabled = false;
				else
					hitEffect.enabled = value;
			}
		}
	}
	
	//CALL THIS ON EVERY FRAME
	public void ModuleUpdate(){
		if (status == moduleStatus.waiting) return;

		if (status == moduleStatus.error){
			Deactivate();
		}	

		if (Ship == null)
		{
			Deactivate();
			return;
		}
		
		if (HitFlash)
			HitFlash = false;

		if (Ship.isNeutralized())
			this.Deactivate();
		
	}
}
