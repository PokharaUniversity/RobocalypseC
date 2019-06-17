using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour {
	public int id;
	public Transform tip;
	public bool active;
	public GameObject Bullet;

	[Range(1,2)]
	public int firemode;
	public bool infiniteAmmo;
	public int ammoCapacityMag;
	public int ammoInMag;
	public int ammoInBag;
    [Header("gunStatus")]
    private bool FireReady;
    public float fireRate;
    [Range(0, 100)]
    public float accuracy;
    [Header("reload")]
	public float reloadTime;
	public float  ReloadCount;
	public bool reloading;
	public bool noAmmo;
	[Header("sounds")]
	public AudioClip[] bulletFireSound;
	public AudioClip emptyAudio;
	public AudioSource BulletAudioSource;
    [Header("Damage")]
    public float damage;
    public float healthDamageRatio;
    public float shieldDamageRatio;

	void Start(){
		//gameObject.SetActive (false);
		ammoInMag=ammoCapacityMag;
		reloading = false;
		ReloadCount = reloadTime;
        FireReady = true;
	}
    public void shoot(bool fromPlayer) {
        if (FireReady){
            //add Bullet Property
            GameObject bullet= Instantiate(Bullet, tip.transform.position, tip.transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = damage;
            bulletScript.healthDamageRatio = healthDamageRatio;
            bulletScript.ShieldDamageRatio = shieldDamageRatio;
            bulletScript.FromPlayer = fromPlayer;
            StartCoroutine(HaltFire(1 / fireRate));
        }
    }

    IEnumerator HaltFire(float time) {
        FireReady = false;
        yield return new WaitForSeconds(time);
        //Debug.Log("h");

        FireReady = true;
    }

	public void clear(){
		ammoInMag = ammoCapacityMag;

	
	}
	public void reload(){
		
		//UI
		//reloadingUI.Reloading = true;
		//
		if (ReloadCount > 0) {
			reloading = true;
		} else {
			reloading = false;
			//UI
			//reloadingUI.Reloading=false;
			//
			if (ammoInBag >= ammoCapacityMag - ammoInMag) {
				ammoInBag -= ammoCapacityMag - ammoInMag;
				ammoInMag = ammoCapacityMag;
			}else if(ammoInBag>0){
				ammoInMag += ammoInBag;
				ammoInBag = 0;

			}
			if (infiniteAmmo) {
				ammoInBag += ammoCapacityMag;
			}
			ReloadCount = reloadTime;
		}
	
	}
	public void haltReload(){
		reloading = false;
		ReloadCount = reloadTime;
	
	}
	public void activate(bool command){
		
		gameObject.SetActive (command);
		active = command;


	}
	void Update(){
		
		if (reloading) {
			ReloadCount -= Time.deltaTime;
			if (ReloadCount < 0) {
			
				reload ();
			}
		}
	
	}
	void NoAmmo(){
		noAmmo = true;
		//noAmmoUI.NoAmmo = true;

	}
	public void playAudio (){
		BulletAudioSource.Stop ();
		int randAudio=Random.Range(0,bulletFireSound.Length);
		BulletAudioSource.clip=bulletFireSound[randAudio];
		BulletAudioSource.Play ();
	
	}



}
